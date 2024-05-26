namespace ViewModel
{
    public class ViewModelTableView : ViewModelBase
    {
        private string test = "Table";

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
