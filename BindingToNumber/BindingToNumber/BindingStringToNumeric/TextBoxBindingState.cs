using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BindingStringToNumeric
{
    public partial class BindToNumericExtension
    {
        /// <summary>Класс-контейнер с данными для конвертера привязки <see cref="TextBox.Text"/>.</summary>
        private class TextBoxBindingState
        {
            /// <summary>Предыдущее значение <see cref="TextBox.Text"/>.</summary>
            public string OldText { get; private set; }
            /// <summary>Текущее значение <see cref="TextBox.Text"/>.</summary>
            public string NewText { get; private set; }

            /// <summary>Возвращает экземпляр.</summary>
            /// <param name="oldText">Предыдущее значение.</param>
            /// <param name="newText">Текущее значение.</param>
            /// <param name="isNumericOnly">Режим ввода только численных значений.</param>
            public TextBoxBindingState(string oldText, string newText, bool isNumericOnly)
            {
                OldText = oldText;
                NewText = newText;
                IsNumericOnly = isNumericOnly;
            }

            /// <summary>Состояние работы метода <see cref="MultiBindingExpression.UpdateSource"/> в привязке свойства <see cref="TextBox.Text"/>.</summary>
            public UpdateSourceStateEnum UpdateSourceState { get; set; } = UpdateSourceStateEnum.NotCalled;

            ///// <summary>Состояние работы метода <see cref="BindingExpression.UpdateTarget"/> в привязке передающей <see cref="TextBoxBindingState"/> из свойства зависимости.</summary>
            //public UpdateTargetStateEnum UpdateTargetState { get;  set; } = UpdateTargetStateEnum.NotCalled;

            /// <summary>Источник изменения свойства <see cref="TextBox.Text"/>.</summary>
            public PropertyChangeSourceEnum TextChangeSource { get; set; } = PropertyChangeSourceEnum.Binding;

            /// <summary>Режим допустимости только численных значений.<br/>
            /// В том числе удаляются крайние пробелы.</summary>
            public bool IsNumericOnly { get; }

            /// <summary>Коллекция объектов, содержащий сведения о произошедших изменениях.</summary>
            public ICollection<TextChange> Changes { get; set; }

            public override string ToString()
                => $"{{{nameof(OldText)}=\"{OldText}\"; {nameof(NewText)}=\"{NewText}\";" +
                $" {nameof(UpdateSourceState)}={UpdateSourceState};" +
                //$" {nameof(UpdateSourceState)}={UpdateSourceState}; {nameof(UpdateTargetState)}={UpdateTargetState};" +
                $" {nameof(TextChangeSource)}={TextChangeSource};}}";
            //$" {nameof(TextChangeSource)}={TextChangeSource}; {nameof(IsNumericOnly)}={IsNumericOnly};}}";

            /// <summary>Обновляет состояние текстов.</summary>
            /// <param name="newText">Новое значение для свойства <see cref="NewText"/>.</param>
            /// <remarks>Текущее значение <see cref="NewText"/> записывается в <see cref="OldText"/>.<br/>
            /// А в <see cref="NewText"/> записывается параметр newText.</remarks>
            public void UpdateText(string newText)
            {
                if (NewText != newText)
                    (OldText, NewText) = (NewText, newText);
            }
        }

        /// <summary>Состояния работы метода <see cref="MultiBindingExpression.UpdateSource"/>.</summary>
        private enum UpdateSourceStateEnum
        {
            /// <summary>Метод не вызывался.</summary>
            NotCalled,
            /// <summary>Метод вызван.</summary>
            Called,
            /// <summary>Метод вызывался, но обновления источника не произошло.</summary>
            CallCanceled
        }

        /// <summary>Состояния работы метода <see cref="MultiBindingExpression.UpdateTarget"/>.</summary>
        private enum UpdateTargetStateEnum
        {
            /// <summary>Метод не вызывался.</summary>
            NotCalled,
            /// <summary>Метод вызван.</summary>
            Called
        }

        /// <summary>Источник изменения свойства.</summary>
        private enum PropertyChangeSourceEnum
        {
            /// <summary>Обновление по привязке.</summary>
            Binding,
            /// <summary>Обновление не из привязки.</summary>
            NotBinding
        }
    }
}