using System;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace MarkupExtensions
{
    /// <summary>Создаёт экземпляр указанного типа конструктором с указанными параметрами.</summary>
    [MarkupExtensionReturnType(typeof(object))]
    public class CreateInstanceExtension : MarkupExtension
    {
        /// <summary>Тип создаваемого экземпляра.</summary>
        public Type? InstanceType { get; set; }

        /// <summary>Параметры конструктора создаваемого экземпляра.</summary>
        public object[]? Parameters { get; set; }

        
        public CreateInstanceExtension()
        { }

        public CreateInstanceExtension(Type? instanceType)
        {
            InstanceType = instanceType;
        }

        public CreateInstanceExtension(Type? instanceType, params object[]? parameters)
            : this(instanceType)
        {
            Parameters = parameters;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (InstanceType == null)
                throw new ArgumentNullException(nameof(InstanceType));

            object[] parameters = Parameters ?? Array.Empty<object>();
            Type[] types = parameters.Select(p => p.GetType()).ToArray();

            ConstructorInfo ctor = InstanceType.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, types)
                ?? throw new ArgumentException("У этого типа нет конструктора принимающего указанные параметры", nameof(InstanceType));

            return ctor.Invoke(parameters);
        }
    }
}
