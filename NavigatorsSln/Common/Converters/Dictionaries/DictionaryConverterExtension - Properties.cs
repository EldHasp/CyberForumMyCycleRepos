using System;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMvvm.Converters
{
    // Свойства
    [DefaultBindingProperty(nameof(Binding))]
    public partial class DictionaryConverterExtension : MarkupExtension
    {
        /// <summary>Привязка для словаря.</summary>
        public Binding Binding { get; set; }


        /// <summary>Определяет какой конвертер бyдет возвращён.</summary>
        [DefaultValue(UseTypesEnum.NotType)]
        public UseTypesEnum UseTypes
        {
            get => _useTypes;
            set
            {
                if (!Enum.IsDefined(typeof(UseTypesEnum), value))
                    throw SetUseTypesException;
                _useTypes = value;
            }
        }
        private UseTypesEnum _useTypes;

        /// <summary>Ошибка при присвоении свойству <see cref="UseTypes"/> недопустимого значения.</summary>
        public static ArgumentException SetUseTypesException { get; } = new ArgumentException("Недопустимое значение", nameof(UseTypes));
    }

}
