namespace InterfacesCommands
{
    ///     /// <summary>Интерфейс производный от <see cref="InterfacesCommands.ICommandInvalidate"/>, <br/>
    /// но у которого метод <see cref="ICommandInvalidate.Invalidate"/> должен выполняться в <see cref="IDispatcher"/>.</summary>

    public interface IWpfCommandInvalidate : ICommandInvalidate, IDispatcher { }
}    


