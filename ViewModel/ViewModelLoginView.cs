using Service;
using System.Windows.Input;

namespace ViewModel
{
    public class ViewModelLoginView : ViewModelBase
    {
        private MenuItemService menuItemService;
        private ViewModelMenuItem viewModelMenuItem;

        public ViewModelMenuItem ViewModelMenuItem => viewModelMenuItem;

        public ICommand CommandLogin { get; }
        public ICommand CommandNavigate { get; }

        public ViewModelLoginView(NavigationService navigationService, MenuItemService menuItemService, ViewModelMenuItem viewModelMenuItem)
        {
            this.menuItemService = menuItemService;
            this.viewModelMenuItem = viewModelMenuItem;

            viewModelMenuItem.MenuItemName = menuItemService.GetAllMenuItems().First().Name;

            CommandLogin = new CommandLogin(navigationService);
            CommandNavigate = new CommandNavigate(navigationService);
        }
    }
}
