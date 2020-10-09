namespace InterfacesCommands
{
    /// <summary>Делегат исполнительного метода команды.</summary>
    /// <param name="parameter">Параметр команды.</param>
    public delegate void ExecuteHandler(object parameter);

    /// <summary>Делегат метода состояния команды.</summary>
    /// <param name="parameter">Параметр команды.</param>
    /// <returns><see langword="true"/> если выполнение команды разрешено.</returns>
    public delegate bool CanExecuteHandler(object parameter);

    /// <summary>Делегат исполнительного метода типизированной команды.</summary>
    /// <param name="parameter">Параметр команды.</param>
    /// <typeparam name="T">Тип параметра.</typeparam>
    public delegate void ExecuteHandler<T>(T parameter);

    /// <summary>Делегат метода состояния типизированной команды.</summary>
    /// <param name="parameter">Параметр команды.</param>
    /// <returns><see langword="true"/> если выполнение команды разрешено.</returns>
    public delegate bool CanExecuteHandler<T>(T parameter);

    /// <summary>Делегат исполнительного метода команды без параметра.</summary>
    public delegate void ExecuteActionHandler();

    /// <summary>Делегат исполнительного метода состояния команды без параметра.</summary>
    /// <returns><see langword="true"/> если выполнение команды разрешено.</returns>
    public delegate bool CanExecuteActionHandler();
}
