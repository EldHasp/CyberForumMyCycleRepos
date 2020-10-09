using BindingStringToNumeric;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppBindingToNumeric
{
    /// <summary>
    /// Логика взаимодействия для ExampleWindow.xaml
    /// </summary>
    public partial class ExampleWindow : Window
    {
        public ExampleWindow()
        {
            InitializeComponent();
        }
        //<TextBox x:Name="tbInteger" Margin="5">
        //    <TextBox.Text>
        //        <MultiBinding Converter = "{StaticResource NumericConverter}" UpdateSourceTrigger="PropertyChanged">
        //            <Binding Path = "IntegerValue" />
        //            <Binding Path="Text" ElementName="tbInteger" Mode="TwoWay"/>
        //        </MultiBinding>
        //    </TextBox.Text>
        //</TextBox>

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            Binding bindingTextBox = new Binding(nameof(TextBox.Text)) { ElementName = nameof(tbInteger), Mode = BindingMode.OneWay };
            Binding bindingNumeric = new Binding(nameof(Numbers.IntegerValue));
            MultiBinding multi = new MultiBinding() { Converter = NumericConverter.Instance, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };

            multi.Bindings.Add(bindingNumeric);
            multi.Bindings.Add(bindingTextBox);

            tbInteger.SetBinding(TextBox.TextProperty, multi);
        }
    }

    //            <TextBox.Text>
    //                <MultiBinding Converter = "{x:Static bnd:NumericConverter.Instance}" UpdateSourceTrigger="PropertyChanged">
    //                    <Binding Path = "DecimalValue" />
    //                    < Binding Path="Text" ElementName="tbDecimal" Mode="OneTime"/>
    //                </MultiBinding>
    //            </TextBox.Text>

    public class ExampleExtension : MarkupExtension
    {
        public PropertyPath Path { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var providerValuetarget = (IProvideValueTarget)serviceProvider
                  .GetService(typeof(IProvideValueTarget));

            //Получим TextBox, вызвавший привязку
            TextBox textBox = providerValuetarget.TargetObject as TextBox;
            DependencyProperty targetProperty = providerValuetarget.TargetProperty as DependencyProperty;

            if (textBox == null || targetProperty != TextBox.TextProperty)
                throw new Exception("Можно использовать только в привязке свойства TextBox.Text");

            Binding bindingTextBox = new Binding(nameof(TextBox.Text)) { Source = textBox, Mode = BindingMode.OneWay };
            Binding bindingNumeric = new Binding() { Path = Path };
            PrivateBinding multi = new PrivateBinding() { Converter = NumericConverter.Instance, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };

            multi.Bindings.Add(bindingNumeric);
            multi.Bindings.Add(bindingTextBox);

            return multi.ProvideValue(serviceProvider);

        }

        public ExampleExtension()
        {
        }

        public ExampleExtension(string path)
        {
            Path = new PropertyPath(path);
        }

        private class PrivateBinding : MultiBinding
        {

        }
    }
}
