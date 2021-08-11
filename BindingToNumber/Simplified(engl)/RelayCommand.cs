using System;
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
        protected readonly CanExecuteHandler<object> canExecute;
        protected readonly ExecuteHandler<object> execute;
        private readonly EventHandler requerySuggested;

        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        public event EventHandler CanExecuteChanged;

        /// <summary> Command constructor. </summary>
        /// <param name = "execute"> Command method to execute. </param>
        /// <param name = "canExecute"> Method that returns the state of the command. </param>
        public RelayCommand(ExecuteHandler<object> execute, CanExecuteHandler<object> canExecute = null)
           : this()
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler{object}, CanExecuteHandler{object})"/>
        public RelayCommand(ExecuteHandler execute, CanExecuteHandler canExecute = null)
                : this
                (
                      p => execute(),
                      p => canExecute?.Invoke() ?? true
                )
        { }

        private readonly Dispatcher dispatcher = Application.Current.Dispatcher;

        /// <summary> The method that raises the event <see cref="CanExecuteChanged"/>.</summary>
        public void RaiseCanExecuteChanged()
        {
            if (dispatcher.CheckAccess())
            {
                invalidate();
            }
            else
            {
                _ = dispatcher.BeginInvoke(invalidate);
            }
        }
        private readonly Action invalidate;
        private RelayCommand()
        {
            invalidate = () => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

            requerySuggested = (o, e) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            CommandManager.RequerySuggested += requerySuggested;
        }

        /// <inheritdoc cref="ICommand.CanExecute(object)"/>
        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        /// <inheritdoc cref="ICommand.Execute(object)"/>
        public void Execute(object parameter)
        {
            execute?.Invoke(parameter);
        }

    }


}
