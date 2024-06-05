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
        private string tableState;
        public string TableState
        {
            get { return tableState; }
            set
            {
                if (tableState != value)
                {
                    tableState = value;
                    OnPropertyChanged("TableState");
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
    }
}