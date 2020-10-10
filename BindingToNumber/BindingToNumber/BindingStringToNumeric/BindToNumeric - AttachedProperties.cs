using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BindingStringToNumeric
{
    public partial class BindToNumericExtension
    {
        /// <summary>Создаёт новый экземпляр TextBoxBindingState, записывает его в 
        /// присоединённое к <see cref="TextBox"/> свойство BindingState
        /// и возвращает его.</summary>
        /// <param name="textBox"><see cref="TextBox"/> чьё свойство инициализируется.</param>
        /// <param name="isNumericOnly">Режим ввода только чисел.</param>
        /// <returns>Созданный в методе <see cref="TextBoxBindingState"/>.</returns>
        private static TextBoxBindingState InitBindingState(TextBox textBox, bool isNumericOnly)
        {
            TextBoxBindingState state = new TextBoxBindingState(null, textBox.Text, isNumericOnly);
            textBox.SetValue(BindingStatePropertyKey, state);
            return state;
        }

        /// <summary>Очистка присоединённого к <see cref="TextBox"/> свойства BindingState.</summary>
        /// <param name="textBox"><see cref="TextBox"/> чьё свойство очищается.</param>
        private void ClearTextBindingState(TextBox textBox)
        {
            textBox.SetValue(BindingStatePropertyKey, DependencyProperty.UnsetValue);
        }

        /// <summary>Возвращает значение присоединённого свойства TextChanges.</summary>
        /// <param name="textBox"><see cref="TextBox"/> чьё свойство заращивается.</param>
        /// <returns><see cref="TextBoxBindingState"/> значение свойства.</returns>
        private static TextBoxBindingState GetBindingState(TextBox textBox)
        {
            return (TextBoxBindingState)textBox.GetValue(BindingStateProperty); ;
        }

        /// <summary>Обновляет значение свойств присоединённого свойства BindingState.</summary>
        /// <param name="textBox"><see cref="TextBox"/> свойство которого обновляется.</param>
        /// <param name="changes">Коллекция объектов, содержащий сведения о произошедших изменениях.</param>
        /// <remarks>Свойство <see cref="TextBoxBindingState.NewText"/> обновляется значением <paramref name="textBox"/>.Text,<br/>
        /// в свойство <see cref="TextBoxBindingState.Changes"/> записывается значение <paramref name="changes"/>.</remarks>
        private static void UpdateTextBindingState(TextBox textBox, ICollection<TextChange> changes)
        {
            TextBoxBindingState state = GetBindingState(textBox);

            //if (state.NewText != textBox.Text)
                state.UpdateText(textBox.Text);

            state.Changes = changes;
        }

        // Using a DependencyProperty as the backing store for TextChanges.  This enables animation, styling, binding, etc...
        private static readonly DependencyPropertyKey BindingStatePropertyKey =
            DependencyProperty.RegisterAttachedReadOnly("BindingState", typeof(TextBoxBindingState), typeof(BindToNumericExtension), new PropertyMetadata(null));

        private static readonly DependencyProperty BindingStateProperty = BindingStatePropertyKey.DependencyProperty;

    }
}