using System;
using System.Windows;
using System.Windows.Data;

namespace BindingStringToNumeric
{
    /// <summary>Триггер обновления источника.</summary>
    public class SourceUpdateTrigger : IDisposable
    {
        /// <summary>Прослушиваемый элемент.</summary>
        public FrameworkElement TargetElement { get; }

        /// <summary>Прослушиваемое свойство.</summary>
        public DependencyProperty TargetProperty { get; }

        /// <summary>Флаг срабатывания обновления источника из прослушиваемого свойства.</summary>
        public bool HasUpdated { get; private set; } = false;
        
        /// <summary>Создаёт экземпляр триггера.</summary>
        /// <param name="targetElement">Прослушиваемый элемент.</param>
        /// <param name="targetProperty">Прослушиваемое свойство.</param>
        public SourceUpdateTrigger(FrameworkElement targetElement, DependencyProperty targetProperty)
        {
            TargetElement = targetElement ?? throw new ArgumentNullException(nameof(targetElement));
            TargetProperty = targetProperty ?? throw new ArgumentNullException(nameof(targetProperty));

            TargetElement.SourceUpdated += OnSourceUpdated;
        }

        /// <summary>Метод прослушка события <see cref="FrameworkElement.SourceUpdated"/>.</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSourceUpdated(object sender, DataTransferEventArgs e)
        {
            if (e.TargetObject == TargetElement && e.Property == TargetProperty)
            {
                TargetElement.SourceUpdated -= OnSourceUpdated;
                HasUpdated = true;
            }
        }

        public void Dispose()
        {
            // Отключение триггера
            TargetElement.SourceUpdated -= OnSourceUpdated;
        }
    }
}
