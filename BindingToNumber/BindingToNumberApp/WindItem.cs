using System.Windows;
using System.Windows.Markup;

namespace AppBindingToNumeric
{
    [ContentProperty(nameof(Window))]
    public class WindItem
    {
        public string Display { get; set; }

        private Window _window;
        public Window Window
        {
            get => _window;
            set
            {
                if (Window != null)
                    Window.Closing -= WindowNotClosing;
                _window = value;
                if (Window != null)
                    Window.Closing += WindowNotClosing;
            }
        }

        private static void WindowNotClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            ((Window)sender).Hide();
        }
    }
}
