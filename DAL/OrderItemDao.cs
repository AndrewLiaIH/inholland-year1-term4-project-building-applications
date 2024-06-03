using Model;
using System.Data;

namespace DAL
{
    // This class is created by Orest Pokotylenko
    public class OrderItemDao : BaseDao
    {
        private const string QueryGetAllOrderItems = $"SELECT {ColumnOrderItemId}, {ColumnOrderNumber}, {ColumnItemNumber}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item";
        private const string QueryGetOrderItemById = $"{QueryGetAllOrderItems} WHERE {ColumnOrderItemId} = {ParameterNameOrderItemId}";
        private const string QueryGetAllItemsOfOrder = $"SELECT {ColumnOrderItemId}, {ColumnOrderNumber}, {ColumnItemNumber}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item WHERE {ColumnOrderNumber} = {ParameterNameOrderNumber}";

        private const string ColumnOrderItemId = "order_id";
        private const string ColumnOrderNumber = "order_number";
        private const string ColumnItemNumber = "item_number";
        private const string ColumnPlacementTime = "placement_time";
        private const string ColumnStatus = "status";
        private const string ColumnChangeOfStatus = "change_of_status";
        private const string ColumnQuantity = "quantity";
        private const string ColumnComment = "comment";

        private const string ParameterNameOrderItemId = "@orderItemId";
        private const string ParameterNameOrderNumber = "@orderNumber";

        private MenuItemDao menuItemDao = new();

        public List<OrderItem> GetAllOrderItems()
        {
            return GetAll(QueryGetAllOrderItems, ReadRow);
        }

        public OrderItem GetOrderItemById(int orderId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameOrderItemId, orderId }
            };

            return GetByIntParameters(QueryGetOrderItemById, ReadRow, parameters);
        }

        public List<OrderItem> GetAllItemsForOrder(int orderNumber)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameOrderNumber, orderNumber }
            };

            return GetAllByIntParameters(QueryGetAllItemsOfOrder, ReadRow, parameters);
        }

        private OrderItem ReadRow(DataRow dr)
        {
            int id = (int)dr[ColumnOrderItemId];
            MenuItem menuItem = menuItemDao.GetMenuItemById((int)dr[ColumnItemNumber]);
            DateTime? placementTime = dr[ColumnPlacementTime] as DateTime?;
            Status? status = (Status)Enum.Parse(typeof(Status), (string)dr[ColumnStatus]);
            DateTime? changeOfStatus = dr[ColumnChangeOfStatus] as DateTime?;
            int? quantity = dr[ColumnQuantity] as int?;
            string? comment = dr[ColumnComment] as string;

            return new(id, menuItem, placementTime, status, changeOfStatus, quantity, comment);
        }
    }
}