﻿using DAL;
using Model;

namespace Service
{
    // This class is written by Sia Iurashchuk
    public class OrderItemService
    {
        private OrderItemDao orderItemDao = new();

        public List<OrderItem> GetAllOrderItems()
        {
            return orderItemDao.GetAllOrderItems();
        }

        public OrderItem GetOrderItemById(int orderId, int itemId)
        {
            return orderItemDao.GetOrderItemById(orderId, itemId);
        }
    }
}