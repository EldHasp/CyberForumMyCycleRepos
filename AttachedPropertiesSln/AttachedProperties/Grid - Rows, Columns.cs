using System;
using System.Windows;
using System.Windows.Controls;

namespace AttachedProperties
{
	/// <summary>Attached Properties for Grid</summary>
	public static partial class Grid
	{
		/// <summary>Возвращает заданное значение количества строк.
		/// Оно может отличаться от реального значения, 
		/// если коллекция строк изменялась иным образом.</summary>
		/// <param name="grid">Grid в котором было задано количество строк.</param>
		/// <returns>Заданное значение.</returns>
		public static int GetRows(System.Windows.Controls.Grid grid)
		{
			return (int)grid.GetValue(RowsProperty);
		}

		/// <summary>Задать количество строк Grid.</summary>
		/// <param name="grid">Grid в котором задаётся количество строк.</param>
		/// <param name="value">Количестово строк.</param>
		public static void SetRows(System.Windows.Controls.Grid grid, int value)
		{
			grid.SetValue(RowsProperty, value);
		}

		// Using a DependencyProperty as the backing store for Rows.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty RowsProperty =
			DependencyProperty.RegisterAttached("Rows", typeof(int), typeof(Grid), new PropertyMetadata(0, ChangeRows));

		/// <summary>Метод задаюший количество строк в Grid при изменении Rows.</summary>
		/// <param name="d">Объект к которому присоединенно свойство. Исключение если не Grid.</param>
		/// <param name="e">Параметры изменения.</param>
		private static void ChangeRows(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Приведение к Grid.
			if (!(d is System.Windows.Controls.Grid grid))
				throw new ArgumentException("Must be a Grid", nameof(d));

			// Приведение к int.
			int rows = (int)e.NewValue;

			// Добавление строк, если их не хватает.
			for (int i = grid.RowDefinitions.Count; i < rows; i++)
				grid.RowDefinitions.Add(new RowDefinition());

			// Удаление строк, если их больше.
			for (int i = grid.RowDefinitions.Count; i > rows;)
				grid.RowDefinitions.RemoveAt(--i);
		}



		/// <summary>Возвращает заданное значение количества колонок.
		/// Оно может отличаться от реального значения, 
		/// если коллекция колонок изменялась иным образом.</summary>
		/// <param name="grid">Grid в котором было задано количество колонок.</param>
		/// <returns>Заданное значение.</returns>
		public static int GetColumns(System.Windows.Controls.Grid grid)
		{
			return (int)grid.GetValue(ColumnsProperty);
		}

		/// <summary>Задать количество колонок Grid.</summary>
		/// <param name="grid">Grid в котором задаётся количество колонок.</param>
		/// <param name="value">Количестово колонок.</param>
		public static void SetColumns(DependencyObject obj, int value)
		{
			obj.SetValue(ColumnsProperty, value);
		}

		// Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ColumnsProperty =
			DependencyProperty.RegisterAttached("Columns", typeof(int), typeof(Grid), new PropertyMetadata(0, ChangeColumns));

		/// <summary>Метод задаюший количество колонок в Grid при изменении Columns.</summary>
		/// <param name="d">Объект к которому присоединенно свойство. Исключение если не Grid.</param>
		/// <param name="e">Параметры изменения.</param>
		private static void ChangeColumns(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			// Приведение к Grid.
			if (!(d is System.Windows.Controls.Grid grid))
				throw new ArgumentException("Должен быть Grid", nameof(d));

			// Приведение к int.
			int columns = (int)e.NewValue;

			// Добавление строк, если их не хватает.
			for (int i = grid.ColumnDefinitions.Count; i < columns; i++)
				grid.ColumnDefinitions.Add(new ColumnDefinition());

			// Удаление строк, если их больше.
			for (int i = grid.ColumnDefinitions.Count; i > columns;)
				grid.ColumnDefinitions.RemoveAt(--i);
		}
	}
}
