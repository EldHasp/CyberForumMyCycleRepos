using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ChangeXmlLanguage
{
    public class StringToNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CultureInfo  current = CultureInfo.CurrentCulture;
            object result = "Ошибка";
            if (decimal.TryParse(value?.ToString(), out decimal num))
               result = num;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static StringToNumberConverter Instance { get; } = new();
    }
    public class StringToNumberExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return StringToNumberConverter.Instance;
        }
    }
}
