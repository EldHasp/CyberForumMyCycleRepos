using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;

namespace Simplified
{
    /// <summary>Базовый класс для ViewModel.</summary>
    public abstract partial class ViewModelBase : BaseInpc
    {
        /// <summary><see cref="Dispatcher"/> основного потока приложения.<br/>
        /// Инициализируется из свойства
        /// <see cref="Application.Current"/>.<see cref="DispatcherObject.Dispatcher">Dispatcher</see>.</summary>
        public static Dispatcher DispatcherStatic { get; } = Application.Current.Dispatcher;

        /// <summary>Возвращает <see langword="true"/>, если обращение происходит
        /// во Время режима Разработки.<br/>
        /// Может использовать для создания Контекста Данных Времени Разработки
        /// отличного от Контекста Данных Времени Исполнения.<br/>
        /// Инициализируется значением возвращаемым методом
        /// <see cref="DesignerProperties.GetIsInDesignMode(DependencyObject)"/>
        /// для new DependencyObject().</summary>
        public static bool IsInDesignModeStatic { get; } = DesignerProperties.GetIsInDesignMode(new DependencyObject());

        /// <summary><see cref="Dispatcher"/> используемый в экземпляре <see cref="ViewModelBase"/>.<br/>
        /// Инициализируется значение свойства <see cref="DispatcherStatic"/>.<br/>
        /// В производных классах может быть, через конструкторы
        /// <see cref="ViewModelBase(Dispatcher)"/> и <see cref="ViewModelBase(Dispatcher, bool)"/>,
        /// задано другое значение.</summary>
        public Dispatcher Dispatcher { get; } = DispatcherStatic;

        /// <summaryВозвращает <see langword="true"/>, если обращение происходит
        /// во Время режима Разработки.<br/>
        /// Инициализируется значение свойства <see cref="IsInDesignModeStatic"/>.<br/>
        /// В производных классах может быть, через конструкторы
        /// <see cref="ViewModelBase(bool)"/> и <see cref="ViewModelBase(Dispatcher, bool)"/>,
        /// задано другое значение.</summary>
        public bool IsInDesignMode { get; } = IsInDesignModeStatic;

        /// <summary>Конструктор со значением свойств <see cref="Dispatcher"/>
        /// и <see cref="IsInDesignMode"/> заданными при инициализации.</summary>
        protected ViewModelBase() { }

        /// <summary>Конструктор со значением свойства
        /// <see cref="IsInDesignMode"/> заданным при инициализации.</summary>
        /// <param name="dispatcher">Значение для свойства <see cref="Dispatcher"/>.</param>
        protected ViewModelBase(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        /// <summary>Конструктор со значением свойства
        /// <see cref="Dispatcher"/> заданным при инициализации.</summary>
        /// <param name="isInDesignMode">Значение для свойства <see cref="IsInDesignMode"/>.</param>
        protected ViewModelBase(bool isInDesignMode)
        {
            IsInDesignMode = isInDesignMode;
        }

        /// <summary>Конструктор с заданием значения обоих свойств.</summary>
        /// <param name="dispatcher">Значение для свойства <see cref="Dispatcher"/>.</param>
        /// <param name="isInDesignMode">Значение для свойства <see cref="IsInDesignMode"/>.</param>
        protected ViewModelBase(Dispatcher dispatcher, bool isInDesignMode)
            : this(dispatcher)
        {
            IsInDesignMode = isInDesignMode;
        }
    }
}
