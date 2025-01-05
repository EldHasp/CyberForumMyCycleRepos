using System.Reflection;
using System.Windows.Data;

namespace CommonCore.AttachedProperties
{
    public partial class DataGridCellValue : MultiBinding
    {
        public DataGridCellValue()
        {
            Converter = Fields.cellValueConverter;
            Bindings.Add(Fields.cellSelf);
            Bindings.Add(Fields.cellSelfColumnBinding);
            Bindings.Add(Fields.cellAncestor);
            Bindings.Add(Fields.cellAncestorAPValue);
            Bindings.Add(Fields.cellSelfAPValue);
            Bindings.Add(Fields.cellAncestorAPValue);
            Bindings.Add(Fields.cellDataContext);

            //this.Sealed();
        }
    }

    /// <summary>Класс с методом запечатывания привязки.</summary>
    public static class BindingHelper
    {
        private static readonly FieldInfo IsSealedInfo = typeof(BindingBase).GetField("_isSealed", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
            ?? throw new NotImplementedException("Что-то не то с реализацией");

        /// <summary>Запечатывание привязки.</summary>
        /// <param name="binding">Запечатываемая привязка.</param>
        public static void Sealed(this BindingBase binding) => IsSealedInfo.SetValue(binding, true);
    }
}
