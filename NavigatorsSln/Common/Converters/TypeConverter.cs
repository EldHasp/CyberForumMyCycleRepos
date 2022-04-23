using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Converters
{
    public class TypeConverter<T> : TypeConverter
    {
        private static Type[] types;
        private static readonly Dictionary<string, Type> typesDictionary = new Dictionary<string, Type>(17);
        static TypeConverter()
        {
            var main = Assembly.GetEntryAssembly();
            List<Type> list = main?.GetTypes().Where(t => typeof(T).IsAssignableFrom(t)).ToList() ?? new List<Type>(100);
            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (a.Equals(main))
                    continue;
                foreach (Type t in a.GetTypes())
                {
                    if (typeof(T).IsAssignableFrom(t))
                        list.Add(t);
                }
            }

            types = list.Distinct().ToArray(); typesDictionary = new Dictionary<string, Type>();
        }

        public override bool IsValid(ITypeDescriptorContext? context, object? value)
        {
            if ((value is Type type && typeof(T).IsAssignableFrom(type)) ||
                (value is string name && FindType(name) is not null))
                return true;
            return base.IsValid(context, value);
        }

        private static Type? FindType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            if (typesDictionary.TryGetValue(name, out Type? type))
                return type;

            type = null;
            foreach (Type t in types)
            {
                if (t.Name == name)
                    type = t;
                else
                {
                    string full = t.FullName ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(full))
                    {
                        if (full == name)
                            type = t;

                        if (full.EndsWith(name) && full[^(name.Length + 1)] == '.')
                            type = t;
                    }
                }
                if (type != null)
                    break;
            }
            if (type != null)
                typesDictionary.Add(name, type);
            return type;
        }

        public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
        {
            if (typeof(Type).IsAssignableFrom(sourceType) || typeof(string).IsAssignableFrom(sourceType))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
        {
            if (value is Type type)
            {
                if (typeof(T).IsAssignableFrom(type))
                    return type;
            }
            if (value is string name)
            {
                Type? t = FindType(name);
                if (t is not null)
                    return t;
            }
            return base.ConvertFrom(context, culture, value);
        }
    }


}
