namespace Model
{
    public class Table
    {
        public uint TableNumber { get; set; }
        public Employee Host { get; set; }
        public bool Occupied { get; set; }

        public Table(uint tableNumber, Employee host, bool occupied)
        {
            TableNumber = tableNumber;
            Host = host;
            Occupied = occupied;
        }
    }
}
