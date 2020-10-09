using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;

namespace AppBindingToNumeric
{
    [ContentProperty(nameof(WindItemsSource))]
    [DefaultProperty(nameof(SelectedItem))]
    public class WindItems : OnPropertyChangedClass
    {
        public List<WindItem> WindItemsSource { get; } = new List<WindItem>();

        private WindItem _selectedItem;
        public WindItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                if (SelectedItem != null)
                {
                    SelectedItem.Window.Activated -= WindowActivated;
                    SelectedItem.Window.IsVisibleChanged -= WindowVisibleChanged;
                    SelectedItem.Window.StateChanged -= WindowStateChanged;
                    SelectedItem.Window.Activated += WindowActivated;
                    SelectedItem.Window.IsVisibleChanged += WindowVisibleChanged;
                    SelectedItem.Window.StateChanged += WindowStateChanged;
                    SelectedItem.Window.Show();
                }
            }
        }

        private void WindowStateChanged(object sender, EventArgs e)
        {
            Window window = (Window)sender;
            if (window.WindowState == WindowState.Minimized && sender.Equals(SelectedItem?.Window))
                SelectedItem = null;
        }

        private void WindowVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Window window = (Window)sender;
            if (window.Visibility != Visibility.Visible  && sender.Equals(SelectedItem?.Window))
                SelectedItem = null;
        }


        private void WindowActivated(object sender, EventArgs e)
        {
            SelectedItem = WindItemsSource.Find(item => sender.Equals(item.Window));
        }
    }
}
