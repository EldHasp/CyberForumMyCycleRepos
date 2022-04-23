using Poco;
using Simplified;
using System;
using System.Windows;
using System.Windows.Markup;

namespace ViewModelProperties
{
    /// <summary>Возвращает команду, показывающую Окно, полученное в параметре команды.</summary>
    [MarkupExtensionReturnType(typeof(RelayCommand))]
    public class ShowWindowExtension : MarkupExtension
    {
        /// <summary>Команда, показывающая Окно, полученное в параметре команды.</summary>
        public static RelayCommand Command { get; } = new RelayCommand
        (
            p =>
            {
                if (p is WindowItem wItem)
                    wItem.Instance.Show();
                else if (p is Window window)
                    window.Show();
            },
            p => p is WindowItem || p is Window
        );

        /// <summary>Возвращает <see cref="Command"/>.</summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Command;
        }
    }
}
