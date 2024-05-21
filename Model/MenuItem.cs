﻿namespace Model
{
    public class MenuItem
    {
        public uint ItemId { get; private set; }
        public Category Category { get; private set; }
        public uint ItemNumber { get; private set; }
        public uint StockAmount { get; private set; }
        public bool OnMenu { get; private set; }
        public decimal Price { get; private set; }
        public string? Description { get; private set; }
        public string? Name { get; private set; }
        public string? ShortName { get; private set; }

        public MenuItem(uint itemId, Category category, uint itemNumber, uint stockAmount, bool onMenu, decimal price, string? description, string? name, string? shortName)
        {
            ItemId = itemId;
            Category = category;
            ItemNumber = itemNumber;
            StockAmount = stockAmount;
            OnMenu = onMenu;
            Price = price;
            Description = description;
            Name = name;
            ShortName = shortName;
        }
    }
}