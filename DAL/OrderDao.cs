using Microsoft.Data.SqlClient;
using Model;
using System.Data;

namespace DAL
{
    public class OrderDao : BaseDao
    {
        // Order
        private const string QueryGetAllOrders = $"SELECT {ColumnOrderId}, {ColumnTableId}, {ColumnPlacedById}, {ColumnOrderNumber}, {ColumnServingNumber}, {ColumnFinished}, {ColumnTotalPrice} FROM [order]";
        private const string QueryGetOrderById = $"{QueryGetAllOrders} WHERE {ColumnOrderId} = {ParameterNameOrderId}";
        private const string QueryUpdateOrderFinishedStatus = $"UPDATE [order] SET {ColumnFinished} = {ParameterNameOrderStatus} WHERE {ColumnOrderId} = {ParameterNameOrderId}";
        private const string QueryGetAllKitchenBarRunningOrders =
            $"SELECT O.{ColumnOrderId} AS ColumnOrderId, O.{ColumnTableId}, O.{ColumnPlacedById}, O.{ColumnOrderNumber}, O.{ColumnServingNumber}, O.{ColumnFinished}, O.{ColumnTotalPrice}, " +
            $"OI.{ColumnOrderItemId} AS ColumnOrderItemId, OI.{ColumnOrderItemNumber}, OI.{ColumnItemNumber}, OI.{ColumnPlacementTime}, OI.{ColumnStatus}, OI.{ColumnChangeOfStatus}, OI.{ColumnQuantity}, OI.{ColumnComment} " +
            $"FROM [order] AS O " +
            $"JOIN order_item AS OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE O.{ColumnOrderId} IN " +
            $"(SELECT O.{ColumnOrderId} FROM [order] AS O " +
            $"JOIN order_item AS OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE (OI.{ColumnStatus} = 'Waiting' OR OI.{ColumnStatus} = 'Preparing') AND MC.menu_type";
        private const string QueryGetAllKitchenBarFinishedOrders =
            $"SELECT O.{ColumnOrderId} AS ColumnOrderId, O.{ColumnTableId}, O.{ColumnPlacedById}, O.{ColumnOrderNumber}, O.{ColumnServingNumber}, O.{ColumnFinished}, O.{ColumnTotalPrice}, " +
            $"OI.{ColumnOrderItemId} AS ColumnOrderItemId, OI.{ColumnOrderItemNumber}, OI.{ColumnItemNumber}, OI.{ColumnPlacementTime}, OI.{ColumnStatus}, OI.{ColumnChangeOfStatus}, OI.{ColumnQuantity}, OI.{ColumnComment} " +
            $"FROM [order] AS O " +
            $"JOIN order_item AS OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE O.{ColumnOrderId} IN " +
            $"(SELECT O.{ColumnOrderId} FROM [order] AS O " +
            $"JOIN order_item AS OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE CAST(OI.{ColumnPlacementTime} AS DATE) = CAST(GETDATE() AS DATE) AND (OI.{ColumnStatus} = 'Done' OR OI.{ColumnStatus} = 'Served') AND MC.menu_type";
        private const string QueryGetAllRunningOrdersTables =
            $"SELECT O.{ColumnOrderId} AS ColumnOrderId, O.{ColumnTableId}, O.{ColumnPlacedById}, O.{ColumnOrderNumber}, O.{ColumnServingNumber}, O.{ColumnFinished}, O.{ColumnTotalPrice}, " +
            $"OI.{ColumnOrderItemId} ColumnOrderItemId, OI.{ColumnOrderItemNumber}, OI.{ColumnItemNumber}, OI.{ColumnPlacementTime}, OI.{ColumnStatus}, OI.{ColumnChangeOfStatus}, OI.{ColumnQuantity}, OI.{ColumnComment} " +
            $"FROM [order] O INNER JOIN order_item OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} WHERE (O.{ColumnFinished} = 0 OR EXISTS " +
            $"(SELECT 1 FROM order_item oi WHERE oi.{ColumnOrderItemNumber} = O.{ColumnOrderId} AND oi.{ColumnStatus} <> 'Served'))";
        private const string QueryGetAllRunningOrdersPerTable = $"{QueryGetAllRunningOrdersTables} AND O.{ColumnTableId} = {ParameterTableId}";

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

        private const string EqualDrinks = " = 'Drinks') AND MC.menu_type = 'Drinks'";
        private const string NotEqualDrinks = " != 'Drinks') AND MC.menu_type != 'Drinks'";

        // OrderItem
        private const string QueryGetAllOrderItems = $"SELECT {ColumnOrderItemId}, {ColumnOrderItemNumber}, {ColumnItemNumber}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item";
        private const string QueryGetOrderItemById = $"{QueryGetAllOrderItems} WHERE {ColumnOrderItemId} = {ParameterNameOrderItemId}";
        private const string QueryGetAllItemsOfOrder = $"{QueryGetAllOrderItems} WHERE {ColumnOrderItemNumber} = {ParameterNameOrderNumber}";
        private const string QueryUpdateOrderItemsStatus = $"UPDATE order_item SET [{ColumnStatus}] = {ParameterNameOrderItemStatus}, {ColumnChangeOfStatus} = {ParameterNameOrderItemChangeOfStatus} WHERE {ColumnOrderItemId} = {ParameterNameOrderItemId}";
        
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
        private const string ParameterNameOrderItemChangeOfStatus = "@orderItemChangeOfStatus";

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
            string query;
            string queryEnd = forKitchen ? NotEqualDrinks : EqualDrinks;

