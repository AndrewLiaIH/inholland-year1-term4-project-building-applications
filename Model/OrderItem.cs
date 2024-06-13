namespace Model
{
    // This class is created by Orest Pokotylenko
    public class OrderItem
    {
        public int DatabaseId { get; private set; }
        public int OrderId { get; private set; }
        public MenuItem Item { get; private set; }
        public DateTime? PlacementTime { get; private set; }
        public Status? ItemStatus { get; private set; }
        public DateTime? ChangeOfStatus { get; private set; }
        public int? Quantity { get; private set; }
        public string? Comment { get; private set; }

        public OrderItem(int databaseId, int orderId, MenuItem item, DateTime? placementTime, Status? status, DateTime? changeOfStatus, int? quantity, string? comment)
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

        public override string ToString()
        {
            return $"Placement time: {PlacementTime}, Status: {ItemStatus}, Change of status: {ChangeOfStatus}, Quantity: {Quantity}, Comment: {Comment}";
        }

        public void IncreaseQuantity()
        {
            Quantity++;
        }

        public void SetItemStatus(Status status)
        {
            ItemStatus = status;
        }
    }
}