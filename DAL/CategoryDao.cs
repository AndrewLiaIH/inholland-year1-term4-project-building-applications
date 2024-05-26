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

        private MenuCardDao menuCardDao = new();

        public List<Category> GetAllCategories()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllCategories, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public Category GetCategoryById(int categoryId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameCategoryId, categoryId }
            };

            return GetById(QueryGetCategoryById, ReadRow, parameters);
        }

        private Category ReadRow(DataRow dr)
        {
            int categoryId = (int)dr[ColumnCategoryId];
            MenuCard menuCard = menuCardDao.GetMenuCardById((int)dr[ColumnMenuId]);
            CategoryType categoryType;
            Enum.TryParse((string)dr[ColumnCategoryType], out categoryType);
            bool? alcoholic = null;

            if (dr[ColumnAlcoholic] != DBNull.Value)
            {
                alcoholic = (bool)dr[ColumnAlcoholic];
            }

            return new(categoryId, menuCard, categoryType, alcoholic);
        }
    }
}