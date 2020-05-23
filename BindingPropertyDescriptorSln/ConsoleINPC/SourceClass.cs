using System.ComponentModel;

namespace ConsoleINPC
{
	// Класс источник значений (ViewModel).
	public class SourceClass : INotifyPropertyChanged
	{
		private string _text;
		private int _number;

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>Текстовое свойство.</summary>
		public string Text
		{
			get => _text;
			set
			{
				if (_text == value) return;

				_text = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Text)));
			}
		}

		/// <summary>Целочисленное свойство.</summary>
		public int Number
		{
			get => _number;
			set
			{
				if (_number == value) return;

				_number = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));
			}
		}

		private double _lenght;

		public double Lenght
		{
			get => _lenght; 
			set
			{
				if (_lenght == value) return;

				_lenght = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Lenght)));
			}
		}
	}

}
