using System;

namespace Simplified
{

    /// <summary>  implementation for generic parameter methods. </summary>
    /// <typeparam name = "T"> Method parameter type. </typeparam>  
    public class RelayCommand<T> : RelayCommand
    {
        /// <summary> Command constructor. </summary>
        /// <param name = "execute"> Command method to execute. </param>
        /// <param name = "canExecute"> Method that returns the state of the command. </param>
        /// <param name="converter">Optional converter to convert <see cref="object"/> to <typeparamref name="T"/>. <br/>
        /// It is called when the parameter
        /// <see href="https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/is">
        /// is not compatible</see> with a <typeparamref name="T"/> type.
        /// </param>
        public RelayCommand(ExecuteHandler<T> execute, CanExecuteHandler<T> canExecute, ConverterFromObjectHandler<T> converter = null)
            : base
            (
                  p =>
                  {
                      if (p is T t ||
                        (converter != null && converter(p, out t)))
                      {
                          execute(t);
                      }
                  },
                  p => ((p is T t) || (converter != null && converter(p, out t))) &&
                        (canExecute?.Invoke(t) ?? true)
            )
        { }

        /// <summary> Command constructor. </summary>
        /// <param name = "execute"> Command method to execute. </param>
        /// <param name="converter">Optional converter to convert <see cref="object"/> to <typeparamref name="T"/>.</param>
        public RelayCommand(ExecuteHandler<T> execute, ConverterFromObjectHandler<T> converter = null)
           : this(execute, null, converter)
        { }
   }

    /// <summary> <see cref="RelayCommandAsync"/>implementation for generic parameter methods. </summary>
    /// <typeparam name = "T"> Method parameter type. </typeparam>  
    public class RelayCommandAsync<T> : RelayCommandAsync
    {
        /// <inheritdoc cref="RelayCommand{T}(ExecuteHandler{T}, CanExecuteHandler{T}, ConverterFromObjectHandler{T})"/>
        public RelayCommandAsync(ExecuteHandler<T> execute, CanExecuteHandler<T> canExecute, ConverterFromObjectHandler<T> converter = null)
            : base
            (
                  p =>
                  {
                      if (p is T t ||
                        (converter != null && converter(p, out t)))
                      {
                          execute(t);
                      }
                  },
                  p => ((p is T t) || (converter != null && converter(p, out t))) &&
                    (canExecute?.Invoke(t) ?? true)
            )
        { }


        /// <inheritdoc cref="RelayCommand{T}(ExecuteHandler{T}, ConverterFromObjectHandler{T})"/>
        public RelayCommandAsync(ExecuteHandler<T> execute, ConverterFromObjectHandler<T> converter = null)
           : this(execute, null, converter)
        { }
    }
}
