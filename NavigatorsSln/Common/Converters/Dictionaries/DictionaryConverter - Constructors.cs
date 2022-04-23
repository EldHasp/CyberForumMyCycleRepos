using System.Collections;
using System.Collections.Generic;

namespace WpfMvvm.Converters
{
    // Конструкторы
    public partial class DictionaryConverter 
    {
        /// <summary>Инициализирует новый экземпляр конвертера <see cref="DictionaryConverter"/>.<br/>
        /// В <see cref="Dictionary"/> записывается новый экземпляр <c>new Dictionary&lt;Type, object&gt;()</c>.</summary>
        public DictionaryConverter()
            : this(new Dictionary<object, object>())
        { }

        /// <summary>Инициализирует новый экземпляр конвертера <see cref="DictionaryConverter"/> переданным словарём.</summary>
        /// <param name="dictionary">Cловарь записываемый в <see cref="Dictionary"/>.</param>
        public DictionaryConverter(IDictionary? dictionary)
            => Dictionary = dictionary;

        /// <summary>Записывает в <see cref="Instance"/> статический замороженный экземпляр конвертера.</summary>
        static DictionaryConverter()
        {
            Instance = new DictionaryConverter(null);
            Instance.Freeze();
        }

    }

}
