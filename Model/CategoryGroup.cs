﻿namespace Model
{
    public class CategoryGroup
    {
        public string Category { get; set; }
        public OrderStatus? CategoryStatus { get; set; }
        public List<OrderItem> Items { get; set; }
    }
}
