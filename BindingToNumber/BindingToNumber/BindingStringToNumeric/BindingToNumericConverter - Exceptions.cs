using System;
using System.Globalization;

namespace BindingStringToNumeric
{
    public partial class NumericConverter
    {
        /// <summary>Исключение возникающее в <see cref="Convert(object[], Type, object, CultureInfo)"/> при неверном целевом типе.</summary>
        public static ArgumentException InvalidTargetType { get; } = new ArgumentException("Целевым типом могут быть только string и object.", "targetType");

        /// <summary>Исключение возникающее в <see cref="ConvertBack(object, Type[], object, CultureInfo)"/> при неверном типе значения  пришедшего от целевого свойства.</summary>
        public static ArgumentException InvalidValueType { get; } = new ArgumentException("Значением должна быть строка (string) или null.", "value");

        /// <summary>Исключение возникающее в <see cref="Convert(object[], Type, object, CultureInfo)"/> при неверном количестве привязок.</summary>
        public static ArgumentException InvalidNumberValues { get; } = new ArgumentException("Конвертеру надо передать два и только два значения.", "values.Length");

        /// <summary>Исключение возникающее в <see cref="ConvertBack(object, Type[], object, CultureInfo)"/> при неверном количестве привязок.</summary>
        public static ArgumentException InvalidNumberTargets { get; } = new ArgumentException("Целевых типов должно быть два и только два.", "targetTypes.Length");

        /// <summary>Исключение возникающее в <see cref="Convert(object[], Type, object, CultureInfo)"/> при неверном типе по первой привязке.</summary>
        public static ArgumentException InvalidNumberType { get; } = new ArgumentException("Первым значением должен быть численный тип.", "values[0]");

        /// <summary>Исключение возникающее в <see cref="ConvertBack(object, Type[], object, CultureInfo)"/> при неверном типе по первой привязке.</summary>
        public static ArgumentException InvalidTargetNumberType { get; } = new ArgumentException("Первым целевым типом должен быть численный тип.", "targetTypes[0]");

        /// <summary>Исключение возникающее в <see cref="Convert(object[], Type, object, CultureInfo)"/> при неверном типе по второй привязке.</summary>
        public static ArgumentException InvalidStringType { get; } = new ArgumentException("Вторым значением должена быть строка (string) или null.", "values[1]");

        /// <summary>Исключение возникающее в <see cref="ConvertBack(object, Type[], object, CultureInfo)"/> при неверном типе во второй привязке.</summary>
        public static ArgumentException InvalidTargetStringType { get; } = new ArgumentException("Вторым целевым типом могут быть только string и object.", "targetTypes[1]");
    }
}
