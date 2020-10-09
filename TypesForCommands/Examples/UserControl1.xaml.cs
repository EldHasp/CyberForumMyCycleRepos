using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Examples
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }



        public double Numeric
        {
            get { return (double)GetValue(NumericProperty); }
            set { SetValue(NumericProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Numeric.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NumericProperty =
            DependencyProperty.Register(nameof(Numeric), typeof(double), typeof(UserControl1), new PropertyMetadata(0.0));


    }
}
