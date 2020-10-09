using System;

namespace MvvmShort
{
    public class LocatorDynamic
    {
        public DataContainer Data { get; }
            = new DataContainer()
            {
                Text = "Динамик",
                Number = 99999
            };

        public DateTimeContainer Dates { get; }
            = new DateTimeContainer()
            {
                Birthday = new DateTime(1991, 10, 25)
            };
    }
}
