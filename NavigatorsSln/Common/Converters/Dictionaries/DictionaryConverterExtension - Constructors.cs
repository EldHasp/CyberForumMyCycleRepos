using System;

namespace WpfMvvm.Converters
{
    // Конструкторы
    public partial class DictionaryConverterExtension 
    {
        /// <summary>Задаёт свойство <see cref="UseTypes"/>.</summary>
        /// <param name="useBaseTypes">Значение для свойства <see cref="UseTypes"/>.</param>
        public DictionaryConverterExtension(UseTypesEnum useBaseTypes)
            => UseTypes = useBaseTypes;

        /// <summary>Создаёт экземпляр Расширения Разметки.</summary>
        public DictionaryConverterExtension() { }
    }
}
