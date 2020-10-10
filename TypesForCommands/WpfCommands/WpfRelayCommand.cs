using Commands;
using InterfacesCommands;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfCommands
{
    public class WpfRelayCommand : RelayCommand, IWpfCommandInvalidate
    {
        /// <summary>Метод подключаяющийся к <see cref="CommandManager.RequerySuggested"/>
        /// для вызова <see cref="RelayCommand.Invalidate"/>.</summary>
        /// <param name="sender">Для сигнатуры метода. Не используется.</param>
        /// <param name="e">Для сигнатуры метода. Не используется.</param>
        private void RequerySuggested(object sender, EventArgs e)
            => Invalidate();

        public Dispatcher Dispatcher { get; }

        /// <inheritdoc cref="WpfRelayCommand(ExecuteHandler, CanExecuteHandler, Dispatcher)"/>
        protected WpfRelayCommand(Dispatcher dispatcher = null)
            : base()
        {
            Dispatcher = dispatcher ?? Application.Current.Dispatcher;

            CommandManager.RequerySuggested += RequerySuggested;
        }
        /// <inheritdoc cref="RelayCommand(ExecuteHandler, CanExecuteHandler)"/>
        /// <param name="dispatcher">Диспетчер.<br/> Событие <see cref="ICommand.CanExecuteChanged"/>
        /// бyдет создаваться в нём,<br/> независимо от того в каком потоке был вызван
        /// метод <see cref="ICommandInvalidate.Invalidate"/>.<br/>
        /// Если dispatcher = null, то диспетчер будет получен от <see cref="Application.Current"/>.</param>
        public WpfRelayCommand(ExecuteHandler execute, CanExecuteHandler canExecute = null, Dispatcher dispatcher = null)
            : this(dispatcher)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));

            this.canExecute = canExecute ?? CanExecuteTrue;
        }

        /// <inheritdoc cref="WpfRelayCommand(ExecuteHandler, CanExecuteHandler, Dispatcher)"/>
        public WpfRelayCommand(ExecuteHandler execute, Dispatcher dispatcher)
            : this(execute, null, dispatcher) { }

        /// <inheritdoc cref="RelayCommand.RiseCanExecuteChanged"/>
        /// <remarks>Метод переопределён для создания события
        /// <see cref="ICommand.CanExecuteChanged"/> в Диспетчере.</remarks>
        protected override void RiseCanExecuteChanged()
        {
            if (Dispatcher.CheckAccess())
                base.RiseCanExecuteChanged();
            else
                Dispatcher.BeginInvoke((Action)base.RiseCanExecuteChanged);
        }
    }
}
