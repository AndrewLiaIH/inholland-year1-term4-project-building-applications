using Model;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class OrderItemDao : BaseDao
    {
        const string QueryGetAllOrderItems = $"SELECT {ColumnOrderId}, {ColumnItemId}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item";
        const string QueryGetOrderItemById = $"{QueryGetAllOrderItems} WHERE {ColumnOrderId} = {ParameterNameOrderId} AND {ColumnItemId} = {ParameterNameItemId}";

        const string ColumnOrderId = "order_id";
        const string ColumnItemId = "item_id";
        const string ColumnPlacementTime = "placement_time";
        const string ColumnStatus = "status";
        const string ColumnChangeOfStatus = "change_of_status";
        const string ColumnQuantity = "quantity";
        const string ColumnComment = "comment";

        const string ParameterNameOrderId = "@OrderId";
        const string ParameterNameItemId = "@ItemId";

        OrderDao orderDao;
        MenuItemDao menuItemDao;

        public OrderItemDao()
        {
            orderDao = new();
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
            Dictionary<string, uint> parameters = new Dictionary<string, uint>()
            {
                { ParameterNameOrderId, orderId },
                { ParameterNameItemId, itemId }
            };

            return GetById(QueryGetOrderItemById, ReadRow, parameters);
        }

        private OrderItem ReadRow(DataRow dr)
        {
            Order order = orderDao.GetOrderById((uint)dr[ColumnOrderId]);
            MenuItem menuItem = menuItemDao.GetMenuItemById((uint)dr[ColumnItemId]);
            DateTime placementTime = (DateTime)dr[ColumnPlacementTime];
            string status = (string)dr[ColumnStatus];
            DateTime changeOfStatus = (DateTime)dr[ColumnChangeOfStatus];
            uint quantity = (uint)dr[ColumnQuantity];
            string? comment = (string?)dr[ColumnComment];

            return new OrderItem(order, menuItem, placementTime, status, changeOfStatus, quantity, comment);
        }
    }
}