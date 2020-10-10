using InterfacesCommands;
using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace WpfCommands
{
    /// <summary>Класс для команд с типизированным параметром.</summary>
    public class WpfRelayCommand<T> : WpfRelayCommand, IWpfCommandInvalidate<T>
    {

        /// <summary>Делегат исполнительного метода команды с типизированным параметром.</summary>
        protected ExecuteHandler<T> executeT;

        /// <summary>Делегат метода состояния команды с типизированным параметром.</summary>
        protected CanExecuteHandler<T> canExecuteT;

        /// <inheritdoc cref="WpfRelayCommand(ExecuteHandler, CanExecuteHandler, Dispatcher)"/>
        public WpfRelayCommand(ExecuteHandler<T> execute, CanExecuteHandler<T> canExecute = null, Dispatcher dispatcher = null)
            : base(dispatcher)
        {
            base.execute = ExecuteT;
            base.canExecute = CanExecuteT;

            executeT = execute ?? throw new ArgumentNullException(nameof(execute));
            canExecuteT = canExecute ?? CanExecuteTrue;
        }

        /// <inheritdoc cref="WpfRelayCommand(ExecuteHandler, CanExecuteHandler, Dispatcher)"/>
        public WpfRelayCommand(ExecuteHandler<T> execute, Dispatcher dispatcher)
            : this(execute, null, dispatcher) { }

        /// <inheritdoc cref="WpfRelayCommand(ExecuteHandler, CanExecuteHandler, Dispatcher)"/>
        public WpfRelayCommand(ExecuteHandler<T> execute)
            : this(execute, null, null) { }

        /// <summary>Метод всегда возвращающий <see langword="true"/>.
        /// Задан для уменьшения создания лямбд.</summary>
        /// <param name="parameter">Проверяется на <see langword="null"/>.</param>
        /// <returns><see langword="true"/> если parameter не <see langword="null"/>.</returns>
        public static bool CanExecuteTrue(T parameter) => parameter != null;

        /// <summary>Метод проверяет возможность преобразования параметра 
        /// в тип команды и вызыающий метод <see cref="Execute{Tparam}(Tparam)"/>.</summary>
        /// <remarks>Сначала проверяется возможноcnm приведения типа по шаблону:<br/>
        /// <see href="https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/keywords/is"/>. <br/>
        /// Если не удаётся, то проверяется возможность конвертации через <see cref="TypeConverter"/>.</remarks>
        protected void ExecuteT(object parameter)
        {
            if (parameter is T t)
                Execute(t);
            else if (TypeDescriptor.GetConverter(typeof(T)).IsValid(parameter))
                Execute((T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(parameter));
        }

        /// <summary>Метод проверяет возможность преобразования параметра 
        /// в тип команды и вызыающий метод <see cref="CanExecute{Tparam}(Tparam)"/>.</summary>
        /// <remarks>Сначала проверяется возможноcnm приведения типа по шаблону:<br/>
        /// <see href="https://docs.microsoft.com/ru-ru/dotnet/csharp/language-reference/keywords/is"/>. <br/>
        /// Если не удаётся, то проверяется возможность конвертации через <see cref="TypeConverter"/>.</remarks>
        protected bool CanExecuteT(object parameter)
        {
            return parameter is T  t ? CanExecute(t)
                : (TypeDescriptor.GetConverter(typeof(T)).IsValid(parameter)
                && canExecute((T)TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(parameter)));
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
