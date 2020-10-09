using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace BindingStringToNumeric
{
    /// <summary>Делегат числового парсера.<br/>
    ///     Предпринимает попытку преобразования числа в формате, который определяется заданным
    ///     стилем и языком и региональными параметрами, в эквивалент числового типа и
    ///     возвращает значение, определяющее, успешно ли выполнено преобразование.
    /// </summary>
    /// <param name="s">Строка, представляющая преобразуемое число.</param>
    /// <param name="style">Побитовая комбинация значений перечисления, которая показывает разрешенный формат
    ///     параметра s. </param>
    /// <param name="provider">Объект, который предоставляет сведения о форматировании параметра s в зависимости
    ///     от языка и региональных параметров.</param>
    /// <param name="result">При возвращении этим методом содержит числовое значение со знаком,
    ///     эквивалентное числу, содержащемуся в параметре s, если преобразование выполнено
    ///     успешно, или нуль, если оно завершилось неудачей. Преобразование завершается
    ///     сбоем, если параметр s равен <see langword="null"/> или <see cref="String.Empty"/>, не находится в формате,
    ///     совместимом с style или представляет собой число меньше MinValue
    ///     или больше MaxValue. Этот параметр передается неинициализированным;
    ///     любое значение, первоначально предоставленное в объекте result, будет перезаписано.</param>
    /// <returns> Значение <see langword="true"/>, если параметр s успешно преобразован; в противном случае — значение <see langword="false"/>.</returns>
    public delegate bool TryParseNumberHandler(string s, NumberStyles style, IFormatProvider provider, out object result);
    public static class NumericTryParse

    {
        ///<inheritdoc cref="sbyte.TryParse(string, NumberStyles, IFormatProvider, out sbyte)"/>
        public static bool TryParseSbyte(string s, NumberStyles style, IFormatProvider provider, out object result)
        {
            if (sbyte.TryParse(s, style, provider, out sbyte number))
            {
                result = number;
                return true;
            }
            result = null;
            return false;
        }

        ///<inheritdoc cref="byte.TryParse(string, NumberStyles, IFormatProvider, out byte)"/>
        public static bool TryParseByte(string s, NumberStyles style, IFormatProvider provider, out object result)
        {
            if (byte.TryParse(s, style, provider, out byte number))
            {
                result = number;
                return true;
            }
            result = null;
            return false;
        }

        ///<inheritdoc cref="short.TryParse(string, NumberStyles, IFormatProvider, out short)"/>
        public static bool TryParseShort(string s, NumberStyles style, IFormatProvider provider, out object result)
        {
            if (short.TryParse(s, style, provider, out short number))
            {
                result = number;
                return true;
            }
            result = null;
            return false;
        }

        ///<inheritdoc cref="ushort.TryParse(string, NumberStyles, IFormatProvider, out ushort)"/>
        public static bool TryParseUshort(string s, NumberStyles style, IFormatProvider provider, out object result)
        {
            if (ushort.TryParse(s, style, provider, out ushort number))
            {
                result = number;
                return true;
            }
            result = null;
            return false;
        }

        ///<inheritdoc cref="int.TryParse(string, NumberStyles, IFormatProvider, out int)"/>
        public static bool TryParseInt(string s, NumberStyles style, IFormatProvider provider, out object result)
        {
            if (int.TryParse(s, style, provider, out int number))
            {
                result = number;
                return true;
            }
            result = null;
            return false;
        }

        ///<inheritdoc cref="uint.TryParse(string, NumberStyles, IFormatProvider, out uint)"/>
        public static bool TryParseUint(string s, NumberStyles style, IFormatProvider provider, out object result)
        {
            if (uint.TryParse(s, style, provider, out uint number))
            {
                result = number;
                return true;
            }
            result = null;
            return false;
        }

        ///<inheritdoc cref="long.TryParse(string, NumberStyles, IFormatProvider, out long)"/>
        public static bool TryParseLong(string s, NumberStyles style, IFormatProvider provider, out object result)
        {
            if (long.TryParse(s, style, provider, out long number))
            {
                result = number;
                return true;
            }
            result = null;
            return false;
        }

        ///<inheritdoc cref="ulong.TryParse(string, NumberStyles, IFormatProvider, out ulong)"/>
        public static bool TryParseUlong(string s, NumberStyles style, IFormatProvider provider, out object result)
        {
            if (ulong.TryParse(s, style, provider, out ulong number))
            {
                result = number;
                return true;
            }
            result = null;
            return false;
        }

        ///<inheritdoc cref="double.TryParse(string, NumberStyles, IFormatProvider, out double)"/>
        public static bool TryParseDouble(string s, NumberStyles style, IFormatProvider provider, out object result)
        {
            if (double.TryParse(s, style, provider, out double number))
            {
                result = number;
                return true;
            }
            result = null;
            return false;
        }

        ///<inheritdoc cref="float.TryParse(string, NumberStyles, IFormatProvider, out float)"/>
        public static bool TryParseFloat(string s, NumberStyles style, IFormatProvider provider, out object result)
        {
            if (float.TryParse(s, style, provider, out float number))
            {
                result = number;
                return true;
            }
            result = null;
            return false;
        }

        ///<inheritdoc cref="decimal.TryParse(string, NumberStyles, IFormatProvider, out decimal)"/>
        public static bool TryParseDecimal(string s, NumberStyles style, IFormatProvider provider, out object result)
        {
            if (decimal.TryParse(s, style, provider, out decimal number))
            {
                result = number;
                return true;
            }
            result = null;
            return false;
        }

        /// <summary>Словарь числовых типов и парсеров для них.</summary>
        public static ReadOnlyDictionary<Type, TryParseNumberHandler> TryParses { get; }
            = new ReadOnlyDictionary<Type, TryParseNumberHandler>(new Dictionary<Type, TryParseNumberHandler>()
            {
                { typeof(sbyte), TryParseSbyte },
                { typeof(byte), TryParseByte },
                { typeof(short), TryParseShort },
                { typeof(ushort), TryParseUshort },
                { typeof(int), TryParseInt },
                { typeof(uint), TryParseUint },
                { typeof(long), TryParseLong },
                { typeof(ulong), TryParseUlong },
                { typeof(double), TryParseDouble },
                { typeof(float), TryParseFloat },
                { typeof(decimal), TryParseDecimal }
            });

        /// <summary>Получение парсера по типу.</summary>
        /// <param name="type">Тип для которого нжен парсер.</param>
        /// <returns>Возвращает парсер? если он есть в словаре <see cref="TryParses"/>,<br/>
        /// иначе - <see langword="null"/>.</returns>
        public static TryParseNumberHandler GetTryParse(Type type)
        {
            if (TryParses.TryGetValue(type, out TryParseNumberHandler tryParse))
                return tryParse;
            return null;
        }

        /// <summary>Коллекция всех числовых типов.</summary>
        public static IReadOnlyCollection<Type> NumberTypes => TryParses.Keys;

        /// <summary>Список нецелочисленных числовых типов.</summary>
        public static IReadOnlyList<Type> FloatingPointTypes { get; } = new List<Type>() { typeof(double), typeof(float), typeof(decimal) }.AsReadOnly();

        /// <summary>Список целочисленных числовых типов.</summary>
        public static IReadOnlyList<Type> IntegerTypes { get; } = new List<Type>() { typeof(sbyte), typeof(short), typeof(int), typeof(long),
            typeof(byte), typeof(ushort), typeof(uint), typeof(ulong) }.AsReadOnly();

        /// <summary>Метод получения <see cref="NumberStyles"/> из значения в любом типе.</summary>
        /// <param name="value">Любое значение.</param>
        /// <param name="typeNumber">Тип для которого нужен <see cref="NumberStyles"/>.</param>
        /// <returns>Если в value значение в типе:<br/>
        /// <see cref="NumberStyles"/> - приводится к типу <see cref="NumberStyles"/>;<br/>
        /// <see cref="Enum"/> или целочисленный числовой тип - методами <see cref="Enum.ToObject(Type, object)"/> и <see cref="Enum.IsDefined(Type, object)"/> проверяет возможность преобразования в <see cref="NumberStyles"/>;<br/>
        /// <see cref="String"/> - парсинг методом <see cref="Enum.TryParse{TEnum}(string, bool, out TEnum)"/> с игнорированием регистра.<br/>
        /// Если не удалось получить из value <see cref="NumberStyles"/>, то возвращается значение по умолчанию для типа typeNumber:<br/>
        /// <see cref="Decimal"/> - <see cref="NumberStyles.Number"/>;<br/>
        /// <see cref="Double"/> или <see cref="Single"/> - <see cref="NumberStyles.Float"/> | <see cref="NumberStyles.AllowThousands"/>;<br/>
        /// для остальных - <see cref="NumberStyles.Integer"/>.</returns>
        public static NumberStyles GetNumberStyle(object value, Type typeNumber)
        {
            if (value != null)
            {
                if (value is NumberStyles style)
                    return style;

                if (IntegerTypes.Contains(value.GetType()) || value is Enum)
                {
                    style = (NumberStyles)Enum.ToObject(typeof(NumberStyles), value);
                    if (Enum.IsDefined(typeof(NumberStyles), style))
                        return style;

                }
                else if (value is string str && Enum.TryParse(str, true, out style))
                    return style;
            }

            // Стиль по умолчанию можно для всех типов возвращать NumberStyles.Any
            // Но не во всех случаях будет ожидаемый результат
            // return NumberStyles.Any;

            if (typeNumber == typeof(decimal))
                return NumberStyles.Number;
            if (FloatingPointTypes.Contains(typeNumber))
                return NumberStyles.Float | NumberStyles.AllowThousands;
            return NumberStyles.Integer;
        }

    }
}
