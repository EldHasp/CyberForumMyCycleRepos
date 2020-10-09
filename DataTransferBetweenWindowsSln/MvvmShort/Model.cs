using System;

namespace MvvmShort
{
    public class Model : IModel
    {
        public event ValueChangedHandler ValueChanged;

        public static string StringValue { get; }
        private string stringValue;
        private void SetText(string newText)
        {
            if (stringValue == newText)
                return;

            string oldText = stringValue;
            stringValue = newText;

            ValueChanged?.Invoke(this, nameof(StringValue), oldText, stringValue);
        }

        public static int IntValue { get; }
        private int intValue;
        private void SetNumber(int newNumber)
        {
            if (intValue == newNumber)
                return;

            int oldNumber = intValue;
            intValue = newNumber;

            ValueChanged?.Invoke(this, nameof(IntValue), oldNumber, intValue);
        }

        public void SendValue(string valueName, object newValue)
        {
            switch (valueName)
            {
                case nameof(StringValue): SetText((string)newValue); break;
                case nameof(IntValue): SetNumber((int)newValue); break;
                default: throw new ArgumentException(nameof(valueName));
            }
        }

        public bool ValidateValue(string valueName, object newValue)
        {
            switch (valueName)
            {
                case nameof(StringValue): return newValue is string;
                case nameof(IntValue): return newValue is int;
                default: return false;
            }
        }

        public void AllValueChanged()
        {
            ValueChanged?.Invoke(this, nameof(StringValue), null, stringValue);
            ValueChanged?.Invoke(this, nameof(IntValue), null, intValue);
        }
    }

}
