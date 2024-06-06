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
        public ObservableCollection<TableProperty> Tables { get; } = new();

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
        }

        private void SetTables(List<Table> tables)
        {
            int tableIndex = 0;

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (tableIndex < tables.Count)
                    {
                        Tables.Add(new TableProperty(tables[tableIndex], i, j));
                        tableIndex++;
                    }
                }
            }
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