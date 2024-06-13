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
            DataContext = this;
        }

        private void GetAllTables()
        {
            TableService tableService = new();
            List<Table> tables = tableService.GetAllTables();
            SetTables(tables);
            UpdateWaitingTime();
        }

        private List<Order> GetAllRunningOrders()
        {
            return orderService.GetAllRunningOrdersForTables();
        }

        private void SetTables(List<Table> tables)
        {
            List<Order> runningOrders = GetAllRunningOrders();

            for (int tableIndex = 0; tableIndex < tables.Count; tableIndex++)
            {
                int row = tableIndex / 5;
                int col = tableIndex % 5;

                List<Order> ordersPerTable = RunningOrderPerTable(tables[tableIndex], runningOrders);
                Tables.Add(new TableViewModel(tables[tableIndex], row, col, ordersPerTable));
            }
        }

        private void UpdateWaitingTime()
        {
            foreach (TableViewModel table in Tables)
            {
                table.UpdateWaitingTime();
            }
        }

        private List<Order> RunningOrderPerTable(Table table, List<Order> runningOrders)
        {
            return runningOrders.FindAll(order => order.Table.DatabaseId == table.DatabaseId);
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

            //The grid won't be recreated again
            (sender as FrameworkElement).Loaded -= ItemsControlGrid_Loaded;
        }
    }
}