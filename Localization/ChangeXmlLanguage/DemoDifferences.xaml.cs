using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Threading;

namespace ChangeXmlLanguage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class DemoDifferences : Window
    {
        private readonly XmlLanguage defaultLanguage;
        private readonly CultureInfo defaultCulture;

        private readonly BindingExpression expressionThread;
        private readonly BindingExpression expressionParse;
        public DemoDifferences()
        {
            defaultLanguage = Language;
            defaultCulture = CultureInfo.CurrentCulture;

            InitializeComponent();
            expressionThread = tBlockThread.GetBindingExpression(TextBlock.TextProperty);
            expressionParse = tBlockParse.GetBindingExpression(TextBlock.TextProperty);
        }

        private void OnSelectLanguage(object sender, SelectionChangedEventArgs e)
        {
            Selector selector = (Selector)sender;
            if (selector.SelectedItem == null)
                Language = defaultLanguage;
            else
                Language = XmlLanguage.GetLanguage(selector.SelectedItem.ToString());
        }

        private void OnSelectCulture(object sender, SelectionChangedEventArgs e)
        {
            Selector selector = (Selector)sender;
            if (selector.SelectedItem == null)
                CultureInfo.CurrentCulture = defaultCulture;
            else
                CultureInfo.CurrentCulture = XmlLanguage.GetLanguage(selector.SelectedItem.ToString()).GetSpecificCulture();
            expressionThread.UpdateTarget();
            expressionParse.UpdateTarget();
        }
    }

    public class DecimalValueVM
    {
        public decimal Number { get; set; }
    }

}
