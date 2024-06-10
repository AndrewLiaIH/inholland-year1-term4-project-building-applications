using Service;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using Model;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlTableViewTables.xaml
    /// </summary>
    public partial class UserControlTableViewTables : UserControl
    {
        public ObservableCollection<TableViewModel> Tables { get; } = new();
        OrderService orderService = new();

        public UserControlTableViewTables()
        {
            InitializeComponent();

            GetAllTables();
            UpdateWaitingTime();
            DataContext = this;
        }

        private void GetAllTables()
        {
            TableService tableService = new();
            List<Table> tables = tableService.GetAllTables();
            SetTables(tables);
        }

        private List<Order> GetAllRunningOrders()
        {
            return orderService.GetAllRunningOrders();
        }

        private void SetTables(List<Table> tables)
        {
            int tableIndex = 0;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    List<Order> ordersPerTable = RunningOrderPerTable(tables[tableIndex]);
                    Tables.Add(new TableViewModel(tables[tableIndex], i, j, ordersPerTable));
                    tableIndex++;
                }
            }
        }

        private void UpdateWaitingTime()
        {
            foreach (TableViewModel table in Tables)
            {
                table.UpdateWaitingTime();
            }
        }

        private List<Order> RunningOrderPerTable(Table table)
        {
            List<Order> runningOrders = GetAllRunningOrders();
            List<Order> ordersPerTable = runningOrders.FindAll(order => order.Table.DatabaseId == table.DatabaseId);

            return ordersPerTable;
        }

        private void ItemsControlGrid_Loaded(object sender, RoutedEventArgs e)
        {
            Grid grid = sender as Grid;

            for (int i = 0; i < 2; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int j = 0; j < 5; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
        }
    }
}