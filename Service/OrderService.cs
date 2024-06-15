using DAL;
using Microsoft.IdentityModel.Tokens;
using Model;

namespace Service
{
    // This class is written by Sia Iurashchuk
    public class OrderService : BaseService
    {
        private OrderDao orderDao = new();

        public event Action RunningOrdersChanged;
        public event Action WaitingTimeChanged;

        // Methods for OrderDao
        protected override void CheckForChanges(object sender, EventArgs e)
        {
            RunningOrdersChanged?.Invoke();
            WaitingTimeChanged?.Invoke();
        }

        public List<Order> GetAllOrders()
        {
            return orderDao.GetAllOrders();
        }

        public Order GetOrderById(int orderId)
        {
            return orderDao.GetOrderById(orderId);
        }

        public List<Order> GetAllKitchenBarOrders(bool forKitchen, bool isRunning)
        {
            return orderDao.GetAllKitchenBarOrders(forKitchen, isRunning);
        }

        public List<Order> GetAllRunningOrdersForTables()
        {
            return orderDao.GetAllRunningOrdersForTables();
        }

        public List<OrderItem> GetAllOrderItems()
        {
            return orderDao.GetAllOrderItems();
        }

        public OrderItem GetOrderItemById(int orderId)
        {
            return orderDao.GetOrderItemById(orderId);
        }

        public void UpdateOrderCategoryStatus(List<OrderItem> orderItems)
        {
            orderDao.UpdateOrderCategoryStatus(orderItems);
        }

        public void UpdateOrderStatus(Order order)
        {
            orderDao.UpdateOrderStatus(order);
        }

        public void UpdateAllOrderItemStatus(Order order)
        {
            orderDao.UpdateAllOrderItemStatus(order);
        }

        public List<Order> GetAllRunningOrdersForTable(Table table)
        {
            return orderDao.GetAllRunningOrdersForTable(table);
        }

        //Service logic
        public bool Paid(List<Order> orders)
        {
            return orders.All(order => order.Finished == true);
        }

        public bool HasRunningOrders(List<Order> orders)
        {
            return !orders.IsNullOrEmpty();
        }

        public OrderItem GetLongestWaitingTime(List<Order>  runningOrders)
        {
            List<OrderItem> allOrderItems = runningOrders.SelectMany(order => order.OrderItems).ToList();
            List<OrderItem> waitingOrderItems = allOrderItems.Where(orderItem => orderItem.ItemStatus != OrderStatus.Served).ToList();
            OrderItem orderItemLongestWaiting = waitingOrderItems.OrderBy(orderItem => orderItem.PlacementTime).FirstOrDefault();

            return orderItemLongestWaiting;
        }

        public void FinishAllOrders(List<Order> orders)
        {
            orders.ForEach(order => order.Finished = true);
            orders.ForEach(order => UpdateOrderStatus(order));
        }

        private List<OrderItem> OrderItemsToServed(List<Order> runningOrders)
        {
            List<OrderItem> changedOrderItems = new();

            foreach (Order order in runningOrders)
            {
                ProcessOrderItemsToServed(order, changedOrderItems);
            }

            return changedOrderItems;
        }

        private void ProcessOrderItemsToServed(Order order, List<OrderItem> changedOrderItems)
        {
            foreach (var orderItem in order.OrderItems)
            {
                if (orderItem.ItemStatus == OrderStatus.Done)
                {
                    orderItem.ItemStatus = OrderStatus.Served;
                    changedOrderItems.Add(orderItem);
                }
            }
        }

        public void SetOrderItemsToServed(List<Order> runningOrders)
        {
            List<OrderItem> servedOrderItems = OrderItemsToServed(runningOrders);
            UpdateOrderCategoryStatus(servedOrderItems);
        }
    }
}