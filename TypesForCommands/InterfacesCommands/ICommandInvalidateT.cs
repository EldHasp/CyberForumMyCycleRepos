namespace InterfacesCommands
{
    /// <summary>Определяет команду с типизированным параметром.</summary>
    /// <typeparam name="T">Тип параметра.</typeparam>
    /// <remarks>Если переданный параметр нуль или нельзя преобразовать в заданный тип,
    /// то команда невыполнима.</remarks>
    public interface ICommandInvalidate<T> : ICommandInvalidate
    {
        /// <summary>Определяет метод с типизированным параметром, вызываемый при исполнении данной команды.</summary>
        /// <param name="parameter">Данные, используемые данной командой.
        /// Если <see langword="null"/> - метод не выполняется.</param>
        /// <typeparam name="Tparam">Тип параметра. Должен быть T или производным от него.</typeparam>
        void Execute<Tparam>(Tparam parameter) where Tparam : T;

        /// <summary>Определяет метод с типизированным параметром, который проверяет, может ли данная команда
        /// исполняться в ее текущем состоянии.</summary>
        /// <param name="parameter">Данные, используемые данной командой.
        /// <typeparam name="Tparam">Тип параметра. Должен быть T или производным от него.</typeparam>
        /// Если <see langword="null"/> - метод возвращает <see langword="false"/>.</param>
        /// <returns>Значение true, если эту команду можно выполнить;
        /// в противном случае — значение false.</returns>
        bool CanExecute<Tparam>(Tparam parameter) where Tparam : T;
    }

}
