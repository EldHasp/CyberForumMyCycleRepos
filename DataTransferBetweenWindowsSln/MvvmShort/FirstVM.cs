namespace MvvmShort
{
    public class FirstVM : OnPropertyChangedClass
    {
        private string _text;
        private int _number;
        private readonly Model model;

        public string Text { get => _text; set => SetProperty(ref _text, value); }
        public int Number { get => _number; set => SetProperty(ref _number, value); }

        public FirstVM(Model model)
        {
            this.model = model;
            model.ValueChanged += ModelValueChanged;
            model.AllValueChanged();
        }

        private void ModelValueChanged(object sender, string valueName, object oldValue, object newValue)
        {
            switch (valueName)
            {
                case nameof(Model.StringValue): Text = (string)newValue; break;
                case nameof(Model.IntValue): Number = (int)newValue; break;
            }
        }

        protected override void PropertyNewValue<T>(ref T fieldProperty, T newValue, string propertyName)
        {
            base.PropertyNewValue(ref fieldProperty, newValue, propertyName);

            switch (propertyName)
            {
                case nameof(Text): model.SendValue(nameof(Model.StringValue), Text); break;
                case nameof(Number): model.SendValue(nameof(Model.IntValue), Number); break;
            }

        }
    }

}
