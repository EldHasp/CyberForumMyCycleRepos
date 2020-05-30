using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AttachedProperties
{
	// В этом файле часть класса с присоединёнными свойствами для безопасной работы с PasswordBox.

	public static partial class PassBox
	{
		/// <summary>Класс для сохранения связи с PassworBox к которому присоединены свойства.</summary>
		private class ListenerSecurityPasswordBox
		{
			/// <summary>PasswordBox к которому присоединены свойства.</summary>
			public PasswordBox PasswordBox { get; }

			/// <summary>Поле с экземпляром шифратора.</summary>
			private readonly SHA256 sha256 = SHA256.Create();

			/// <summary>Единственный конструктор.</summary>
			/// <param name="passwordBox">PasswordBox к которому присоединены свойства.
			/// Исключение - если <see langword="null"/>.</param>
			public ListenerSecurityPasswordBox(PasswordBox passwordBox)
			{
				PasswordBox = passwordBox ?? throw new ArgumentNullException(nameof(passwordBox));
			}

			/// <summary>Метод возвращающий ХЕШ-код пароля с подмешанной "солью".</summary>
			/// <param name="salt">"Соль" - дополнительная информация.
			/// Может быть полезной или случайной.
			/// Удобно использовать для получения общего ХЕШ от пароля и логина.
			/// Допускается <see langword="null"/>.</param>
			/// <returns>Байтовый массив с  ХЕШ-кодом.</returns>
			public byte[] GetHashCode256(IEnumerable<char> salt = null)
			{
				if (PasswordBox.Password == null && salt == null)
					return null;

				byte[] bytesPass;
				if (PasswordBox.Password == null)
					bytesPass = Array.Empty<byte>();
				else
					bytesPass = Encoding.Unicode.GetBytes(PasswordBox.Password);

				byte[] bytesSalt;
				if (salt == null)
					bytesSalt = Array.Empty<byte>();
				else
				{
					if (salt is string slt)
						bytesSalt = Encoding.Unicode.GetBytes(slt);
					else
						bytesSalt = Encoding.Unicode.GetBytes(salt.ToArray());
				}

				byte[] bytes = new byte[bytesPass.Length + bytesSalt.Length];
				Array.Copy(bytesPass, bytes, bytesPass.Length);
				Array.Copy(bytesSalt, 0, bytes, bytesPass.Length, bytesSalt.Length);

				byte[] hash = sha256.ComputeHash(bytes);

				Random random = new Random();
				foreach (byte[] bts in new byte[][] { bytes, bytesPass, bytesSalt })
					for (int i = 0; i < bts.Length; i++)
						bts[i] = (byte)random.Next(256);

				return hash;
			}

			/// <summary>Метод возвращающий зашифрованный пароль.</summary>
			/// <param name="xmlPublicKey">Публичный RSA ключ.</param>
			/// <returns>Байтовый массив с зашифрованным паролем.</returns>
			public byte[] GetRSA(string xmlPublicKey)
			{
				using (var provide = new RSACryptoServiceProvider())
				{
					byte[] bytesPass;
					if (PasswordBox.Password == null)
						bytesPass = Array.Empty<byte>();
					else
						bytesPass = Encoding.Unicode.GetBytes(PasswordBox.Password);
					provide.FromXmlString(xmlPublicKey);

					byte[] enc = provide.Encrypt(bytesPass, false);

					Random random = new Random();
					for (int i = 0; i < bytesPass.Length; i++)
						bytesPass[i] = (byte)random.Next(256);

					return enc;
				}
			}
		}

		/// <summary>Приватное свойство возвращающее объект с методами шифрования.</summary>
		/// <param name="passwordBox">PasswordBox  с котрым работаю методы.</param>
		/// <returns>Объект с методами шифрвания.</returns>
		private static ListenerSecurityPasswordBox GetListenerSecurity(PasswordBox passwordBox)
		{
			ListenerSecurityPasswordBox listener = (ListenerSecurityPasswordBox)passwordBox.GetValue(ListenerSecurityPropertyKey.DependencyProperty);

			// Если элемент не задан, то его создание и сохранение.
			if (listener == null)
				passwordBox.SetValue(ListenerSecurityPropertyKey, listener = new ListenerSecurityPasswordBox(passwordBox));

			return listener;
		}

		// Using a DependencyProperty as the backing store for Listener.  This enables animation, styling, binding, etc...
		private static readonly DependencyPropertyKey ListenerSecurityPropertyKey =
			DependencyProperty.RegisterAttachedReadOnly("ListenerSecurity", typeof(ListenerSecurityPasswordBox), typeof(PassBox), new PropertyMetadata(null));


		/// <summary>Возвращает делегат метода для получения ХЕШ-кода.</summary>
		/// <param name="passwordBox">PasswordBox с которым работает метод.</param>
		/// <returns>Делегат с сигнатурой Func IEnumerable char , byte[] .
		/// При вызове в параметр делегата можно добавить набор символов.</returns>
		public static Func<IEnumerable<char>, byte[]> GetGetHashCode256(PasswordBox passwordBox)
		{
			return (Func<IEnumerable<char>, byte[]>)passwordBox.GetValue(GetHashCode256Property);
		}

		/// <summary>Задаёт делегат метода для получения ХЕШ-кода.
		/// Этот метод нужен только для возможности задания привязки.
		/// Изменить им значение - не возможно.</summary>
		/// <param name="passwordBox">PasswordBox с которым работает метод.</param>
		/// <param name="value">Делегат с сигнатурой Func IEnumerable char , byte[] .</param>
		public static void SetGetHashCode256(PasswordBox passwordBox, Func<IEnumerable<char>, byte[]> value)
		{
			passwordBox.SetValue(GetHashCode256Property, value);
		}

		// Using a DependencyProperty as the backing store for GetHashCode256.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty GetHashCode256Property =
			DependencyProperty.RegisterAttached("GetHashCode256", typeof(Func<IEnumerable<char>, byte[]>), typeof(PassBox),
				new FrameworkPropertyMetadata((Func<IEnumerable<char>, byte[]>)GetHashCode256Default, ChangeHash)
				{ BindsTwoWayByDefault = true });

		/// <summary>Дефолтное значение свойства.
		/// Необходимо для правильной работы при задании привязки.</summary>
		private static byte[] GetHashCode256Default(IEnumerable<char> salt)
			=> throw new NotImplementedException();

		/// <summary>Метод обратного вызова при изменении значения свойства.
		/// Метод всегда возвращает значение к делегату метода GetHashCode256.</summary>
		private static void ChangeHash(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Приведение к PasswordBox.
			if (!(d is PasswordBox passwordBox))
				throw new ArgumentException("Должен быть PasswordBox", nameof(d));

			// Получение объекта слушателя.
			ListenerSecurityPasswordBox listener = GetListenerSecurity(passwordBox);

			// Передача ссылки на метод для привязки обратного вызова, если новое значение иное.
			if (e.NewValue == null || !e.NewValue.Equals((Func<IEnumerable<char>, byte[]>)listener.GetHashCode256))
				SetGetHashCode256(passwordBox, listener.GetHashCode256);
		}

		/// <summary>Возвращает делегат метода для получения зашифрованного пароля.</summary>
		/// <param name="passwordBox">PasswordBox с которым работает метод.</param>
		/// <returns>Делегат с сигнатурой Func string , byte[] .
		/// При вызове в параметре делегата нужно передать строку с публичным ключом.</returns>
		public static Func<string, byte[]> GetGetRSA(PasswordBox obj)
		{
			return (Func<string, byte[]>)obj.GetValue(GetRSAProperty);
		}

		/// <summary>Задаёт делегат метода для получения зашифрованного пароля.
		/// Этот метод нужен только для возможности задания привязки.
		/// Изменить им значение - не возможно.</summary>
		/// <param name="passwordBox">PasswordBox с которым работает метод.</param>
		/// <param name="value">Делегат с сигнатурой Func string , byte[] .</param>
		public static void SetGetRSA(PasswordBox obj, Func<string, byte[]> value)
		{
			obj.SetValue(GetRSAProperty, value);
		}

		// Using a DependencyProperty as the backing store for GetRSA.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty GetRSAProperty =
			DependencyProperty.RegisterAttached("GetRSA", typeof(Func<string, byte[]>), typeof(PassBox),
				new FrameworkPropertyMetadata((Func<string, byte[]>)GetRSADefault, ChangeRSA) { BindsTwoWayByDefault = true });

		/// <summary>Дефолтное значение свойства.
		/// Необходимо для правильной работы при задании привязки.</summary>
		private static byte[] GetRSADefault(string xmlPublicKey)
				=> throw new NotImplementedException();

		/// <summary>Метод обратного вызова при изменении значения свойства.
		/// Метод всегда возвращает значение к делегату метода GetRSA.</summary>
		private static void ChangeRSA(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Приведение к PasswordBox.
			if (!(d is PasswordBox passwordBox))
				throw new ArgumentException("Должен быть PasswordBox", nameof(d));

			// Получение объекта слушателя.
			ListenerSecurityPasswordBox listener = GetListenerSecurity(passwordBox);

			// Передача ссылки на метод для привязки обратного вызова.
			if (e.NewValue == null || !e.NewValue.Equals((Func<string, byte[]>)listener.GetRSA))
				SetGetRSA(passwordBox, listener.GetRSA);
		}
	}
}
