using System;
using System.Windows;

namespace MvvmShort
{
    /// <summary>
    /// Логика взаимодействия для FirstWind.xaml
    /// </summary>
    public partial class FirstWind : Window
    {
        public FirstWind()
        {
            Loaded += Wind_Loaded;
            Unloaded += Wind_Unloaded;
            InitializeComponent();

        }

        private void Wind_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<ConditionSecondWind>(Condition);
        }

        private void Wind_Loaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Register<ConditionSecondWind>(Condition);
        }

        private void Condition(ConditionSecondWind obj)
        {
            SecondCondition = obj;

            btnShowSecond.IsEnabled = !(SecondCondition == ConditionSecondWind.Shown);
        }


        private ConditionSecondWind SecondCondition = ConditionSecondWind.Closed;
        private SecondWind SecondWind;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (SecondCondition == ConditionSecondWind.Closed)
                SecondWind = new SecondWind();

            if (SecondWind.Visibility != Visibility.Visible)
                SecondWind.Show();

            if (SecondWind.WindowState == WindowState.Minimized)
                SecondWind.WindowState = WindowState.Normal;

        }
    }

}
