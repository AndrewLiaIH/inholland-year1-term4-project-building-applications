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
        private OrderService orderService = new();

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

            for (int row = 0; row < 2; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    List<Order> ordersPerTable = RunningOrderPerTable(tables[tableIndex]);
                    Tables.Add(new TableViewModel(tables[tableIndex], row, col, ordersPerTable));
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

            int numberOfRows = 2;
            int numberOfColumns = 5;

            for (int i = 0; i < numberOfRows; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int j = 0; j < numberOfColumns; j++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
        }
    }
}