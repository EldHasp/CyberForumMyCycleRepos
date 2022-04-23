using Localization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChangeXmlLanguage
{
    /// <summary>
    /// Логика взаимодействия для XmlLocalizerFirstWindow1.xaml
    /// </summary>
    public partial class XmlLocalizerFirstWindow : Window
    {
        public XmlLocalizerFirstWindow()
        {
            InitializeComponent();

            //Loaded += OnLoaded;
        }

        //private void OnLoaded(object sender, RoutedEventArgs e)
        //{
        //    XmlLocalizer.Default.AddLanguage("ru");
        //    XmlLocalizer.Default.AddLanguage("en");
        //    XmlLocalizer.Default.AddLanguage("de");
        //}

        static XmlLocalizerFirstWindow()
        {
            XmlLocalizer.Default.GetLanguage("ru");
            XmlLocalizer.Default.GetLanguage("en");
            XmlLocalizer.Default.GetLanguage("de");

            XmlLocalizer.Default.UpdateTimer.Start();
        }
    }


}
