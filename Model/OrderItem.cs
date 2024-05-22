namespace Model
{
    public class OrderItem
    {
        public uint OrderId { get; private set; }
        public uint ItemId { get; private set; }
        public DateTime PlacementTime { get; private set; }
        public string Status { get; private set; }
        public DateTime ChangeOfStatus { get; private set; }
        public uint Quantity { get; private set; }
        public string? Comment { get; private set; }

        public OrderItem(uint orderId, uint itemId, DateTime placementTime, string status, DateTime changeOfStatus, uint quantity, string? comment)
        {
            OrderId = orderId;
            ItemId = itemId;
            PlacementTime = placementTime;
            Status = status;
            ChangeOfStatus = changeOfStatus;
            Quantity = quantity;
            Comment = comment;
        }

        public override string ToString()
        {
            return $"Placement time: {PlacementTime}, Status: {Status}, Change of status: {ChangeOfStatus}, Quantity: {Quantity}, Comment: {Comment}";
        }
    }
}