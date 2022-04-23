using System.Windows;

namespace Localization;
public partial class XmlLocalizer : Freezable
{
    protected override Freezable CreateInstanceCore()
        => new XmlLocalizer();
}
