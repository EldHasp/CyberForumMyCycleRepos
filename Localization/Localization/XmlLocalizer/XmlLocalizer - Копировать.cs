using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Localization;
    public partial class XmlLocalizer : Freezable
{
    protected override Freezable CreateInstanceCore()
        => new XmlLocalizer();
}
public partial class XmlLocalizer
{
    /// <summary>
    /// Текущий язык.
    /// </summary>
    public XmlLanguage CurrentLanguage
    {
        get { return (XmlLanguage)GetValue(CurrentLanguageProperty); }
        set { SetValue(CurrentLanguageProperty, value); }
    }

    /// <summary><see cref="DependencyProperty"/> для свойства <see cref="CurrentLanguage"/>.</summary>
    public static readonly DependencyProperty CurrentLanguageProperty =
        DependencyProperty.Register(nameof(CurrentLanguage), typeof(XmlLanguage), typeof(XmlLocalizer),
            new PropertyMetadata(null, (d, e) =>
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



    /// <summary>
    /// Текущая культура.
    /// </summary>
    public CultureInfo CurrentCulture
    {
        get { return (CultureInfo)GetValue(CurrentCultureProperty); }
        private set { SetValue(CurrentCulturePropertyKey, value); }
    }


    private static readonly DependencyPropertyKey CurrentCulturePropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(CurrentCulture), typeof(CultureInfo), typeof(XmlLocalizer), new PropertyMetadata(null));
    /// <summary><see cref="DependencyProperty"/> для свойства <see cref="CurrentCulture"/>.</summary>
    public static readonly DependencyProperty CurrentCultureProperty = CurrentCulturePropertyKey.DependencyProperty;

    /// <summary>
    /// Для чего это свойство?
    /// </summary>
    public Dictionary<string, XmlLanguage> Languages
    {
        get { return (Dictionary<string, XmlLanguage>)GetValue(LanguagesProperty); }
        private set { SetValue(LanguagesPropertyKey, value); }
    }

    private static readonly DependencyPropertyKey LanguagesPropertyKey =
        DependencyProperty.RegisterReadOnly(nameof(Languages), typeof(Dictionary<string, XmlLanguage>), typeof(XmlLocalizer),
            new PropertyMetadata(null));
    /// <summary><see cref="DependencyProperty"/> для свойства <see cref="Languages"/>.</summary>
    public static readonly DependencyProperty LanguagesProperty = LanguagesPropertyKey.DependencyProperty;


    /// <summary>
    /// Локализируемое Приложение.
    /// </summary>
    public Application App
    {
        get { return (Application)GetValue(AppProperty); }
        set { SetValue(AppProperty, value); }
    }

    /// <summary><see cref="DependencyProperty"/> для свойства <see cref="App"/>.</summary>
    public static readonly DependencyProperty AppProperty =
        DependencyProperty.Register(nameof(App), typeof(Application), typeof(XmlLocalizer),
            new PropertyMetadata(null, (d, e) =>
            {
                XmlLocalizer localizer = (XmlLocalizer)d;
                localizer.privateApp = (Application)e.NewValue;
                localizer.OnChangedLanguageOrApp();
            }));

    /// <summary>Экземпляр локализотора по умолчанию. Автоматически устаналивается на текущее приложение.</summary>
    public static XmlLocalizer Default { get; } = new();

    static XmlLocalizer()
    {
        Default.App = Application.Current;
    }

    private XmlLanguage? privateCurrentLanguage;
    private Application? privateApp;
    private void OnChangedLanguageOrApp()
    {
        var language = privateCurrentLanguage;
        if (language != null)
        {
            var tags = ietfLanguageTags;
            if (!tags.ContainsKey(language.IetfLanguageTag))
                tags.Add(language.IetfLanguageTag, language.IetfLanguageTag);
        }

        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(Window), new PropertyMetadata(language));

        if (privateApp != null)
            foreach (Window window in privateApp.Windows.OfType<Window>())
            {
                window.Language = language;
            }
    }
    private readonly Dictionary<string, string> ietfLanguageTags = new();

    public bool TrySetLanguage(string ietfLanguageTag)
    {
        var language = AddLanguage(ietfLanguageTag);
        if (language == null)
            return false;
        CurrentLanguage = language;
        return true;
    }

