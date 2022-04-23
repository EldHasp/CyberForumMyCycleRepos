using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace WpfMvvm.Converters
{
    // Реализация MarkupExtension

    /// <summary>Возвращает <see cref="DictionaryConverter"/> 
    /// или <see cref="DictionaryTypeConverter"/>.</summary>
    /// <remarks>Если не задана привязка <see cref="Binding"/>, то возвращается один из статических экземпляров:
    /// <see cref="DictionaryConverter.Instance"/>, <see cref="DictionaryTypeConverter.InstanceBaseTypes"/>
    /// или <see cref="DictionaryTypeConverter.InstanceEqualsTypes"/>.</remarks>
    [MarkupExtensionReturnType(typeof(DictionaryConverter))]
    public partial class DictionaryConverterExtension : MarkupExtension
    {
        /// <summary>Возвращает <see cref="DictionaryConverter"/> 
        /// или <see cref="DictionaryTypeConverter"/>.</summary>
        /// <param name="serviceProvider">Вспомогательный объект поставщика служб,
        /// способный предоставлять службы для расширения разметки.<para/>
        /// Не используется.</param>
        /// <returns>Если не заданно значение <see cref="Binding"/>,
        /// то возвращает для значений <see cref="UseTypes"/>:<br/>
        /// <see cref="UseTypesEnum.NotType"/> - <see cref="DictionaryConverter.Instance"/>;<br/>
        /// <see cref="UseTypesEnum.BaseType"/> - <see cref="DictionaryTypeConverter.InstanceBaseTypes"/>;<br/>
        /// <see cref="UseTypesEnum.EqualsType"/> - <see cref="DictionaryTypeConverter.InstanceEqualsTypes"/>.<para/>
        /// Иначе возвращает для значений <see cref="UseTypes"/>:<br/>
        ///  <see cref="UseTypesEnum.NotType"/> - новый эеземпляр <see cref="DictionaryConverter"/>;<br/>
        /// <see cref="UseTypesEnum.BaseType"/> - новый экземпляр <see cref="DictionaryTypeConverter"/> с <see cref="DictionaryTypeConverter.UseBasicTypes"/>=<see langword="true"/>;<br/>
        /// <see cref="UseTypesEnum.EqualsType"/> - новый экземпляр <see cref="DictionaryTypeConverter"/> с <see cref="DictionaryTypeConverter.UseBasicTypes"/>=<see langword="false"/>.<br/>
        /// В экземплярах конвертеров свойство <see cref="DictionaryConverter.Dictionary"/> привязывается по привязке <see cref="Binding"/>.</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Binding == null)
            {
                switch (UseTypes)
                {
                    case UseTypesEnum.NotType:
                        return DictionaryConverter.Instance;
                    case UseTypesEnum.BaseType:
                        return DictionaryTypeConverter.InstanceBaseTypes;
                    case UseTypesEnum.EqualsType:
                        return DictionaryTypeConverter.InstanceEqualsTypes;
                    default:
                        throw SetUseTypesException;
                }
            }

            DictionaryConverter converter = UseTypes == UseTypesEnum.NotType
                ? new DictionaryConverter()
                : new DictionaryTypeConverter() { UseBasicTypes = UseTypes == UseTypesEnum.BaseType};
                BindingOperations.SetBinding(converter, DictionaryConverter.DictionaryProperty, Binding);

            return converter;
        }
    }
}
