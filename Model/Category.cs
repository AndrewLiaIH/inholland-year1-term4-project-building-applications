namespace Model
{
    public class Category
    {
        public uint CategoryId { get; private set; }
        public MenuCard MenuCard { get; private set; }
        public string CategoryType { get; private set; }
        public bool Alcoholic { get; private set; }

        public Category(uint categoryId, MenuCard menuCard, string categoryType, bool alcoholic)
        {
            CategoryId = categoryId;
            MenuCard = menuCard;
            CategoryType = categoryType;
            Alcoholic = alcoholic;
        }

        public override string ToString()
        {
            return $"Category Type: {CategoryType}";
        }
    }
}