using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace BindingStringToNumeric
{
    /// <summary>Сравнивает полученное <see cref="Double"/> число со <see cref="String"/> текстом.<br/>
    /// Если из текста получается такое же число, то присвоение значение по привязке отменяется.</summary>
    /// <remarks>Значения должны приходить в массиве в параметре values метода <see cref="IMultiValueConverter.Convert(object[], Type, object, CultureInfo)"/><br/>
    /// В массиве должо быть два значения:<br/>
    /// 0 - значение источника в <see cref="Double"/> типе,<br/>
    /// 1 - <see cref="String"/> Text с которым надо сравнить число.</remarks>
    public class DoubleConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Write(GetType() + $".Convert.values: {values[0]}, \"{values[1]}\"");
            double source = (double)values[0];
            string text = (string)values[1];

            object ret;
            // Получение из текста числа (в переданной культуре) и сравнение его с числом источника.
            // Если они равны, то отменяется присвоение значения.
            if (double.TryParse(text, NumberStyles.Any, culture, out double target) && target == source)
                ret = Binding.DoNothing;

            // Иначе число источника переводится в строку в заданнной культуре  и возвращается.
            else
                ret = source.ToString(culture);

            Debug.WriteLine($"; return: {ret ?? "null"}");
            return ret; ;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Debug.Write(GetType() + $".ConvertBack.value: \"{value}\" to ");
            object ret = null;

            string text = (string)value;

            // Если строка пустая, то это считается эквивалентом нуля.
            if (string.IsNullOrWhiteSpace(text))
                ret = 0.0;

            // Иначе проверяется возвожность перевода строки в число в заданной культуре.
            // Если перевод возможен, то возвращается полученное число.
            else if (double.TryParse(text, NumberStyles.Any, culture, out double target))
                ret = target;

            Debug.WriteLine($"return: {ret ?? "null"}");

            // Если ret значение не присваивалось, то значит строка некорректна
            // Тогда возвращается null, что вызывает ошибку валидации.
            if (ret == null)
                return null;

            // Иначе возвращается массив с одним элементом: полученным числом.
            return new object[] { ret };
        }

    }
}
