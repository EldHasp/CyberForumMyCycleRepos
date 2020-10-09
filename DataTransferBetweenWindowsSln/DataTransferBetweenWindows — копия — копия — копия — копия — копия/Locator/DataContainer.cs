using DataTransferBetweenWindows;

namespace Locator
{
    public class DataContainer : OnPropertyChangedClass
    {
        private string _text;
        private int _number;

        public string Text { get => _text; set => SetProperty(ref _text , value); }
        public int Number { get => _number; set => SetProperty(ref  _number, value); }
    }
}
