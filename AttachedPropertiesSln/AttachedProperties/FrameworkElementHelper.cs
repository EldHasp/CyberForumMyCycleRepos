using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace AttachedProperties
{
    /// <summary>Attached Properties for FrameworkElement</summary>
    public static class FrameworkElementHelper
    {
        /// <summary>Получение заданной пропорции отношения ширины к высоте элемента.
        /// Задаётся коэффициент для получения размера ширины из высоты: Width = Height * WidthToHeight.</summary>
        /// <param name="element">FrameworkElement чьи размеры должны быть пропорциональны.</param>
        public static double GetWidthToHeight(FrameworkElement element)
        {
            return (double)element.GetValue(WidthToHeightProperty);
        }

        /// <summary>Задание пропорции отношения ширины к высоте элемента.
        /// Задаётся коэффициент для получения размера ширины из высоты: Width = Height * WidthToHeight.</summary>
        /// <param name="element">FrameworkElement чьи размеры должны быть пропорциональны.</param>
        /// <param name="widthToHeight">Коэфициент пропорции:  Width = Height * WidthToHeight.</param>
        /// <remarks>Отслеживается изменение размера контейнера предоставленного элемента.
        /// При его изменении определяется максимально возможный размер элемента который может
        /// быть достигнут в контейнере при соблюдении заданных пропорций.</remarks>
        public static void SetWidthToHeight(FrameworkElement element, double widthToHeight)
        {
            element.SetValue(WidthToHeightProperty, widthToHeight);
        }

        // Using a DependencyProperty as the backing store for Proportionate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WidthToHeightProperty =
            DependencyProperty.RegisterAttached("WidthToHeight", typeof(double), typeof(FrameworkElementHelper), new PropertyMetadata(-1.0, ProportionalChanged));

        /// <summary>Метод обратного вызова после изменения значения свойства.</summary>
        /// <param name="d">FrameworkElement иначе исключение.</param>
        /// <param name="e">Параметры изменения.</param>
        private static void ProportionalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
                throw new ArgumentException("Must be a FrameworkElement");

            // Получение элемента сохраняющего пропорции.
            if (!sizeChangeds.TryGetValue(element, out SizeChangedElement changedElement))
            {
                // Если элемент не задан, то его создание и сохранение.
                changedElement = new SizeChangedElement(element);
                sizeChangeds.Add(element, changedElement);
            }

            // Передача нового значения пропорции.
            changedElement.SetWidthToHeight((double)e.NewValue);
        }

        private static readonly ConditionalWeakTable<FrameworkElement, SizeChangedElement> sizeChangeds
            = new ConditionalWeakTable<FrameworkElement, SizeChangedElement>();


        /// <summary>Вспомогательный приватный класс отслеживающий изменения размера элемента.</summary>
        private class SizeChangedElement
        {
            /// <summary>FrameworkElement. Не может быть null.</summary>
            public FrameworkElement Element { get; }

            /// <summary>Задётся коэфициент для получения размера ширины из высоты: Width = Height * WidthToHeight.
            /// Если равен или меньше нуля, то соблюдение пропорции не производится.</summary>
            public double WidthToHeight { get; private set; } = 1;

            /// <summary>Конструктор принимающий элемент чьи пропорции не должны меняться.</summary>
            /// <param name="element">FrameworkElement. Не может быть null.</param>
            public SizeChangedElement(FrameworkElement element)
            {
                Element = element ?? throw new ArgumentNullException(nameof(element));

                /// Событие происходит при изменении выделяемого для элемента места.
                element.LayoutUpdated += Element_LayoutUpdated;
            }

            /// <summary>Обработчик вызываемый при изменении выделяемого для элемента места.</summary>
            /// <param name="sender"><see langword="null"/>.</param>
            /// <param name="e">Empty or <see langword="null"/>.</param>
            private void Element_LayoutUpdated(object sender = null, EventArgs e = null)
            {
                if (WidthToHeight <= 0)
                    return;

                // Получение информации о выделенном месте.
                Rect rect = LayoutInformation.GetLayoutSlot(Element);

                // Размеры выделенной области с учётом Margin элемента.
                double widthArea = rect.Width - Element.Margin.Left - Element.Margin.Right;
                double heightArea = rect.Height - Element.Margin.Top - Element.Margin.Bottom;

                double width = widthArea;
                double height = width / WidthToHeight;
                if (height > heightArea)
                {
                    height = heightArea;
                    width = height * WidthToHeight;
                }
                Element.Width = width > 0.0 ? width : 0.0;
                Element.Height = height > 0.0 ? height : 0.0;
            }

            /// <summary>Задание коэффициента пропорции ширины к высоте.</summary>
            /// <param name="widthToHeight">Коэффициент пропорции ширины к высоте: Width = Height * WidthToHeight.</param>
            public void SetWidthToHeight(double widthToHeight)
            {
                WidthToHeight = widthToHeight;
                Element_LayoutUpdated();
            }
        }
    }
}
