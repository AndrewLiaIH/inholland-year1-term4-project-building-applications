using Microsoft.Data.SqlClient;
using Model;
using System.Data;

namespace DAL
{
    public class OrderItemDao : BaseDao
    {
        private const string QueryGetAllOrderItems = $"SELECT {ColumnOrderId}, {ColumnItemId}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item";
        private const string QueryGetOrderItemById = $"{QueryGetAllOrderItems} WHERE {ColumnOrderId} = {ParameterNameOrderId} AND {ColumnItemId} = {ParameterNameItemId}";

        private const string ColumnOrderId = "order_id";
        private const string ColumnItemId = "item_id";
        private const string ColumnPlacementTime = "placement_time";
        private const string ColumnStatus = "status";
        private const string ColumnChangeOfStatus = "change_of_status";
        private const string ColumnQuantity = "quantity";
        private const string ColumnComment = "comment";

        private const string ParameterNameOrderId = "@orderId";
        private const string ParameterNameItemId = "@itemId";

        private OrderDao orderDao = new();
        private MenuItemDao menuItemDao = new();

        public List<OrderItem> GetAllOrderItems()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllOrderItems, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public OrderItem GetOrderItemById(int orderId, int itemId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameOrderId, orderId },
                { ParameterNameItemId, itemId }
            };

            return GetById(QueryGetOrderItemById, ReadRow, parameters);
        }

        private OrderItem ReadRow(DataRow dr)
        {
            Order order = orderDao.GetOrderById((int)dr[ColumnOrderId]);
            MenuItem menuItem = menuItemDao.GetMenuItemById((int)dr[ColumnItemId]);
            DateTime? placementTime = dr[ColumnPlacementTime] as DateTime?;
            string? status = dr[ColumnStatus] as string;
            DateTime? changeOfStatus = dr[ColumnChangeOfStatus] as DateTime?;
            int? quantity = dr[ColumnQuantity] as int?;
            string? comment = dr[ColumnComment] as string;

            return new(order, menuItem, placementTime, status, changeOfStatus, quantity, comment);
        }
    }
}