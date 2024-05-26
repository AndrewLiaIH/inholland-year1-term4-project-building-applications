namespace Model
{
    public class Table
    {
        public int DatabaseId { get; private set; }
        public Employee Host { get; private set; }
        public bool Occupied { get; private set; }
        public int TableNumber { get; private set; }

        public Table(int databaseId, Employee host, bool occupied, int tableNumber)
        {
            DatabaseId = databaseId;
            Host = host;
            Occupied = occupied;
            TableNumber = tableNumber;
        }

        public override string ToString()
        {
            return $"#{TableNumber}, hosted by {Host.FirstName} {Host.LastName}";
        }
    }
}
