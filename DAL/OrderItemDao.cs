using Microsoft.Data.SqlClient;
using Model;
using System.Data;

namespace DAL
{
    public class OrderItemDao : BaseDao
    {
        const string QueryGetAllOrderItems = $"SELECT {ColumnOrderId}, {ColumnItemId}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item";
        const string QueryGetOrderItemById = $"{QueryGetAllOrderItems} WHERE {ColumnOrderId} = @orderId AND {ColumnItemId} = @itemId";
        const string ColumnOrderId = "order_id";
        const string ColumnItemId = "item_id";
        const string ColumnPlacementTime = "placement_time";
        const string ColumnStatus = "status";
        const string ColumnChangeOfStatus = "change_of_status";
        const string ColumnQuantity = "quantity";
        const string ColumnComment = "comment";
/*        OrderDao orderDao;*/
        MenuItemDao menuItemDao;

        public OrderItemDao()
        {
/*            OrderDao = new();*/
            menuItemDao = new();
        }

        public List<OrderItem> GetAllOrderItems()
        {
            SqlParameter[] sqlParameters = Array.Empty<SqlParameter>();
            DataTable dataTable = ExecuteSelectQuery(QueryGetAllOrderItems, sqlParameters);
            return ReadTable(dataTable, ReadRow);
        }

        public OrderItem GetOrderItemById(uint orderId, uint itemId)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] { new("@orderId", orderId), new("@itemId", itemId) };
            DataTable dataTable = ExecuteSelectQuery(QueryGetOrderItemById, sqlParameters);

            return ReadTable(dataTable, ReadRow).FirstOrDefault();
        }

        private OrderItem ReadRow(DataRow dr)
        {
            uint orderId = (uint)dr[ColumnOrderId];
            uint itemId = (uint)dr[ColumnItemId];
            DateTime placementTime = (DateTime)dr[ColumnPlacementTime];
            string status = (string)dr[ColumnStatus];
            DateTime changeOfStatus = (DateTime)dr[ColumnChangeOfStatus];
            uint quantity = (uint)dr[ColumnQuantity];
            string? comment = (string?)dr[ColumnComment];

            return new OrderItem(orderId, menuItemDao.GetMenuItemById(itemId), placementTime, status, changeOfStatus, quantity, comment);
        }
    }
}