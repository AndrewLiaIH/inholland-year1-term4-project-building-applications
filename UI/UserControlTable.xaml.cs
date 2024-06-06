using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlTable.xaml
    /// </summary>
    public partial class UserControlTable : UserControl
    {
        private TableProperty tableProperty;

        public UserControlTable()
        {
            InitializeComponent();
        }

        private void ButtonFree_Click(object sender, RoutedEventArgs e)
        {
            tableProperty.TableState = "Free";
        }

        private void ButtonReserve_Click(object sender, RoutedEventArgs e)
        {
            tableProperty.TableState = "Reserved";
        }

        private void ButtonOrder_Click(object sender, RoutedEventArgs e)
        {
            tableProperty.TableState = "Occupied";
        }

        private void ButtonServed_Click(object sender, RoutedEventArgs e)
        {
            tableProperty.TableState = "Occupied";
        }

        private void ButtonEditOrder_Click(object sender, RoutedEventArgs e)
        {
        }

        private void TableLoaded_Load(object sender, RoutedEventArgs e)
        {
            tableProperty = DataContext as TableProperty;
        }
    }
}