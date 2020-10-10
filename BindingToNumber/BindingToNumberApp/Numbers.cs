using System;
using System.Diagnostics;

namespace AppBindingToNumeric
{
    public class Numbers : OnPropertyChangedClass
    {
        private double _doubleValue;
        private decimal _decimalValue;
        private int _integerValue;

        public double DoubleValue
        {
            get => _doubleValue;
            set => SetProperty(ref _doubleValue, value);
        }

        public decimal DecimalValue
        {
            get => _decimalValue;
            set => SetProperty(ref _decimalValue, value);
        }

        public int IntegerValue
        {
            get => _integerValue;
            set => SetProperty(ref _integerValue, value);
        }

    }

}
