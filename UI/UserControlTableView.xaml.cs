using Model;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlTableView.xaml
    /// </summary>
    public partial class UserControlTableView : UserControl
    {
        public List<Folder> FoldersTables;

        public UserControlTableView()
        {
            InitializeComponent();

            FoldersTables = new()
            {
                new("Table View", userControlTableViewTables)
            };

            userControlHeader.Folders = FoldersTables;
            userControlHeader.SelectedFolder = FoldersTables.First();
            userControlHeader.SelectedFolderChanged += UserControlHeader_SelectedFolderChanged;
        }

        private void UserControlHeader_SelectedFolderChanged(object sender, RoutedEventArgs e)
        {
            Folder selectedFolder = userControlHeader.SelectedFolder;

            if (selectedFolder != null)
            {
                foreach (Folder folder in FoldersTables)
                {
                    folder.UserControl.Visibility = Visibility.Hidden;
                    folder.IsActive = false;
                }

                selectedFolder.UserControl.Visibility = Visibility.Visible;
                selectedFolder.IsActive = true;
            }
        }

        public void SetLoggedInEmployee(Employee employee)
        {
            userControlHeader.LoggedInEmployee = employee;
        }
    }
}