using Microsoft.Data.SqlClient;
using System.Data;
using Model;

namespace DAL
{
    public class OrderDao : BaseDao
    {
        // Order
        private const string QueryGetAllOrders = $"SELECT {ColumnOrderId}, {ColumnTableId}, {ColumnPlacedById}, {ColumnOrderNumber}, {ColumnServingNumber}, {ColumnFinished}, {ColumnTotalPrice} FROM [order]";
        private const string QueryGetOrderById = $"{QueryGetAllOrders} WHERE {ColumnOrderId} = {ParameterNameOrderId}";
        private const string QueryGetAllRunningOrdersPerTable = ""; //$"{QueryGetAllRunningOrders} AND {ColumnTableId} = {ParameterTableId}";
        private const string QueryUpdateOrderStatus = $"UPDATE [order] SET {ColumnFinished} = {ParameterNameOrderStatus} WHERE {ColumnOrderId} = {ParameterNameOrderId}";
        private const string QueryGetAllKitchenBarOrders = 
            $"SELECT DISTINCT O.{ColumnOrderId}, {ColumnTableId}, {ColumnPlacedById}, O.{ColumnOrderNumber}, {ColumnServingNumber}, {ColumnFinished}, {ColumnTotalPrice}, {ColumnStatus} FROM [order] AS O " +
            $"JOIN order_item AS OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE {ColumnStatus} = {ParameterStatus2} OR " +
            $"({ColumnStatus} = {ParameterStatus1} AND " +
            $"O.{ColumnOrderId} NOT IN " +
            $"(SELECT DISTINCT O.{ColumnOrderId} " +
            $"FROM [order] AS O JOIN order_item AS OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} " +
            $"WHERE {ColumnStatus} = {ParameterStatus2})) AND menu_type";

        private const string ColumnOrderId = "order_id";
        private const string ColumnTableId = "table_number";
        private const string ColumnPlacedById = "placed_by";
        private const string ColumnOrderNumber = "order_number";
        private const string ColumnServingNumber = "serving_number";
        private const string ColumnFinished = "finished";
        private const string ColumnTotalPrice = "total_price";

        private const string ParameterNameOrderId = "@orderId";
        private const string ParameterTableId = "@table_number";
        private const string ParameterNameOrderStatus = "@finished";
        private const string ParameterStatus1 = "@status1";
        private const string ParameterStatus2 = "@status2";

        // OrderItem
        private const string QueryGetAllOrderItems = $"SELECT {ColumnOrderItemId}, {ColumnOrderItemNumber}, {ColumnItemNumber}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item";
        private const string QueryGetOrderItemById = $"{QueryGetAllOrderItems} WHERE {ColumnOrderItemId} = {ParameterNameOrderItemId}";
        private const string QueryGetAllItemsOfOrder = $"{QueryGetAllOrderItems} WHERE {ColumnOrderItemNumber} = {ParameterNameOrderNumber}";
        private const string QueryUpdateAllOrderItemStatus = $"UPDATE order_item SET {ColumnStatus} = {ParameterNameOrderItemStatus} WHERE {ColumnOrderItemNumber} = {ParameterNameOrderNumber}";
        private const string QueryUpdateOrderItemStatusByCategory = $"UPDATE order_item SET {ColumnStatus} = {ParameterNameOrderItemStatus} WHERE {ColumnOrderItemId} = {ParameterNameOrderItemId}";
        private const string QueryGetAllKitchenBarOrderItems = 
            $"SELECT OI.{ColumnOrderItemId}, OI.{ColumnOrderItemNumber}, OI.{ColumnItemNumber}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item AS OI " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE ({ColumnStatus} = {ParameterStatus1} OR {ColumnStatus} = {ParameterStatus2}) AND menu_type";
        private const string EqualDrinks = " = 'Drinks'";
        private const string NotEqualDrinks = " != 'Drinks'";

        private const string ColumnOrderItemId = "order_id";
        private const string ColumnOrderItemNumber = "order_number";
        private const string ColumnItemNumber = "item_number";
        private const string ColumnPlacementTime = "placement_time";
        private const string ColumnStatus = "status";
        private const string ColumnChangeOfStatus = "change_of_status";
        private const string ColumnQuantity = "quantity";
        private const string ColumnComment = "comment";

        private const string ParameterNameOrderItemId = "@orderItemId";
        private const string ParameterNameOrderNumber = "@orderNumber";
        private const string ParameterNameOrderItemStatus = "@orderItemStatus";

        private TableDao tableDao = new();
        private EmployeeDao employeeDao = new();
        private MenuDao menuDao = new();

        public List<Order> GetAllOrders()
        {
            List<Order> orders = GetAll(QueryGetAllOrders, ReadRowOrder);

            foreach (Order order in orders)
            {
                GetAndSetAllItemsForOrder(order);
            }

            return orders;
        }

        public void UpdateOrderStatus(Order order)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new(ParameterNameOrderId, order.DatabaseId),
                new(ParameterNameOrderStatus, order.Finished.ToString())
            };

