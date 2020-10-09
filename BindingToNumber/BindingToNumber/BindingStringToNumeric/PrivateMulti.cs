using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace BindingStringToNumeric
{
    public partial class BindToNumericExtension
    {
        /// <summary>Приватный класс для <see cref="MultiBinding"/> создаваемых в <see cref="BindToNumericExtension"/>.<br/>
        /// Используется для того, чтобы можно различать привязки созданные <see cref="BindToNumericExtension"/> от других привязок.</summary>
        private class PrivateMulti : MultiBinding
        {

            /// <summary>Запрещает или разрешает <see cref="TextBox.Text"/> ввод нечисленных значений.</summary>
            /// <returns><see langword="true"/> (значение по умолчанию) - любой ввод создающий в <see cref="TextBox.Text"/> значение не приводимое к числу будет игнорироваться,<br/>
            /// <see langword="false"/> - при вводе в <see cref="TextBox.Text"/> значения неприводимого к числу будет устанавливаться ошибка валидации присоединённого свойства Validation.HasError.</returns>
            [DefaultValue(true)]
            public bool IsNumericOnly { get; set; } = true;


            /// <summary>Цифровой стиль для парсера. Если он не задан, то используется стиль по умолчанию.</summary>
            [DefaultValue(null)]
            public NumberStyles? NumberStyle
            {
                get => (NumberStyles?)ConverterParameter;
                set => ConverterParameter = value;
            }

        }

    }
}