    public XmlLanguage? AddLanguage(string ietfLanguageTag)
    {
        try
        {
            var languages = Languages;
            var tags = ietfLanguageTags;
            XmlLanguage? language;
            if (!tags.TryGetValue(ietfLanguageTag, out var tag))
            {
                language = XmlLanguage.GetLanguage(ietfLanguageTag);
                if (languages.ContainsKey(language.IetfLanguageTag))
                {
                    languages.Add(language.IetfLanguageTag, language);
                    tags.Add(ietfLanguageTag, language.IetfLanguageTag);
                    if (!tags.ContainsKey(language.IetfLanguageTag))
                        tags.Add(language.IetfLanguageTag, language.IetfLanguageTag);
                }
            }
            else
            {
                language = languages[tag];
            }
            return language;
        }
        catch
        {
            return null;
        }

    }
}

public partial class XmlLocalizer
{
    private class PrivateCommand : ICommand
        //{
        //    public EventHandler? CanExecuteChangedDelegate;

            //    public event EventHandler? CanExecuteChanged
            //    {
            add => CanExecuteChangedDelegate += value;

            remove => CanExecuteChangedDelegate -= value;
            }

public bool CanExecute(object? parameter)
{
    return parameter is string;
}

public void Execute(object? parameter)
{
    if (parameter is not string tag)
        throw new InvalidCastException("Параметр должен быть ненулевой строкой.");
    localizer.AddLanguage(tag);
}
private XmlLocalizer localizer;
private readonly EventHandler requerySuggested;
public PrivateCommand(XmlLocalizer localizer)
{
    this.localizer = localizer;

    CommandManager.RequerySuggested += requerySuggested;
}
}
private class CommandAddTag : ICommand
{
    public EventHandler? CanExecuteChangedDelegate;

    public event EventHandler? CanExecuteChanged
    {
        add => CanExecuteChangedDelegate += value;
        remove => CanExecuteChangedDelegate -= value;
    }

    public bool CanExecute(object? parameter)
    {
        return parameter is string;
    }

    public void Execute(object? parameter)
    {
        if (parameter is not string tag)
            throw new InvalidCastException("Параметр должен быть ненулевой строкой.");
        localizer.AddLanguage(tag);
    }
    private XmlLocalizer localizer;
    public CommandAddTag(XmlLocalizer localizer)
        => this.localizer = localizer;
}

private class CommandSetLanguage : ICommand
{
    public EventHandler? CanExecuteChangedDelegate;
    public event EventHandler? CanExecuteChanged
    {
        add => CanExecuteChangedDelegate += value;
        remove => CanExecuteChangedDelegate -= value;
    }
    public bool CanExecute(object? parameter)
    {
        return parameter is string or XmlLanguage;
    }

    public void Execute(object? parameter)
    {
        if (parameter is string tag)
            localizer.TrySetLanguage(tag);
        else if (parameter is XmlLanguage language)
            localizer.CurrentLanguage = language;
        else
            throw new InvalidCastException("Параметр должен быть ненулевой строкой или XmlLanguage.");
    }

    private XmlLocalizer localizer;
    public CommandSetLanguage(XmlLocalizer localizer)
        => this.localizer = localizer;
}

public ICommand AddTagCommand { get; }
public ICommand SetLanguageCommand { get; }
    }
    public partial class XmlLocalizer
{
    private readonly EventHandler requerySuggested;
    public XmlLocalizer()
    {
        Languages = new();
        var addTagCommand = new CommandAddTag(this);
        var setLanguageCommand = new CommandSetLanguage(this);
        requerySuggested = (s, e) =>
        {
            addTagCommand.CanExecuteChangedDelegate?.Invoke(addTagCommand, EventArgs.Empty);
            setLanguageCommand.CanExecuteChangedDelegate?.Invoke(setLanguageCommand, EventArgs.Empty);
        };
        CommandManager.RequerySuggested += requerySuggested;
        AddTagCommand = addTagCommand;
        SetLanguageCommand = setLanguageCommand;
    }

    private void CommandManager_RequerySuggested(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}
}
