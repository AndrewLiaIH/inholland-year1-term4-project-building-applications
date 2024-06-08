using DAL;
using Model;

namespace Service
{
    // This class is written by Sia Iurashchuk
    public class OrderService : BaseService
    {
        private OrderDao orderDao = new();
        public event Action RunningOrdersChanged;

        public List<Order> GetAllOrders()
        {
            return orderDao.GetAllOrders();
        }

        public Order GetOrderById(int orderId)
        {
            return orderDao.GetOrderById(orderId);
        }

        public List<Order> GetAllRunningOrders()
        {
            return orderDao.GetAllRunningOrders();
        }

        public List<Order> GetAllFinishedOrders()
        {
            return orderDao.GetAllFinishedOrders();
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

        protected override void CheckForChanges(object sender, EventArgs e)
        {
            RunningOrdersChanged?.Invoke();
        }

        public bool EqualRunningOrders(List<Order> updatedOrders, List<Order> currentOrders)
        {
            if (currentOrders != null)
            {
                if (updatedOrders.Count != currentOrders.Count)
                    return false;

                for (int i = 0; i < updatedOrders.Count; i++)
                {
                    if (updatedOrders[i].Table.DatabaseId != currentOrders[i].Table.DatabaseId)
                        return false;
                    else if (!EqualOrderItemsStatus(updatedOrders[i].OrderItems, currentOrders[i].OrderItems))
                        return false;
                }
            }

            return true;
        }

        private bool EqualOrderItemsStatus(List<OrderItem> updatedOrderItems, List<OrderItem> currentOrderItems)
        {
            if (currentOrderItems != null)
            {
                if (updatedOrderItems.Count != currentOrderItems.Count)
                    return false;

                for (int i = 0; i < updatedOrderItems.Count; i++)
                {
                    if (updatedOrderItems[i].ItemStatus != currentOrderItems[i].ItemStatus)
                        return false;
                }
            }

            return true;
        }
    }
}