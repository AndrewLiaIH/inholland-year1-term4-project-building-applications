namespace ViewModel
{
    public class ViewModelKitchenView : ViewModelBase
    {
        private string test = "Kitchen";

        public string Test
        {
            get => test;

            set
            {
                test = value;
                OnPropertyChanged(nameof(Test));
            }
        }
    }
}
