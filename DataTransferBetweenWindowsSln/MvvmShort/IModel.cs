namespace MvvmShort
{
    /// <summary>Делегат события ValueChanged, извещающего об изменении хранимого параметра.</summary>
    /// <param name="sender">Источник события (экземпляр Модели).</param>
    /// <param name="valueName">Название параметра.</param>
    /// <param name="oldValue">Старое значение параметра.</param>
    /// <param name="newValue">Новое значение параметра.</param>
    public delegate void ValueChangedHandler(object sender, string valueName, object oldValue, object newValue);

    /// <summary>Интерфейс Модели.</summary>
    public interface IModel
    {
        /// <summary>Событие извещающее об изменении хранимого параметра.</summary>
        event ValueChangedHandler ValueChanged;

        /// <summary>Метод задания нового значения параметру.</summary>
        /// <param name="valueName">Название параметра.</param>
        /// <param name="newValue">Новое значение параметра.</param>
        void SendValue(string valueName, object newValue);

        /// <summary>Метод проверки нового значения параметра.</summary>
        /// <param name="valueName">Название параметра.</param>
        /// <param name="newValue">Новое значение параметра.</param>
        /// <returns><see langword="true"/> если новое значение допустимо.</returns>
        bool ValidateValue(string valueName, object newValue);

        /// <summary>Метод вызова события ValueChanged для всех параметров.</summary>
        void AllValueChanged();
    }
}
