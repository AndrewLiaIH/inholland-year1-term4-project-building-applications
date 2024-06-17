using Model;
using System.Data;
using Microsoft.Data.SqlClient;

namespace DAL
{
    public class MenuDao : BaseDao
    {
        // Menu Card
        private const string QueryGetAllMenuCards = $"SELECT {ColumnCardId}, {ColumnMenuType} FROM menu_card";
        private const string QueryGetMenuCardById = $"{QueryGetAllMenuCards} WHERE {ColumnCardId} = {ParameterNameCardId}";

        private const string ColumnCardId = "card_id";
        private const string ColumnMenuType = "menu_type";

        private const string ParameterNameCardId = "@cardId";

        // Category
        private const string QueryGetAllCategories = $"SELECT {ColumnCategoryId}, {ColumnMenuId}, {ColumnCategoryType}, {ColumnAlcoholic} FROM category";
        private const string QueryGetCategoryById = $"{QueryGetAllCategories} WHERE {ColumnCategoryId} = {ParameterNameCategoryId}";

        private const string ColumnCategoryId = "category_id";
        private const string ColumnMenuId = "menu_id";
        private const string ColumnCategoryType = "category_type";
        private const string ColumnAlcoholic = "alcoholic";

        private const string ParameterNameCategoryId = "@categoryId";

        // Menu Item
        private const string QueryGetAllMenuItems = $"SELECT {ColumnItemId}, {ColumnItemCategoryId}, {ColumnItemNumber}, {ColumnStockAmount}, {ColumnOnMenu}, {ColumnPrice}, {ColumnDescription}, {ColumnName}, {ColumnShortName} FROM menu_item";
        private const string QueryGetMenuItemById = $"{QueryGetAllMenuItems} WHERE {ColumnItemId} = {ParameterNameItemId}";
        private const string QueryUpdateStock = $"UPDATE menu_item SET {ColumnStockAmount} = {ParameterStockAmount} WHERE {ColumnItemId} = {ParameterNameItemId}";

        private const string ColumnItemId = "item_id";
        private const string ColumnItemCategoryId = "category_id";
        private const string ColumnItemNumber = "item_number";
        private const string ColumnStockAmount = "stock_amount";
        private const string ColumnOnMenu = "on_menu";
        private const string ColumnPrice = "price";
        private const string ColumnDescription = "description";
        private const string ColumnName = "name";
        private const string ColumnShortName = "short_name";

        private const string ParameterNameItemId = "@itemId";
        private const string ParameterStockAmount = "@stockAmount";

        public List<MenuCard> GetAllMenuCards()
        {
            return GetAll(QueryGetAllMenuCards, ReadRowMenuCard);
        }

        public MenuCard GetMenuCardById(int cardId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameCardId, cardId }
            };

            return GetByIntParameters(QueryGetMenuCardById, ReadRowMenuCard, parameters);
        }

        public List<Category> GetAllCategories()
        {
            return GetAll(QueryGetAllCategories, ReadRowCategory);
        }

        public Category GetCategoryById(int categoryId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameCategoryId, categoryId }
            };

            return GetByIntParameters(QueryGetCategoryById, ReadRowCategory, parameters);
        }

        public List<MenuItem> GetAllMenuItems()
        {
            return GetAll(QueryGetAllMenuItems, ReadRowMenuItem);
        }

        public MenuItem GetMenuItemById(int itemId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameItemId, itemId }
            };

            return GetByIntParameters(QueryGetMenuItemById, ReadRowMenuItem, parameters);
        }

        public void UpdateStock(int itemId, int newStock)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new(ParameterNameItemId, itemId),
                new(ParameterStockAmount, newStock)
            };

            ExecuteEditQuery(QueryUpdateStock, parameters);
        }

        private MenuCard ReadRowMenuCard(DataRow dr)
        {
            int cardId = (int)dr[ColumnCardId];
            bool parsedMenuType = Enum.TryParse((string)dr[ColumnMenuType], out MenuType menuType);
            return new MenuCard(cardId, menuType);
        }

        private Category ReadRowCategory(DataRow dr)
        {
            int categoryId = (int)dr[ColumnCategoryId];
            MenuCard menuCard = GetMenuCardById((int)dr[ColumnMenuId]);
            bool parsedCategoryType = Enum.TryParse((string)dr[ColumnCategoryType], true, out CategoryType categoryType);
            bool? alcoholic = dr[ColumnAlcoholic] as bool?;

            return new(categoryId, menuCard, categoryType, alcoholic);
        }

        private MenuItem ReadRowMenuItem(DataRow dr)
        {
            int itemId = (int)dr[ColumnItemId];
            Category category = GetCategoryById((int)dr[ColumnCategoryId]);
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
