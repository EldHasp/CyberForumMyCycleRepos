using System.Windows;

namespace ListeningDependecyProperties
{
        /// <summary>Делегат метода прослушки изменения <see cref="DependencyProperty"/>.</summary>
        /// <param name="sender">Объект в котором изменилось свойство.</param>
        /// <param name="args">Аргументы изменения.</param>
        public delegate void DependencyPropertyChangedHandler(DependencyObject sender, DependencyPropertyChangedEventArgs args);
}
