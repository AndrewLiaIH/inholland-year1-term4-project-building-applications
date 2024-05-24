namespace Model
{
    public class Order
    {
        public uint DatabaseId { get; private set; }
        public Table Table { get; private set; }
        public Employee PlacedBy { get; private set; }
        public uint OrderNumber { get; private set; }
        public uint? ServingNumber { get; private set; }
        public bool Finished { get; private set; }
        public decimal TotalPrice { get; private set; }

        public Order(uint databaseId, Table table, Employee placedBy, uint orderNumber, uint servingNumber, bool finished, decimal totalPrice)
        {
            DatabaseId = databaseId;
            Table = table;
            PlacedBy = placedBy;
            OrderNumber = orderNumber;
            ServingNumber = servingNumber;
            Finished = finished;
            TotalPrice = totalPrice;
        }

        public override string ToString()
        {
            return $"#{OrderNumber}: ${TotalPrice}";
        }
    }
}
