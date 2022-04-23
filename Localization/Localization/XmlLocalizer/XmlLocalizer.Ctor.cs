using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;

namespace Localization;

public partial class XmlLocalizer
{
    public XmlLocalizer()
    {
        App = Application.Current;
        var languages = new Dictionary<string, XmlLanguage>();

        AddLanguage(null, defaulLanguage, languages);

        Languages = languages;
    }
}
