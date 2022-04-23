using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Localization;

public interface IXmlLocalizerTimer
{
    double Interval { get; set; }
    void Stop();
    void Start();
}
public partial class XmlLocalizer
{
    private class XmlLocalizerTimer : IXmlLocalizerTimer
    {

        private readonly DispatcherTimer timer = new();
        private readonly XmlLocalizer localizer;

        public XmlLocalizerTimer(XmlLocalizer localizer)
        {
            this.localizer = localizer;
            timer.Interval = TimeSpan.FromMilliseconds(100);

            timer.Tick += OnTick;
        }

        private XmlLanguage currentLanguage = defaultCurrentLanguage;
        private void OnTick(object? sender, EventArgs e)
        {
            if (Equals(currentLanguage, localizer.privateCurrentLanguage))
                return;

            currentLanguage = localizer.privateCurrentLanguage;
            if (localizer.App?.Windows.Count is int count && count > 0)
            {
                foreach (Window window in localizer.App.Windows)
                {
                    window.Language = currentLanguage;
                }
            }
        }
        public double Interval
        {
            get => timer.Interval.TotalMilliseconds;
            set => timer.Interval = TimeSpan.FromMilliseconds(value);
        }

        public void Stop() => timer.Stop();
        public void Start() => timer.Start();
    }

    private IXmlLocalizerTimer? _updateTimer;

    public IXmlLocalizerTimer UpdateTimer => _updateTimer ??= new XmlLocalizerTimer(this);
}