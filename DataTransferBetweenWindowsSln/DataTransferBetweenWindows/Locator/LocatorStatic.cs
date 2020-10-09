namespace MvvmShort
{
    public static class LocatorStatic
    {
        public static DataContainer Data { get; }
            = new DataContainer()
            {
                Text = "Начальный текст",
                Number = 123456
            };
    }
}
