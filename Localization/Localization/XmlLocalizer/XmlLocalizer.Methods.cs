using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Markup;

namespace Localization;

public partial class XmlLocalizer
{
    private readonly CultureInfo? defaultThreadCurrentCulture = CultureInfo.DefaultThreadCurrentCulture;
    private readonly CultureInfo? defaultThreadCurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture;
    private readonly CultureInfo defaultCurrentCulture = CultureInfo.CurrentCulture;
    private readonly CultureInfo defaultCurrentUICulture = CultureInfo.CurrentUICulture;

    private readonly XmlLanguage defaulLanguage = (XmlLanguage)FrameworkElement.LanguageProperty.GetMetadata(typeof(Window)).DefaultValue;

    private void OnChangedLanguageOrApp()
    {
        XmlLanguage? language = privateCurrentLanguage ?? defaulLanguage;

        AddLanguage(null, language, Languages);

        //FrameworkElement.LanguageProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(language));


        if (privateIsChangeThreadCulture)
        {
            if (CurrentCulture is CultureInfo culture)
            {
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;
            }
            else
            {
                CultureInfo.DefaultThreadCurrentCulture = defaultThreadCurrentCulture;
                CultureInfo.DefaultThreadCurrentUICulture = defaultThreadCurrentUICulture;
                CultureInfo.CurrentCulture = defaultCurrentCulture;
                CultureInfo.CurrentUICulture = defaultCurrentUICulture;
            }
        }
        if (privateApp != null)
        {
            foreach (Window window in privateApp.Windows.OfType<Window>())
            {
                window.Language = language;
            }
        }
    }

    /// <summary>����� ������� ����;</summary>
    /// <param name="ietfLanguageTag">��� �����.</param>
    public void SetLanguage(string ietfLanguageTag)
    {
        XmlLanguage language = GetLanguage(ietfLanguageTag);
        CurrentLanguage = language;
    }


    private static readonly XmlLanguageConverter xmlLanguageConverter = new();

    /// <summary>���������� ���� �� ���������� ����.</summary>
    /// <param name="ietfLanguageTag">��� �����.</param>
    /// <returns>���� �������������� ����.</returns>
    public XmlLanguage GetLanguage(string ietfLanguageTag)
    {
        try
        {
            var languages = Languages;
            if (!languages.TryGetValue(ietfLanguageTag, out var language))
            {
                language = (XmlLanguage?)xmlLanguageConverter.ConvertFromString(ietfLanguageTag) ?? defaulLanguage;
                AddLanguage(ietfLanguageTag, language, Languages);
            }
            return language;
        }
        catch
        {
            return defaulLanguage;
        }
    }

    /// <summary>��������� ���� � ���� � �������.</summary>
    /// <param name="tag">���.</param>
    /// <param name="language">����.</param>
    /// <param name="languages">������� ���-����.</param>
    /// <remarks>����� <paramref name="tag"/> ��������� ��� ���� ��� ����� � ��� ����������� ��������.</remarks>
    protected static void AddLanguage(string? tag, XmlLanguage language, Dictionary<string, XmlLanguage> languages)
    {
        if (language == null)
            return;

        CultureInfo? culture = language.GetSpecificCulture();
        string ietfLanguageTag = language.IetfLanguageTag;
        string? cultureTag = culture?.Name;

        Add(ietfLanguageTag);
        Add(cultureTag);
        Add(tag);

        void Add(string? tag)
        {
            if (tag != null && !languages.ContainsKey(tag))
            {
                languages.Add(tag, language);
            }
        }
    }
}