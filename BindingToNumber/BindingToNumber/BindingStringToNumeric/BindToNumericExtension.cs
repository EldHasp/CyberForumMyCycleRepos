using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace BindingStringToNumeric
{
    /// <summary> Предоставляет высокоуровневый доступ к определению привязки, соединяющей свойства
    /// целевых объектов привязки (как правило, элементов WPF) и любой источник данных
    /// (например, базу данных, файл XML или любой объект, который содержит данные).<para/>
    /// Целевое свойство должно быть типизировано <see cref="string"/> или <see cref="object"/>. <br/>
    /// Свойство источник должно быть одним из простых числовых типов.</summary>
    public partial class BindToNumericExtension : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var providerValuetarget = (IProvideValueTarget)serviceProvider
                  .GetService(typeof(IProvideValueTarget));

            //Получим TextBox, вызвавший привязку
            TextBox targetTextBox = providerValuetarget.TargetObject as TextBox;
            DependencyProperty targetProperty = providerValuetarget.TargetProperty as DependencyProperty;

            if (targetTextBox == null || targetProperty != TextBox.TextProperty)
                throw new Exception("Можно использовать только в привязке свойства TextBox.Text");

            // Инициализация присоединённого свойства
            InitBindingState(targetTextBox, IsNumericOnly);

            bindTextBox.Source = targetTextBox;

            targetTextBox.TextChanged -= TextBoxTextChanged;
            targetTextBox.TextChanged += TextBoxTextChanged;

            return multi.ProvideValue(serviceProvider);
        }

        /// <summary>Метод-прослушка изменения текста в <see cref="TextBox"/> в котором была создана привязка <see cref="BindToNumericExtension"/>.</summary>
        /// <param name="sender"><see cref="TextBox"/> в котором изменился текст.</param>
        /// <param name="e">Параметры события. Не используются.</param>
        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            // Извлечение TetxBox и привязки его свойства Text
            TextBox textBox = (TextBox)sender;
            MultiBindingExpression multiBindingExpression = BindingOperations.GetMultiBindingExpression(textBox, TextBox.TextProperty);

            // Если привязка - это не PrivateMulti, то отключается прослушка и выход из метода.
            if (!(multiBindingExpression?.ParentMultiBinding is PrivateMulti bindPriv))
            {
                textBox.TextChanged -= TextBoxTextChanged;
                // Очистка приисоединённого свойства.
                ClearTextBindingState(textBox);
                return;
            }

            // Получение состояния привязки
            TextBoxBindingState bindingState = GetBindingState(textBox);

            // Сохранить новый текст и параметры изменения
            UpdateTextBindingState(textBox, e.Changes);

            // Если изменение произошло по привязке - выйти из метода
            if (bindingState.TextChangeSource == PropertyChangeSourceEnum.Binding)
            {
                // Сброс состояния обновления по привязке.
                bindingState.TextChangeSource = PropertyChangeSourceEnum.NotBinding;

                return;
            }

            // Создание триггера обновления источника и обновления источника в нём,
            // запоминание состояния триггера
            bool triggerHasUpdated;
            using (var trigger = new SourceUpdateTrigger(textBox, TextBox.TextProperty))
            {
                // Обновление источника с флагом обновления из метода UpdateSource().
                bindingState.UpdateSourceState = UpdateSourceStateEnum.Called;
                multiBindingExpression.UpdateSource();

                // Запоминание состояния триггера.
                triggerHasUpdated = trigger.HasUpdated;
            }

            // Получение привязки к источнику.
            BindingExpressionBase bindingSource = multiBindingExpression.BindingExpressions[0];

            // Проверка триггера обновления источника
            if (triggerHasUpdated)
            {
                // Вызов обновления привязки к источнику для передачи нового значения.
                //bindingSource.UpdateSource();

            }
            // Если обновления не было - значит была ошибка конвертера
            // Для режима ввода "Только числа" надо вызвать ковертер.
            else if (!triggerHasUpdated && bindPriv.IsNumericOnly)
            {
                // Установка флага отменённого обновления.
                bindingState.UpdateSourceState = UpdateSourceStateEnum.CallCanceled;

                // Вызов обновления привязки от источника для обработки причины прерывания обновления.
                bindingSource.UpdateTarget();

            }

            // Сброс флага обновления из метода UpdateSource().
            bindingState.UpdateSourceState = UpdateSourceStateEnum.NotCalled;
        }
    }
}
