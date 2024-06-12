using Model;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlTableView.xaml
    /// </summary>
    public partial class UserControlTableView : UserControl, ILoggedInEmployeeHandler
    {
        public List<Folder> FoldersTables;
        public UserControlHeader UserControlHeader => userControlHeader;
        private UserControlTableViewTables userControlTableViewTables;

        public UserControlTableView()
        {
            InitializeComponent();

            FoldersTables = new()
            {
                new("Table View", ShowTableViewTables)
            };

            userControlHeader.Folders = FoldersTables;
            userControlHeader.SelectedFolder = FoldersTables.First();
        }

        public void SetLoggedInEmployee(Employee employee)
        {
            userControlHeader.LoggedInEmployee = employee;
        }

        private void ShowTableViewTables()
        {
            if (userControlTableViewTables == null)
                userControlTableViewTables = new();

            TableViewContentControl.Content = userControlTableViewTables;
        }
    }
}