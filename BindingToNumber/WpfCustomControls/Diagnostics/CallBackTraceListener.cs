using System;
using System.Diagnostics;

namespace WpfCustomControls.Diagnostics
{
    /// <summary>Listener for <see cref="Debug.Listeners"/>.</summary>
    public class CallBackTraceListener : TraceListener
    {
        /// <summary>Event after adding text to <see cref="DebugFullText"/>.</summary>
        public static event Action<string> DebugFullTextAdded;

        /// <summary>The full text of the Debug window.</summary>
        public static string DebugFullText { get; private set; } = string.Empty;

        /// <summary>Adding <see cref="CallBackTraceListener"/> to the <see cref="Debug.Listeners"/>.</summary>
        static CallBackTraceListener()
        {
            Debug.Listeners.Add(new CallBackTraceListener());
        }

        /// <summary>Method adding text from <see cref="Debug"/> to <see cref="DebugFullText"/>.</summary>
        /// <param name="message">Added text.</param>
        private static void DebugAddedText(string message)
        {
            if (string.IsNullOrEmpty(message))
                return;
            DebugFullText += message;
            DebugFullTextAdded?.Invoke(message);
        }
        /// <summary>Hiding an instance constructor.
        /// It is only created in the singular to populate the property <see cref="DebugFullText"/>.</summary>
        private CallBackTraceListener()
        { }

        public override void Write(string message) => DebugAddedText(message);

        public override void WriteLine(string message) => DebugAddedText(message + Environment.NewLine);

    }

}
