namespace Model
{
    public class Category
    {
        public int CategoryId { get; private set; }
        public int MenuId { get; private set; }
        public string CategoryType { get; private set; }
        public bool Alcoholic { get; private set; }

        public Category(int categoryId, int menuId, string categoryType, bool alcoholic)
        {
            CategoryId = categoryId;
            MenuId = menuId;
            CategoryType = categoryType;
            Alcoholic = alcoholic;
        }
    }
}