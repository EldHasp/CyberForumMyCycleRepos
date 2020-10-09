using System.Windows;
using System.Windows.Data;

namespace BindingStringToNumeric
{
    public partial class BindToNumericExtension
    {

        private readonly Binding bindNumber = new Binding()
        { 
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };

        private readonly Binding bindTextBox = new Binding() { Mode = BindingMode.OneTime};

        private readonly PrivateMulti multi = new PrivateMulti()
        {
            Mode = BindingMode.TwoWay,
            Converter = PrivateConverter.Instance,
            UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
            NotifyOnSourceUpdated = true
        };

        /// <summary>Инициализирует новый экземпляр класса <see cref="BindToNumericExtension"/>.</summary>
        public BindToNumericExtension()
        {
            multi.Bindings.Add(bindNumber);
            multi.Bindings.Add(bindTextBox);
        }

        /// <summary>Инициализирует новый экземпляр класса <see cref="BindToNumericExtension"/> с начальным путем./// </summary>
        /// <param name="path">Начальный <see cref="Path"/> для привязки.</param>
        public BindToNumericExtension(string path)
            : this()
        {
            Path = new PropertyPath(path, (object[])null);
        }
    }
}
