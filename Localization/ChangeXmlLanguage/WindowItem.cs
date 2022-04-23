using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Windows;

namespace ChangeXmlLanguage
{
    public class WindowItem
    {
        private Type _windowType = typeof(Window);
#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
        private ConstructorInfo windowConstructor = typeof(Window).GetConstructor(Array.Empty<Type>());
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.

        private static readonly TypeConverter<Window> windowTypeConverter = new();

        [TypeConverter(typeof(TypeConverter<Window>))]
        public object WindowType
        {
            get => _windowType;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                if (!windowTypeConverter.IsValid(value))
                    throw new ArgumentException("Может быть только Type или string с именем типа.", nameof(value));

                var type = (Type?)windowTypeConverter.ConvertFrom(value);
                if (type == null)
                    throw new ArgumentException("Может быть только тип Window и производный от него.", nameof(value));

                windowConstructor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, Type.EmptyTypes)
                    ?? throw new ArgumentException("У типа должен быть конструктор по умолчанию.", nameof(value));

                _windowType = type;
            }
        }

        private Window? _instance;
        private string _title = string.Empty;

        public Window Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (Window)windowConstructor.Invoke(null);
                    _instance.Closed += OnClosed;
                }
                return _instance;
            }
        }

        private void OnClosed(object? sender, EventArgs e)
        {
            if (ReferenceEquals(_instance, sender))
                _instance = null;
        }

        public string Title { get => _title; set => _title = value ?? string.Empty; }
        public object? ToolTip { get; set; }

    }

    public class WindowList : ObservableCollection<WindowItem> { }
}
