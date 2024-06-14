using DAL;
using Model;

namespace Service
{
    // This class is written by Sia Iurashchuk
    public class OrderService : BaseService
    {
        private OrderDao orderDao = new();

        public event Action RunningOrdersChanged;
        public event Action WaitingTimeChanged;

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

        public bool EqualRunningOrders(List<Order> updatedOrders, List<Order> currentOrders)
        {
            if (updatedOrders == null || currentOrders == null)
                return updatedOrders == currentOrders;
            
            if (updatedOrders.Count != currentOrders.Count)
                return false;

            for (int i = 0; i < updatedOrders.Count; i++)
            {
                if (!EqualOrdersAndItems(updatedOrders[i], currentOrders[i]))
                    return false;
            }
            
            return true;
        }

        private bool EqualOrdersAndItems(Order updatedOrder, Order currentOrder)
        {
            return updatedOrder.Table.DatabaseId == currentOrder.Table.DatabaseId &&
                   EqualOrderItemsStatus(updatedOrder.OrderItems, currentOrder.OrderItems);
        }

        private bool EqualOrderItemsStatus(List<OrderItem> updatedOrderItems, List<OrderItem> currentOrderItems)
        {
            if (updatedOrderItems == null || currentOrderItems == null)
                return updatedOrderItems == currentOrderItems;

            if (updatedOrderItems.Count != currentOrderItems.Count)
                return false;

            for (int i = 0; i < updatedOrderItems.Count; i++)
            {
                if (updatedOrderItems[i].ItemStatus != currentOrderItems[i].ItemStatus)
                    return false;
            }

            return true;
        }
    }
}