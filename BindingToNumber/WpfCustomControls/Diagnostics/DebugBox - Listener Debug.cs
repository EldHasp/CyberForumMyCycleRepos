using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;

namespace WpfCustomControls.Diagnostics
{

    /// <summary>A control that adds lines from the Debug Window to the Inlines collection.</summary>
    public partial class DebugBox 
    {
        /// <summary>Instance constructor. It only connects wiretapping to adding text to the Debug window.</summary>
        public DebugBox()
        {
            CallBackTraceListener.DebugFullTextAdded += DebugAddedText;
        }

        /// <summary>Line separators</summary>
        protected static readonly string[] LineSeparators = { Environment.NewLine, "\r", "\n" };
        /// <summary>A method that converts the message text line by line into Run and adds them to the Inlines collection.</summary>
        /// <param name="message">Text to add to Inlines.</param>
        protected void DebugAddedText(string message)
        {
            if (Inlines == null || ! IsOutputsText)
                return;

            string[] lines = message.Split(LineSeparators, StringSplitOptions.None);
            if (lines.Length == 0)
                return;

            if (Inlines.Count == 0)
                Inlines.Add(new Run(lines[0]));
            else
                ((Run)Inlines.ElementAt(Inlines.Count - 1)).Text += lines[0];

            for (int i = 1; i < lines.Length; i++)
            {
                ((Run)Inlines.ElementAt(Inlines.Count - 1)).Text += Environment.NewLine;

                Run run = new Run(lines[i]);
                Inlines.Add(run);

                if (Inlines.Count % HighlightedInterval == 0)
                    run.Foreground = Highlighted;
            }
        }
    }

}
