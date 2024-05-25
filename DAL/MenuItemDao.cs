using Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MenuItemDao : BaseDao
    {
        const string QueryGetAllMenuItems = $"SELECT {ColumnItemId}, {ColumnCategoryId}, {ColumnItemNumber}, {ColumnStockAmount}, {ColumnOnMenu}, {ColumnPrice}, {ColumnDescription}, {ColumnName}, {ColumnShortName} FROM menu_item";
        const string QueryGetMenuItemById = $"{QueryGetAllMenuItems} WHERE {ColumnItemId} = {ParameterNameItemId}";

        const string ColumnItemId = "item_id";
        const string ColumnCategoryId = "category_id";
        const string ColumnItemNumber = "item_number";
        const string ColumnStockAmount = "stock_amount";
        const string ColumnOnMenu = "on_menu";
        const string ColumnPrice = "price";
        const string ColumnDescription = "description";
        const string ColumnName = "name";
        const string ColumnShortName = "short_name";

        const string ParameterNameItemId = "@itemId";

        CategoryDao categoryDao = new();

        public List<MenuItem> GetAllMenuItems()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllMenuItems, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public MenuItem GetMenuItemById(uint itemId)
        {
            Dictionary<string, uint> parameters = new()
            {
                { ParameterNameItemId, itemId }
            };

            return GetById(QueryGetMenuItemById, ReadRow, parameters);
        }

        private MenuItem ReadRow(DataRow dr)
        {
            uint itemId = (uint)dr[ColumnItemId];
            Category category = categoryDao.GetCategoryById((uint)dr[ColumnCategoryId]);
            uint itemNumber = (uint)dr[ColumnItemNumber];
            uint stockAmount = (uint)dr[ColumnStockAmount];
            bool onMenu = (bool)dr[ColumnOnMenu];
            decimal price = (decimal)dr[ColumnPrice];
            string description = (string)dr[ColumnDescription];
            string name = (string)dr[ColumnName];
            string shortName = (string)dr[ColumnShortName];

            return new(itemId, category, itemNumber, stockAmount, onMenu, price, description, name, shortName);
        }
    }
}