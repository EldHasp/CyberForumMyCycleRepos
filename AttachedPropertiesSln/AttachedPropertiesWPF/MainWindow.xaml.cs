using AttachedProperties;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AttachedPropertiesWPF
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}
	}

}
//namespace AttachedProperties
//{
//	public static class Uniform
//	{
//		/// <summary>Возвращает заданное значение количества строк.
//		/// Оно может отличаться от реального значения, 
//		/// если коллекция строк изменялась иным образом.</summary>
//		/// <param name="grid">Grid в котором было задано количество строк.</param>
//		/// <returns>Заданное значение.</returns>
//		public static int GetRows(Grid grid)
//		{
//			return (int)grid.GetValue(RowsProperty);
//		}

//		/// <summary>Задать количество строк Grid.</summary>
//		/// <param name="grid">Grid в котором задаётся количество строк.</param>
//		/// <param name="value">Количестово строк.</param>
//		public static void SetRows(Grid grid, int value)
//		{
//			grid.SetValue(RowsProperty, value);
//		}

//		// Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
//		public static readonly DependencyProperty RowsProperty =
//			DependencyProperty.RegisterAttached("Rows", typeof(int), typeof(Grid), new PropertyMetadata(0, ChangeRows));

//		/// <summary>Метод задаюший количество строк в Grid при изменении Rows.</summary>
//		/// <param name="d">Объект к которому присоединенно свойство. Исключение если не Grid.</param>
//		/// <param name="e">Параметры изменения.</param>
//		private static void ChangeRows(DependencyObject d, DependencyPropertyChangedEventArgs e)
//		{
//			// Приведение к Grid.
//			if (!(d is Grid grid))
//				throw new ArgumentException("Должен быть Grid", nameof(d));

//			// Приведение к int.
//			int rows = (int)e.NewValue;

//			// Добавление строк, если их не хватает.
//			for (int i = grid.RowDefinitions.Count; i < rows; i++)
//				grid.RowDefinitions.Add(new RowDefinition());

//			// Удаление строк, если их больше.
//			for (int i = grid.RowDefinitions.Count; i > rows;)
//				grid.RowDefinitions.RemoveAt(--i);
//		}



//		/// <summary>Возвращает заданное значение количества колонок.
//		/// Оно может отличаться от реального значения, 
//		/// если коллекция колонок изменялась иным образом.</summary>
//		/// <param name="grid">Grid в котором было задано количество колонок.</param>
//		/// <returns>Заданное значение.</returns>
//		public static int GetColumns(Grid grid)
//		{
//			return (int)grid.GetValue(ColumnsProperty);
//		}

//		/// <summary>Задать количество колонок Grid.</summary>
//		/// <param name="grid">Grid в котором задаётся количество колонок.</param>
//		/// <param name="value">Количестово колонок.</param>
//		public static void SetColumns(DependencyObject obj, int value)
//		{
//			obj.SetValue(ColumnsProperty, value);
//		}

//		// Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
//		public static readonly DependencyProperty ColumnsProperty =
//			DependencyProperty.RegisterAttached("Columns", typeof(int), typeof(Uniform), new PropertyMetadata(0, ChangeColumns));

//		/// <summary>Метод задаюший количество колонок в Grid при изменении Columns.</summary>
//		/// <param name="d">Объект к которому присоединенно свойство. Исключение если не Grid.</param>
//		/// <param name="e">Параметры изменения.</param>
//		private static void ChangeColumns(DependencyObject d, DependencyPropertyChangedEventArgs e)
//		{
//			// Приведение к Grid.
//			if (!(d is Grid grid))
//				throw new ArgumentException("Должен быть Grid", nameof(d));

//			// Приведение к int.
//			int columns = (int)e.NewValue;

//			// Добавление строк, если их не хватает.
//			for (int i = grid.ColumnDefinitions.Count; i < columns; i++)
//				grid.ColumnDefinitions.Add(new ColumnDefinition());

//			// Удаление строк, если их больше.
//			for (int i = grid.ColumnDefinitions.Count; i > columns;)
//				grid.ColumnDefinitions.RemoveAt(--i);
//		}
//	}
//}

