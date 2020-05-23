using System.ComponentModel;

namespace ConsolePropertyDescriptorSource
{
	// Класс источник значений (ViewModel).
	public class SourceClassPDS
	{
		private string _text;
		private int _number;
		private double _length;

		/// <summary>Текстовое свойство.</summary>
		public string Text
		{
			get => _text;
			set
			{
				if (_text == value) return;
				PropertyCollection.Find(nameof(Text), false).SetValue(this, _text = value);
				Length = Text?.Length ?? -1;
			}
		}

		/// <summary>Целочисленное свойство.</summary>
		public int Number
		{
			get => _number;
			set
			{
				if (_number == value) return;
				PropertyCollection.Find(nameof(Number), false).SetValue(this, _number = value);
			}
		}

		/// <summary>Числовое свойство.</summary>
		public double Length
		{
			get => _length;
			set
			{
				if (_length == value) return;
				PropertyCollection.Find(nameof(Length), false).SetValue(this, _length = value);
			}
		}

		private static readonly PropertyDescriptorCollection PropertyCollection
			= TypeDescriptor.GetProperties(typeof(SourceClassPDS));
	}
}
