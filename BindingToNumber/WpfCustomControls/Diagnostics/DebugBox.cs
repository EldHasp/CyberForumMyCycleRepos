using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfCustomControls.Diagnostics
{
    ///// <summary>
    ///// Выполните шаги 1a или 1b, а затем 2, чтобы использовать этот пользовательский элемент управления в файле XAML.
    /////
    ///// Шаг 1a. Использование пользовательского элемента управления в файле XAML, существующем в текущем проекте.
    ///// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    ///// будет использоваться:
    /////
    /////     xmlns:MyNamespace="clr-namespace:BindingToNumberApp"
    /////
    /////
    ///// Шаг 1б. Использование пользовательского элемента управления в файле XAML, существующем в другом проекте.
    ///// Добавьте атрибут XmlNamespace в корневой элемент файла разметки, где он 
    ///// будет использоваться:
    /////
    /////     xmlns:MyNamespace="clr-namespace:BindingToNumberApp;assembly=BindingToNumberApp"
    /////
    ///// Потребуется также добавить ссылку из проекта, в котором находится файл XAML,
    ///// на данный проект и пересобрать во избежание ошибок компиляции:
    /////
    /////     Щелкните правой кнопкой мыши нужный проект в обозревателе решений и выберите
    /////     "Добавить ссылку"->"Проекты"->[Поиск и выбор проекта]
    /////
    /////
    ///// Шаг 2)
    ///// Теперь можно использовать элемент управления в файле XAML.
    /////
    /////     <MyNamespace:DebugBox/>
    /////
    ///// </summary>
    public partial class DebugBox : Control
    {
        static DebugBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DebugBox), new FrameworkPropertyMetadata(typeof(DebugBox)));
        }


        /// <summary>Inline collection filled with strings from Debug.</summary>
        public InlineCollection Inlines
        {
            get => (InlineCollection)GetValue(InlinesProperty);
            protected set => SetValue(InlinesPropertyKey, value);
        }

        // Using a DependencyProperty as the backing store for Inlines.  This enables animation, styling, binding, etc...
        protected static readonly DependencyPropertyKey InlinesPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Inlines), typeof(InlineCollection), typeof(DebugBox), new PropertyMetadata(null, InlinesChanged));
        public static readonly DependencyProperty InlinesProperty = InlinesPropertyKey.DependencyProperty;

        private static void InlinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DebugBox debugBox = (DebugBox)d;
            if (e.NewValue is InlineCollection inlines)
                inlines.Clear();
            //debugBox.CountLines = 0;
            debugBox.DebugAddedText(CallBackTraceListener.DebugFullText);
        }

        /// <summary>Brush for the font of the selected lines.</summary>
        public Brush Highlighted
        {
            get => (Brush)GetValue(HighlightedProperty);
            set => SetValue(HighlightedProperty, value);
        }

        // Using a DependencyProperty as the backing store for Highlighted.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightedProperty =
            DependencyProperty.Register(nameof(Highlighted), typeof(Brush), typeof(DebugBox), new PropertyMetadata(Brushes.Green));



        /// <summary>The interval for the highlighted lines.</summary>
        public int HighlightedInterval
        {
            get => (int)GetValue(HighlightedIntervalProperty);
            set => SetValue(HighlightedIntervalProperty, value);
        }

        // Using a DependencyProperty as the backing store for HighlightedInterval.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightedIntervalProperty =
            DependencyProperty.Register(nameof(HighlightedInterval), typeof(int), typeof(DebugBox), new PropertyMetadata(5));


        /// <summary><see langword="true"/> - text from Debug is outputs,
        /// <see langword="false"/> - not displayed(skipped).</summary>
        public bool IsOutputsText
        {
            get { return (bool)GetValue(IsOutputsTextProperty); }
            set { SetValue(IsOutputsTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsOutputsText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsOutputsTextProperty =
            DependencyProperty.Register(nameof(IsOutputsText), typeof(bool), typeof(DebugBox), new PropertyMetadata(true));


    }

}
