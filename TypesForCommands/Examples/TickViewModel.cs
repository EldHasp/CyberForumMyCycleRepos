using System.Timers;
using WpfCommands;

namespace Examples
{
    public class TickViewModel
    {
        public WpfRelayCommand TickCommand { get; } = new WpfRelayCommand(p => { });

        private readonly Timer timer = new Timer(1000);
        public TickViewModel()
        {
            // Событие Elapsed создаётся в произвольном потоке.
            timer.Elapsed += (a, b) => TickCommand.IsEnabled ^= true;
            timer.Start();
        }
    }
}
