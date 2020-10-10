using System.ComponentModel;
using System.Windows.Input;

namespace InterfacesCommands
{
    /// <summary>Интерфейс ICommand дополненный методом создания события CanExecuteChanged.</summary>
    public interface ICommandInvalidate : ICommand
    {
        /// <summary>Метод вызываемый для создания события CanExecuteChanged.</summary>
        void Invalidate();

        /// <summary>Получает или задает значение, указывающее, включена ли эта команда.<br/>
        /// Если <see langword="false"/>, то <see cref="ICommand.CanExecute(object)"/>
        /// этой команды - всегда <see langword="false"/>.</summary>
        /// <remarks>Eсли это не DependecyProperty, то после изменения значения свойства должно
        /// вызываться событие <see cref="INotifyPropertyChanged.PropertyChanged"/>.<br/>
        /// Также после изменения всегда должен вызываться метод <see cref="Invalidate"/> этой команды.</remarks>
        bool IsEnabled { get; set; }
    }
}
