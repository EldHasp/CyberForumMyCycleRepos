using System.Windows.Threading;

namespace InterfacesCommands
{
    /// <summary>Интерфейс со свойством Dispatcher.</summary>
    public interface IDispatcher
    {
        Dispatcher Dispatcher { get; }
    }
}    