            ExecuteEditQuery(QueryUpdateOrderStatus, parameters);
        }

        public Order GetOrderById(int orderId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameOrderId, orderId }
            };

            Order order = GetByIntParameters(QueryGetOrderById, ReadRowOrder, parameters);

            GetAndSetAllItemsForOrder(order);

            return order;
        }

        public List<Order> GetAllKitchenBarOrders(bool forKitchen, bool isRunning)
        {
            string queryEnd = forKitchen ? NotEqualDrinks : EqualDrinks;
            string status1 = isRunning ? "Waiting" : "Done";
            string status2 = isRunning ? "Preparing" : "Served";

            SqlParameter[] parameters1 = new SqlParameter[]
            {
                new(ParameterStatus1, status1),
                new(ParameterStatus2, status2)
            };

            SqlParameter[] parameters2 = new SqlParameter[]
            {
                new(ParameterStatus1, status1),
                new(ParameterStatus2, status2)
            };

            List<OrderItem> orderItems = GetAll(QueryGetAllKitchenBarOrderItems + queryEnd, ReadRowOrderItem, parameters1);
            List<Order> orders = GetAll(QueryGetAllKitchenBarOrders + queryEnd, ReadRowOrderWithStatus, parameters2);

            foreach (OrderItem item in orderItems)
                orders.Find(i => i.DatabaseId == item.OrderId).OrderItems.Add(item);

            return orders;
        }

        public List<Order> GetAllRunningOrdersForTable(Table table)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new(ParameterTableId, table.DatabaseId)
            };

            DataTable ordersTable = ExecuteSelectQuery(QueryGetAllRunningOrdersPerTable, parameters);
            List<Order> orders = ReadTable(ordersTable, ReadRowOrder);

            foreach (Order order in orders)
            {
                GetAndSetAllItemsForOrder(order);
            }

            return orders;
        }

        private void GetAndSetAllItemsForOrder(Order order)
        {
            List<OrderItem> items = GetAllItemsForOrder(order.DatabaseId);
            order.SetOrderItems(items);
        }

        public List<OrderItem> GetAllOrderItems()
        {
            return GetAll(QueryGetAllOrderItems, ReadRowOrderItem);
        }

        public OrderItem GetOrderItemById(int orderId)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameOrderItemId, orderId }
            };

            return GetByIntParameters(QueryGetOrderItemById, ReadRowOrderItem, parameters);
        }

        public void UpdateOrderCategoryStatus(List<OrderItem> orderItems)
        {
            foreach (OrderItem orderItem in orderItems)
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new(ParameterNameOrderItemId, orderItem.DatabaseId),
                    new(ParameterNameOrderItemStatus, orderItem.ItemStatus.ToString())
                };

                ExecuteEditQuery(QueryUpdateOrderItemStatusByCategory, parameters);
            }
        }

        public void UpdateAllOrderItemStatus(Order order)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new(ParameterNameOrderNumber, order.DatabaseId),
                new(ParameterNameOrderItemStatus, Status.Served.ToString())
            };

            ExecuteEditQuery(QueryUpdateAllOrderItemStatus, parameters);
        }

        private List<OrderItem> GetAllItemsForOrder(int orderNumber)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameOrderNumber, orderNumber }
            };

            return GetAllByIntParameters(QueryGetAllItemsOfOrder, ReadRowOrderItem, parameters);
        }

        private Order ReadRowOrder(DataRow dr)
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

        private Order ReadRowOrderWithStatus(DataRow dr)
        {
            int id = (int)dr[ColumnOrderId];
            Table table = tableDao.GetTableById((int)dr[ColumnTableId]);
            Employee employee = employeeDao.GetEmployeeById((int)dr[ColumnPlacedById]);
            int orderNumber = (int)dr[ColumnOrderNumber];
            int? servingNumber = dr[ColumnServingNumber] as int?;
            bool finished = (bool)dr[ColumnFinished];
            decimal totalPrice = (decimal)dr[ColumnTotalPrice];
            Status? status = (Status)Enum.Parse(typeof(Status), (string)dr[ColumnStatus]);

            return new(id, table, employee, orderNumber, servingNumber, finished, totalPrice, status);
        }

        private OrderItem ReadRowOrderItem(DataRow dr)
        {
            int id = (int)dr[ColumnOrderItemId];
            int orderId = (int)dr[ColumnOrderItemNumber];
            MenuItem menuItem = menuDao.GetMenuItemById((int)dr[ColumnItemNumber]);
            DateTime? placementTime = dr[ColumnPlacementTime] as DateTime?;
            Status? status = (Status)Enum.Parse(typeof(Status), (string)dr[ColumnStatus]);
            DateTime? changeOfStatus = dr[ColumnChangeOfStatus] as DateTime?;
            int? quantity = dr[ColumnQuantity] as int?;
            string? comment = dr[ColumnComment] as string;

            return new(id, orderId, menuItem, placementTime, status, changeOfStatus, quantity, comment);
        }
    }
}