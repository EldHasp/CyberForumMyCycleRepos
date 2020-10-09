namespace InterfacesCommands
{
    /// <summary>Интерфейс производный от <see cref="InterfacesCommands.ICommandActionInvalidate"/>, <br/>
    /// но у которого метод <see cref="ICommandInvalidate.Invalidate"/> должен выполняться в <see cref="IDispatcher"/>.</summary>
    public interface IWpfCommandActionInvalidate : ICommandInvalidate, IDispatcher { }
}    

