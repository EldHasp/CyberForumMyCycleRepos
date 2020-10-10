using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BindingStringToNumeric
{
    public partial class BindToNumericExtension
    {
        ///<inheritdoc cref="Binding.UpdateSourceTrigger"/>
        [DefaultValue(UpdateSourceTrigger.Default)]
        public UpdateSourceTrigger UpdateSourceTrigger { get => bindNumber.UpdateSourceTrigger; set => bindNumber.UpdateSourceTrigger = value; }

        ///<inheritdoc cref="Binding.NotifyOnSourceUpdated"/>
        [DefaultValue(false)]
        public bool NotifyOnSourceUpdated { get => bindNumber.NotifyOnSourceUpdated; set => bindNumber.NotifyOnSourceUpdated = value; }

        ///<inheritdoc cref="Binding.NotifyOnTargetUpdated"/>
        [DefaultValue(false)]
        public bool NotifyOnTargetUpdated { get => bindNumber.NotifyOnTargetUpdated; set => bindNumber.NotifyOnTargetUpdated = value; }

        ///<inheritdoc cref="Binding.NotifyOnValidationError"/>
        [DefaultValue(false)]
        public bool NotifyOnValidationError { get => bindNumber.NotifyOnValidationError; set => bindNumber.NotifyOnValidationError = value; }

        ///<inheritdoc cref="Binding.Converter"/>
        [DefaultValue(null)]
        public IValueConverter Converter { get => bindNumber.Converter; set => bindNumber.Converter = value; }

        ///<inheritdoc cref="Binding.ConverterParameter"/>
        [DefaultValue(null)]
        public object ConverterParameter { get => bindNumber.ConverterParameter; set => bindNumber.ConverterParameter = value; }

        ///<inheritdoc cref="Binding.ConverterCulture"/>
        [DefaultValue(null)]
        [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
        public CultureInfo ConverterCulture { get => bindNumber.ConverterCulture; set => multi.ConverterCulture = bindNumber.ConverterCulture = value; }

        ///<inheritdoc cref="Binding.Source"/>
        public object Source { get => bindNumber.Source; set => bindNumber.Source = value; }

        ///<inheritdoc cref="Binding.RelativeSource"/>
        [DefaultValue(null)]
        public RelativeSource RelativeSource { get => bindNumber.RelativeSource; set => bindNumber.RelativeSource = value; }


        /// <summary>Получает или задаёт значение, указывающеее допустимы ли в <see cref="TextBox.Text"/> значения не яляющиеся числом.</summary>
        /// <returns><see langword="true"/> (значение по умолчанию) - любой ввод создающий в <see cref="TextBox.Text"/> значение не приводимое к числу будет игнорироваться,<br/>
        /// <see langword="false"/> - при вводе в <see cref="TextBox.Text"/> значения не приводимого к числу будет устанавливаться ошибка валидации присоединённого свойства Validation.HasError.</returns>
        [DefaultValue(true)]
        public bool IsNumericOnly { get => multi.IsNumericOnly; set => multi.IsNumericOnly = value; }

        ///<inheritdoc cref="Binding.XPath"/>
        [DefaultValue(null)]
        public string XPath { get => bindNumber.XPath; set => bindNumber.XPath = value; }

        ///<inheritdoc cref="Binding.ValidationRules"/>
        public Collection<ValidationRule> ValidationRules => multi.ValidationRules;

        /// <summary>Получает коллекцию правил, проверяющих правильность пользовательского ввода
        /// после удачной конвертации в числовой тип.</summary>
        /// <remarks>Коллекция объектов System.Windows.Controls.ValidationRule.</remarks>
        public Collection<ValidationRule> ValidationNumberRules => bindNumber.ValidationRules;

        ///<inheritdoc cref="Binding.Path"/>
        public PropertyPath Path { get => bindNumber.Path; set => bindNumber.Path = value; }

        ///<inheritdoc cref="Binding.UpdateSourceExceptionFilter"/>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter { get => bindNumber.UpdateSourceExceptionFilter; set => bindNumber.UpdateSourceExceptionFilter = value; }

        /// <summary>Цифровой стиль для парсера. Если он не задан, то используется стиль по умолчанию.</summary>
        [DefaultValue(null)]

        public NumberStyles? NumberStyle
        {
            get => multi.NumberStyle;
            set => multi.NumberStyle = value;
        }
    }
}
