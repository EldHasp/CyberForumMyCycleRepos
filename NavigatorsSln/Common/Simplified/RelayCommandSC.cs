using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input;

namespace Simplified
{
    /// <summary>Класс команд с реализацией поднятия события
    /// <see cref="ICommand.CanExecuteChanged"/>
    /// в том же потоке в котором слушатель подписывался,
    /// если у этого потока был контекст
    /// синхронизации <see cref="SynchronizationContext.Current"/>.
    /// Для слушателей из потоков без контекста синхронизации событие
    /// <see cref="ICommand.CanExecuteChanged"/> подымается в потоке
    /// в котором был вызван метод <see cref="RaiseCanExecuteChanged"/>.</summary>
    public partial class RelayCommand : ICommand
    {
        private readonly CanExecuteHandler<object> canExecute;
        private readonly ExecuteHandler<object> execute;
        //private readonly EventHandler requerySuggested;


        /// <summary>Локер для синхронизации изменения события и его поднятия.</summary>
        private readonly object lockCanExecuteChanged = new object();

        /// <summary>Делегат для слушателей из потоков без контекста синхронизации.</summary>
        private event EventHandler? PrivateCanExecuteChanged;

        /// <summary>Словарь делегатов для слушателей из потоков с контекстом синхронизации.</summary>
        private readonly Dictionary<SynchronizationContext, EventHandlerItem> doCanExecuteHandlers
            = new Dictionary<SynchronizationContext, EventHandlerItem>();


        /// <summary>Словарь контекстов синхронизации для делегатов.</summary>
        private readonly Dictionary<EventHandler, SynchronizationContext> contexts
            = new Dictionary<EventHandler, SynchronizationContext>();

        /// <summary>Событие извещающее об изменении состояния команды.</summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                lock (lockCanExecuteChanged)
                {
                    SynchronizationContext sc = SynchronizationContext.Current;
                    if (sc != null)
                    {
                        if (!doCanExecuteHandlers.TryGetValue(sc, out EventHandlerItem item))
                        {
                            item = new EventHandlerItem();
                            doCanExecuteHandlers.Add(sc, item);
                        }
                        item.EventsField += value;
                        contexts.Add(value, sc);
                    }
                    else
                    {
                        PrivateCanExecuteChanged += value;
                    }
                }
            }
            remove
            {
                lock (lockCanExecuteChanged)
                {
                    if (contexts.TryGetValue(value, out SynchronizationContext sc))
                    {
                        if (doCanExecuteHandlers.TryGetValue(sc, out EventHandlerItem item))
                        {
                            item.EventsField -= value;
                        }
                        contexts.Remove(value);
                    }
                    else
                    {
                        PrivateCanExecuteChanged -= value;
                    }
                }
            }
        }

        private class EventHandlerItem
        {
            public EventHandler? EventsField;
        }

        /// <summary>Конструктор команды.</summary>
        /// <param name="execute">Выполняемый метод команды.</param>
        /// <param name="canExecute">Метод, возвращающий состояние команды.</param>
        /// <exception cref="ArgumentNullException"><paramref name="execute"/> или
        /// <paramref name="canExecute"/> равно <see langword="null"/>.</exception>
        public RelayCommand(ExecuteHandler<object> execute, CanExecuteHandler<object> canExecute)
        {
            actionInvalidate = handler => ((EventHandler)handler)?.Invoke(this, EventArgs.Empty);

            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));

            //requerySuggested = (o, e) => RaiseCanExecuteChanged();
            //CommandManager.RequerySuggested += requerySuggested;
        }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler{object}, CanExecuteHandler{object})"/>
        public RelayCommand(ExecuteHandler<object> execute)
            : this(execute, p => true)
        { }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler, CanExecuteHandler)"/>
        public RelayCommand(ExecuteHandler execute, CanExecuteHandler canExecute)
                : this
                (
                      p => execute(),
                      p => (canExecute ?? throw new ArgumentNullException(nameof(canExecute)))()
                )
        { }

        /// <inheritdoc cref="RelayCommand(ExecuteHandler, CanExecuteHandler)"/>
        public RelayCommand(ExecuteHandler execute)
                : this
                (
                      p => execute(),
                      p => true
                )
        { }


        /// <summary>Метод, подымающий событие <see cref="CanExecuteChanged"/>.</summary>
        public void RaiseCanExecuteChanged()
        {
            PrivateCanExecuteChanged?.Invoke(this, EventArgs.Empty);
            KeyValuePair<SynchronizationContext, EventHandlerItem>[] handlers;
            lock (lockCanExecuteChanged)
            {
                handlers = doCanExecuteHandlers.ToArray();
            }

            SynchronizationContext sc = SynchronizationContext.Current;
            foreach (KeyValuePair<SynchronizationContext, EventHandlerItem> pair in handlers)
            {
                if (pair.Value.EventsField == null)
                    continue;
                if (pair.Key.Equals(sc))
                {
                    pair.Value.EventsField?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    pair.Key.Post(actionInvalidate, pair.Value.EventsField);
                }
            }
        }
        private readonly SendOrPostCallback actionInvalidate;

        /// <summary>Вызов метода, возвращающего состояние команды.</summary>
        /// <param name="parameter">Параметр команды.</param>
        /// <returns><see langword="true"/> - если выполнение команды разрешено.</returns>
        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;

        /// <summary>Вызов выполняющего метода команды.</summary>
        /// <param name="parameter">Параметр команды.</param>
        public void Execute(object parameter) => execute?.Invoke(parameter);
    }

}
