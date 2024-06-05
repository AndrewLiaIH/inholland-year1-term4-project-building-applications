using Model;
using System.ComponentModel;

namespace UI
{
    public class TableProperty : INotifyPropertyChanged
    {
        public Table Table { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

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

        public TableProperty(Table table, int rowIndex, int columnIndex)
        {
            Table = table;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;

            if (table.Occupied)
                TableState = "Occupied";
            else
                TableState = "Free";
        }
    }
}