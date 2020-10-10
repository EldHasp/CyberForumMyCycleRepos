using InterfacesCommands;
using System;
using System.ComponentModel;

namespace Commands
{
    /// <summary>Класс реализующий интерфейс ICommand для создания команд с методом Invalidate.</summary>
    /// <remarks>Класс не подключается к CommandManager и не поддерживает
    /// автоматического создания события CanExecuteChanged при изменении в WPF GUI.</remarks>
    public class RelayCommand : ICommandInvalidate
    {
        /// <summary>Делегат исполнительного метода команды.</summary>
        protected CanExecuteHandler canExecute;

        /// <summary>Делегат метода состояния команды.</summary>
        protected ExecuteHandler execute;


        /// <summary>Конструктор по умолчанию для создания производных типов.</summary>
        protected RelayCommand() { }

        /// <summary>Метод всегда возвращающий <see langword="true"/>.
        /// Задан для уменьшения создания лямбд.</summary>
        /// <param name="parameter">Не используется.</param>
        /// <returns><see langword="true"/>.</returns>
        public static bool CanExecuteTrue(object parameter) => true;

        /// <summary>Конструктор команды.</summary>
        /// <param name="execute">Выполняемый метод команды.</param>
        /// <param name="canExecute">Метод проверяющий состояние команды.</param>
        public RelayCommand(ExecuteHandler execute, CanExecuteHandler canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));

            this.canExecute = canExecute ?? CanExecuteTrue;
        }

        #region Реализация ICommandInvalidate

        /// <summary>Виртуальный защищённый метод создающий событие CanExecuteChanged.</summary>
        protected virtual void RiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

        public void Invalidate() => RiseCanExecuteChanged();
        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled == value)
                    return;

                _isEnabled = value;
                PropertyChanged?.Invoke(this, IsEnabledEventArgs);
                Invalidate();
            }
        }

        #region Реализация ICommand
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => IsEnabled && canExecute(parameter);

        public void Execute(object parameter) => execute.Invoke(parameter);

        #endregion
        #endregion

        #region Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public static PropertyChangedEventArgs IsEnabledEventArgs { get; }
            = new PropertyChangedEventArgs(nameof(IsEnabled));
        #endregion

    }
}
