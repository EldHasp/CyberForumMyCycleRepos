using System;
using System.Windows;
using System.Windows.Media;

namespace AttachedProperties
{
    public static class Canvas
	{
		public static double GetTop(FrameworkElement element)
		{
			return (double)element.GetValue(TopProperty);
		}

		public static void SetTop(FrameworkElement element, double value)
		{
			element.SetValue(TopProperty, value);
		}

		// Using a DependencyProperty as the backing store for Top.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty TopProperty =
			DependencyProperty.RegisterAttached("Top", typeof(double), typeof(Canvas),
				new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, TopChanged));

		private static void TopChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is FrameworkElement element))
				throw new ArgumentException("Должен быть  FrameworkElement", nameof(d));

			FrameworkElement parent;

			while ((parent = VisualTreeHelper.GetParent(element) as FrameworkElement) != null && !(parent is System.Windows.Controls.Canvas))
				element = parent;

			if (parent != null)
				element.SetValue(System.Windows.Controls.Canvas.TopProperty, (double)e.NewValue);
		}

		public static double GetLeft(FrameworkElement element)
		{
			return (double)element.GetValue(LeftProperty);
		}

		public static void SetLeft(FrameworkElement element, double value)
		{
			element.SetValue(LeftProperty, value);
		}

		// Using a DependencyProperty as the backing store for Left.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty LeftProperty =
			DependencyProperty.RegisterAttached("Left", typeof(double), typeof(Canvas),
				new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, LeftChanged));

		private static void LeftChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is FrameworkElement element))
				throw new ArgumentException("Должен быть  FrameworkElement", nameof(d));

			FrameworkElement parent;

			while ((parent = VisualTreeHelper.GetParent(element) as FrameworkElement) != null && !(parent is System.Windows.Controls.Canvas))
				element = parent;

			if (parent != null)
				element.SetValue(System.Windows.Controls.Canvas.LeftProperty, (double)e.NewValue);
		}


		public static double GetBottom(FrameworkElement element)
		{
			return (double)element.GetValue(BottomProperty);
		}

		public static void SetBottom(FrameworkElement element, double value)
		{
			element.SetValue(BottomProperty, value);
		}

		// Using a DependencyProperty as the backing store for Bottom.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty BottomProperty =
			DependencyProperty.RegisterAttached("Bottom", typeof(double), typeof(Canvas),
				new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, BottomChanged));

		private static void BottomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is FrameworkElement element))
				throw new ArgumentException("Должен быть  FrameworkElement", nameof(d));

			FrameworkElement parent;

			while ((parent = VisualTreeHelper.GetParent(element) as FrameworkElement) != null && !(parent is System.Windows.Controls.Canvas))
				element = parent;

			if (parent != null)
				element.SetValue(System.Windows.Controls.Canvas.BottomProperty, (double)e.NewValue);
		}

		public static double GetRight(FrameworkElement element)
		{
			return (double)element.GetValue(RightProperty);
		}

		public static void SetRight(FrameworkElement element, double value)
		{
			element.SetValue(RightProperty, value);
		}

		// Using a DependencyProperty as the backing store for Right.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty RightProperty =
			DependencyProperty.RegisterAttached("Right", typeof(double), typeof(Canvas),
				new FrameworkPropertyMetadata(double.NaN, FrameworkPropertyMetadataOptions.AffectsParentArrange | FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RightChanged));

		private static void RightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is FrameworkElement element))
				throw new ArgumentException("Должен быть  FrameworkElement", nameof(d));

			FrameworkElement parent;

			while ((parent = VisualTreeHelper.GetParent(element) as FrameworkElement) != null && !(parent is System.Windows.Controls.Canvas))
				element = parent;

			if (parent != null)
				element.SetValue(System.Windows.Controls.Canvas.RightProperty, (double)e.NewValue);
		}
	}
}
