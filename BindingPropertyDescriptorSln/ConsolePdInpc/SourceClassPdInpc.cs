using System.ComponentModel;

namespace ConsolePdInpc
{
	// Класс источник значений (ViewModel).
	public class SourceClassPdInpc : INotifyPropertyChanged
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

				_number = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Number)));
			}
		}

		private double _length;

		public double Length
		{
			get => _length;
			set
			{
				if (_length == value) return;

				_length = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Length)));
			}
		}
	}

}
