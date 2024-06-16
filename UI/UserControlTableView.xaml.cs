using Model;
using Service;
using System.Windows;
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
        
        private OrderService orderService = new();
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
            orderService.NetworkExceptionOccurred += NetworkExceptionOccurred;
            tableService.NetworkExceptionOccurred += NetworkExceptionOccurred;
        }

        private void NetworkExceptionOccurred()
        {
            Dispatcher.Invoke(() =>
            {
                ShowNetworkErrorView();
                tableService.TableOccupiedChanged += UpdateTables;
                orderService.RunningOrdersChanged += UpdateTables;
            });
        }

        private void UpdateTables()
        {
            Dispatcher.Invoke(async () =>
            {
                tableService.TableOccupiedChanged -= UpdateTables;
                orderService.RunningOrdersChanged -= UpdateTables;

                if (tableService.ConnectionAvalible())
                {
                    await Task.Delay(4000);
                    ShowTableViewTables();
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