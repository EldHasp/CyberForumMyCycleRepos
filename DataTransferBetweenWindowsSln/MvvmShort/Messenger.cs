using System;
using System.Collections.Generic;

namespace MvvmShort
{
    public class Messenger
    {
        public static Messenger Default { get; } = new Messenger();

        protected readonly Dictionary<Type, List<Delegate>> actions = new Dictionary<Type, List<Delegate>>();

        public void Register<T>(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            lock (actions)
            {
                Type type = typeof(T);
                if (actions.TryGetValue(type, out List<Delegate> list))
                {
                    if (!list.Contains(action))
                        list.Add(action);
                }
                else
                {
                    actions.Add(type, new List<Delegate>(1) { action });
                }
            }
        }

        public void Unregister<T>(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            lock (actions)
            {
                Type type = typeof(T);
                if (actions.TryGetValue(type, out List<Delegate> list))
                    list.RemoveAll(act => (Action<T>)act == action);
            }

        }

        public void Send<T>(T message)
        {
            lock (actions)
            {
                Type type = typeof(T);
                if (actions.TryGetValue(type, out List<Delegate> list))
                    list.ForEach(dlgt => ((Action<T>)dlgt)(message));
            }
        }
    }
}
