using Microsoft.Data.SqlClient;
using Model;
using System.Data;

namespace DAL
{
    public class MenuCardDao : BaseDao
    {
        private const string QueryGetAllMenuCards = $"SELECT {ColumnCardId}, {ColumnMenuType} FROM menu_card";
        private const string QueryGetMenuCardById = $"{QueryGetAllMenuCards} WHERE {ColumnCardId} = {ParameterNameCardId}";

        private const string ColumnCardId = "card_id";
        private const string ColumnMenuType = "menu_type";

        private const string ParameterNameCardId = "@cardId";

        public List<MenuCard> GetAllMenuCards()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllMenuCards, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public MenuCard GetMenuCardById(int cardId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameCardId, cardId }
            };

            return GetById(QueryGetMenuCardById, ReadRow, parameters);
        }

        private MenuCard ReadRow(DataRow dr)
        {
            int cardId = (int)dr[ColumnCardId];
            bool parsedMenuType = Enum.TryParse((string)dr[ColumnMenuType], out MenuType menuType);
            return new MenuCard(cardId, menuType);
        }
    }
}