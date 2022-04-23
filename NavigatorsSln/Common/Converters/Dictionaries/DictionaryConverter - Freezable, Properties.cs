using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace WpfMvvm.Converters
{
    // Реализация Freezable и свойств.

    /// <summary>Конвертер преобразующий ключ в значение по словарю.</summary>
    [ContentProperty(nameof(Dictionary))]
    public partial class DictionaryConverter : Freezable
    {
        /// <summary>Словарь для поиска значений.<br/>
        /// Используется в случае когда в parameter нет словаря.</summary>
        public IDictionary? Dictionary
        {
            get => (IDictionary)GetValue(DictionaryProperty);
            set => SetValue(DictionaryProperty, value);
        }

        /// <summary>Using a DependencyProperty as the backing store for Dictionary.  This enables animation, styling, binding, etc...</summary>
        public static readonly DependencyProperty DictionaryProperty =
            DependencyProperty.Register(nameof(Dictionary), typeof(IDictionary), typeof(DictionaryConverter), new PropertyMetadata(null));

        /// <summary>Создаёт новый экземпляр <see cref="DictionaryConverter"/>.</summary>
        /// <returns>Новый экземпляр <see cref="DictionaryConverter"/>.</returns>
        protected override Freezable CreateInstanceCore()
            => new DictionaryConverter();

        /// <summary>Экземпляр конвертера.<br/>
        /// Экземпляр заморожен: свойство <see cref="Dictionary"/>=<see langword="null"/> и неизменяемо.<br/>
        /// Словарь должен передаваться через parameters метода <see cref="Convert(object, Type, object, CultureInfo)"/>.</summary>
        public static DictionaryConverter Instance { get; }
    }
}
