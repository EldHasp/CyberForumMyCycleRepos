using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;

namespace WpfCommands.Bindings
{
    public class CommandBindingData : System.Windows.Input.CommandBinding
    {
        private Binding _bind;

        [ContentProperty(nameof(Command))]
        [DefaultProperty(nameof(Command))]
        public class ProxyBinding : Freezable, ICreateProxyBinding
        {

            public System.Windows.Input.CommandBinding Parent { get; }

            private ProxyBinding(System.Windows.Input.CommandBinding parent)
            {
                if (parent == null)
                    return;
                Parent = parent;

                Parent.Executed += OnExecuted;
                Parent.CanExecute += OnExecute;
            }

            private void OnExecuted(object sender, ExecutedRoutedEventArgs e)
            {
                Command?.Execute(e.Parameter);
                e.Handled = Handled;
            }

            private void OnExecute(object sender, CanExecuteRoutedEventArgs e)
            {
                e.CanExecute = Command != null && Command.CanExecute(e.Parameter);
                e.Handled = Handled;
            }

            public ICommand Command
            {
                get { return (ICommand)GetValue(CommandProperty); }
                set { SetValue(CommandProperty, value); }
            }

            // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty CommandProperty =
                DependencyProperty.Register(nameof(Command), typeof(ICommand), typeof(ProxyBinding), new PropertyMetadata(null));




            public bool Handled
            {
                get { return (bool)GetValue(HandledProperty); }
                set { SetValue(HandledProperty, value); }
            }

            // Using a DependencyProperty as the backing store for Handled.  This enables animation, styling, binding, etc...
            public static readonly DependencyProperty HandledProperty =
                DependencyProperty.Register(nameof(Handled), typeof(bool), typeof(ProxyBinding), new PropertyMetadata(true));

            protected override Freezable CreateInstanceCore() => throw new NotImplementedException();

            ProxyBinding ICreateProxyBinding.CreateInstance(System.Windows.Input.CommandBinding parent) => new ProxyBinding(parent);

            public static ProxyBinding Empty { get; } = new ProxyBinding(null);
        }

        private interface ICreateProxyBinding
        {
            ProxyBinding CreateInstance(System.Windows.Input.CommandBinding parent);
        }

        public ProxyBinding Proxy { get; }
        public CommandBindingData()
        {
            Proxy = ((ICreateProxyBinding)ProxyBinding.Empty).CreateInstance(this);
        }

        public Binding Binding
        {
            get => _bind;
            set { _bind = value; BindingOperations.SetBinding(Proxy, ProxyBinding.CommandProperty, Binding); }
        }

        public bool Handled
        {
            get => Proxy.Handled;
            set { Proxy.Handled = value;BindingOperations.SetBinding(Proxy, ProxyBinding.HandledProperty, Binding); }
        }

    }

    //public class Binding : BindingExtension
    //{
    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //      var obj =  serviceProvider.GetService(typeof(IProvideValueTarget));

    //        throw new NotImplementedException();
    //    }
    //}
}
