using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WpfCommands.Bindings
{
    public class CommandsControl : ContentControl
    {
        private readonly Dictionary<ICommand, CommandBinding> _dict;

        public CommandsCollection Commands
        {
            get { return (CommandsCollection)GetValue(CommandsProperty); }
            set { SetValue(CommandsProperty, value); }
        }

        public static readonly DependencyProperty CommandsProperty =
            DependencyProperty.Register(nameof(Commands), typeof(CommandsCollection), typeof(CommandsControl), new PropertyMetadata(null));

        public CommandsControl()
        {
            Commands = new CommandsCollection();
            Commands.Changed += FreezableCollectionChanged;
            _dict = new Dictionary<ICommand, CommandBinding>();

            CommandManager.AddCanExecuteHandler(this, CanExecute);
            CommandManager.AddExecutedHandler(this, OnExecute);
        }

        private void FreezableCollectionChanged(object sender, EventArgs e)
        {
            _dict.Clear();
            foreach (var i in Commands) { _dict.Add(i.RoutedCommand, i); }
        }


        private void OnExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (_dict.TryGetValue(e.Command, out CommandBinding binding))
            {
                if (binding?.RelayCommand != null) binding.RelayCommand.Execute(e.Parameter);
            }
        }

        private void CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (_dict.TryGetValue(e.Command, out CommandBinding binding))
            {
                if (binding?.RelayCommand != null) e.CanExecute = binding.RelayCommand.CanExecute(e.Parameter);
            }
        }
    }

}
