using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CommonCore.AttachedProperties
{
    public partial class DataGridCellContent
    {
        private  static partial class Fields
        {
            /// <summary>Привязка для получения текущего элемента.
            /// Нужно для определения его типа.</summary>
            public static readonly Binding cellSelf = new()
            {
                RelativeSource = RelativeSource.Self
            };

            /// <summary>Привязка для получения колонки <see cref="DataGridColumn"/>,
            /// если текущий элемент <see cref="DataGridCell"/>.</summary>
            public static readonly Binding cellSelfColumn = new()
            {
                RelativeSource = RelativeSource.Self,
                Path = new PropertyPath(DataGridCell.ColumnProperty),
                
            };

            /// <summary>Привязка для получения колонки <see cref="DataGridColumn"/>,
            /// если текущий элемент не <see cref="DataGridCell"/>.</summary>
            public static readonly Binding cellAncestorColumn = new()
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGridCell), 1),
                Path = new PropertyPath(DataGridCell.ColumnProperty)
            };

            /// <summary>Привязка для получения строки <see cref="DataGridRow"/>.</summary>
            public static readonly Binding rowAncestor = new()
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGridRow), 1)
            };

            /// <summary>Пустой объект для возвращения вместо ошибки привязки.</summary>
            public static readonly object notValueSource = ValueTuple.Create("Ничего нет.");

            /// <summary>Привязка для возвращения пустого значения, вместо ошибки.</summary>
            public static readonly Binding notValue = new()
            {
                Source = notValueSource
            };

            /// <summary>Экземпляр конвертера.</summary>
            public static readonly IMultiValueConverter cellValueConverter = new CellContentConverter();
        }
    }
}
