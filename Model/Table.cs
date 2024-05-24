namespace Model
{
    public class Table
    {
        public uint DatabaseId { get; private set; }
        public Employee Host { get; private set; }
        public bool Occupied { get; private set; }
        public uint TableNumber { get; private set; }

        public Table(uint databaseId, Employee host, bool occupied, uint tableNumber)
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
