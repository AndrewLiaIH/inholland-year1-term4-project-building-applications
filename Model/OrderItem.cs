﻿namespace Model
{
    public class OrderItem
    {
        public Order Order { get; private set; }
        public MenuItem Item { get; private set; }
        public DateTime PlacementTime { get; private set; }
        public string Status { get; private set; }
        public DateTime ChangeOfStatus { get; private set; }
        public uint Quantity { get; private set; }
        public string? Comment { get; private set; }

        public OrderItem(Order order, MenuItem item, DateTime placementTime, string status, DateTime changeOfStatus, uint quantity, string? comment)
        {
            Order = order;
            Item = item;
            PlacementTime = placementTime;
            Status = status;
            ChangeOfStatus = changeOfStatus;
            Quantity = quantity;
            Comment = comment;
        }
    }
}