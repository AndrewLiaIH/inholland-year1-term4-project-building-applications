namespace Model
{
    public class Category
    {
        public uint CategoryId { get; private set; }
        public uint MenuId { get; private set; }
        public string CategoryType { get; private set; }
        public bool Alcoholic { get; private set; }

        public Category(uint categoryId, uint menuId, string categoryType, bool alcoholic)
        {
            CategoryId = categoryId;
            MenuId = menuId;
            CategoryType = categoryType;
            Alcoholic = alcoholic;
        }

        public override string ToString()
        {
            return $"Category Type: {CategoryType}";
        }
    }
}