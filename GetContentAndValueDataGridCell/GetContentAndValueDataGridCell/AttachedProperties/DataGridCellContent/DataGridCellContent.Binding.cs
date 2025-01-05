using System.Windows.Controls;
using System.Windows.Data;

namespace CommonCore.AttachedProperties
{
    /// <summary>Расширение разметки от <see cref="MultiBinding"/>,
    /// создающее привязку к контенту ячейки <see cref="DataGridCell"/>.</summary>
    /// <remarks>Для получения контента используется метод <see cref="DataGridColumn.GetCellContent(DataGridRow)"/>.
    /// Для получения колонки происходит обращение к свойству <see cref="DataGridCell.Column"/>.
    /// Ячейка <see cref="DataGridCell"/> определяется либо как текущий объект <see cref="RelativeSource.Self"/>,
    /// либо как предок текущего элемента <see cref="RelativeSource.AncestorType"/>.
    /// Строка <see cref="DataGridCell"/> определяется как предок текущего элемента.</remarks>
    public partial class DataGridCellContent : MultiBinding
    {
        public DataGridCellContent()
        {
            // Добавление конвертера в привязку.
            Converter = Fields.cellValueConverter;

            // Привязка к текущему элементу.
            Bindings.Add(Fields.cellSelf);

            // Привязка к колонке текущего элемента.
            // Если нет колонки у текущего элемента, то будет возвращено notValueSource
            //PriorityBinding cellSelfColumn = new();
            //cellSelfColumn.Bindings.Add(Fields.cellSelfColumn);
            //cellSelfColumn.Bindings.Add(Fields.notValue);
            //Bindings.Add(cellSelfColumn);
            Bindings.Add(Fields.cellSelfColumn);

            // Привязка к колонке DataGridCell, содержащей текущей элемент.
            // Если нет колонки у текущего элемента, то будет возвращено notValueSource
            //PriorityBinding cellAncestorColumn = new();
            //cellAncestorColumn.Bindings.Add(Fields.cellAncestorColumn);
            //cellAncestorColumn.Bindings.Add(Fields.notValue);
            //Bindings.Add(cellAncestorColumn);
            Bindings.Add(Fields.cellAncestorColumn);

            Bindings.Add(Fields.rowAncestor);
        }
    }
}
