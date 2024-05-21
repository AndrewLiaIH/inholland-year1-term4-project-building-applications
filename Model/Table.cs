namespace Model
{
    public class Table
    {
        public uint TableNumber { get; private set; }
        public Employee Host { get; private set; }
        public bool Occupied { get; private set; }

        public Table(uint tableNumber, Employee host, bool occupied)
        {
            TableNumber = tableNumber;
            Host = host;
            Occupied = occupied;
        }
    }
}
