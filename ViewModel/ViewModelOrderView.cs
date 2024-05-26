namespace ViewModel
{
    public class ViewModelOrderView : ViewModelBase
    {
        private string test = "Order";

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
