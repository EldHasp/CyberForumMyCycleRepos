using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CommonCore.AttachedProperties
{
    public partial class DataGridCellContent
    {
        private static partial class Fields
        {
            /// <summary>Приватный мултиконвертер для получения контента ячейки.</summary>
            /// <remarks>Конвертер ожидает массив с элементами:<br/>
            /// 0 - текущий элемент;<br/>
            /// 1 - колонка <see cref="DataGridCell.Column"/> текущего элемента;<br/>
            /// 2 - колонка <see cref="DataGridCell.Column"/> предка текущего элемента;<br/>
            /// 3 - строка <see cref="DataGridRow"/> текущего элемента.<br/></remarks>
            private class CellContentConverter : IMultiValueConverter
            {
                public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
                {
                    // Получение колонки
                    DataGridColumn? column;

                    // Если текущий элемент это DataGridCell, то колонка содержится в первой привязке
                    if (values[0] is DataGridCell)
                    {
                        column = values[1] as DataGridColumn;
                    }
                    // Если текущий элемент это не DataGridCell, то колонка содержится в второй привязке
                    else
                    {
                        column = values[2] as DataGridColumn;
                    }

                    // Если колонку получить не удалось, то выход с неопределённым значением.
                    if (column is null)
                    {
                        return DependencyProperty.UnsetValue;
                    }


                    // Если строку получить не удалось, то выход с неопределённым значением.
                    if (values[3] is not DataGridRow row)
                    {
                        return DependencyProperty.UnsetValue;
                    }

                    // Возврат контента пполученных колонки и строки.
                    object value = column.GetCellContent(row);

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
