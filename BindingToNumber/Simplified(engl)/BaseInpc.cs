using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Simplified
{
    /// <summary>Base class with implementation of the <see cref="INotifyPropertyChanged"/> interface.</summary>
    public abstract class BaseInpc : INotifyPropertyChanged
    {
        /// <inheritdoc cref="INotifyPropertyChanged"/>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>The protected method for raising the event <see cref = "PropertyChanged"/>.</summary>
        /// <param name="propertyName">The name of the changed property.
        /// If the value is not specified, the name of the method in which the call was made is used.</param>
        protected void RaisePropertyChanged([CallerMemberName] in string? propertyName = null)
        {
            PropertyChangedEventHandler? propertyChanged = PropertyChanged;
            if (propertyChanged is not null)
            {
                PropertyChangedEventArgs args = string.IsNullOrEmpty(propertyName)
                                ? allProperties
                                : new PropertyChangedEventArgs(propertyName);
                propertyChanged(this, args);
            }
        }

        private static readonly PropertyChangedEventArgs allProperties = new PropertyChangedEventArgs(string.Empty);

        /// <summary> Protected method for assigning a value to a field and raising 
        /// an event <see cref = "PropertyChanged" />. </summary>
        /// <typeparam name = "T"> The type of the field and assigned value. </typeparam>
        /// <param name = "propertyFiled"> Field reference. </param>
        /// <param name = "newValue"> The value to assign. </param>
        /// <param name = "propertyName"> The name of the changed property.
        /// If no value is specified, then the name of the method 
        /// in which the call was made is used. </param>
        /// <param name="isAlways">If <see langword="true"/>, then the assignment to the field,
        /// the raising of the <see cref="PropertyChanged"/> event and
        /// the calling of the <see cref="OnPropertyChanged(in string, in object?, in object?)"/> method
        /// always occur, regardless of the comparison of the new value with the old one.</param>
        /// <remarks> The method is intended for use in the property setter. <br/>
        /// To check for changes,
        /// used the <see cref = "object.Equals (object, object)" /> method.
        /// If the assigned value is not equivalent to the field value,
        /// then it is assigned to the field. <br/>
        /// After the assignment, an event is created <see cref = "PropertyChanged" />
        /// by calling the method <see cref = "RaisePropertyChanged (string)" />
        /// passing the parameter <paramref name = "propertyName" />. <br/>
        /// After the event is created,
        /// the <see cref = "OnPropertyChanged (string, object, object)" />
        /// method is called. </remarks>
        protected void Set<T>(ref T propertyFiled, in T newValue, in bool isAlways = false, [CallerMemberName] in string? propertyName = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(propertyName);
            if (isAlways || !Equals(propertyFiled, newValue))
            {
                T oldValue = propertyFiled;
                propertyFiled = newValue;
                RaisePropertyChanged(propertyName);

                OnPropertyChanged(propertyName, oldValue, newValue);
            }
        }

        /// <summary> The protected virtual method is called after the property has been assigned a value and after the event is raised <see cref = "PropertyChanged" />. </summary>
        /// <param name = "propertyName"> The name of the changed property. </param>
        /// <param name = "oldValue"> The old value of the property. </param>
        /// <param name = "newValue"> The new value of the property. </param>
        /// <remarks> Can be overridden in derived classes to respond to property value changes. <br/>
        /// It is recommended to call the base method as the first operator in the overridden method. <br/>
        /// If the overridden method does not call the base class, then an unwanted change in the base class logic is possible. </remarks>
        protected virtual void OnPropertyChanged(in string propertyName, in object? oldValue, in object? newValue) { }
    }
}