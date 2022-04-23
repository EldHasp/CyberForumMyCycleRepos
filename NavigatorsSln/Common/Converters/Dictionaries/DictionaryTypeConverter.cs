using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WpfMvvm.Converters
{
    /// <summary>Конвертер получает тип value и по нему возвращает значение из словаря.<br/>
    ///  Свойство <see cref="UseBasicTypes"/> задаёт возможность использования базовых типов.</summary>
    /// <remarks> В классе переопределён метод базового класса <see cref="DictionaryConverter.GetValue(IDictionary, object)"/> на поиск по типу полученного ключа. <br/>
    /// Удобно использовать как селектор шаблонов или стилей.</remarks>
    [ValueConversion(typeof(Type), typeof(object))]
    public class DictionaryTypeConverter : DictionaryConverter
    {
        /// <summary>Если <see langword="false"/>, то ищется только ключ полностью совпадающий с заданным типом.<br/>
        /// Если <see langword="true"/>, то также используются базовые типы. 
        /// Если их несколько, то выбирается ближайший предок.</summary>
        public bool UseBasicTypes
        {
            get { return (bool)GetValue(UseBasicTypesProperty); }
            set { SetValue(UseBasicTypesProperty, value); }
        }

        /// <summary>Using a DependencyProperty as the backing store for UseBasicTypes.  This enables animation, styling, binding, etc...</summary>
        public static readonly DependencyProperty UseBasicTypesProperty =
            DependencyProperty.Register(nameof(UseBasicTypes), typeof(bool), typeof(DictionaryTypeConverter), new PropertyMetadata(true));

        /// <summary>Инициализирует новый экземпляр конвертера <see cref="DictionaryTypeConverter"/>.<br/>
        /// В <see cref="DictionaryConverter.Dictionary"/> записывается новый экземпляр <c>new Dictionary&lt;Type, object&gt;()</c>.</summary>
        public DictionaryTypeConverter()
            : this(new Dictionary<Type, object>())
        { }

        /// <summary>Инициализирует новый экземпляр конвертера <see cref="DictionaryTypeConverter"/> переданным словарём.</summary>
        /// <param name="dictionary">Cловарь записываемый в <see cref="DictionaryConverter.Dictionary"/>.</param>
        public DictionaryTypeConverter(IDictionary dictionary)
            : base(dictionary)
        { }

        /// <summary>Возвращает значение из словаря по полученному типу значения ключа.<br/>
        /// Может быть переопределён в производных классах.</summary>
        /// <param name="dictionary">Словарь для поиска. Не может быть <see langword="null"/>.</param>
        /// <param name="key">Значение. Не может быть <see langword="null"/>.<br/>
        /// Если передан не тип <see cref="Type"/>, то для поиска по словарю используется <see cref="object.GetType()"/> от <paramref name="key"/>.</param>
        /// <returns><see cref="object"/> со значением.</returns>
        protected override object GetValue(IDictionary dictionary, object key)
        {
            Type keyType;
            if (key is Type type)
                keyType = type;
            else
                keyType = key.GetType();

            if (!dictionary.Contains(keyType))
            {
                if (!UseBasicTypes)
                    return null;
                else
                {
                    Type baseType = typeof(object);
                    foreach (Type tp in dictionary.Keys.OfType<Type>().Where(t => t.IsAssignableFrom(keyType)))
                    {
                        if (baseType.IsAssignableFrom(tp))
                            baseType = tp;
                    }
                    keyType = baseType;
                }

            }

            return base.GetValue(dictionary, keyType);
        }

        /// <summary>Создаёт новый экземпляр <see cref="DictionaryTypeConverter"/>.</summary>
        /// <returns>Новый экземпляр <see cref="DictionaryTypeConverter"/>.</returns>
        protected override Freezable CreateInstanceCore()
            => new DictionaryTypeConverter();

        /// <summary>Экземпляр конвертера с <see cref="UseBasicTypes"/>=<see langword="true"/>.<br/>
        /// Экземпляр заморожен.<br/>
        /// Свойствам заданы значения и они не изменяемы: 
        /// <see cref="DictionaryConverter.Dictionary"/>=<see langword="null"/>, 
        /// <see cref="UseBasicTypes"/>=<see langword="true"/>.<br/>
        /// Словарь должен передаваться через parameters метода 
        /// <see cref="DictionaryConverter.Convert(object, Type, object, CultureInfo)"/>.</summary>
        public static DictionaryTypeConverter InstanceBaseTypes { get; }

        /// <summary>Экземпляр конвертера с <see cref="UseBasicTypes"/>=<see langword="false"/>.<br/>
        /// Экземпляр заморожен.<br/>
        /// Свойствам заданы значения и они не изменяемы: 
        /// <see cref="DictionaryConverter.Dictionary"/>=<see langword="null"/>, 
        /// <see cref="UseBasicTypes"/>=<see langword="false"/>.<br/>
        /// Словарь должен передаваться через parameters метода
        /// <see cref="DictionaryConverter.Convert(object, Type, object, CultureInfo)"/>.</summary>
        public static DictionaryTypeConverter InstanceEqualsTypes { get; }

        /// <summary>Записывает в <see cref="InstanceBaseTypes"/> и <see cref="InstanceEqualsTypes"/>
        /// статические замороженные экземпляры конвертеров.</summary>
        static DictionaryTypeConverter()
        {
            InstanceBaseTypes = new DictionaryTypeConverter(null) { UseBasicTypes = true };
            InstanceBaseTypes.Freeze();

            InstanceEqualsTypes = new DictionaryTypeConverter(null) { UseBasicTypes = false };
            InstanceEqualsTypes.Freeze();
        }

    }

}
