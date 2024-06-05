using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlTable.xaml
    /// </summary>
    public partial class UserControlTable : UserControl, INotifyPropertyChanged
    {
        private TableProperty tableProperty;
        private string tableState;
        public string TableState
        {
            get { return tableState; }
            set
            {
                if (tableState != value)
                {
                    tableState = value;
                    OnPropertyChanged(nameof(TableState));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TableProperty TableProperty
        {
            get { return (TableProperty)GetValue(TablePropertyProperty); }
            set { SetValue(TablePropertyProperty, value); }
        }

        public static readonly DependencyProperty TablePropertyProperty =
            DependencyProperty.Register("TableProperty", typeof(TableProperty), typeof(UserControlTable), new PropertyMetadata(null));

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