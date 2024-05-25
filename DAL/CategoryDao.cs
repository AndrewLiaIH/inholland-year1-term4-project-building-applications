using Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CategoryDao : BaseDao
    {
        const string QueryGetAllCategories = $"SELECT {ColumnCategoryId}, {ColumnMenuId}, {ColumnCategoryType}, {ColumnAlcoholic} FROM category";
        const string QueryGetCategoryById = $"{QueryGetAllCategories} WHERE {ColumnCategoryId} = {ParameterNameCategoryId}";

        const string ColumnCategoryId = "category_id";
        const string ColumnMenuId = "menu_id";
        const string ColumnCategoryType = "category_type";
        const string ColumnAlcoholic = "alcoholic";

        const string ParameterNameCategoryId = "@categoryId";

        const string CategoryErrorMessage = "Unknown category type.";

        MenuCardDao menuCardDao = new();

        public List<Category> GetAllCategories()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllCategories, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public Category GetCategoryById(uint categoryId)
        {
            Dictionary<string, uint> parameters = new()
            {
                { ParameterNameCategoryId, categoryId }
            };

            return GetById(QueryGetCategoryById, ReadRow, parameters);
        }

        private Category ReadRow(DataRow dr)
        {
            uint categoryId = (uint)dr[ColumnCategoryId];
            MenuCard menuCard = menuCardDao.GetMenuCardById((uint)dr[ColumnMenuId]);
            CategoryType categoryType = ConvertToEnum((string)dr[ColumnCategoryType]);
            bool alcoholic = (bool)dr[ColumnAlcoholic];

            return new(categoryId, menuCard, categoryType, alcoholic);
        }

        private CategoryType ConvertToEnum(string categoryType)
        {
            return categoryType switch
            {
                "Starters" => CategoryType.Starters,
                "Entrements" => CategoryType.Entrements,
                "Main" => CategoryType.Mains,
                "Desserts" => CategoryType.Desserts,
                "Soft drinks" => CategoryType.SoftDrinks,
                "Beers on tap" => CategoryType.BeersOnTap,
                "Wines" => CategoryType.Wines,
                "Spirit drinks" => CategoryType.SpiritDrinks,
                "Coffee/Tea" => CategoryType.CoffeeTea,
                _ => throw new ArgumentException(CategoryErrorMessage)
            };
        }
    }
}