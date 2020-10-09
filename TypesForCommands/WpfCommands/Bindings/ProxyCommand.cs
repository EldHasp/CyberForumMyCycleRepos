using InterfacesCommands;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace WpfCommands.Bindings
{
    public class ProxyCommand : Freezable, IWpfCommandInvalidate
    {
        /// <summary>Реализация команды.</summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ProxyCommand), new PropertyMetadata(null, CommandChanged));

        /// <summary>Метод вызываемой при изменении привязанной команды.</summary>
        /// <param name="d">Объект-источник вызвавщий метод.</param>
        /// <param name="e">Праметры изменения.</param>
        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // Приведение к фактическому типу.
            ProxyCommand proxy = (ProxyCommand)d;

            // Если отключаемая команда не null, то отключение "прослушки".
            if (e.OldValue != null)
                ((ICommand)e.OldValue).CanExecuteChanged -= proxy.OnCanExecuteChanged;

            // Если подключаемая команда не null, то подключение "прослушки".
            if (e.NewValue != null)
                ((ICommand)e.NewValue).CanExecuteChanged += proxy.OnCanExecuteChanged;
        }

        /// <summary>Метод-прослушка события CanExecuteChanged привязанной команды.</summary>
        /// <param name="sender">Объект-источник вызвавщий событие.</param>
        /// <param name="e">Аргументы события. Всегда <see cref="EventArgs.Empty"/>.</param>
        /// <remarks>В прослушке только вызов метода <see cref="Invalidate"/>.</remarks>
        protected virtual void OnCanExecuteChanged(object sender, EventArgs e) => Invalidate();

        #region Реализация ICommandInvalidate

        /// <summary>Виртуальный защищённый метод создающий событие CanExecuteChanged.</summary>
        protected virtual void RiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public void Invalidate()
        {
            if (CanExecuteChanged != null)
            {
                if (Dispatcher.CheckAccess())
                    RiseCanExecuteChanged();
                else
                    Dispatcher.BeginInvoke((Action)RiseCanExecuteChanged);
            }
        }



        public bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register(
                nameof(IsEnabled),
                typeof(bool),
                typeof(ProxyCommand),
                new PropertyMetadata(true, IsEnabledChanged));

        private static void IsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
            => ((ProxyCommand)d).Invalidate();



        //private bool _isEnabled;
        //public bool IsEnabled
        //{
        //    get => _isEnabled;
        //    set
        //    {
        //        if (_isEnabled == value)
        //            return;

        //        _isEnabled = value;
        //        PropertyChanged?.Invoke(this, IsEnabledEventArgs);
        //        Invalidate();
        //    }
        //}
        #region Реализация ICommand
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => IsEnabled && Command != null && Command.CanExecute(parameter);

        public void Execute(object parameter) => Command?.Execute(parameter);

        #endregion
        #endregion


        #region Реализация абстрактного метода Freezable
        protected override Freezable CreateInstanceCore() => new ProxyCommand();
        #endregion
    }
}
