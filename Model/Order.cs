namespace Model
{
    public class Order
    {
        public Table Table { get; set; }
        public Employee PlacedBy { get; set; }
        public uint OrderNumber { get; set; }
        public uint ServingNumber { get; set; }
        public bool Finished { get; set; }
        public decimal TotalPrice { get; set; }
        public Order(Table table, Employee placedBy, uint orderNumber, uint servingNumber, bool finished, decimal totalPrice) 
        {
            Table = table;
            PlacedBy = placedBy;
            OrderNumber = orderNumber;
            ServingNumber = servingNumber;
            Finished = finished;
            TotalPrice = totalPrice;
        }
    }
}
