using Proxy;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace ListeningDependecyProperties
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void OnSubscribe(object sender, RoutedEventArgs e)
        {
            // Получение дескриптора свойства.
            PropertyDescriptor? descriptorActualWidth = TypeDescriptor.GetProperties(border).Find(FrameworkElement.ActualWidthProperty.Name, false);
            PropertyDescriptor? descriptorColumn = TypeDescriptor.GetProperties(border).Find(Grid.ColumnProperty.Name, false); // Для AP-свойств не работает.

            // Подсоединение прослушики (типа EventHandler) свойства у нужного объекта.
            descriptorActualWidth?.AddValueChanged(border, WidthListening);
            descriptorColumn?.AddValueChanged(border, ColumnListening);

            // Получение дескриптора свойства зависимости.
            DependencyPropertyDescriptor descriptorDpActualWidth = DependencyPropertyDescriptor.FromProperty(FrameworkElement.ActualWidthProperty, typeof(FrameworkElement));
            DependencyPropertyDescriptor descriptorDpColumn = DependencyPropertyDescriptor.FromProperty(Grid.ColumnProperty, typeof(FrameworkElement)); // Для AP-свойств работает.

            // Подсоединение прослушики (типа EventHandler) свойства у нужного объекта.
            descriptorDpActualWidth.AddValueChanged(border, WidthDListening);
            descriptorDpColumn.AddValueChanged(border, ColumnDListening);

            // Прослушка с использование хелпера.
            border.AddPropertyChanged(FrameworkElement.ActualWidthProperty, PropertyListening);
            border.AddPropertyChanged(Grid.ColumnProperty, PropertyListening);

            StringProxy proxy = (StringProxy)Resources["proxy"];
            proxy.ValueChanged  += OnValueChanged;
        }

        private void OnUnsubscribe(object sender, RoutedEventArgs e)
        {
            // Получение дескриптора свойства.
            PropertyDescriptor? descriptorActualWidth = TypeDescriptor.GetProperties(border).Find(FrameworkElement.ActualWidthProperty.Name, false);
            PropertyDescriptor? descriptorColumn = TypeDescriptor.GetProperties(border).Find(Grid.ColumnProperty.Name, false); // Для AP-свойств не работает.

            // Отсоединение прослушки (типа EventHandler) свойства у нужного объекта.
            descriptorActualWidth?.RemoveValueChanged(border, WidthListening);
            descriptorColumn?.RemoveValueChanged(border, ColumnListening);

            // Получение дескриптора свойства зависимости.
            DependencyPropertyDescriptor descriptorDpActualWidth = DependencyPropertyDescriptor.FromProperty(FrameworkElement.ActualWidthProperty, typeof(FrameworkElement));
            DependencyPropertyDescriptor descriptorDpColumn = DependencyPropertyDescriptor.FromProperty(Grid.ColumnProperty, typeof(FrameworkElement)); // Для AP-свойств работает.

            // Подсоединение прослушики (типа EventHandler) свойства у нужного объекта.
            descriptorDpActualWidth.RemoveValueChanged(border, WidthDListening);
            descriptorDpColumn.RemoveValueChanged(border, ColumnDListening);

            // Прослушка с использование хелпера.
            border.RemovePropertyChanged(FrameworkElement.ActualWidthProperty, PropertyListening);
            border.RemovePropertyChanged(Grid.ColumnProperty, PropertyListening);

            StringProxy proxy = (StringProxy)Resources["proxy"];
            proxy.ValueChanged -= OnValueChanged;
        }

        /// <summary>Метод-Прослушки.</summary>
        /// <param name="sender">Источник события.</param>
        /// <param name="e">Всегда <see cref="EventArgs.Empty"/>.</param>
        /// <remarks>В метод передаётся только источник события.
        /// Если у объекта нужно прослушивать несколько свойств и реагировать на их изменение по разному,
        /// то на каждое свойство нужен свой метод-прослушки,
        /// так как по параметрам метода невозможно выяснить изменение какого свойства его вызывало.</remarks>
        private void ColumnListening(object? sender, EventArgs e)
        {
            colTBlock.Text = $"Колонка {Grid.GetColumn(border)}";
        }

        private void WidthListening(object? sender, EventArgs e)
        {
            widthTBlock.Text = $"Ширина {border.ActualWidth}";
        }
        private void ColumnDListening(object? sender, EventArgs e)
        {
            colDTBlock.Text = $"Колонка {Grid.GetColumn(border)}";
        }

        private void WidthDListening(object? sender, EventArgs e)
        {
            widthDTBlock.Text = $"Ширина {border.ActualWidth}";
        }

        /// <summary>Можно создать общий метод прослушки для всех объектов и свойств.</summary>
        /// <param name="sender">Объект в котором изменилось свойство.</param>
        /// <param name="args">Аргументы изменения.</param>
        private void PropertyListening(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            if (args.Property == FrameworkElement.ActualWidthProperty)
            {
                widthHDTBlock.Text = $"Ширина {border.ActualWidth}";
            }
            else if (args.Property == Grid.ColumnProperty)
            {
                colHDTBlock.Text = $"Колонка {Grid.GetColumn(border)}";
            }
        }

        private void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            string[]? split = args.NewValue.ToString()?.Split(';');
            widthPTBlock.Text = $"Ширина {split[0]}";
            colPTBlock.Text = $"Колонка {split[1]}";
        }
    }
}
