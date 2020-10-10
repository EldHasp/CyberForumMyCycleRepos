
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace BindingStringToNumeric
{
    public partial class BindToNumericExtension
    {
        /// <summary>Сравнивает полученное число с <see cref="TextBox.Text"/>.<br/>
        /// Если из текста получается такое же число, то присвоение значение по привязке отменяется.</summary>
        /// <remarks>Значения должны приходить в массиве в параметре values метода <see cref="Convert"/><br/>
        /// В массиве должно быть два значения:<br/>
        /// 0 - значение источника в числовом типе,<br/>
        /// 1 - <see cref="TextBox"/> к свойству <see cref="TextBox.Text"/> которого задана привязка использующая конвертер.</remarks>
        private partial class PrivateConverter : IMultiValueConverter
        {
            public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
            {
                object ret = null;

                object source = values[0];
                TextBox textBox = (TextBox)values[1];
                string newText = textBox.Text;

                // Получение состояния привязки TextBox
                TextBoxBindingState bindingState = GetBindingState(textBox);
                string oldText = bindingState.OldText;

                // Получение парсера для типа в первой привязке (привязка к Источнику).
                TryParseNumberHandler tryParse = NumericTryParse.GetTryParse(source.GetType());

                // Если парсер получить не удалось, значит первая привязка получила значение недопустимого типа.
                if (tryParse == null)
                    throw InvalidNumberType;

                Debug.Write(GetType().Name + $".Convert.values: {source}, \"{oldText}\", \"{newText}\"");

                // Получение цифрового стиля из параметра. Если он не задан, то используется стиль по умолчанию для этого типа.
                NumberStyles style = NumericTryParse.GetNumberStyle(parameter, values[0].GetType());

                // Проверка причины вызова конвертера.
                switch (bindingState.UpdateSourceState)
                {
                    // Метод UpdateSource() не вызывался, значит значение обновляется по привязке от Источника.
                    case UpdateSourceStateEnum.NotCalled:

                        // Получение из TextBox числа в том же типе, что получено от Источника (в первой привязке).
                        // И сравнение этого числа с числом Источника.
                        if (tryParse(newText, style, culture, out object target) && target.Equals(source))
                        {
                            // Отменяется присвоение значения привязкой.
                            ret = Binding.DoNothing;
                        }

                        // Иначе возвращается число Источника в текстовом виде в заданной культуре.
                        else
                        {
                            ret = System.Convert.ToString(source, culture);
                        }
                        break;

                    // Обновление источника произошло в ходе выполнения метода UpdateSource().
                    // Значит в TextBox.Text корректное значение и его надо проверить только на крайние пробелы
                    // для режима ввода "Tолько Числа".
                    case UpdateSourceStateEnum.Called:
                        if (bindingState.IsNumericOnly)
                        {
                            if (newText.Length < 1 || char.IsWhiteSpace(newText[0]) || char.IsWhiteSpace(newText[newText.Length - 1]))
                            {
                                // Устанавливается флаг обновления по привязке
                                bindingState.TextChangeSource = PropertyChangeSourceEnum.Binding;
                                // Возращается TextBox старое значение.
                                UndoText(textBox, oldText, bindingState.Changes);
                            }
                        }
                        // Отменяется присваивание.
                        ret = Binding.DoNothing;

                        break;

                    // После вызова UpdateSource() обновление источника не произошло
                    // Значит в TextBox.Text некорректное значение и надо вернуть предыдущее его значение.
                    case UpdateSourceStateEnum.CallCanceled:

                        // Устанавливается флаг обновления из конвертера привязки
                        bindingState.TextChangeSource = PropertyChangeSourceEnum.Binding;

                        // Проверяется на пустую строку.
                        // Если она пустая, то надо её заменить на "0".
                        // Это автоматически сделает метод UndoText().
                        if (string.IsNullOrWhiteSpace(newText))
                            ZeroText(textBox);

                        else if (!BeginScientific(newText))
                            // Возращается TextBox старое значение.
                            UndoText(textBox, oldText, bindingState.Changes);

                        // Сброс флага обновления из конвертера привязки
                        bindingState.TextChangeSource = PropertyChangeSourceEnum.NotBinding;

                        // Проверка строки в TextBox.
                        // Если она корректна, то для сброса валидации надо её же и вернуть
                        if (tryParse(textBox.Text, style, culture, out _))
                            ret = textBox.Text;
                        else
                            // Иначе - отмена присвоения по привязке
                            ret = Binding.DoNothing;

                        break;
                    default:
                        break;
                }

                Debug.WriteLine($"; return: {ret ?? "null"}");

                if (ret is string str && str != textBox.Text)
                    // Устанавливается флаг обновления по привязке
                    bindingState.TextChangeSource = PropertyChangeSourceEnum.Binding;

                return ret; ;

                // Проверка строки на начало научной записи числа
                bool BeginScientific(string text)
                {
                    if (string.IsNullOrWhiteSpace(text))
                        return false;
                    if (style.HasFlag(NumberStyles.AllowExponent))
                    {
                        if (text[text.Length - 1] == 'e' || text[text.Length - 1] == 'E')
                            text = text.Remove(text.Length - 1, 1);
                        else if (text.Length > 1 && (text[text.Length - 1] == '-' || text[text.Length - 1] == '+')
                            && (text[text.Length - 2] == 'e' || text[text.Length - 2] == 'E'))
                            text = text.Remove(text.Length - 2, 2);
                    }
                    return tryParse(text, style, culture, out _);
                }
            }


            /// <summary>Метод возвращающий значение <see cref="TextBox.Text"/> в состояние до изменений.</summary>
            /// <param name="textBox"><see cref="TextBox"/>.</param>
            /// <param name="oldText">Предыдущее значение.</param>
            /// <param name="changes">Коллекция объектов, содержащий сведения о произошедших изменениях.</param>
            private static void UndoText(TextBox textBox, string oldText, ICollection<TextChange> changes)
            {
                if (string.IsNullOrWhiteSpace(oldText))
                    ZeroText(textBox);
                else
                {
                    if (changes.Count != 1)
                        throw InvalidChangesCount;

                    // Сведения об изменении
                    TextChange change = changes.ElementAt(0);

                    int caret = change.Offset;
                    textBox.Text = oldText;

                    if (caret < 0)
                        caret = 0;
                    else if (caret > oldText.Length)
                        caret = oldText.Length;

                    textBox.CaretIndex = caret;
                }
            }
            /// <summary>Метод присваивающий "0" свойству <see cref="TextBox.Text"/>.</summary>
            private static void ZeroText(TextBox textBox) => textBox.SelectedText = textBox.Text = "0";

            public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            {
                Debug.Write(GetType().Name + $".ConvertBack.value: \"{value}\" to ");
                object ret = null;

                string text = (string)value;

                // Получение парсера для типа в первой привязке (привязка к Источнику).
                TryParseNumberHandler tryParse = NumericTryParse.GetTryParse(targetTypes[0]);
                // Если парсер получить не удалось, значит первая привязка получила значение недопустимого типа.
                if (tryParse == null)
                    throw InvalidTargetNumberType;

                // Получение цифрового стиля из параметра. Если он не задан, то используется стиль по умолчанию для этого типа.
                NumberStyles style = NumericTryParse.GetNumberStyle(parameter, targetTypes[0]);

                // Получение из строки числа заданного типа в заданной культуре
                // Если удалось, то оно возвращается.
                if (tryParse(text, style, culture, out object target))
                    ret = target;

                Debug.WriteLine($"return: {(ret == null ? "null" : target)}");

                // Dозвращается массив с одним элементом: полученным числом или null.
                return new object[] { ret };
            }

            /// <summary>Экземпляр конвертера.<br/>
            /// Упрощает использование конвертера:
            /// можно обращаться к нему, а не создавать экземпляр в ресурсах.</summary>
            public static PrivateConverter Instance { get; } = new PrivateConverter();

        }

        /// <summary>Исключение возникающее в <see cref="PrivateConverter.Convert"/> при неверном типе по первой привязке.</summary>
        public static ArgumentException InvalidNumberType { get; }
            = new ArgumentException("Первым значением должен быть численный тип.", "values[0]");

        /// <summary>Исключение возникающее в <see cref="PrivateConverter.ConvertBack"/> при неверном типе по первой привязке.</summary>
        public static ArgumentException InvalidTargetNumberType { get; }
            = new ArgumentException("Первым целевым типом должен быть численный тип.", "targetTypes[0]");

        /// <summary>Ожидается, что в коллекции <see cref="TextChangedEventArgs.Changes"/> один и только один элемент.</summary>
        /// <remarks>Возможно в этой коллекции может быть и иное количество элементов, но получить подобное состояние мне не удалось.<br/>
        /// Поэтому для количества отличного от одного элемента реализация не создана.<br/>
        /// Если это исключение будет выпадать, то сообщите мне - я добавлю реализацию.</remarks>
        public static NotImplementedException InvalidChangesCount { get; }
            = new NotImplementedException("Метод реализован для коллекции изменений TextBox полученной в параметре changes содержащей один и только один элемент.");

    }
}