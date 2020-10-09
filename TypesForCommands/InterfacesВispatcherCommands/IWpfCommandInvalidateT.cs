namespace InterfacesCommands
{
    /// <summary>Интерфейс производный от <see cref="InterfacesCommands.ICommandInvalidate&lt;T&gt;"/>, <br/>
    /// но у которого метод <see cref="ICommandInvalidate.Invalidate"/> должен выполняться в <see cref="IDispatcher"/>.</summary>
    public interface IWpfCommandInvalidate<T> : ICommandInvalidate<T>, IDispatcher { }
}    


