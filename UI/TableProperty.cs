using Model;

namespace UI
{
    public class TableProperty
    {
        public Table Table { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public string TableState { get; set; }

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