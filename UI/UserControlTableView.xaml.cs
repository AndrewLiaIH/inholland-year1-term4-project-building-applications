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
        }

        public void SetLoggedInEmployee(Employee employee)
        {
            userControlHeader.LoggedInEmployee = employee;
        }
    }
}