using Model;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string WindowViewError = "Error occupied while displaying of a window";
        private Dictionary<EmployeeType, UserControl> VisibilityMap;

        public MainWindow()
        {
            InitializeComponent();

            List<Folder> folders = new()
            {
                new Folder { Name = "Running" },
                new Folder { Name = "Finished" }
            };

            List<Folder> temporaryFolders = new()
            {
                new Folder { Name = "Login View", Type = "LoginView" },
                new Folder { Name = "Table View", Type = "TableView" },
                new Folder { Name = "Order View", Type = "OrderView" },
                new Folder { Name = "Kitchen View", Type = "KitchenView" }
            };

            Dictionary<EmployeeType, UserControl> visibilityMap = new()
            {
                { EmployeeType.NotSpecified, userControlTableView },
                { EmployeeType.Manager, userControlTableView },
                { EmployeeType.Chef, userControlKitchenView },
                { EmployeeType.Bartender, userControlKitchenView },
                { EmployeeType.Waiter, userControlTableView }
            };

            userControlHeader.Folders = folders;
            userControlHeader.TemporaryFolders = temporaryFolders;
            VisibilityMap = visibilityMap;
        }

        private void UserControlHeader_SelectedFolderChanged(object sender, RoutedEventArgs e)
        {
            var selectedFolder = userControlHeader.SelectedFolder;
            if (selectedFolder != null)
            {
                // Hide all views
                userControlLoginView.Visibility = Visibility.Hidden;
                userControlTableView.Visibility = Visibility.Hidden;
                userControlOrderView.Visibility = Visibility.Hidden;
                userControlKitchenView.Visibility = Visibility.Hidden;

                // Show the selected view
                switch (selectedFolder.Type)
                {
                    case "LoginView":
                        userControlLoginView.Visibility = Visibility.Visible;
                        break;
                    case "TableView":
                        userControlTableView.Visibility = Visibility.Visible;
                        break;
                    case "OrderView":
                        userControlOrderView.Visibility = Visibility.Visible;
                        break;
                    case "KitchenView":
                        userControlKitchenView.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void UserControlLoginView_LoginSuccessful(object sender, RoutedEventArgs e)
        {
            UpdateCurrentView(userControlLoginView.LoggedInEmployee);
        }

        public void UpdateCurrentView(Employee currentEmployee)
        {
            if (VisibilityMap.TryGetValue(currentEmployee.Type, out UserControl currentView))
            {
                foreach (var visibility in VisibilityMap)
                {
                    visibility.Value.Visibility = Visibility.Hidden;
                }

                currentView.Visibility = Visibility.Visible;
            }
            else
            {
                throw new Exception(WindowViewError);
            }
        }
    }
}