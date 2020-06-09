using System;
using System.Windows;
using System.Windows.Media;

namespace AttachedProperties
{
    public static partial class Grid
    {
        public static int GetRow(FrameworkElement element)
        {
            return (int)element.GetValue(RowProperty);
        }

        public static void SetRow(FrameworkElement element, int value)
        {
            element.SetValue(RowProperty, value);
        }

        // Using a DependencyProperty as the backing store for Row.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RowProperty =
            DependencyProperty.RegisterAttached("Row", typeof(int), typeof(Grid),
                new PropertyMetadata(0, RowChanged));

        private static void RowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
                throw new ArgumentException("Должен быть  FrameworkElement", nameof(d));

            FrameworkElement parent;

            while ((parent = VisualTreeHelper.GetParent(element) as FrameworkElement) != null && !(parent is System.Windows.Controls.Grid))
                element = parent;

            if (parent is System.Windows.Controls.Grid grid)
                element.Dispatcher.BeginInvoke((Action<FrameworkElement, DependencyProperty, object>)SetValueAsync, element, System.Windows.Controls.Grid.RowProperty, (int)e.NewValue);
        }

        private static void SetValueAsync(FrameworkElement element, DependencyProperty property, object value)
            => element.SetValue(property, value);

        public static int GetColumn(FrameworkElement element)
        {
            return (int)element.GetValue(ColumnProperty);
        }

        public static void SetColumn(FrameworkElement element, int value)
        {
            element.SetValue(ColumnProperty, value);
        }

        // Using a DependencyProperty as the backing store for Column.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColumnProperty =
            DependencyProperty.RegisterAttached("Column", typeof(int), typeof(Grid),
                new PropertyMetadata(0, ColumnChanged));

        private static void ColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
                throw new ArgumentException("Должен быть  FrameworkElement", nameof(d));

            FrameworkElement parent;

            while ((parent = VisualTreeHelper.GetParent(element) as FrameworkElement) != null && !(parent is System.Windows.Controls.Grid))
                element = parent;

            if (parent is System.Windows.Controls.Grid grid)
                element.Dispatcher.BeginInvoke((Action<FrameworkElement, DependencyProperty, object>)SetValueAsync, element, System.Windows.Controls.Grid.ColumnProperty, (int)e.NewValue);
        }
    }
}
