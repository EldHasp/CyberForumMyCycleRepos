namespace WpfMvvm.Converters
{
    /// <summary>Перечисление задающее тип конвертера.</summary>
    public enum UseTypesEnum
    {
        /// <summary>Без получения типа. Используется базовый конвертер <see cref="DictionaryConverter"/>.</summary>
        NotType,
        /// <summary>Сравнивается тип значения с типом ключа. Используется конвертер <see cref="DictionaryTypeConverter"/> с <see cref="DictionaryTypeConverter.UseBasicTypes"/>=<see langword="false"/>.</summary>
        EqualsType,
        /// <summary>Проверяется приводимость типа значения к типу ключа. Используется конвертер <see cref="DictionaryTypeConverter"/> с <see cref="DictionaryTypeConverter.UseBasicTypes"/>=<see langword="true"/>.</summary>
        BaseType
    }
}
