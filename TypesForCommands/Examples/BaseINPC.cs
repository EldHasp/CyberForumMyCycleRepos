using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Examples
{

    /// <summary>Base class implementing INotifyPropertyChanged.</summary>
    public abstract class BaseINPC : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>Called AFTER the property value changes.</summary>
        /// <param name="propertyName">The name of the property.
        /// In the property setter, the parameter is not specified. </param>
        public void RaisePropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary> A virtual method that defines changes in the value field of a property value. </summary>
        /// <typeparam name = "T"> Type of property value. </typeparam>
        /// <param name = "oldValue"> Reference to the field with the old value. </param>
        /// <param name = "newValue"> New value. </param>
        /// <param name = "propertyName"> The name of the property. If <see cref = "string.IsNullOrWhiteSpace (string)" />,
        /// then ArgumentNullException. </param> 
        /// <remarks> If the base method is not called in the derived class,
        /// then the value will not change.</remarks>
        protected virtual void Set<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            if ((oldValue == null && newValue != null) || (oldValue != null && !oldValue.Equals(newValue)))
                OnValueChange(ref oldValue, newValue, propertyName);
        }

        /// <summary> A virtual method that changes the value of a property. </summary>
        /// <typeparam name = "T"> Type of property value. </typeparam>
        /// <param name = "oldValue"> Reference to the property value field. </param>
        /// <param name = "newValue"> New value. </param>
        /// <param name = "propertyName"> The name of the property. </param>
        /// <remarks> If the base method is not called in the derived class,
        /// then the value will not change.</remarks>
        protected virtual void OnValueChange<T>(ref T oldValue, T newValue, string propertyName)
        {
            oldValue = newValue;
            RaisePropertyChanged(propertyName);
        }

    }

}
