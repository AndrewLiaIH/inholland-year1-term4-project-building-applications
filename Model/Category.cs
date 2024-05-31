namespace Model
{
    // This class is created by Orest Pokotylenko
    public class Category
    {
        public int CategoryId { get; private set; }
        public MenuCard MenuCard { get; private set; }
        public CategoryType CategoryType { get; private set; }
        public bool? Alcoholic { get; private set; }

        public Category(int categoryId, MenuCard menuCard, CategoryType categoryType, bool? alcoholic)
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