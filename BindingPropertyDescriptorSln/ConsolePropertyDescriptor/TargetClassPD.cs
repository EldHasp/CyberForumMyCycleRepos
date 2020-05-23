using System;
using System.ComponentModel;

namespace ConsolePropertyDescriptor
{
	// Класс цель значений (View).
	public class TargetClassPD
	{
		/// <summary>Имя экземпляра.</summary>
		public string Name { get; }

		/// <summary>Источник данных (ViewModel).</summary>
		public object Source { get; }

		/// <summary>Имя отслеживаемого свойства.</summary>
		public string PropertyName { get; }

		/// <summary>Дескриптор свойства</summary>
		public PropertyDescriptor PropertyDescriptor { get; }

		private object _value;

		/// <summary>Значение отслеживаемого свойства (Представление).</summary>
		public object Value
		{
			get => _value;
			set
			{
				// Проверка изменения значения. Если оно не изменилось, то выход.
				if ((_value == null && value == null) || (_value != null && _value.Equals(value))) return;


				if (value.GetType() == PropertyDescriptor.PropertyType)
					PropertyDescriptor.SetValue(Source, _value = value);

				else
					try
					{
						PropertyDescriptor.SetValue(Source, _value = PropertyDescriptor.Converter.ConvertFrom(value));
					}
					catch (Exception)
					{
						_value = value;
					}
					Console.WriteLine($"{Name}.Value={value}");
					}
			}

		/// <summary>Конструтор с заданем всех значений.</summary>
		/// <param name="name">Имя экземпляра.</param>
		/// <param name="source">Источник данных (ViewModel).</param>
		/// <param name="propertyName">Имя отслеживаемого свойства.</param>
		public TargetClassPD(string name, object source, string propertyName)
		{
			// Поверки на null и присвоения.
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));
			Name = name;
			Source = source ?? throw new ArgumentNullException(nameof(source));
			PropertyName = propertyName;

			// Получение дескриптора свойства по имеющемуся объекту и имени свойства.
			PropertyDescriptor = TypeDescriptor.GetProperties(Source).Find(PropertyName, false)
				?? throw new ArgumentException("Такого свойства нет.", nameof(PropertyName));

			// Подсоединение обработчика изменения значения.
			PropertyDescriptor.AddValueChanged(Source, PropertyValueChanged);
		}

		/// <summary>Обработчик изменения значения.</summary>
		/// <param name="sender">Источник события - не используется.</param>
		/// <param name="e">Параметр без данных - не используется.</param>
		private void PropertyValueChanged(object sender, EventArgs e)
		{
			Value = PropertyDescriptor.GetValue(Source);
		}
	}

}
