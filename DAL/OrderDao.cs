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
        private const string QueryGetAllRunningOrdersTables =
            $"SELECT O.{ColumnOrderId} AS ColumnOrderId, O.{ColumnTableId}, O.{ColumnPlacedById}, O.{ColumnOrderNumber}, O.{ColumnServingNumber}, O.{ColumnFinished}, O.{ColumnTotalPrice}, " +
            $"OI.{ColumnOrderItemId} ColumnOrderItemId, OI.{ColumnOrderItemNumber}, OI.{ColumnItemNumber}, OI.{ColumnPlacementTime}, OI.{ColumnStatus}, OI.{ColumnChangeOfStatus}, OI.{ColumnQuantity}, OI.{ColumnComment} " +
            $"FROM [order] O INNER JOIN order_item OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} WHERE (O.{ColumnFinished} = 0 OR EXISTS " +
            $"(SELECT 1 FROM order_item oi WHERE oi.{ColumnOrderItemNumber} = O.{ColumnOrderId} AND oi.{ColumnStatus} <> 'Served'))";
        private const string QueryGetAllRunningOrdersPerTable = $"{QueryGetAllRunningOrdersTables} AND O.{ColumnTableId} = {ParameterTableId}";
        private const string QueryCreateOrder = $"INSERT INTO [order] VALUES ({ParameterTableId}, {ParameterEmployeeId}, {ParameterOrderNumber}, {ParameterServingNumber}, {ParameterIsFinished}, {ParameterTotalPrice}); SELECT CAST(scope_identity() AS int);";
        private const string QueryGetMostRecentOrder = $"SELECT {ColumnOrderId}, {ColumnTableId}, {ColumnPlacedById}, {ColumnOrderNumber}, {ColumnServingNumber}, {ColumnFinished}, {ColumnTotalPrice} FROM [order] WHERE {ColumnOrderNumber} = (SELECT MAX({ColumnOrderNumber}) FROM [order])";

        private const string ColumnOrderId = "order_id";
        private const string ColumnTableId = "table_number";
        private const string ColumnPlacedById = "placed_by";
        private const string ColumnOrderNumber = "order_number";
        private const string ColumnServingNumber = "serving_number";
        private const string ColumnFinished = "finished";
        private const string ColumnTotalPrice = "total_price";

        private const string ParameterNameOrderId = "@orderId";
        private const string ParameterTableId = "@table_number";
        private const string ParameterEmployeeId = "@employee_id";
        private const string ParameterOrderNumber = "@order_number";
        private const string ParameterServingNumber = "@serving_number";
        private const string ParameterIsFinished = "@is_finished";
        private const string ParameterOrderStatus = "@status";
        private const string ParameterNameOrderStatus = "@finished";
        private const string ParameterTotalPrice = "@total_price";
        private const string ParameterStatus1 = "@status1";
        private const string ParameterStatus2 = "@status2";

        // OrderItem
        private const string QueryGetAllOrderItems = $"SELECT {ColumnOrderItemId}, {ColumnOrderItemNumber}, {ColumnItemNumber}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item";
        private const string QueryGetOrderItemById = $"{QueryGetAllOrderItems} WHERE {ColumnOrderItemId} = {ParameterNameOrderItemId}";
        private const string QueryGetAllItemsOfOrder = $"{QueryGetAllOrderItems} WHERE {ColumnOrderItemNumber} = {ParameterNameOrderNumber}";
        private const string QueryUpdateAllOrderItemsStatus = $"UPDATE order_item SET {ColumnStatus} = {ParameterNameOrderItemStatus} WHERE {ColumnOrderItemNumber} = {ParameterNameOrderNumber}";
        private const string QueryUpdateOrderItemStatus = $"UPDATE order_item SET [{ColumnStatus}] = {ParameterNameOrderItemStatus} WHERE {ColumnOrderItemId} = {ParameterNameOrderItemId}";
        private const string QueryGetAllKitchenBarOrderItems =
            $"SELECT OI.{ColumnOrderItemId} AS ColumnOrderItemId, OI.{ColumnOrderItemNumber}, OI.{ColumnItemNumber}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item AS OI " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE ({ColumnStatus} = {ParameterStatus1} OR {ColumnStatus} = {ParameterStatus2}) AND menu_type";
        private const string QueryCreateOrderItem = $"INSERT INTO [order_item] VALUES ({ParameterNameOrderNumber}, {ParameterNameOrderItemId}, {ParameterNamePlacementTime}, {ParameterNameOrderItemStatus}, {ParameterNameChangeOfStatus}, {ParameterNameQuantity}, {ParameterNameComment}); SELECT CAST(scope_identity() AS int);";

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

        private const string ParameterNameOrderNumber = "@orderNumber";
        private const string ParameterNameOrderItemId = "@orderItemId";
        private const string ParameterNamePlacementTime = "@orderItemPlacementTime";
        private const string ParameterNameOrderItemStatus = "@orderItemStatus";
        private const string ParameterNameChangeOfStatus = "@orderItemChangeofStatus";
        private const string ParameterNameQuantity = "@orderItemQuantity";
        private const string ParameterNameComment = "@orderItemComment";


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

        public int CreateOrderAndGetId(Order order)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter(ParameterTableId, order.Table.DatabaseId),
                new SqlParameter(ParameterEmployeeId, order.PlacedBy.DatabaseId),
                new SqlParameter(ParameterOrderNumber, order.OrderNumber),
                new SqlParameter(ParameterServingNumber, order.ServingNumber),
                new SqlParameter(ParameterIsFinished, order.Finished),
                new SqlParameter(ParameterTotalPrice, order.TotalPrice)
            };

            return ExecuteScalarQuery(QueryCreateOrder, sqlParameters);
        }

        public void CreateOrderItem(OrderItem orderItem)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter(ParameterNameOrderNumber, orderItem.OrderId),
                new SqlParameter(ParameterNameOrderItemId, orderItem.Item.ItemId),
                new SqlParameter(ParameterNamePlacementTime, orderItem.PlacementTime),
                new SqlParameter(ParameterNameOrderItemStatus, orderItem.ItemStatus.ToString()),
                new SqlParameter(ParameterNameChangeOfStatus, orderItem.ChangeOfStatus),
                new SqlParameter(ParameterNameQuantity, orderItem.Quantity),
                new SqlParameter(ParameterNameComment, orderItem.Comment)
            };

            ExecuteEditQuery(QueryCreateOrderItem, sqlParameters);
        }

        public Order GetMostRecentOrder()
        {
            DataTable dataTable = ExecuteSelectQuery(QueryGetMostRecentOrder, new SqlParameter[] { });
            return ReadTable(dataTable, ReadRowOrder_Andrew).First();
        }

        public void UpdateOrderFinishedStatus(Order order)
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
            Dictionary<int, Order> ordersDictionary = new();
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new(ParameterTableId, table.DatabaseId)
            };

            DataTable dataTabel = ExecuteSelectQuery(QueryGetAllRunningOrdersPerTable, sqlParameters);

            foreach (DataRow row in dataTabel.Rows)
            {
                ReadCombinedRow(row, ordersDictionary);
            }

            return ordersDictionary.Values.ToList();
        }

        public List<Order> GetAllRunningOrdersForTables()
        {
            Dictionary<int, Order> ordersDictionary = new();

            DataTable dataTabel = ExecuteSelectQuery(QueryGetAllRunningOrdersTables);

            foreach (DataRow row in dataTabel.Rows)
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
                    new(ParameterNameOrderItemStatus, orderItem.ItemStatus.ToString())
                };

                ExecuteEditQuery(QueryUpdateOrderItemStatus, parameters);
            }
        }

        public void UpdateAllOrderItemsStatus(Order order)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new(ParameterNameOrderNumber, order.DatabaseId),
                new(ParameterNameOrderItemStatus, OrderStatus.Served.ToString())
            };

            ExecuteEditQuery(QueryUpdateAllOrderItemsStatus, parameters);
        }

        private List<OrderItem> GetAllItemsForOrder(int orderNumber)
        {
            Dictionary<string, int> parameters = new()
            {
                { ParameterNameOrderNumber, orderNumber }
            };

            return GetAllByIntParameters(QueryGetAllItemsOfOrder, ReadRowOrderItem, parameters);
        }

        private Order ReadRowOrder_Andrew(DataRow dr)
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
    }
}