using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace BindingStringToNumeric
{
    /// <summary>Сравнивает полученное число со <see cref="String"/> текстом.<br/>
    /// Если из текста получается такое же число, то присвоение значение по привязке отменяется.</summary>
    /// <remarks>Значения должны приходить в массиве в параметре values метода <see cref="Convert"/><br/>
    /// В массиве должно быть два значения:<br/>
    /// 0 - значение источника в числовом типе,<br/>
    /// 1 - <see cref="String"/> Text с которым надо сравнить число.</remarks>
    public partial class NumericConverter : IMultiValueConverter
    {
        public virtual object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Write(GetType().Name + $".Convert.values: {values[0]}, \"{values[1]}\"");
            object ret = null;

            // Целевой тип должен быть или строкой или object.
            if (targetType != typeof(string) && targetType != typeof(object))
                throw InvalidTargetType;

            // Должно передаваться две и только две привязки.
            if (values?.Length != 2)
                throw InvalidNumberValues;

            // Получение парсера для типа в первой привязке (привязка к Источнику).
            TryParseNumberHandler tryParse = NumericTryParse.GetTryParse(values[0]?.GetType());
            // Если парсер получить не удалось, значит первая привязка получила значение недопустимого типа.
            if (tryParse == null)
                throw InvalidNumberType;
            object source = values[0];

            // Тип значения по второй привязке (сравниваемый текст) должен быть только string.
            if (values[1]?.GetType() != typeof(string))
                throw InvalidStringType;
            string text = (string)values[1];

            // Получение цифрового стиля из параметра. Если он не задан, то используется стиль по умолчанию для этого типа.
            NumberStyles style = NumericTryParse.GetNumberStyle(parameter, values[0].GetType());

            // Получение из строки (во второй привязке) числа в том же типе, что получено от Источника (в первой привязке).
            // И сравнение этого числа с числом Источника.
            if (tryParse(text, style, culture, out object target) && target.Equals(source))
            {
                // Если числа равны, то проверяется наличие крайних пробелов в строке.
                // Если пробелов нет, то отменяется присвоение значения привязкой.
                string trim = text.Trim();
                if (trim == text)
                    ret = Binding.DoNothing;

                // Иначе присваивается строка с удалёнными крайними пробелами.
                else
                    ret = trim;
            }

            // Если из строки не удалось получить число, или это число не равно числу Источника,
            // то возвращается число Источника в текстовом виде в заданной культуре.
            else
                ret = System.Convert.ToString(source, culture);


            Debug.WriteLine($"; return: {ret ?? "null"}");
            return ret; ;

        }

        public virtual object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Debug.Write(GetType().Name + $".ConvertBack.value: \"{value}\" to ");
            object ret = null;

            // Полученной значение может быть только строкой или null.
            if (value != null && value.GetType() != typeof(string))
                throw InvalidValueType;

            // Если получен null или строка из пробелов, то используется "0".
            string text = (string)value;
            if (string.IsNullOrWhiteSpace(text))
                text = "0";

            // Должно быть две и только две привязки.
            if (targetTypes?.Length != 2)
                throw InvalidNumberTargets;

            // Получение парсера для типа в первой привязке (привязка к Источнику).
            TryParseNumberHandler tryParse = NumericTryParse.GetTryParse(targetTypes[0]);
            // Если парсер получить не удалось, значит первая привязка получила значение недопустимого типа.
            if (tryParse == null)
                throw InvalidTargetNumberType;

            // Тип значения по второй привязке (сравниваемый текст) должен быть только string.
            if (targetTypes[1] != typeof(string))
                throw InvalidTargetStringType;

            // Получение цифрового стиля из параметра. Если он не задан, то используется стиль по умолчанию для этого типа.
            NumberStyles style = NumericTryParse.GetNumberStyle(parameter, targetTypes[0]);

            // Получение из полученного значения числа заданного типа в заданной культуре
            // Если удалось, то оно возвращается.
            if (tryParse(text, style, culture, out object target))
                ret = target;


            Debug.WriteLine($"return: {ret ?? "null"}");

            // Если ret значение не присваивалось, то значит строка некорректна
            // Тогда возвращается null, что вызывает ошибку валидации.
            if (ret == null)
                return null;

            // Иначе возвращается массив с одним элементом: полученным числом.
            return new object[] { ret };
        }

        /// <summary>Экземпляр конвертера.<br/>
        /// Упрощает использование конвертера:
        /// можно обращаться к нему, а не создавать экземпляр в ресурсах.</summary>
        public static NumericConverter Instance { get; } = new NumericConverter();
    }

}
