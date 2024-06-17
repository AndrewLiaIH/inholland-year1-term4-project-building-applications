using DAL;
using Model;
using Service;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlTableView.xaml
    /// </summary>
    public partial class UserControlTableView : UserControl, ILoggedInEmployeeHandler
    {
        internal List<Folder> FoldersTables;
        public UserControlHeader UserControlHeader => userControlHeader;
        private UserControlTableViewTables userControlTableViewTables;
        private UserControlNetworkError userControlNetworkError;
        
        private TableService tableService = new();

        public UserControlTableView()
        {
            InitializeComponent();

            FoldersTables = new()
            {
                new("Table View", ShowTableViewTables)
            };

            userControlHeader.Folders = FoldersTables;
            userControlHeader.SelectedFolder = FoldersTables.First();
            tableService.NetworkExceptionOccurred += NetworkExceptionOccurred;
        }

        private void NetworkExceptionOccurred()
        {
            Dispatcher.Invoke(() =>
            {
                ShowNetworkErrorView();
                tableService.TableOccupiedChanged += UpdateTables;
            });
        }

        private void UpdateTables()
        {
            Task.Run(async () =>
            {
                tableService.TableOccupiedChanged -= UpdateTables;

            if (tableService.ConnectionAvalible<TableDao>())
                {
                    await Task.Delay(6000);

                    Dispatcher.Invoke(() =>
                    {
                        ShowTableViewTables();
                    });
                }
            });
        }

        public void SetLoggedInEmployee(Employee employee)
        {
            userControlHeader.LoggedInEmployee = employee;
        }

        private void ShowTableViewTables()
        {
            userControlTableViewTables ??= new();
            TableViewContentControl.Content = userControlTableViewTables;
        }

        private void ShowNetworkErrorView()
        {
            userControlNetworkError ??= new();
            TableViewContentControl.Content = userControlNetworkError;
        }
    }
}