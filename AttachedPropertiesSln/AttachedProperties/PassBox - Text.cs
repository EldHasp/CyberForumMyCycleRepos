using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AttachedProperties
{
	// В этом файле часть класса с присоединённым свойством для привязки PasswordBox.Password.

	/// <summary>Класс с присоединёнными свойствами для PasswordBox.</summary>
	public static partial class PassBox
	{

		/// <summary>Класс объекта-слушателя события PasswordChanged.</summary>
		private class ListenerTextPasswordBox
		{
			/// <summary>PasswordBox чьё событие PasswordChanged прослушивается.</summary>
			public PasswordBox PasswordBox { get; }

			/// <summary>Единственный конструктор.</summary>
			/// <param name="passwordBox">PasswordBox чьё событие PasswordChanged прослушивается.
			/// Исключение - если <see langword="null"/>.</param>
			public ListenerTextPasswordBox(PasswordBox passwordBox)
			{
				PasswordBox = passwordBox ?? throw new ArgumentNullException(nameof(passwordBox));
				PasswordBox.PasswordChanged += PasswordChanged;
			}

			/// <summary>Флаг изменения данных внутри объекта-слушателя.
			/// Необходим для избежания зацикливания из-за двусторонней привязки
			/// присоединённого свойства.</summary>
			public bool IsDataTransfer { get; private set; }

			/// <summary>Метод-слушатель.</summary>
			private void PasswordChanged(object sender, RoutedEventArgs e)
			{
				// Проверка флага.
				if (IsDataTransfer)
					return;

				// Установка флага, нового значения присоединённому свойству и сброс флага.
				IsDataTransfer = true;
				SetText(PasswordBox, PasswordBox.Password);
				IsDataTransfer = false;
			}

			/// <summary>Метод задания нового пароля.</summary>
			/// <param name="text">Новый пароль.</param>
			public void ChangePassword(string text)
			{

				// Проверка флага.
				if (IsDataTransfer)
					return;

				// Установка флага, нового значения пароля и сброс флага.
				IsDataTransfer = true;
				PasswordBox.Password = text;
				IsDataTransfer = false;
			}

		}

		/// <summary>Приватное свойство для получения объекта-слушателя.</summary>
		private static ListenerTextPasswordBox GetListenerText(PasswordBox passwordBox)
		{
			ListenerTextPasswordBox listener = (ListenerTextPasswordBox)passwordBox.GetValue(ListenerTextPropertyKey.DependencyProperty);

			// Если элемент не задан, то его создание и сохранение.
			if (listener == null)
				passwordBox.SetValue(ListenerTextPropertyKey, listener = new ListenerTextPasswordBox(passwordBox));

			return listener;
		}

		// Using a DependencyProperty as the backing store for Listener.  This enables animation, styling, binding, etc...
		private static readonly DependencyPropertyKey ListenerTextPropertyKey =
			DependencyProperty.RegisterAttachedReadOnly("Listener", typeof(ListenerTextPasswordBox), typeof(PassBox), new PropertyMetadata(null));

		/// <summary>Возвращает текстовое значение пароля.</summary>
		/// <param name="passwordBox">PasswordBox к которому присоединено свойство.</param>
		/// <returns>string с паролем.</returns>
		public static string GetText(PasswordBox passwordBox)
		{
			return (string)passwordBox.GetValue(TextProperty);
		}

		/// <summary>Задаёт текстовое значение пароля.</summary>
		/// <param name="passwordBox">PasswordBox к которому присоединено свойство.</param>
		/// <param name="value">Новое значение пароля.</param>
		public static void SetText(PasswordBox passwordBox, string value)
		{
			passwordBox.SetValue(TextProperty, value);
		}

		// Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TextProperty =
			DependencyProperty.RegisterAttached("Text", typeof(string), typeof(PassBox),
				new FrameworkPropertyMetadata(null, ChangeText)
				{
					BindsTwoWayByDefault = true,
					DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
				});

		/// <summary>Обработчик изменения значения свойства.</summary>
		/// <param name="d">Объект - владелец свойства. Исключение - если не PasswordBox.</param>
		/// <param name="e">Аргументы изменения.</param>
		private static void ChangeText(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Если старое и новое значения равны, то выход из метода.
			if (
					(e.OldValue == null && e.NewValue == null)
					|| (e.OldValue != null && e.OldValue.Equals(e.NewValue))
				)
				return;


			// Приведение к PasswordBox.
			if (!(d is PasswordBox passwordBox))
				throw new ArgumentException("Должен быть PasswordBox", nameof(d));

			// Получение объекта-слушателя события PasswordChanged.
			ListenerTextPasswordBox listener = GetListenerText(passwordBox);

			// Передача нового значения пароля.
			listener.ChangePassword((string)e.NewValue);

		}
	}
}
