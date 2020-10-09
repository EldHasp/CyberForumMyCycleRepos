using System.Timers;
using WpfCommands;

namespace Examples
{
    public class ViewModel : BaseINPC
    {
        private int _sum;

        public int Sum { get => _sum; private set => Set(ref _sum, value); }

        /// <summary>Команда лобавления числа к сумме.</summary>
        public WpfRelayCommand<int> AddNumberCommand { get; }
        public ViewModel()
        {
            AddNumberCommand = new WpfRelayCommand<int>(num => Sum += num);
            timer.Interval = 1000;
            timer.Elapsed += (a, b) => TickCommand.IsEnabled ^= true;
        }

        /// <summary>Команда отключающаяся по таймеру.</summary>
        public WpfRelayCommand TickCommand { get; } = new WpfRelayCommand(p => { });

        private readonly Timer timer = new Timer();


    }

}
