using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WpfMvvm.Converters
{
    /// <summary>Конвертер преобразующий ключ в значение по словарю.</summary>
    [ValueConversion(typeof(object), typeof(object))]
    public partial class DictionaryConverter : IValueConverter
    {
        /// <summary>Возвращает значение из словаря по заданному ключу.</summary>
        /// <param name="value">Ключ.</param>
        /// <param name="targetType">Не используется.</param>
        /// <param name="parameter">Если содержит словарь, то поиск производится по нему.
        /// Иначе используется словарь из <see cref="Dictionary"/>.</param>
        /// <param name="culture">Не используется.</param>
        /// <returns>Найденное значение ключа или <see cref="DependencyProperty.UnsetValue"/>.<br/>
        /// Если ни в <paramref name="parameter"/>, ни в <see cref="Dictionary"/> нет словаря - возвращается <see cref="DependencyProperty.UnsetValue"/>.</returns>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                IDictionary? dictionary = parameter as IDictionary
                    ?? Dictionary;

                if (dictionary != null)
                    return GetValue( dictionary, value);
            }
            return DependencyProperty.UnsetValue;
        }

        /// <summary>Возвращает значение из словаря по полученному ключу.<br/>
        /// Может быть переопределён в производных классах.</summary>
        /// <param name="dictionary">Словарь для поиска. Не может быть <see langword="null"/>.</param>
        /// <param name="key">Значение. Не может быть <see langword="null"/>.</param>
        /// <returns><see cref="object"/> со значением.</returns>
        protected virtual object? GetValue(IDictionary dictionary, object key)
            =>  dictionary[key];

        /// <summary>Не реализован.</summary>
        /// <returns>Всегда исключение <see cref="NotImplementedConvertBackException"/>.</returns>
        /// <exception cref="NotImplementedException">Всегда <see cref="NotImplementedConvertBackException"/>.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw NotImplementedConvertBackException;

        /// <summary>Ошибка при обращении к методу <see cref="ConvertBack(object, Type, object, CultureInfo)"/>.</summary>
        public static NotImplementedException NotImplementedConvertBackException { get; }
            = new NotImplementedException($"Метод {nameof(ConvertBack)} не реализован.");
    }
}
