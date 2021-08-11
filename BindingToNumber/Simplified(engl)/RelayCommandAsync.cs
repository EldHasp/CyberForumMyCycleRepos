using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Simplified
{
    /// <summary>A class that implements asynchronous commands with the execution
    /// of the <see cref="RelayCommand.Execute(object)"/>  method in Task. </summary>
    /// <remarks>Only one call to the <see cref="RelayCommand.Execute(object)"/>
    /// method is allowed at a time.
    /// During the execution of the method, the <see cref="IsBusy"/> flag is set and
    /// the <see cref="RelayCommand.CanExecute(object)"/> method of the command will return false.
    /// The <see cref="INotifyDataErrorInfo"/> interface is implemented to notify about
    /// an erroneous call to the <see cref="RelayCommand.Execute(object)"/> method before
    /// the previous execution completes and about exceptions during the
    /// execution of <see cref="RelayCommand.Execute(object)"/>.
    /// To notify about changes in property values, the <see cref="INotifyPropertyChanged"/>
    /// interface is implemented.</remarks>
    public class RelayCommandAsync : RelayCommand, ICommand, INotifyPropertyChanged, INotifyDataErrorInfo
    {
        /// <inheritdoc cref="INotifyPropertyChanged.PropertyChanged"/>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <inheritdoc cref="INotifyDataErrorInfo.ErrorsChanged"/>
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        /// <summary>The command is in the execution state of the <see cref="RelayCommand.Execute(object)"/> method.</summary>
        public bool IsBusy { get; private set; }

        /// <inheritdoc cref="INotifyDataErrorInfo.HasErrors"/>
        public bool HasErrors { get; private set; }

        /// <summary>Exception from the last execution of the <see cref="RelayCommand.Execute(object)"/> method.</summary>
        public Exception ExecuteException { get; private set; }

        // A flag indicating a "call to execute busy a command" error.
        private bool isBusyExecuteError;

        /// <summary>Sets a value to the <see cref="IsBisy"/> property and notifies of its change.</summary>
        /// <param name="isBusy">The value for the property.</param>
        protected void SetIsBusy(bool isBusy)
        {
            if (IsBusy != isBusy)
            {
                IsBusy = isBusy;
                PropertyChanged?.Invoke(this, Args.IsBusyPropertyEventArgs);
                RaiseCanExecuteChanged();
            }
        }

        /// <summary>Sets the HasErrors property and reports an entity-level error.</summary>
        /// <param name="hasErrors">The value for the property.</param>
        protected void SetEntityHasErrors(bool hasErrors)
              => SetHasErrors(hasErrors, Args.EntityLevelErrorsEventArgs);

        /// <summary>Sets the HasErrors property and reports an entity or property level error.</summary>
        /// <param name="hasErrors">The value for the property.</param>
        /// <param name="args">Argument with data about the error level.</param>
        protected void SetHasErrors(bool hasErrors, DataErrorsChangedEventArgs args)
        {
            if (HasErrors != hasErrors)
            {
                HasErrors = hasErrors;
                PropertyChanged?.Invoke(this, Args.HasErrorsPropertyEventArgs);
            }
            ErrorsChanged?.Invoke(this, args);
        }


        /// <summary>Sets a value to the <see cref="ExecuteException"/> property and notifies of its change.</summary>
        /// <param name="exception">The value for the property.</param>
        protected void SetExecuteException(Exception exception)
        {
            if (ExecuteException != exception)
            {
                ExecuteException = exception;
                PropertyChanged?.Invoke(this, Args.ExecuteExceptionPropertyEventArgs);
            }
        }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler{object}, CanExecuteHandler{object})"/>
        public RelayCommandAsync(ExecuteHandler<object> execute, CanExecuteHandler<object> canExecute = null)
            : this(new AsyncData(execute, canExecute))
        { }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler, CanExecuteHandler)"/>
        public RelayCommandAsync(ExecuteHandler execute, CanExecuteHandler canExecute = null)
            : this(new AsyncData(execute, canExecute))
        { }

        // The field for storing additional, auxiliary data generated
        // during the generation of the asynchronous method, wrapping
        // the one obtained in the constructor.
        private readonly AsyncData data;

        /// <inheritdoc cref="RelayCommand(ExecuteHandler{object}, CanExecuteHandler{object})"/>
        protected RelayCommandAsync(AsyncData data)
            : base(data.ExecuteAsync, data.CanExecuteAsync)
        {
            this.data = data;
            this.data.commandAsync = this;
        }

        /// <inheritdoc cref="INotifyDataErrorInfo.GetErrors(string)"/>
        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                if (isBusyExecuteError)
                {
                    yield return Args.BusyExecuteErrorMessage;
                }

                if (ExecuteException != null)
                {
                    yield return ExecuteException;
                }
            }
            IEnumerable errors = GetErrorsOverride(propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    yield return error;
                }
            }
        }

        /// <summary>Method overridden in derived classes to add error information</summary>
        /// <param name="propertyName">The name of the property to retrieve validation
        /// errors for; or null or Empty, to retrieve entity-level errors.</param>
        /// <returns>The validation errors for the property or entity.</returns>
        protected virtual IEnumerable GetErrorsOverride(string propertyName)
            => null;

        /// <summary>A class with persistent elements to avoid re-creating them frequently.</summary>
        public static class Args
        {
            public const string BusyExecuteErrorMessage = "Called the execution of a command when it is busy.";

            public static readonly PropertyChangedEventArgs IsBusyPropertyEventArgs = new PropertyChangedEventArgs(nameof(IsBusy));
            public static readonly PropertyChangedEventArgs HasErrorsPropertyEventArgs = new PropertyChangedEventArgs(nameof(HasErrors));
            public static readonly PropertyChangedEventArgs ExecuteExceptionPropertyEventArgs = new PropertyChangedEventArgs(nameof(ExecuteException));

            public static readonly DataErrorsChangedEventArgs EntityLevelErrorsEventArgs = new DataErrorsChangedEventArgs(string.Empty);
        }

        /// <summary>A class for storing additional, auxiliary data and methods that are generated
        /// when generating asynchronous methods that wrap the synchronous methods received
        /// in the constructor.</summary>
        protected class AsyncData
        {
            public RelayCommandAsync commandAsync;
            public async void ExecuteAsync(object parameter)
            {
                if (commandAsync.IsBusy)
                {
                    commandAsync.isBusyExecuteError = true;
                    commandAsync.SetEntityHasErrors(true);
                }
                else
                {
                    commandAsync.SetIsBusy(true);

                    try
                    {
                        await Task.Run(() => execute(parameter));

                        commandAsync.isBusyExecuteError = false;
                        commandAsync.SetExecuteException(null);
                        commandAsync.SetEntityHasErrors(false);
                    }
                    catch (Exception ex)
                    {
                        commandAsync.SetExecuteException(ex);
                        commandAsync.SetEntityHasErrors(true);
                    }
                    finally
                    {
                        commandAsync.SetIsBusy(false);
                    }
                }
            }

            public CanExecuteHandler<object> CanExecuteAsync { get; }
            private bool canExecuteNullAsync(object parameter) => !commandAsync.IsBusy;
            private bool canExecuteAsync(object parameter) => !commandAsync.IsBusy && canExecute(parameter);

            private readonly ExecuteHandler<object> execute;
            private readonly CanExecuteHandler<object> canExecute;

            /// <inheritdoc cref="AsyncData(ExecuteHandler, CanExecuteHandler)"/>
            public AsyncData(ExecuteHandler<object> execute, CanExecuteHandler<object> canExecute)
            {
                this.execute = execute ?? throw new ArgumentNullException(nameof(execute));


                if (canExecute == null)
                {
                    CanExecuteAsync = canExecuteNullAsync;
                }
                else
                {
                    this.canExecute = canExecute;
                    CanExecuteAsync = canExecuteAsync;
                }
            }

            /// <summary>Creates an instance.</summary>
            /// <param name="execute">Synchronous Execute method.</param>
            /// <param name="canExecute">Synchronous CanExecute method.</param>
            public AsyncData(ExecuteHandler execute, CanExecuteHandler canExecute)
            {
                if (execute == null)
                {
                    throw new ArgumentNullException(nameof(execute));
                }

                this.execute = p => execute();


                if (canExecute == null)
                {
                    CanExecuteAsync = canExecuteNullAsync;
                }
                else
                {
                    this.canExecute = p => canExecute();
                    CanExecuteAsync = canExecuteAsync;
                }
            }
        }

    }


}
