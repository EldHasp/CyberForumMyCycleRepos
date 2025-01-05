using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CommonCore.AttachedProperties
{
    public partial class DataGridCellValue
    {
        private static partial class Fields
        {
            /// <summary>Привязка для получения текущего элемента.
            /// Нужно для определения его типа.</summary>
            public static readonly Binding cellSelf = new()
            {
                RelativeSource = RelativeSource.Self
            };

            /// <summary>Привязка для получения привязки колонки <see cref="DataGridColumn"/>,
            /// если текущий элемент <see cref="DataGridCell"/>.</summary>
            public static readonly Binding cellSelfColumnBinding = new()
            {
                RelativeSource = RelativeSource.Self,
                Path = new PropertyPath("(0).(1)", DataGridCell.ColumnProperty, typeof(DataGridBoundColumn).GetProperty(nameof(DataGridBoundColumn.Binding))),
                FallbackValue = null
            };


            /// <summary>Привязка для получения предка <see cref="DataGridCell"/> текущего элемента.</summary>
            public static readonly Binding cellAncestor = new()
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGridCell), 1),
                FallbackValue = null
            };

            /// <summary>Привязка для получения привязки колонки <see cref="DataGridColumn"/>,
            /// если есть предок <see cref="DataGridCell"/> у текущего элемента.</summary>
            public static readonly Binding cellAncestorColumnBinding = new()
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGridCell), 1),
                Path = new PropertyPath("(0).(1)", DataGridCell.ColumnProperty, typeof(DataGridBoundColumn).GetProperty(nameof(DataGridBoundColumn.Binding))),
                FallbackValue = null
            };

            /// <summary>Привязка к AP-свойству <see cref="CellValueProperty"/> текущего элеента.</summary>
            public static readonly Binding cellSelfAPValue = new()
            {
                RelativeSource = RelativeSource.Self,
                Path = new PropertyPath(CellValueProperty),
                FallbackValue = null,
            };

            /// <summary>Привязка к AP-свойству <see cref="CellValueProperty"/> предка <see cref="DataGridCell"/> текущего элеента.</summary>
            public static readonly Binding cellAncestorAPValue = new()
            {
                RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGridCell), 1),
                Path = new PropertyPath(CellValueProperty),
                FallbackValue = null
            };

            /// <summary>Привязка к текущему Контексту привязок по умолчанию.
            /// Для <see cref="FrameworkElementFactory"/>
            /// - это значение <see cref="FrameworkElement.DataContext"/>.</summary>
            public static readonly Binding cellDataContext = new();

            /// <summary>Экземпляр конвертера.</summary>
            public static readonly IMultiValueConverter cellValueConverter = new CellValueConverter();

            /// <summary>Пустой объект для возвращения вместо ошибки привязки.</summary>
            public static readonly object notValueSource = ValueTuple.Create("Ничего нет.");

            /// <summary>Привязка для возвращения пустого значения, вместо ошибки.</summary>
            public static readonly Binding notValue = new()
            {
                Source = notValueSource
            };
        }
    }
}
