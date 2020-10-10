using DataTransferBetweenWindows;
using System;

namespace MvvmShort
{
    public class DateTimeContainer : OnPropertyChangedClass
    {
        private DateTime _birthday;

        public DateTime Birthday { get => _birthday; set => SetProperty(ref  _birthday, value); }
    }
}
