using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ChangeXmlLanguage
{
    public class LanguageToCultureConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is XmlLanguage language)
                return language.GetSpecificCulture();
            return CultureInfo.CurrentCulture;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public static LanguageToCultureConverter Instance { get; } = new();
    }
    public class LanguageToCultureExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return LanguageToCultureConverter.Instance;
        }
    }
}
