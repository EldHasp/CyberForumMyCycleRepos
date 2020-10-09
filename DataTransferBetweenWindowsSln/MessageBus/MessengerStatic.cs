using System;

namespace MessageBus
{
    public static class MessengerStatic
    {
        public static event Action<object> Bus;

        public static void Send(object data)
            => Bus?.Invoke(data);
    }
}
