using DataTransferBetweenWindows;
using System;

namespace Locator
{
    public class DateTimeContainer : OnPropertyChangedClass
    {
        private DateTime _birthday;

        public DateTime Birthday { get => _birthday; set => SetProperty(ref  _birthday, value); }
    }
}
