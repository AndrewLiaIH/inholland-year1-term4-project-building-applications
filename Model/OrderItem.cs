namespace Model
{
    // This class is created by Orest Pokotylenko
    public class OrderItem
    {
        public Order Order { get; private set; }
        public MenuItem Item { get; private set; }
        public DateTime? PlacementTime { get; private set; }
        public string? Status { get; private set; }
        public DateTime? ChangeOfStatus { get; private set; }
        public int? Quantity { get; private set; }
        public string? Comment { get; private set; }

        public OrderItem(Order order, MenuItem item, DateTime? placementTime, string? status, DateTime? changeOfStatus, int? quantity, string? comment)
        {
            Order = order;
            Item = item;
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

        public void IncreaseQuantity()
        {
            Quantity++;
        }
    }
}