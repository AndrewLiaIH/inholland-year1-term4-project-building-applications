using Model;
using System.ComponentModel;

namespace UI
{
    //Created by Orest Pokotylenko
    /// <summary>
    /// TableViewModel class is used to represent the table from the Model layer in the TableViewTables user control.
    /// </summary>
    public class TableViewModel : INotifyPropertyChanged
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

        public TableViewModel(Table table,  int rowIndex, int columnIndex)
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