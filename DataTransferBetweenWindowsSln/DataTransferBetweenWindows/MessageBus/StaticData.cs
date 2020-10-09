namespace MessageBus
{
    public delegate void MessageObjectHandler(object data);

    public static class MessageBusObject
    {
        public static event MessageObjectHandler Bus;

        public static void Send(object data)
            => Bus?.Invoke(data);
    }
}
