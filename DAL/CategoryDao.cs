using Microsoft.Data.SqlClient;
using Model;
using System.Data;

namespace DAL
{
    public class CategoryDao : BaseDao
    {
        const string QueryGetAllCategories = $"SELECT {ColumnCategoryId}, {ColumnMenuId}, {ColumnCategoryType}, {ColumnAlcoholic} FROM category";
        const string QueryGetCategoryById = $"{QueryGetAllCategories} WHERE {ColumnCategoryId} = @categoryId";
        const string ColumnCategoryId = "category_id";
        const string ColumnMenuId = "menu_id";
        const string ColumnCategoryType = "category_type";
        const string ColumnAlcoholic = "alcoholic";
        const string CategoryErrorMessage = "Unknown category type.";
        MenuCardDao menuCardDao;

        public CategoryDao()
        {
            menuCardDao = new();
        }

        public List<Category> GetAllCategories()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllCategories, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public Category GetCategoryById(uint categoryId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] { new("@categoryId", categoryId) };
            DataTable dataTable = ExecuteSelectQuery(QueryGetCategoryById, sqlParameters);

            return ReadTable(dataTable, ReadRow).FirstOrDefault();
        }

        private Category ReadRow(DataRow dr)
        {
            uint categoryId = (uint)dr[ColumnCategoryId];
            uint menuId = (uint)dr[ColumnMenuId];
            string categoryType = (string)dr[ColumnCategoryType];
            bool alcoholic = (bool)dr[ColumnAlcoholic];

            return new Category(categoryId, menuCardDao.GetMenuCardById(menuId), ConvertToEnum(categoryType), alcoholic);
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