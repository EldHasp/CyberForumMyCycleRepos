using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace WpfCustomControls.Diagnostics
{
    public partial class DebugBox
    {
        /// <summary>Gets the value of the DebugBox.ParentBox attached property for the specified <see cref="TextBlock"/>.</summary>
        /// <param name="textBlock">The <see cref="TextBlock"/> from which to read the property value.</param>
        /// <returns>DebugBox.ParentBox attached property value.</returns>
        public static DebugBox GetParentBox(TextBlock textBlock)
        {
            return (DebugBox)textBlock.GetValue(ParentBoxProperty);
        }

        /// <summary>Sets the value of the DebugBox.ParentBox attached property for the specified <see cref="TextBlock"/>.</summary>
        /// <param name="textBlock">The <see cref="TextBlock"/> for which you want to set the attached property.</param>
        /// <param name="value">Parent <see cref="DebugBox"/> with which the <see cref="TextBlock"/> should have an associated <see cref="TextBlock.Inlines"/> collection.</param>
        public static void SetParentBox(TextBlock textBlock, DebugBox value)
        {
            textBlock.SetValue(ParentBoxProperty, value);
        }

        /// <summary>Using a DependencyProperty as the backing store for ParentBox.  This enables animation, styling, binding, etc...</summary>
        public static readonly DependencyProperty ParentBoxProperty =
            DependencyProperty.RegisterAttached("ParentBox", typeof(DebugBox), typeof(DebugBox),
                new PropertyMetadata(null, ParentBoxChanged));

        /// <summary>Private method that binds the <see cref="Inlines"/> property of the parent <see cref="DebugBox"/> to the <see cref="TextBlock.Inlines"/> of the <see cref="TextBlock"/>.</summary>
        /// <param name="d"><see cref="TextBlock"/>.</param>
        /// <param name="e">Parameters the changes.</param>
        private static void ParentBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldBox = (DebugBox)e.OldValue;
            var newBox = (DebugBox)e.NewValue;
            if (d is TextBlock textBlock)
            {
                if (oldBox != null) oldBox.Inlines = null;
                if (newBox != null) newBox.Inlines = textBlock.Inlines;
            }
        }


        /// <summary>Gets the value of the DebugBox.NestedTextBlock attached property for the specified <see cref="ScrollViewer"/>.</summary>
        /// <param name="textBlock">The <see cref="ScrollViewer"/> from which to read the property value.</param>
        /// <returns>DebugBox.NestedTextBlock attached property value.</returns>
        public static TextBlock GetNestedTextBlock(ScrollViewer scroll)
        {
            return (TextBlock)scroll.GetValue(NestedTextBlockProperty);
        }

        /// <summary>Sets the value of the DebugBox.NestedTextBlock attached property for the specified <see cref="ScrollViewer"/>.</summary>
        /// <param name="textBlock">The <see cref="ScrollViewer"/> for which you want to set the attached property.</param>
        /// <param name="value">The <see cref="TextBlock"/> is nested within the <see cref="ScrollViewer"/>.
        ///  When the <see cref="TextBlock"/> is resized, the <see cref="ScrollViewer"/> automatically scrolls bottom.</param>
        public static void SetNestedTextBlock(ScrollViewer scroll, TextBlock value)
        {
            scroll.SetValue(NestedTextBlockProperty, value);
        }

        /// <summary>Using a DependencyProperty as the backing store for NestedTextBlock.  This enables animation, styling, binding, etc...</summary>
        public static readonly DependencyProperty NestedTextBlockProperty =
            DependencyProperty.RegisterAttached("NestedTextBlock", typeof(TextBlock), typeof(DebugBox),
                new PropertyMetadata(null, NestedTextBlockChanged));

        private static void NestedTextBlockChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var oldTblock = (TextBlock)e.OldValue;
            var newTblock = (TextBlock)e.NewValue;
            if (d is ScrollViewer scroll)
            {
                RemoveScroll(oldTblock, scroll);

                scroll.Loaded -= ScrollLoaded;
                scroll.Unloaded -= ScrollUnloaded;
                scroll.Loaded += ScrollLoaded;
                scroll.Unloaded += ScrollUnloaded;

                if (scroll.IsLoaded) AddScroll(newTblock, scroll);
            }
            else
                throw new ArgumentException("Cannot be used for this type of controls", nameof(d));
        }

        private static void ScrollUnloaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer scroll = (ScrollViewer)sender;
            RemoveScroll(GetNestedTextBlock(scroll), scroll);
        }

        private static void ScrollLoaded(object sender, RoutedEventArgs e)
        {
            ScrollViewer scroll = (ScrollViewer)sender;
            AddScroll(GetNestedTextBlock(scroll), scroll);
        }

        private static readonly Dictionary<TextBlock, List<ScrollViewer>> scrolls = new Dictionary<TextBlock, List<ScrollViewer>>();
        private static void AddScroll(TextBlock textBlock, ScrollViewer scroll)
        {
            if (textBlock == null)
                return;
            if (scrolls.TryGetValue(textBlock, out List<ScrollViewer> list))
                list.Add(scroll);
            else
                scrolls.Add(textBlock, new List<ScrollViewer>() { scroll });

            textBlock.SizeChanged -= TextBlockSizeChanged;
            textBlock.SizeChanged += TextBlockSizeChanged;
            TextBlockSizeChanged(textBlock, null);
        }

        private static void TextBlockSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (scrolls.TryGetValue((TextBlock)sender, out List<ScrollViewer> list))
                list.ForEach(scr => scr.ScrollToEnd());
        }

        private static void RemoveScroll(TextBlock textBlock, ScrollViewer scroll)
        {
            if (textBlock == null)
                return;

            if (scrolls.TryGetValue(textBlock, out List<ScrollViewer> list))
            {
                list.Remove(scroll);
                if (list.Count == 0)
                {
                    scrolls.Remove(textBlock);
                    textBlock.SizeChanged -= TextBlockSizeChanged;
                }
            }

        }

    }

}
