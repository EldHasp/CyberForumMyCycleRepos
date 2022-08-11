using ListeningDependecyProperties;
using System;
using System.Windows;
using System.Windows.Data;

namespace Proxy
{
    /// <summary>Предоставляет прокси <see cref="DependencyObject"/> с одним свойством и 
    /// событием уведомляющем о его изменении.</summary>
    /// <typeparam name="T">Тип свойства <see cref="Value"/>.</typeparam>
    public class ValueProxy<T> : Freezable
    {
        /// <summary>Свойство для задания внешних привязок.</summary>
        public T Value
        {
            get => (T)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(T), typeof(ValueProxy<T>), new PropertyMetadata(null));

        /// <summary>Привязка по умолсанию - пустой экземпляр <see cref="Binding()"/>.
        /// </summary>
        public static readonly Binding DefaultBinding = new Binding();

        public ValueProxy()
            => SetValueBinding(DefaultBinding);

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            ValueChanged?.Invoke(this, e);
        }

        /// <summary>Событие, возникающее при изменении значения любого <see cref="DependencyProperty"/>.</summary>
        public event DependencyPropertyChangedHandler? ValueChanged;

        /// <summary>Возвращает <see langword="true"/>, если значение свойства <see cref="Value"/> не задано.</summary>
        public bool IsUnsetValue => Equals(ReadLocalValue(ValueProperty), DependencyProperty.UnsetValue);

        /// <summary>Очистка всех <see cref="DependencyProperty"/> этого <see cref="ProxyDO"/>.</summary>
        public void Reset()
        {
            LocalValueEnumerator locallySetProperties = GetLocalValueEnumerator();
            while (locallySetProperties.MoveNext())
            {
                DependencyProperty propertyToClear = locallySetProperties.Current.Property;
                if (!propertyToClear.ReadOnly)
                {
                    ClearValue(propertyToClear);
                }
            }

        }

        /// <summary><see langword="true"/> если свойству задана Привязка.</summary>
        public bool IsValueBinding => BindingOperations.GetBindingExpressionBase(this, ValueProperty) != null;

        /// <summary><see langword="true"/> если свойству задана привязка
        /// и она в состоянии <see cref="BindingStatus.Active"/>.</summary>
        public bool IsActiveValueBinding
        {
            get
            {
                BindingExpressionBase exp = BindingOperations.GetBindingExpressionBase(this, ValueProperty);
                if (exp == null)
                {
                    return false;
                }

                BindingStatus status = exp.Status;
                return status == BindingStatus.Active;
            }
        }

        public void SetValueBinding(BindingBase binding)
        {
            if (binding != null)
                BindingOperations.SetBinding(this, ValueProperty, binding);
            else
                BindingOperations.ClearBinding(this, ValueProperty);
        }

        protected override Freezable CreateInstanceCore()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>Прокси <see cref="DependencyObject"/> с одним свойством типа <see cref="object"/>
    /// и событием уведомляющем о его изменении.</summary>
    public class Proxy : ValueProxy<object>
    {

    }
    /// <summary>Прокси <see cref="DependencyObject"/> с одним свойством типа <see cref="string"/>
    /// и событием уведомляющем о его изменении.</summary>
    public class StringProxy : ValueProxy<string> { }
}
