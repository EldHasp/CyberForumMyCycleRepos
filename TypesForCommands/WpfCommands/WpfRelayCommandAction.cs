using InterfacesCommands;
using System;
using System.Windows.Threading;

namespace WpfCommands
{

    /// <summary>Класс для команд без параметров</summary>
    public class WpfRelayCommandAction : WpfRelayCommand, IWpfCommandActionInvalidate
    {
        /// <summary>Делегат исполнительного метода команды без параметра.</summary>
        protected ExecuteActionHandler executeAction;
        /// <summary>Делегат метода состояния команды без параметра.</summary>
        protected CanExecuteActionHandler canExecuteAction;

        /// <inheritdoc cref="WpfRelayCommand(ExecuteHandler,CanExecuteHandler,Dispatcher)"/>
        public WpfRelayCommandAction(ExecuteActionHandler execute, CanExecuteActionHandler canExecute = null, Dispatcher dispatcher = null)
            :base(null, null, dispatcher)
        {
            this.execute = ExecuteAction;
            this.canExecute = CanExecuteAction;

            executeAction = execute ?? throw new ArgumentNullException(nameof(execute));
            canExecuteAction = canExecute ?? CanExecuteActionTrue;
        }

        /// <inheritdoc cref="WpfRelayCommandAction(ExecuteActionHandler,CanExecuteActionHandler,Dispatcher)"/>
        public WpfRelayCommandAction(ExecuteActionHandler execute, Dispatcher dispatcher = null)
            : this(execute, null, dispatcher) { }


        /// <summary>Метод всегда возвращающий <see langword="true"/>.
        /// Задан для уменьшения создания лямбд.</summary>
        /// <returns><see langword="true"/>.</returns>
        public static bool CanExecuteActionTrue() => true;

        /// <summary>Метод, вызывающий <see cref="Execute"/>.
        /// Задан чтобы не создавать лямбды.</summary>
        /// <param name="parameter">Проверяется на <see langword="null"/>.
        /// Если <see langword="null"/>, то вызывается метод <seealso cref="Execute"/>.</param>
        protected void ExecuteAction(object parameter)
        {
            if (parameter == null)
                Execute();
        }

        /// <summary>Метод, вызывающий <see cref="CanExecute"/>.
        /// Задан чтобы не создавать лямбды.</summary>
        /// <param name="parameter">Проверяется на <see langword="null"/>.
        /// Если <see langword="null"/>, то вызывается метод <seealso cref="CanExecute"/>.</param>
        protected bool CanExecuteAction(object parameter) => parameter == null && CanExecute();


        /// <inheritdoc cref="ICommandActionInvalidate.Execute"/>
        public void Execute() => executeAction();


        /// <inheritdoc cref="ICommandActionInvalidate.CanExecute"/>
        public bool CanExecute() => canExecuteAction();

    }
}
