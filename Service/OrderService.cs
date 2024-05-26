using DAL;
using Model;

namespace Service
{
    // This class is written by Sia Iurashchuk
    public class OrderService
    {
        private OrderDao orderDao = new();

        public List<Order> GetAllOrders()
        {
            return orderDao.GetAllOrders();
        }

        public Order GetOrderById(int orderId)
        {
            return orderDao.GetOrderById(orderId);
        }
    }
}
