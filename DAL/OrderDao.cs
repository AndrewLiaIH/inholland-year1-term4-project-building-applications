using Model;
using System.Data;

namespace DAL
{
    public class OrderDao : BaseDao
    {
        private const string QueryGetAllOrders = $"SELECT {ColumnOrderId}, {ColumnTableId}, {ColumnPlacedById}, {ColumnOrderNumber}, {ColumnServingNumber}, {ColumnFinished}, {ColumnTotalPrice} FROM [order]";
        private const string QueryGetOrderById = $"{QueryGetAllOrders} WHERE {ColumnOrderId} = {ParameterNameOrderId}";
        private const string QueryGetAllRunningOrders = $"SELECT {ColumnOrderId}, {ColumnTableId}, {ColumnPlacedById}, {ColumnOrderNumber}, {ColumnServingNumber}, {ColumnFinished}, {ColumnTotalPrice} FROM [order] WHERE {ColumnFinished} = 0";
        private const string QueryGetAlFinishedOrders = $"SELECT {ColumnOrderId}, {ColumnTableId}, {ColumnPlacedById}, {ColumnOrderNumber}, {ColumnServingNumber}, {ColumnFinished}, {ColumnTotalPrice} FROM [order] WHERE {ColumnFinished} = 1";

        private const string ColumnOrderId = "order_id";
        private const string ColumnTableId = "table_number";
        private const string ColumnPlacedById = "placed_by";
        private const string ColumnOrderNumber = "order_number";
        private const string ColumnServingNumber = "serving_number";
        private const string ColumnFinished = "finished";
        private const string ColumnTotalPrice = "total_price";

        private const string ParameterNameOrderId = "@orderId";

        private TableDao tableDao = new();
        private EmployeeDao employeeDao = new();
        private OrderItemDao orderItemDao = new();

        public List<Order> GetAllOrders()
        {
            List<Order> orders = GetAll(QueryGetAllOrders, ReadRow);

            foreach (Order order in orders)
            {
                GetAndSetAllItemsForOrder(order);
            }

            return orders;
        }

        public Order GetOrderById(int orderId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameOrderId, orderId }
            };

            Order order = GetByIntParameters(QueryGetOrderById, ReadRow, parameters);

            GetAndSetAllItemsForOrder(order);

            return order;
        }

        public List<Order> GetAllRunningOrders()
        {
            List<Order> orders = GetAll(QueryGetAllRunningOrders, ReadRow);

            foreach (Order order in orders)
            {
                GetAndSetAllItemsForOrder(order);
            }

            return orders;
        }

        public List<Order> GetAllFinishedOrders()
        {
            List<Order> orders = GetAll(QueryGetAlFinishedOrders, ReadRow);

            foreach (Order order in orders)
            {
                GetAndSetAllItemsForOrder(order);
            }

            return orders;
        }

        private void GetAndSetAllItemsForOrder(Order order)
        {
            List<OrderItem> items = orderItemDao.GetAllItemsForOrder(order.DatabaseId);
            order.SetOrderItems(items);
        }

        private Order ReadRow(DataRow dr)
        {
            int id = (int)dr[ColumnOrderId];
            Table table = tableDao.GetTableById((int)dr[ColumnTableId]);
            Employee employee = employeeDao.GetEmployeeById((int)dr[ColumnPlacedById]);
            int orderNumber = (int)dr[ColumnOrderNumber];
            int? servingNumber = dr[ColumnServingNumber] as int?;
            bool finished = (bool)dr[ColumnFinished];
            decimal totalPrice = (decimal)dr[ColumnTotalPrice];

            return new(id, table, employee, orderNumber, servingNumber, finished, totalPrice);
        }
    }
}