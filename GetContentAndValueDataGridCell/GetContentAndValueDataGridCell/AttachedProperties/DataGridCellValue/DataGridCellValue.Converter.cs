using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CommonCore.AttachedProperties
{
    public partial class DataGridCellValue
    {
        private static partial class Fields
        {
            /// <summary>Приватный мултиконвертер для получения контента ячейки.</summary>
            /// <remarks>Конвертер ожидает массив с элементами:<br/>
            /// 0 - текущий элемент;<br/>
            /// 1 - <see cref="BindingBase"/> <see cref="DataGridBoundColumn.Binding"/> текущего элемента;<br/>
            /// 2 - предок <see cref="DataGridCell"/> текущего элемента;<br/>
            /// 3 - <see cref="BindingBase"/> <see cref="DataGridBoundColumn.Binding"/> предка <see cref="DataGridCell"/> текущего элемента;<br/>
            /// 4 - значение <see cref="CellValueProperty"/> текущего элемента;<br/>
            /// 5 - значение <see cref="CellValueProperty"/> предка <see cref="DataGridCell"/> текущего элемента;<br/>
            /// 6 - значение контекста привязки по умолчанию текущего элемента.
            /// </remarks>
            private class CellValueConverter : IMultiValueConverter
            {
                public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
                {
                    bool isAncestor = false;
                    BindingBase? columnBinding;

                    // Проверка текущего элемента - является ли он DataGridCell.
                    DataGridCell? cell = values[0] as DataGridCell;

                    // Если текущий элемент DataGridCell,
                    // то получение привязки из первого элемента.
                    if (cell is not null)
                    {
                        columnBinding = values[1] as BindingBase;
                    }
                    // Если текущий элемент не DataGridCell,
                    // то получение предка DataGridCell из второго элемента
                    // и привязки из третьего элемента элемента.
                    else
                    {
                        cell = values[2] as DataGridCell;
                        columnBinding = values[3] as BindingBase;
                        isAncestor = true;
                    }

                    // Если не удалось получить DataGridCell,
                    // или если его колонка не DataGridBoundColumn,
                    // или если в колонке не задан Binding,
                    // то возврат неопределёного значения.
                    if (cell is null ||
                        cell.Column is not DataGridBoundColumn ||
                        columnBinding is null)
                        return DependencyProperty.UnsetValue;

                    object value;
                    // Проверка привязки установленой в AP-свойстве CellValueProperty DataGridCell.
                    if (BindingOperations.GetBindingBase(cell, CellValueProperty) != columnBinding)
                    {
                        // Если привязка не такая же как в колонке, то задание этой привязки и выход без установки значения.
                        cell.SetBinding(CellValueProperty, columnBinding);
                        return Binding.DoNothing;
                    }
                    else
                    {
                        // Если привязка не такая же как в колонке, то получение значения свойства.
                        //value = cell.GetValue(CellValueProperty);
                        value = isAncestor ? values[5] : values[4];
                    }

                    // Возврат значение с применением конвертера.
                    return parameter is IValueConverter converter
                        ? converter.Convert(value, targetType, null, culture)
                        : value;
                }

                public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
