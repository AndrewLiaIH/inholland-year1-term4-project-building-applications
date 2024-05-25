using DAL;
using Model;

namespace Service
{
    // This class is written by Sia Iurashchuk
    internal class OrderService
    {
        private OrderDao orderDao = new();

        public List<Order> GetAllOrders()
        {
            return orderDao.GetAllOrders();
        }

        public Order GetOrderById(uint orderId)
        {
            return orderDao.GetOrderById(orderId);
        }
    }
}
