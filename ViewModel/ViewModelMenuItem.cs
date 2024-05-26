namespace ViewModel
{
    public class ViewModelMenuItem : ViewModelBase
    {
        private string menuItemName;

        public string MenuItemName
        {
            get => menuItemName;
            set
            {
                menuItemName = value;
                OnPropertyChanged(nameof(MenuItemName));
            }
        }
    }
}
