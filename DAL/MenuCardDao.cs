using Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class MenuCardDao : BaseDao
    {
        const string QueryGetAllMenuCards = $"SELECT {ColumnCardId}, {ColumnMenuType} FROM menu_card";
        const string QueryGetMenuCardById = $"{QueryGetAllMenuCards} WHERE {ColumnCardId} = {ParameterNameCardId}";

        const string ColumnCardId = "card_id";
        const string ColumnMenuType = "menu_type";

        const string ParameterNameCardId = "@cardId";

        const string MenuCardErrorMessage = "Unknown menu type.";

        public List<MenuCard> GetAllMenuCards()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllMenuCards, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public MenuCard GetMenuCardById(uint cardId)
        {
            Dictionary<string, uint> parameters = new()
            {
                { ParameterNameCardId, cardId }
            };

            return GetById(QueryGetMenuCardById, ReadRow, parameters);
        }

        private MenuCard ReadRow(DataRow dr)
        {
            uint cardId = (uint)dr[ColumnCardId];
            MenuType menuType = ConvertToEnum((string)dr[ColumnMenuType]);

            return new MenuCard(cardId, menuType);
        }

        private MenuType ConvertToEnum(string menuType)
        {
            return menuType switch
            {
                "Lunch" => MenuType.Lunch,
                "Dinner" => MenuType.Dinner,
                "Drinks" => MenuType.Drinks,
                _ => throw new ArgumentException(MenuCardErrorMessage)
            };
        }
    }
}