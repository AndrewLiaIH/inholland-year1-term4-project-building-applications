using Microsoft.Data.SqlClient;
using Model;
using System.Data;

namespace DAL
{
    public class MenuCardDao : BaseDao
    {
        const string QueryGetAllMenuCards = $"SELECT {ColumnCardId}, {ColumnMenuType} FROM menu_card";
        const string QueryGetMenuCardById = $"{QueryGetAllMenuCards} WHERE {ColumnCardId} = @cardId";
        const string ColumnCardId = "card_id";
        const string ColumnMenuType = "menu_type";
        const string MenuCardErrorMessage = "Invalid menu type.";

        public List<MenuCard> GetAllMenuCards()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllMenuCards, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public MenuCard GetMenuCardById(uint cardId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] { new("@cardId", cardId) };
            DataTable dataTable = ExecuteSelectQuery(QueryGetMenuCardById, sqlParameters);

            return ReadTable(dataTable, ReadRow).FirstOrDefault();
        }

        private MenuCard ReadRow(DataRow dr)
        {
            uint cardId = (uint)dr[ColumnCardId];
            string menuType = (string)dr[ColumnMenuType];

            return new MenuCard(cardId, ConvertToEnum(menuType));
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