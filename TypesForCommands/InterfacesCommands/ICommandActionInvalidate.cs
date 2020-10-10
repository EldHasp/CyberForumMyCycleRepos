namespace InterfacesCommands
{
    /// <summary>Определяет команду без параметра.</summary>
    /// <remarks>Если переданный параметр не нуль,
    /// то команда невыполнима.</remarks>
    public interface ICommandActionInvalidate : ICommandInvalidate
    {
        /// <summary>Определяет метод, исполняемый при вызове данной команды без параметров.</summary>
        void Execute();

        /// <summary>Определяет метод, который проверяет, может ли данная команда без параметров
        /// выполняться в ее текущем состоянии.</summary>
        /// <returns>Значение true, если эту команду можно выполнить;
        /// в противном случае — значение false.</returns>
        bool CanExecute();
    }    

}
