using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using ViewModelProperties;

namespace ViewNavigators
{
    /// <summary>Возвращает или всплывающую команду <see cref="SetMode"/>, или делегат <see cref="SetModeExecute"/>
    /// для события <see cref="CommandBinding.Executed"/>.</summary>
    public class AuthorizationCommand : MarkupExtension
    {
        /// <summary>Всплывающая команда для установки режима представления Авторизации.</summary>
        public static RoutedUICommand SetMode { get; } = new("Установка режима представления Авторизации", "SetMode", typeof(AuthorizationCommand));

        // Конвертер для конвертации параметра команды в AuthorizationMode, так же как это происходит в XAML и привязках.
        private static readonly EnumConverter enumConverter = new(typeof(AuthorizationMode));

        /// <summary>Делегат для события <see cref="CommandBinding.Executed"/>.</summary>
        public static ExecutedRoutedEventHandler SetModeExecute { get; } = (s, e) =>
        {
            // Получение Навигатора из ресурсов.
            if (((FrameworkElement)s).FindResource("authorizationModeNavigator") is AuthorizationModeNavigator authorizationModeNavigator)
            {
                // Если параметр не AuthorizationMode, то попытка его конвертации 
                if (e.Parameter is not AuthorizationMode mode)
                {
                    if (enumConverter.CanConvertFrom(e.Parameter.GetType()) &&
                        enumConverter.IsValid(e.Parameter))
                    {
                        AuthorizationMode? m = enumConverter.ConvertFrom(e.Parameter) as AuthorizationMode?;
                        if (m is null || !Enum.IsDefined(typeof(AuthorizationMode), m))
                            return;
                        mode = m.Value;
                    }
                    else
                    {
                        return;
                    }
                }

                // Установка режима представления.
                authorizationModeNavigator.AuthorizationMode = mode;
            }
        };

        /// <summary>Параметр команды.</summary>
        public AuthorizationMode? Mode { get; set; }

        /// <summary>Возвращает всплывающую команду <see cref="SetMode"/> или делегат <see cref="SetModeExecute"/>.</summary>
        /// <param name="serviceProvider">Отсюда извлекается информация о целевых типе и свойстве.</param>
        /// <returns><see cref="SetMode"/> или <see cref="SetModeExecute"/>.</returns>
        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget? service = (IProvideValueTarget?)serviceProvider.GetService(typeof(IProvideValueTarget));
            if (service != null)
            {
                object targetProperty = service.TargetProperty; // Получение информации о свойстве.

                // Если это свойство имеет тип ICommand, то возвращается команда SetMode.
                if ((targetProperty is DependencyProperty dp && dp.PropertyType.IsAssignableFrom(typeof(ICommand))) ||
                    (targetProperty is PropertyInfo pi && pi.PropertyType.IsAssignableFrom(typeof(ICommand))))
                {
                    // Если целевой олбъект - CommandBinding, то сразу производится подписка на событие Executed.
                    if (service.TargetObject is CommandBinding commandBinding)
                    {
                        commandBinding.Executed += SetModeExecute;
                    }

                    // Если целевой олбъект - источник команд и его свойтсво CommandParameter можно записать,
                    // то в него записывается Mode, если он есть.
                    if (service.TargetObject is ICommandSource commandSource && Mode != null)
                    {
                        var parameterProperty = commandSource.GetType().GetProperty(nameof(ICommandSource.CommandParameter), typeof(object));
                        if (parameterProperty != null && parameterProperty.CanWrite)
                        {
                            parameterProperty.SetValue(commandSource, Mode.Value);
                        }
                    }
                    return SetMode;
                }
            }
            return null;
        }

        public AuthorizationCommand()
        {
        }

        public AuthorizationCommand(AuthorizationMode? mode)
        {
            Mode = mode;
        }
    }
}
