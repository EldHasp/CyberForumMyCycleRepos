using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Markup;

namespace Localization;
public partial class XmlLocalizer
{
    /// <summary>
    /// ������� ����.
    /// </summary>
    public XmlLanguage CurrentLanguage
    {
        get { return (XmlLanguage)GetValue(CurrentLanguageProperty); }
        set { SetValue(CurrentLanguageProperty, value); }
    }
    private static XmlLanguage defaultCurrentLanguage = (XmlLanguage)FrameworkElement.LanguageProperty.DefaultMetadata.DefaultValue;
    /// <summary><see cref="DependencyProperty"/> ��� �������� <see cref="CurrentLanguage"/>.</summary>
    public static readonly DependencyProperty CurrentLanguageProperty =
        DependencyProperty.Register(nameof(CurrentLanguage), typeof(XmlLanguage), typeof(XmlLocalizer),
            new PropertyMetadata(defaultCurrentLanguage, (d, e) =>
            {
                XmlLocalizer localizer = (XmlLocalizer)d;
                localizer.privateCurrentLanguage = (XmlLanguage)e.NewValue;
                try
                {
                    localizer.CurrentCulture = localizer.privateCurrentLanguage.GetSpecificCulture();
                }
                catch
                {
                    localizer.ClearValue(CurrentCulturePropertyKey);
                }
                localizer.OnChangedLanguageOrApp();
            }));
    private XmlLanguage privateCurrentLanguage = defaultCurrentLanguage;

    /// <summary>
    /// ������� ��������.
    /// </summary>
    public CultureInfo CurrentCulture
    {
        get { return (CultureInfo)GetValue(CurrentCultureProperty); }
        private set { SetValue(CurrentCulturePropertyKey, value); }
    }


    private static readonly DependencyPropertyKey CurrentCulturePropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(CurrentCulture), typeof(CultureInfo), typeof(XmlLocalizer), new PropertyMetadata(null));
    /// <summary><see cref="DependencyProperty"/> ��� �������� <see cref="CurrentCulture"/>.</summary>
    public static readonly DependencyProperty CurrentCultureProperty = CurrentCulturePropertyKey.DependencyProperty;

    /// <summary>
    /// ��� ���� ��� ��������?
    /// </summary>
    public Dictionary<string, XmlLanguage> Languages
    {
        get { return (Dictionary<string, XmlLanguage>)GetValue(LanguagesProperty); }
        private set { SetValue(LanguagesPropertyKey, value); }
    }

    private static readonly DependencyPropertyKey LanguagesPropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(Languages), typeof(Dictionary<string, XmlLanguage>), typeof(XmlLocalizer),
            new PropertyMetadata(null));
    /// <summary><see cref="DependencyProperty"/> ��� �������� <see cref="Languages"/>.</summary>
    public static readonly DependencyProperty LanguagesProperty = LanguagesPropertyKey.DependencyProperty;


    /// <summary>
    /// �������������� ����������.
    /// </summary>
    public Application App
    {
        get { return (Application)GetValue(AppProperty); }
        set { SetValue(AppProperty, value); }
    }

    /// <summary><see cref="DependencyProperty"/> ��� �������� <see cref="App"/>.</summary>
    public static readonly DependencyProperty AppProperty =
        DependencyProperty.Register(nameof(App), typeof(Application), typeof(XmlLocalizer),
            new PropertyMetadata(null, (d, e) =>
            {
                XmlLocalizer localizer = (XmlLocalizer)d;
                localizer.privateApp = (Application)e.NewValue;
                localizer.OnChangedLanguageOrApp();
            }));
    private Application? privateApp;

    /// <summary>��������� ������������ �� ���������. ������������� �������������� �� ������� ����������.</summary>
    public static XmlLocalizer Default { get; } = new();


    /// <summary>
    /// ����� �� �������� �������� ������.
    /// </summary>
    public bool IsChangeThreadCulture
    {
        get { return (bool)GetValue(IsChangeThreadCultureProperty); }
        set { SetValue(IsChangeThreadCultureProperty, value); }
    }

    /// <summary><see cref="DependencyProperty"/> ��� �������� <see cref="IsChangeThreadCulture"/>.</summary>
    public static readonly DependencyProperty IsChangeThreadCultureProperty =
        DependencyProperty.Register(nameof(IsChangeThreadCulture), typeof(bool), typeof(XmlLocalizer),
            new PropertyMetadata(false, (d, e) =>
            {
                XmlLocalizer localizer = (XmlLocalizer)d;
                localizer.privateIsChangeThreadCulture = (bool)e.NewValue;
                localizer.OnChangedLanguageOrApp();
            }));
    private bool privateIsChangeThreadCulture;
}

