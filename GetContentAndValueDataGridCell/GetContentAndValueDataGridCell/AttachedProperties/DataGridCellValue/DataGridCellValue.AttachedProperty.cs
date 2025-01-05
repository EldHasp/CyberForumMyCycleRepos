using System;
using System.Windows;
using System.Windows.Controls;

namespace CommonCore.AttachedProperties
{
    public partial class DataGridCellValue
    {
        /// <summary>Возвращает значение присоединённого свойства CellValue для <paramref name="cell"/>.</summary>
        /// <param name="cell"><see cref="DataGridCell"/> значение свойства которого будет возвращено.</param>
        /// <returns><see cref="object"/> значение свойства.</returns>
        public static object GetCellValue(DataGridCell cell)
        {
            return cell.GetValue(CellValueProperty);
        }

        /// <summary>Задаёт значение присоединённого свойства CellValue для <paramref name="cell"/>.</summary>
        /// <param name="cell"><see cref="DataGridCell"/> значение свойства которого будет возвращено.</param>
        /// <param name="value"><see cref="object"/> значение для свойства.</param>
        public static void SetCellValue(DataGridCell cell, object value)
        {
            cell.SetValue(CellValueProperty, value);
        }

        /// <summary><see cref="DependencyProperty"/> для методов <see cref="GetCellValue(DataGridCell)"/> и <see cref="SetCellValue(DataGridCell, object)"/>.</summary>
        public static readonly DependencyProperty CellValueProperty =
            DependencyProperty.RegisterAttached(
                nameof(GetCellValue).Substring(3),
                typeof(object),
                typeof(DataGridCellValue),
                new PropertyMetadata(null, (d, _) => { if (d is not DataGridCell) throw new NotImplementedException("Реализовано только для DataGridCell"); }));
    }
}
