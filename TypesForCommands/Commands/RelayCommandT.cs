using InterfacesCommands;
using System;

namespace Commands
{
    /// <summary>Класс для команд с типизированным параметром.</summary>
    public class RelayCommand<T> : RelayCommand, ICommandInvalidate<T>
    {

        /// <summary>Делегат исполнительного метода команды с типизированным параметром.</summary>
        protected ExecuteHandler<T> executeT;

        /// <summary>Делегат метода состояния команды с типизированным параметром.</summary>
        protected CanExecuteHandler<T> canExecuteT;

        /// <summary>Конструктор команды</summary>
        /// <param name="executeT">Выполняемый метод команды</param>
        /// <param name="canExecuteT">Метод разрешающий выполнение команды</param>
        public RelayCommand(ExecuteHandler<T> executeT, CanExecuteHandler<T> canExecuteT = null)
        {
            execute = ExecuteT;
            canExecute = CanExecuteT;

            this.executeT = this.executeT ?? throw new ArgumentNullException(nameof(executeT));
            this.canExecuteT = this.canExecuteT ?? CanExecuteTrue;
        }

        /// <summary>Метод всегда возвращающий <see langword="true"/>.
        /// Задан для уменьшения создания лямбд.</summary>
        /// <param name="parameter">Проверяется на <see langword="null"/>.</param>
        /// <returns><see langword="true"/> если parameter не <see langword="null"/>.</returns>
        public static bool CanExecuteTrue(T parameter) => parameter != null;

        /// <summary>Метод проверяет возможность преобразования параметра 
        /// в тип команды и вызыающий метод <see cref="Execute{Tparam}(Tparam)"/>.</summary>
        protected void ExecuteT(object parameter)
        {
            if (parameter is T t)
                Execute(t);
        }

        /// <summary>Метод проверяет возможность преобразования параметра 
        /// в тип команды и вызыающий метод <see cref="CanExecute{Tparam}(Tparam)"/>.</summary>
        protected bool CanExecuteT(object parameter)
        {
            return parameter is T t && CanExecute(t);
        }

        ///<inheritdoc cref="ICommandInvalidate&lt;T&gt;.Execute{Tparam}(Tparam)"/>
        public void Execute<Tparam>(Tparam parameter) where Tparam : T
        {
            if (parameter != null)
                executeT(parameter);
        }

        ///<inheritdoc cref="ICommandInvalidate&lt;T&gt;.CanExecute{Tparam}(Tparam)"/>
        public bool CanExecute<Tparam>(Tparam parameter) where Tparam : T => parameter != null && canExecuteT(parameter);

    }
}
