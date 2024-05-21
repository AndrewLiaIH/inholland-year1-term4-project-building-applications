namespace Model
{
    public class MenuItem
    {
        public uint ItemId { get; private set; }
        public string CategoryId { get; private set; }
        public uint ItemNumber { get; private set; }
        public uint StockAmount { get; private set; }
        public bool OnMenu { get; private set; }
        public decimal Price { get; private set; }
        public string? Description { get; private set; }
        public string? Name { get; private set; }
        public string? ShortName { get; private set; }

        public MenuItem(uint itemId, string categoryId, uint itemNumber, uint stockAmount, bool onMenu, decimal price, string? description, string? name, string? shortName)
        {
            ItemId = itemId;
            CategoryId = categoryId;
            ItemNumber = itemNumber;
            StockAmount = stockAmount;
            OnMenu = onMenu;
            Price = price;
            Description = description;
            Name = name;
            ShortName = shortName;
        }

        public override string ToString()
        {
            return $"Name: {Name}, Short name: {ShortName}, Item number: {ItemNumber}, Stock amount: {StockAmount}, Description: {Description}, Price: {Price}";
        }
    }
}