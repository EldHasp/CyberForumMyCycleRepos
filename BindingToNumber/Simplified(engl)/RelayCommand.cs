using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Simplified
{
    /// <summary> A class that implements <see cref = "ICommand" />. <br/>
    /// Implementation taken from <see href="https://www.cyberforum.ru/wpf-silverlight/thread2390714-page4.html#post13535649"/>
    /// and added a constructor for methods without a parameter.</summary>
    public class RelayCommand : ICommand
    {
        protected readonly CanExecuteHandler<object?> canExecute;
        protected readonly ExecuteHandler<object?> execute;
        private readonly EventHandler requerySuggested;

        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler? CanExecuteChanged;

        /// <summary> Command constructor. </summary>
        /// <param name = "execute"> Command method to execute. </param>
        /// <param name = "canExecute"> Method that returns the state of the command. </param>
        public RelayCommand(ExecuteHandler<object?> execute, CanExecuteHandler<object?> canExecute)
        {
            ArgumentNullException.ThrowIfNull(execute);
            ArgumentNullException.ThrowIfNull(canExecute);
            this.execute = execute;
            this.canExecute = canExecute;

            requerySuggested = (o, e) => Invalidate();
            CommandManager.RequerySuggested += requerySuggested;
        }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler{object?}, CanExecuteHandler{object?})"/>
        public RelayCommand(ExecuteHandler<object?> execute)
            : this(execute, _ => true)
        { }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler{object?}, CanExecuteHandler{object?})"/>
        public RelayCommand(ExecuteHandler execute, CanExecuteHandler canExecute)
                : this
                (
                      p => execute(),
                      (canExecute is null ? null : p => canExecute())!
                )
        { }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler{object?}, CanExecuteHandler{object?})"/>
        public RelayCommand(ExecuteHandler execute)
                : this
                (
                      p => execute()
                )
        { }

        private readonly Dispatcher dispatcher = Application.Current.Dispatcher;

        /// <summary> The method that raises the event <see cref="CanExecuteChanged"/>.</summary>
        public void RaiseCanExecuteChanged()
        {
            if (dispatcher.CheckAccess())
            {
                Invalidate();
            }
            else
            {
                _ = dispatcher.BeginInvoke(Invalidate);
            }
        }

        private void Invalidate()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);


        /// <inheritdoc cref="ICommand.CanExecute(object)"/>
        public bool CanExecute(object? parameter)
        {
            return canExecute(parameter);
        }

        /// <inheritdoc cref="ICommand.Execute(object)"/>
        public void Execute(object? parameter)
        {
            execute(parameter);
        }

    }
}
