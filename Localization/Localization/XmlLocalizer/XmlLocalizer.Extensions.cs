using System;
using System.Windows.Data;
using System.Windows.Markup;

namespace Localization;
public class XLzerDefauiltExtension : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return XmlLocalizer.Default;
    }
}
public class XLzerBindingExtension : Binding
{
    public XLzerBindingExtension()
    {
        Source = XmlLocalizer.Default;
    }

    public XLzerBindingExtension(string path)
        : base(path)
    {
        Source = XmlLocalizer.Default;
        Path = new(path);
    }
}
