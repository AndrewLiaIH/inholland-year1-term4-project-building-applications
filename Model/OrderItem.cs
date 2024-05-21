namespace Model
{
    public class OrderItem
    {
        public int OrderId { get; private set; }
        public int ItemId { get; private set; }
        public DateTime PlacementTime { get; private set; }
        public string Status { get; private set; }
        public DateTime ChangeOfStatus { get; private set; }
        public int Quantity { get; private set; }
        public string? Comment { get; private set; }

        public OrderItem(int orderId, int itemId, DateTime placementTime, string status, DateTime changeOfStatus, int quantity, string? comment)
        {
            OrderId = orderId;
            ItemId = itemId;
            PlacementTime = placementTime;
            Status = status;
            ChangeOfStatus = changeOfStatus;
            Quantity = quantity;
            Comment = comment;
        }
    }
}