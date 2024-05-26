using Service;
using System.Windows;
using ViewModel;

namespace View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NavigationStore navigationStore;
        private MenuItemService menuItemService;
        private NavigationService navigationService;
        private ViewModelMenuItem viewModelMenuItem;

        public App()
        {
            navigationStore = new NavigationStore();
            menuItemService = new MenuItemService();
            navigationService = new NavigationService(navigationStore, CreateViewModelTable);
            viewModelMenuItem = new ViewModelMenuItem();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            navigationStore.CurrentViewModel = CreateViewModelLogin();

            MainWindow = new MainWindow()
            {
                DataContext = new ViewModelMainWindow(navigationStore)
            };
            MainWindow.WindowState = WindowState.Maximized;
            MainWindow.WindowStyle = WindowStyle.None;
            MainWindow.Show();

            base.OnStartup(e);
        }

        private ViewModelTableView CreateViewModelTable()
        {
            return new ViewModelTableView();
        }

        private ViewModelLoginView CreateViewModelLogin()
        {
            return new ViewModelLoginView(navigationService, menuItemService, viewModelMenuItem);
        }
    }
}
