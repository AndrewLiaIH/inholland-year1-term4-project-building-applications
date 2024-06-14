using Service;
using System.Windows.Controls;
using System.Windows;
using System.Collections.ObjectModel;
using Model;

namespace UI
{
    /// <summary>
    /// This class was created by Orest Pokotylenko. It handles the creation and displaying of tables
    /// </summary>
    public partial class UserControlTableViewTables : UserControl
    {
        public ObservableCollection<TableViewModel> Tables { get; } = new();
        private OrderService orderService = new();
        private TableService tableService = new();

        public UserControlTableViewTables()
        {
            InitializeComponent();
            GetAllTables();
            DataContext = this;
        }

        private void GetAllTables()
        {
            List<Table> tables = tableService.GetAllTables();
            SetTables(tables);
        }

        private void SetTables(List<Table> tables)
        {
            for (int tableIndex = 0; tableIndex < tables.Count; tableIndex++)
            {
                int row = tableIndex / 5;
                int col = tableIndex % 5;

                List<Order> ordersPerTable = orderService.GetAllRunningOrdersForTable(tables[tableIndex]);
                Tables.Add(new TableViewModel(tables[tableIndex], row, col, ordersPerTable));
                Tables[tableIndex].UpdateWaitingTime();
            }
        }

        /// <summary>
        /// This event creates the grid on load to make it possible to display tables dynamically
        /// </summary>
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