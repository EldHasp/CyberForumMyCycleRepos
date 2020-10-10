using System.Windows;
using System.Windows.Input;

namespace WpfCommands.Bindings
{
    public class CommandBinding : Freezable
    {
        public ICommand RoutedCommand
        {
            get { return (ICommand)GetValue(RoutedCommandProperty); }
            set { SetValue(RoutedCommandProperty, value); }
        }
        public static readonly DependencyProperty RoutedCommandProperty =
            DependencyProperty.Register(nameof(RoutedCommand), typeof(ICommand), typeof(CommandBinding), new PropertyMetadata(null));


        public ICommand RelayCommand
        {
            get { return (ICommand)GetValue(RelayCommandProperty); }
            set { SetValue(RelayCommandProperty, value); }
        }
        public static readonly DependencyProperty RelayCommandProperty =
            DependencyProperty.Register(nameof(RelayCommand), typeof(ICommand), typeof(CommandBinding), new PropertyMetadata(null));

        protected override Freezable CreateInstanceCore()
        {
            return new CommandBinding();
        }
    }

}
