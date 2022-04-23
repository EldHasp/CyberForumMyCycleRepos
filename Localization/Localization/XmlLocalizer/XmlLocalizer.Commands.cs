using System;
using System.Windows.Input;
using System.Windows.Markup;

namespace Localization;
public partial class XmlLocalizer
{
    private class PrivateCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => canExecute(parameter);

        public void Execute(object? parameter)=> execute(parameter);

        private readonly Action<object?> execute;
        private readonly Func<object?, bool> canExecute;
        private readonly EventHandler requerySuggested;

        public PrivateCommand(Action<object?> execute, Func<object?, bool> canExecute)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));

            requerySuggested = (s, e) => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            CommandManager.RequerySuggested += requerySuggested;
        }
    }

    private ICommand? _addTagCommand;
    public ICommand AddTagCommand => _addTagCommand ??= new PrivateCommand
                (
            p =>
            {
                if (p is not string tag)
                    throw new InvalidCastException("Параметр должен быть ненулевой строкой.");
                GetLanguage(tag);
            },
            p => p is string
        );

    private ICommand? _setLanguageCommand;
    public ICommand SetLanguageCommand => _setLanguageCommand ??= new PrivateCommand
        (
            p =>
            {
                if (p is string tag)
                    SetLanguage(tag);
                else if (p is XmlLanguage language)
                    CurrentLanguage = language;
                else
                    throw new InvalidCastException("Параметр должен быть ненулевой строкой или XmlLanguage.");
            },
            p => p is string or XmlLanguage
        );
}
