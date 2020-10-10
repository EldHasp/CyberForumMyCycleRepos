using System.Windows;

namespace MvvmShort
{
    /// <summary>
    /// Логика взаимодействия для SecondWind.xaml
    /// </summary>
    public partial class SecondWind : Window
    {
        public SecondWind()
        {
            Closed += Window_Closed;
            IsVisibleChanged += Window_IsVisibleChanged;
            StateChanged += Window_StateChanged;

            InitializeComponent();

        }

        private void Window_StateChanged(object sender, System.EventArgs e)
        {
           if (WindowState == WindowState.Minimized)
                Messenger.Default.Send(ConditionSecondWind.Hidden);
           else
                Messenger.Default.Send(ConditionSecondWind.Shown);
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            Messenger.Default.Send(ConditionSecondWind.Closed);
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
                Messenger.Default.Send(ConditionSecondWind.Shown);
            else
                Messenger.Default.Send(ConditionSecondWind.Hidden);
        }
    }

}
