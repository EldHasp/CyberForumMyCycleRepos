using InterfacesCommands;
using System;

namespace Commands
{

    /// <summary>Класс для команд без параметров</summary>
    public class RelayCommandAction : RelayCommand, ICommandActionInvalidate
    {
        /// <summary>Делегат исполнительного метода команды без параметра.</summary>
        protected ExecuteActionHandler executeAction;
        /// <summary>Делегат метода состояния команды без параметра.</summary>
        protected CanExecuteActionHandler canExecuteAction;

        /// <summary>Конструктор команды</summary>
        /// <param name="executeAction">Выполняемый метод команды</param>
        /// <param name="canExecuteAction">Метод разрешающий выполнение команды</param>
        public RelayCommandAction(ExecuteActionHandler execute, CanExecuteActionHandler canExecute = null)
        {
            this.execute = ExecuteAction;
            this.canExecute = CanExecuteAction;

            executeAction = execute ?? throw new ArgumentNullException(nameof(execute));
            canExecuteAction = canExecute ?? CanExecuteActionTrue;
        }

        /// <summary>Метод всегда возвращающий <see langword="true"/>.
        /// Задан для уменьшения создания лямбд.</summary>
        /// <returns><see langword="true"/>.</returns>
        public static bool CanExecuteActionTrue() => true;

        /// <summary>Определяет метод, вызываемый при вызове данной команды.
        /// Задан чтобы не создавать лямбды.</summary>
        /// <param name="parameter">Проверяется на <see langword="null"/>.
        /// Если <see langword="null"/>, то вызывается метод <seealso cref="Execute"/>.</param>
        protected void ExecuteAction(object parameter)
        {
            if (parameter == null)
                Execute();
        }

        /// <summary>Определяет метод, который определяет, может ли данная команда
        /// выполняться в ее текущем состоянии.
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
