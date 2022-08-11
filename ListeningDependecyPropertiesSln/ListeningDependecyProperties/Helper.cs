using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ListeningDependecyProperties
{
    public static class Helper
    {
        /// <summary>Присоединение прослушки изменения свойства.</summary>
        /// <typeparam name="T">Тип прослушиваемоего объекта. <see cref="DependencyObject"/> или производный от него.</typeparam>
        /// <param name="source">Прослушиваемый объект.</param>
        /// <param name="dependencyProperty">Прослушиваемое свойство.</param>
        /// <param name="listener">Метод прослушки.</param>
        public static void AddPropertyChanged<T>(this T source, DependencyProperty dependencyProperty, DependencyPropertyChangedHandler listener)
            where T : DependencyObject
        {
            var descriptor = DependencyPropertyDescriptor.FromProperty(dependencyProperty, source.GetType());
            var propertyHandler = AddPropertyHandler(source, listener, dependencyProperty);
            descriptor.AddValueChanged(source, propertyHandler.Raise);
        }

        /// <summary>Отсоединение прослушки изменения свойства.</summary>
        /// <typeparam name="T">Тип прослушиваемоего объекта. <see cref="DependencyObject"/> или производный от него.</typeparam>
        /// <param name="source">Прослушиваемый объект.</param>
        /// <param name="dependencyProperty">Прослушиваемое свойство.</param>
        /// <param name="listener">Метод прослушки.</param>
        public static void RemovePropertyChanged<T>(this T source, DependencyProperty dependencyProperty, DependencyPropertyChangedHandler listener)
            where T : DependencyObject
        {
            var descriptor = DependencyPropertyDescriptor.FromProperty(dependencyProperty, source.GetType());
            var propertyHandler = RemovePropertyHandler(source, listener, dependencyProperty);
            if (propertyHandler != null)
                descriptor.RemoveValueChanged(source, propertyHandler.Raise);
        }

        private static readonly ConditionalWeakTable<DependencyObject, Dictionary<(DependencyPropertyChangedHandler handler, DependencyProperty property), PropertyHandler>> handlers = new();

        //private static readonly Dictionary<(DependencyObject source, DependencyPropertyChangedHandler handler, DependencyProperty property), PropertyHandler> handlers = new();

        private static PropertyHandler AddPropertyHandler(DependencyObject source, DependencyPropertyChangedHandler handler, DependencyProperty property)
        {

            if (!handlers.TryGetValue(source, out var properties))
            {
                properties = new();
                handlers.Add(source, properties);
            }

            if (properties.TryGetValue((handler, property), out var pHandler))
            {
                pHandler.Count++;
            }
            else
            {
                pHandler = new(handler, property, source.GetValue(property));
                properties.Add((handler, property), pHandler);
            }
            return pHandler;
        }

        private static PropertyHandler? RemovePropertyHandler(DependencyObject source, DependencyPropertyChangedHandler handler, DependencyProperty property)
        {
            PropertyHandler? pHandler = null;
            if (handlers.TryGetValue(source, out var properties))
            {
                if (properties.TryGetValue((handler, property), out pHandler))
                {
                    pHandler.Count--;
                    if (pHandler.Count == 0)
                    {
                        properties.Remove((handler, property));
                    }
                    if (properties.Count == 0)
                    {
                        handlers.Remove(source);
                    }
                }
            }

            return pHandler;
        }


        private class PropertyHandler
        {
            public DependencyPropertyChangedHandler Handler { get; }

            public int Count { get; set; }

            public DependencyProperty Property { get; }

            public object? OldValue { get; private set; }

            public void Raise(object? sender, EventArgs e)
            {
                if (sender is not DependencyObject dObj)
                {
                    throw new InvalidCastException("sender может быть только DependecyObject.");
                }

                object? oldValue = OldValue;
                object? newValue = dObj.GetValue(Property);

                Handler(dObj, new DependencyPropertyChangedEventArgs(Property, oldValue, newValue));

                OldValue = newValue;
            }

            public PropertyHandler(DependencyPropertyChangedHandler handler, DependencyProperty property, object? oldValue)
            {
                Handler = handler;
                Property = property;
                Count = 1;
                OldValue = oldValue;
            }
        }
    }
}
