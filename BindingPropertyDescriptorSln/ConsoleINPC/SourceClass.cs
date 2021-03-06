﻿using System.ComponentModel;

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
