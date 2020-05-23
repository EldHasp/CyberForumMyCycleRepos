using System;
using System.ComponentModel;

namespace ConsoleINPC
{
	// Класс цель значений (View).
	public class TargetClass<T> where T : SourceClass
	{
		/// <summary>Имя экземпляра.</summary>
		public string Name { get; }

		/// <summary>Источник данных (ViewModel).</summary>
		public T Source { get; }

		/// <summary>Имя отслеживаемого свойства.</summary>
		public string PropertyName { get; }

		private object _value;

		/// <summary>Значение отслеживаемого свойства (Представление).</summary>
		public object Value
		{
			get => _value;
			set
			{
				// Проверка изменения значения. Если он не изменилось, то выход.
				if ((_value == null && value == null) || (_value != null && _value.Equals(value))) return;

				// Проверка, что Представление - это свойство Text.
				if (PropertyName == nameof(SourceClass.Text))
				{
					// Конвертация и присвоение значения.
					_value = value.ToString();
					Source.Text = value.ToString();
				}

				// Проверка, что Представление - это свойство Number и можно конвертировать в число.
				else if (PropertyName == nameof(SourceClass.Number) && int.TryParse(value.ToString(), out int val))
				{
					// Конвертация и присвоение значения.
					_value = val;
					Source.Number = val;
				}

				// Дефолтное присвоение значения.
				else
					_value = value;

				Console.WriteLine($"{Name}.Value={value}");
			}
		}

		/// <summary>Конструтор с заданем всех значений.</summary>
		/// <param name="name">Имя экземпляра.</param>
		/// <param name="source">Источник данных (ViewModel).</param>
		/// <param name="propertyName">Имя отслеживаемого свойства.</param>
		public TargetClass(string name, T source, string propertyName)
		{
			// Поверки на null и присвоения.
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));
			Name = name;
			Source = source ?? throw new ArgumentNullException(nameof(source));
			PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));

			// Подсоединение "прослушки" к PropertyChanged
			Source.PropertyChanged += Source_PropertyChanged;
		}

		/// <summary>Прослушка PropertyChanged.</summary>
		/// <param name="sender">Источник события - не используется.</param>
		/// <param name="e">В свойстве PropertyName содержится имя изменившегося свойства.</param>
		private void Source_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			// Проверка имени свойства и считывание соответствующего значения.
			if (e.PropertyName == nameof(SourceClass.Text))
				Value = Source.Text;
			if (e.PropertyName == nameof(SourceClass.Number))
				Value = Source.Number;
		}
	}
}
