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
            $"SELECT DISTINCT O.{ColumnOrderId}, O.{ColumnTableId}, O.{ColumnPlacedById}, O.{ColumnOrderNumber}, O.{ColumnServingNumber}, O.{ColumnFinished}, O.{ColumnTotalPrice}, " +
            $"CASE WHEN EXISTS " +
            $"(SELECT 1 FROM order_item AS OI WHERE OI.{ColumnOrderItemNumber} = O.{ColumnOrderId} AND OI.{ColumnStatus} = 'Preparing') " +
            $"THEN 'Preparing' WHEN EXISTS " +
            $"(SELECT 1 FROM order_item AS OI WHERE OI.{ColumnOrderItemNumber} = O.{ColumnOrderId} AND OI.{ColumnStatus} = 'Waiting') " +
            $"THEN 'Waiting' ELSE 'Unknown' END AS {ColumnStatus} " +
            $"FROM [order] AS O " +
            $"JOIN order_item AS OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE O.{ColumnOrderId} IN " +
            $"(SELECT DISTINCT OI.{ColumnOrderItemNumber} FROM order_item AS OI WHERE OI.{ColumnStatus} IN ('Waiting', 'Preparing')) " +
            $"AND MC.menu_type != 'Drinks'";
        private const string QueryGetAllKitchenBarFinishedOrders =
            $"SELECT DISTINCT O.{ColumnOrderId}, O.{ColumnTableId}, O.{ColumnPlacedById}, O.{ColumnOrderNumber}, O.{ColumnServingNumber}, O.{ColumnFinished}, O.{ColumnTotalPrice}, " +
            $"CASE WHEN EXISTS " +
            $"(SELECT 1 FROM order_item AS OI WHERE OI.{ColumnOrderItemNumber} = O.{ColumnOrderId} AND OI.{ColumnStatus} = 'Done') " +
            $"THEN 'Done' WHEN EXISTS " +
            $"(SELECT 1 FROM order_item AS OI WHERE OI.{ColumnOrderItemNumber} = O.{ColumnOrderId} AND OI.{ColumnStatus} = 'Served') " +
            $"THEN 'Served' ELSE 'Unknown' END AS {ColumnStatus} " +
            $"FROM [order] AS O " +
            $"JOIN order_item AS OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE O.{ColumnOrderId} IN " +
            $"(SELECT DISTINCT OI.{ColumnOrderItemNumber} FROM order_item AS OI WHERE OI.{ColumnStatus} IN ('Done', 'Served')) " +
            $"AND MC.menu_type = 'Drinks'";
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
        private const string ParameterStatusWaiting = "@statusWaiting";
        private const string ParameterStatusPreparing = "@statusPreparing";
        private const string ParameterStatusDone = "@statusDone";
        private const string ParameterStatusServed = "@statusServed";

        private const string EqualDrinksOrders = " OR menu_type != 'Drinks')) AND menu_type = 'Drinks'";
        private const string NotEqualDrinksOrders = " OR menu_type = 'Drinks')) AND menu_type != 'Drinks'";

        // OrderItem
        private const string QueryGetAllOrderItems = $"SELECT {ColumnOrderItemId}, {ColumnOrderItemNumber}, {ColumnItemNumber}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item";
        private const string QueryGetOrderItemById = $"{QueryGetAllOrderItems} WHERE {ColumnOrderItemId} = {ParameterNameOrderItemId}";
        private const string QueryGetAllItemsOfOrder = $"{QueryGetAllOrderItems} WHERE {ColumnOrderItemNumber} = {ParameterNameOrderNumber}";
        private const string QueryUpdateOrderItemsStatus = $"UPDATE order_item SET [{ColumnStatus}] = {ParameterNameOrderItemStatus}, {ColumnChangeOfStatus} = {ParameterNameOrderItemChangeOfStatus} WHERE {ColumnOrderItemId} = {ParameterNameOrderItemId}";
        private const string QueryGetAllKitchenBarRunningOrderItems =
            $"SELECT OI.{ColumnOrderItemId} AS ColumnOrderItemId, OI.{ColumnOrderItemNumber}, OI.{ColumnItemNumber}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item AS OI " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE OI.{ColumnOrderItemNumber} IN ({QueryGetAllKitchenBarRunningOrderItemsSubquery}";
        private const string QueryGetAllKitchenBarRunningOrderItemsSubquery =
            $"SELECT DISTINCT O.{ColumnOrderId} FROM [order] AS O " +
            $"JOIN order_item AS OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE {ColumnStatus} = {ParameterStatusPreparing} OR " +
            $"({ColumnStatus} = {ParameterStatusWaiting} AND " +
            $"O.{ColumnOrderId} NOT IN " +
            $"(SELECT DISTINCT O.{ColumnOrderId} " +
            $"FROM [order] AS O JOIN order_item AS OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} " +
            $"WHERE {ColumnStatus} = {ParameterStatusPreparing}";
        private const string QueryGetAllKitchenBarFinishedOrderItems =
            $"SELECT OI.{ColumnOrderItemId} AS ColumnOrderItemId, OI.{ColumnOrderItemNumber}, OI.{ColumnItemNumber}, {ColumnPlacementTime}, {ColumnStatus}, {ColumnChangeOfStatus}, {ColumnQuantity}, {ColumnComment} FROM order_item AS OI " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE OI.{ColumnOrderItemNumber} IN ({QueryGetAllKitchenBarFinishedOrderItemsSubquery}";
        private const string QueryGetAllKitchenBarFinishedOrderItemsSubquery =
            $"SELECT DISTINCT O.{ColumnOrderId} FROM [order] AS O " +
            $"JOIN order_item AS OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} " +
            $"JOIN menu_item AS MI ON OI.{ColumnItemNumber} = MI.item_id " +
            $"JOIN category AS C ON MI.category_id = C.category_id " +
            $"JOIN menu_card AS MC ON C.menu_id = MC.card_id " +
            $"WHERE {ColumnStatus} = {ParameterStatusServed} OR " +
            $"({ColumnStatus} = {ParameterStatusDone} AND " +
            $"O.{ColumnOrderId} NOT IN " +
            $"(SELECT DISTINCT O.{ColumnOrderId} " +
            $"FROM [order] AS O JOIN order_item AS OI ON O.{ColumnOrderId} = OI.{ColumnOrderItemNumber} " +
            $"WHERE {ColumnStatus} = {ParameterStatusWaiting} OR {ColumnStatus} = {ParameterStatusPreparing} OR {ColumnStatus} = {ParameterStatusServed}";

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

        private const string EqualDrinksOrderItems = " OR menu_type != 'Drinks')) AND menu_type = 'Drinks') AND menu_type = 'Drinks'";
        private const string NotEqualDrinksOrderItems = " OR menu_type = 'Drinks')) AND menu_type  != 'Drinks') AND menu_type != 'Drinks'";

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
            string queryOrders;
            string queryOrderItems;
            string queryEndOrders = forKitchen ? NotEqualDrinksOrders : EqualDrinksOrders;
            string queryEndOrderItems = forKitchen ? NotEqualDrinksOrderItems : EqualDrinksOrderItems;
            SqlParameter[] parametersOrders;
            SqlParameter[] parametersOrderItems;

            if (isRunning)
            {
                queryOrders = QueryGetAllKitchenBarRunningOrders;
                queryOrderItems = QueryGetAllKitchenBarRunningOrderItems;

                parametersOrders = new SqlParameter[]
                {
                    new(ParameterStatusWaiting, OrderStatus.Waiting.ToString()),
                    new(ParameterStatusPreparing, OrderStatus.Preparing.ToString())
                };

                parametersOrderItems = new SqlParameter[]
                {
                    new(ParameterStatusWaiting, OrderStatus.Waiting.ToString()),
                    new(ParameterStatusPreparing, OrderStatus.Preparing.ToString())
                };
            }
            else
            {
                queryOrders = QueryGetAllKitchenBarFinishedOrders;
                queryOrderItems = QueryGetAllKitchenBarFinishedOrderItems;

                parametersOrders = new SqlParameter[]
                {
                    new(ParameterStatusWaiting, OrderStatus.Waiting.ToString()),
                    new(ParameterStatusPreparing, OrderStatus.Preparing.ToString()),
                    new(ParameterStatusDone, OrderStatus.Done.ToString()),
                    new(ParameterStatusServed, OrderStatus.Served.ToString())
                };

                parametersOrderItems = new SqlParameter[]
                {
                    new(ParameterStatusWaiting, OrderStatus.Waiting.ToString()),
                    new(ParameterStatusPreparing, OrderStatus.Preparing.ToString()),
                    new(ParameterStatusDone, OrderStatus.Done.ToString()),
                    new(ParameterStatusServed, OrderStatus.Served.ToString())
                };
            }

            List<Order> orders = GetAll(queryOrders, ReadRowOrderWithStatus);
            //List<OrderItem> orderItems = GetAll(queryOrderItems + queryEndOrderItems, ReadRowOrderItem, parametersOrderItems);

            //foreach (OrderItem item in orderItems)
            //    orders.Find(i => i.DatabaseId == item.OrderId).OrderItems.Add(item);

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

            return GetAllByIntParameters(QueryGetAllItemsOfOrder, ReadRowOrderItem, parameters);
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
            int id = (int)dr[ColumnOrderId];
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