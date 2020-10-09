using System.Diagnostics;
using System.Globalization;
using System.Windows.Controls;

namespace AppBindingToNumeric
{
    /// <summary>Клас для трасировки валидации.</summary>
    public class DebugValidationRule : ValidationRule
    {
        /// <summary>Значение для вывода в трассировку.</summary>
        public string Title { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // Если value строка - значение выводится в кавычках, иначе - без оных с добавлением типа.
            Debug.WriteLine
            (
                value is string str
                ? $"{Title}.Validate.value=\"{str}\""
                : $"{Title}.Validate.value=({value?.GetType()}){value}"
            );
            return ValidationResult.ValidResult;
        }
    }

}
