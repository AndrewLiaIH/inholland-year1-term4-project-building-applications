namespace Model
{
    public class MenuItem
    {
        public int ItemId { get; private set; }
        public Category Category { get; private set; }
        public int ItemNumber { get; private set; }
        public int StockAmount { get; private set; }
        public bool OnMenu { get; private set; }
        public decimal Price { get; private set; }
        public string? Description { get; private set; }
        public string? Name { get; private set; }
        public string? ShortName { get; private set; }

        public MenuItem(int itemId, Category category, int itemNumber, int stockAmount, bool onMenu, decimal price, string? description, string? name, string? shortName)
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

        public override string ToString()
        {
            return $"Name: {Name}, Short name: {ShortName}, Item number: {ItemNumber}, Stock amount: {StockAmount}, Description: {Description}, Price: {Price}";
        }
    }
}