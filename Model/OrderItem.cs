namespace Model
{
    // This class is created by Orest Pokotylenko
    public class OrderItem
    {
        public int DatabaseId { get; private set; }
        public int OrderId { get; private set; }
        public MenuItem Item { get; private set; }
        public DateTime? PlacementTime { get; private set; }
        public OrderStatus? ItemStatus { get; set; }
        public DateTime? ChangeOfStatus { get; private set; }
        public int? Quantity { get; private set; }
        public string? Comment { get; set; }
        public TimeSpan RunningTime
        {
            get
            {
                return DateTime.Now - PlacementTime.Value;
            }
        }

        public decimal? TotalPrice
        {
            get
            {
                return Item.Price * Quantity;
            }
        }

        public OrderItem(int databaseId, int orderId, MenuItem item, DateTime? placementTime, OrderStatus? status, DateTime? changeOfStatus, int? quantity, string? comment)
        {
            DatabaseId = databaseId;
            OrderId = orderId;
            Item = item;
            PlacementTime = placementTime;
            ItemStatus = status;
            ChangeOfStatus = changeOfStatus;
            Quantity = quantity;
            Comment = comment;
        }

        public OrderItem(int orderId, MenuItem item, DateTime? placementTime, OrderStatus? status, DateTime? changeOfStatus, int? quantity, string? comment)
            : this(0, orderId, item, placementTime, status, changeOfStatus, quantity, comment)
        { }

        public OrderItem(MenuItem item, int quantity)
            : this(0, 0, item, null, null, null, quantity, null)
        { }

        public override string ToString()
        {
            return $"Placement time: {PlacementTime}, Status: {ItemStatus}, Change of status: {ChangeOfStatus}, Quantity: {Quantity}, Comment: {Comment}";
        }

        public void IncreaseQuantity()
        {
            Quantity++;
        }

        public void DecreaseQuantity()
        {
            Quantity--;
        }

        public void SetItemStatus(OrderStatus status)
        {
            ItemStatus = status;
            ChangeOfStatus = DateTime.Now;
        }
    }
}