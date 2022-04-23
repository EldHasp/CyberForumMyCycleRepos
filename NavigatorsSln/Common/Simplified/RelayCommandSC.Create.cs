using System;

namespace Simplified
{
    public partial class RelayCommand
    {
        /// <summary>Конструктор-фабрика команды.</summary>
        /// <typeparam name="T">Тип параметра команды.</typeparam>
        /// <param name="execute">Выполняемый метод команды.</param>
        /// <param name="canExecute">Метод, возвращающий состояние команды.</param>
        /// <param name="converter">Делегат метода преобразующего <see cref="object"/> в тип <typeparamref name="T"/>.</param>
        /// <returns>Возвращает экземпляр <see cref="RelayCommand"/>.</returns>
        public static RelayCommand Create<T>(ExecuteHandler<T> execute, CanExecuteHandler<T> canExecute, ConverterFromObjectHandler<T> converter)
            => new RelayCommand(new RelayExecute<T>(execute ?? throw new ArgumentNullException(nameof(execute)), converter ?? throw new ArgumentNullException(nameof(converter))).executeO,
                                  new RelayCanExecute<T>(canExecute ?? throw new ArgumentNullException(nameof(execute)), converter ?? throw new ArgumentNullException(nameof(converter))).canExecuteO);

        /// <inheritdoc cref="Create{T}(ExecuteHandler{T}, CanExecuteHandler{T}, ConverterFromObjectHandler{T})"/>
        public static RelayCommand Create<T>(ExecuteHandler<T> execute, CanExecuteHandler<T> canExecute)
            => new RelayCommand(new RelayExecute<T>(execute ?? throw new ArgumentNullException(nameof(execute))).executeO,
                                  new RelayCanExecute<T>(canExecute ?? throw new ArgumentNullException(nameof(execute))).canExecuteO);

        public static RelayCommand Create<T>(ExecuteHandler<T> execute, ConverterFromObjectHandler<T> converter)
            => new RelayCommand(new RelayExecute<T>(execute ?? throw new ArgumentNullException(nameof(execute)), converter ?? throw new ArgumentNullException(nameof(converter))).executeO);

        /// <inheritdoc cref="Create{T}(ExecuteHandler{T}, CanExecuteHandler{T}, ConverterFromObjectHandler{T})"/>
        public static RelayCommand Create<T>(ExecuteHandler<T> execute)
            => new RelayCommand(new RelayExecute<T>(execute ?? throw new ArgumentNullException(nameof(execute))).executeO);


        /// <inheritdoc cref="Create{T}(ExecuteHandler{T}, CanExecuteHandler{T}, ConverterFromObjectHandler{T})"/>
        public static RelayCommand Create(ExecuteHandler<object> execute, CanExecuteHandler<object> canExecute, ConverterFromObjectHandler<object> converter)
            => new RelayCommand(new RelayExecute<object>(execute ?? throw new ArgumentNullException(nameof(execute)), converter ?? throw new ArgumentNullException(nameof(converter))).executeO,
                                  new RelayCanExecute<object>(canExecute ?? throw new ArgumentNullException(nameof(execute)), converter ?? throw new ArgumentNullException(nameof(converter))).canExecuteO);

        /// <inheritdoc cref="Create{T}(ExecuteHandler{T}, CanExecuteHandler{T}, ConverterFromObjectHandler{T})"/>
        public static RelayCommand Create(ExecuteHandler<object> execute, CanExecuteHandler<object> canExecute)
            => new RelayCommand(execute ?? throw new ArgumentNullException(nameof(execute)),
                                  canExecute ?? throw new ArgumentNullException(nameof(execute)));

        /// <inheritdoc cref="Create{T}(ExecuteHandler{T}, CanExecuteHandler{T}, ConverterFromObjectHandler{T})"/>
        public static RelayCommand Create(ExecuteHandler<object> execute, ConverterFromObjectHandler<object> converter)
            => new RelayCommand(new RelayExecute<object>(execute ?? throw new ArgumentNullException(nameof(execute)), converter ?? throw new ArgumentNullException(nameof(converter))).executeO);


        /// <inheritdoc cref="Create{T}(ExecuteHandler{T}, CanExecuteHandler{T}, ConverterFromObjectHandler{T})"/>
        public static RelayCommand Create(ExecuteHandler<object> execute)
            => new RelayCommand(execute ?? throw new ArgumentNullException(nameof(execute)));


        /// <inheritdoc cref="Create{T}(ExecuteHandler{T}, CanExecuteHandler{T}, ConverterFromObjectHandler{T})"/>
        public static RelayCommand Create(ExecuteHandler execute, CanExecuteHandler canExecute)
            => new RelayCommand(execute ?? throw new ArgumentNullException(nameof(execute)),
                                  canExecute ?? throw new ArgumentNullException(nameof(execute)));

        /// <inheritdoc cref="Create{T}(ExecuteHandler{T}, CanExecuteHandler{T}, ConverterFromObjectHandler{T})"/>
        public static RelayCommand Create(ExecuteHandler execute)
            => new RelayCommand(execute ?? throw new ArgumentNullException(nameof(execute)));

        public struct RelayExecute<T>
        {
            public readonly ExecuteHandler<object> executeO;

            public RelayExecute(ExecuteHandler<T> execute, ConverterFromObjectHandler<T> converter)
            {
                executeO = p =>
                {
                    if (p is T t || converter(p, out t))
                    {
                        execute(t);
                    }
                };
            }
            public RelayExecute(ExecuteHandler<T> execute)
            {
                executeO = p =>
                {
                    if (p is T t)
                    {
                        execute(t);
                    }
                };
            }
            public RelayExecute(ExecuteHandler<object> execute, ConverterFromObjectHandler<object> converter)
            {
                executeO = p =>
                {
                    if (converter(p, out object t))
                    {
                        execute(t);
                    }
                };
            }

        }

        public struct RelayCanExecute<T>
        {
            public readonly CanExecuteHandler<object> canExecuteO;
            public RelayCanExecute(CanExecuteHandler<T> canExecute, ConverterFromObjectHandler<T> converter)
            {
                canExecuteO = p => (p is T t || converter(p, out t)) && canExecute(t);
            }
            public RelayCanExecute(CanExecuteHandler<T> canExecute)
            {
                canExecuteO = p => (p is T t) && canExecute(t);
            }
            public RelayCanExecute(CanExecuteHandler<object> canExecute, ConverterFromObjectHandler<object> converter)
            {
                canExecuteO = p => converter(p, out object t) && canExecute(t);
            }
        }
    }

}
