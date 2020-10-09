namespace MvvmShort
{
    public class Locator
    {

        private readonly Model model = new Model();

        public FirstVM FirstVM { get; }
        public SecondVM SecondVM { get; }

        public Locator()
        {
            FirstVM = new FirstVM(model)
            { 
                Text = "Начало !!!",
                Number= 12345
            };

            SecondVM = new SecondVM(model);
        }
    }
}
