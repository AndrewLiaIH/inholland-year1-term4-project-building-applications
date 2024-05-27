namespace Model
{
    public class Order
    {
        public int DatabaseId { get; private set; }
        public Table Table { get; private set; }
        public Employee PlacedBy { get; private set; }
        public int OrderNumber { get; private set; }
        public int? ServingNumber { get; private set; }
        public bool Finished { get; private set; }
        public decimal TotalPrice { get; private set; }
        public List<OrderItem> orderItems { get; private set; }

        public Order(int databaseId, Table table, Employee placedBy, int orderNumber, int? servingNumber, bool finished, decimal totalPrice)
        {
            DatabaseId = databaseId;
            Table = table;
            PlacedBy = placedBy;
            OrderNumber = orderNumber;
            ServingNumber = servingNumber;
            Finished = finished;
            TotalPrice = totalPrice;
            orderItems = new();
        }

        public override string ToString()
        {
            return $"#{OrderNumber}: ${TotalPrice}";
        }

        public void AddOrderItem(OrderItem newItem) 
        {
            if (orderItems.Contains(newItem))
                foreach(OrderItem item in orderItems) 
                    if(newItem.Item.ItemId == item.Item.ItemId && newItem.Comment == item.Comment)
                    {
                        item.IncreaseQuantity();
                        return;
                    }

            orderItems.Add(newItem);
        }
    }
}