            if (isRunning)
                query = QueryGetAllKitchenBarRunningOrders + queryEnd;
            else
                query = QueryGetAllKitchenBarFinishedOrders + queryEnd;

            Dictionary<int, Order> ordersDictionary = new();

            DataTable dataTable = ExecuteSelectQuery(query, out bool error);

            foreach (DataRow row in dataTable.Rows)
            {
                ReadCombinedRowWithOrderStatus(row, ordersDictionary);
            }

            return ordersDictionary.Values.ToList();
        }

        public List<Order> GetAllRunningOrdersForTable(Table table)
        {
            Dictionary<int, Order> ordersDictionary = new();
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new(ParameterTableId, table.DatabaseId)
            };

            DataTable dataTable = ExecuteSelectQuery(QueryGetAllRunningOrdersPerTable, out bool error, sqlParameters);

            foreach (DataRow row in dataTable.Rows)
            {
                ReadCombinedRow(row, ordersDictionary);
            }

            return ordersDictionary.Values.ToList();
        }

        public List<Order> GetAllRunningOrdersForTables()
        {
            Dictionary<int, Order> ordersDictionary = new();

            DataTable dataTable = ExecuteSelectQuery(QueryGetAllRunningOrdersTables, out bool error);

            foreach (DataRow row in dataTable.Rows)
            {
                ReadCombinedRow(row, ordersDictionary);
            }

            return ordersDictionary.Values.ToList();
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

        public void UpdateOrderItemsStatus(List<OrderItem> orderItems)
        {
            foreach (OrderItem orderItem in orderItems)
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new(ParameterNameOrderItemId, orderItem.DatabaseId),
                    new(ParameterNameOrderItemStatus, orderItem.ItemStatus.ToString()),
                    new(ParameterNameOrderItemChangeOfStatus, orderItem.ChangeOfStatus)
                };

                ExecuteEditQuery(QueryUpdateOrderItemsStatus, parameters);
            }
        }

        public void UpdateOrderFinishedStatus(Order order)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new(ParameterNameOrderId, order.DatabaseId),
                new(ParameterNameOrderStatus, order.Finished.ToString())
            };

            ExecuteEditQuery(QueryUpdateOrderFinishedStatus, parameters);
        }

        private List<OrderItem> GetAllItemsForOrder(int orderNumber)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameOrderNumber, orderNumber }
            };

            return GetAllByIntParameters(QueryGetAllItemsOfOrder, ReadRowOrderItem, parameters, out bool nError);
        }

        private Order ReadRowOrder(DataRow dr)
        {
            int id = (int)dr["ColumnOrderId"];
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
            int id = (int)dr["ColumnOrderId"];
            Table table = tableDao.GetTableById((int)dr[ColumnTableId]);
            Employee employee = employeeDao.GetEmployeeById((int)dr[ColumnPlacedById]);
            int orderNumber = (int)dr[ColumnOrderNumber];
            int? servingNumber = dr[ColumnServingNumber] as int?;
            bool finished = (bool)dr[ColumnFinished];
            decimal totalPrice = (decimal)dr[ColumnTotalPrice];
            OrderStatus? status = (OrderStatus)Enum.Parse(typeof(OrderStatus), (string)dr[ColumnStatus]);

            return new(id, table, employee, orderNumber, servingNumber, finished, totalPrice, status);
        }

        private OrderItem ReadRowOrderItem(DataRow dr)
        {
            int id = (int)dr["ColumnOrderItemId"];
            int orderId = (int)dr[ColumnOrderItemNumber];
            MenuItem menuItem = menuDao.GetMenuItemById((int)dr[ColumnItemNumber]);
            DateTime? placementTime = dr[ColumnPlacementTime] as DateTime?;
            OrderStatus? status = (OrderStatus)Enum.Parse(typeof(OrderStatus), (string)dr[ColumnStatus]);
            DateTime? changeOfStatus = dr[ColumnChangeOfStatus] as DateTime?;
            int? quantity = dr[ColumnQuantity] as int?;
            string? comment = dr[ColumnComment] as string;

            return new(id, orderId, menuItem, placementTime, status, changeOfStatus, quantity, comment);
        }

        private void ReadCombinedRow(DataRow dr, Dictionary<int, Order> orderDictionary)
        {
            int orderId = (int)dr["ColumnOrderId"];

            if (!orderDictionary.TryGetValue(orderId, out Order order))
            {
                order = ReadRowOrder(dr);
                orderDictionary[orderId] = order;
            }

            OrderItem orderItem = ReadRowOrderItem(dr);
            order.OrderItems.Add(orderItem);
        }

        private void ReadCombinedRowWithOrderStatus(DataRow dr, Dictionary<int, Order> orderDictionary)
        {
            int orderId = (int)dr["ColumnOrderId"];

            if (!orderDictionary.TryGetValue(orderId, out Order order))
            {
                order = ReadRowOrderWithStatus(dr);
                orderDictionary[orderId] = order;
            }

            OrderItem orderItem = ReadRowOrderItem(dr);
            order.OrderItems.Add(orderItem);
        }
    }
}