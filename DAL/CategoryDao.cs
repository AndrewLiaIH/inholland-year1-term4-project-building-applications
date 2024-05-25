using Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class CategoryDao : BaseDao
    {
        private const string QueryGetAllCategories = $"SELECT {ColumnCategoryId}, {ColumnMenuId}, {ColumnCategoryType}, {ColumnAlcoholic} FROM category";
        private const string QueryGetCategoryById = $"{QueryGetAllCategories} WHERE {ColumnCategoryId} = {ParameterNameCategoryId}";

        private const string ColumnCategoryId = "category_id";
        private const string ColumnMenuId = "menu_id";
        private const string ColumnCategoryType = "category_type";
        private const string ColumnAlcoholic = "alcoholic";

        private const string ParameterNameCategoryId = "@categoryId";

        private const string CategoryErrorMessage = "Unknown category type.";

        private MenuCardDao menuCardDao = new();

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