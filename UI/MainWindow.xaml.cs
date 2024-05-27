using System.Windows;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

            userControlHeader.Folders = folders;
            userControlHeader.TemporaryFolders = temporaryFolders;
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
    }
}