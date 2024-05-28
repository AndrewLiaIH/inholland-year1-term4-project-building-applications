using Microsoft.Data.SqlClient;
using Model;
using System.Data;

namespace DAL
{
    public class MenuItemDao : BaseDao
    {
        private const string QueryGetAllMenuItems = $"SELECT {ColumnItemId}, {ColumnCategoryId}, {ColumnItemNumber}, {ColumnStockAmount}, {ColumnOnMenu}, {ColumnPrice}, {ColumnDescription}, {ColumnName}, {ColumnShortName} FROM menu_item";
        private const string QueryGetMenuItemById = $"{QueryGetAllMenuItems} WHERE {ColumnItemId} = {ParameterNameItemId}";

        private const string ColumnItemId = "item_id";
        private const string ColumnCategoryId = "category_id";
        private const string ColumnItemNumber = "item_number";
        private const string ColumnStockAmount = "stock_amount";
        private const string ColumnOnMenu = "on_menu";
        private const string ColumnPrice = "price";
        private const string ColumnDescription = "description";
        private const string ColumnName = "name";
        private const string ColumnShortName = "short_name";

        private const string ParameterNameItemId = "@itemId";

        private CategoryDao categoryDao = new();

        public List<MenuItem> GetAllMenuItems()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllMenuItems, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public MenuItem GetMenuItemById(int itemId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameItemId, itemId }
            };

            return GetById(QueryGetMenuItemById, ReadRow, parameters);
        }

        private MenuItem ReadRow(DataRow dr)
        {
            int itemId = (int)dr[ColumnItemId];
            Category category = categoryDao.GetCategoryById((int)dr[ColumnCategoryId]);
            int? itemNumber = dr[ColumnItemNumber] as int?;
            int? stockAmount = dr[ColumnStockAmount] as int?;
            bool onMenu = (bool)dr[ColumnOnMenu];
            decimal? price = dr[ColumnPrice] as decimal?;
            string? description = dr[ColumnDescription] as string;
            string? name = dr[ColumnName] as string;
            string? shortName = dr[ColumnShortName] as string;

            return new(itemId, category, itemNumber, stockAmount, onMenu, price, description, name, shortName);
        }
    }
}