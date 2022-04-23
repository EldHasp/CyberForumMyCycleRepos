using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ViewModelProperties;

namespace ViewModelProperties
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private static readonly EnumConverter enumConverter = new EnumConverter(typeof(AuthorizationMode));
        private void OnAuthorizationMode(object sender, ExecutedRoutedEventArgs e)
        {
            if (DataContext is AuthorizationVM vm)
            {
                if (e.Parameter is not AuthorizationMode mode)
                {
                    if (enumConverter.IsValid(e.Parameter))
                        mode = (AuthorizationMode)enumConverter.ConvertFrom(e.Parameter);
                    else
                        return;
                }
                vm.AuthorizationMode = mode;
            }
        }
    }
}
