﻿using DAL;
using Microsoft.IdentityModel.Tokens;
using Model;
using System.Collections.ObjectModel;

namespace Service
{
    // This class is written by Sia Iurashchuk
    public class OrderService : BaseService
    {
        private OrderDao orderDao = new();
        private MenuService menuService = new();

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


        public void LoadNewOrder(Order order)
        {
            int orderId = CreateOrder(order);
            CreateOrderItems(orderId, order.OrderItems);
            menuService.UpdateStockOfMenuItems(order.OrderItems);
        }

        private int CreateOrder(Order order)
        {
            Order mostRecentOrder = GetMostRecentOrder();
            int servingNumber = ((int)mostRecentOrder.ServingNumber) < 999 ? (int)mostRecentOrder.ServingNumber + 1 : 1;
            Order fullOrder = new(order.Table, order.PlacedBy, mostRecentOrder.OrderNumber + 1, servingNumber);
            fullOrder.SetOrderItems(order.OrderItems);

            return orderDao.CreateOrderAndGetId(fullOrder);
        }

        private void CreateOrderItems(int orderId, List<OrderItem> orderItems)
        {
            orderItems = FillOrderItemsInformation(orderId, orderItems);

            foreach (OrderItem orderItem in orderItems)
                orderDao.CreateOrderItem(orderItem);
        }

        private List<OrderItem> FillOrderItemsInformation(int orderNumber, List<OrderItem> orderItems)
        {
            for (int i = 0; i < orderItems.Count; i++)
            {
                string comment = orderItems[i].Comment == null ? String.Empty : orderItems[i].Comment;
                orderItems[i] = new(orderNumber, orderItems[i].Item, DateTime.Now, OrderStatus.Waiting, DateTime.Now, orderItems[i].Quantity, comment);
            }

            return orderItems;
        }

        public Order GetMostRecentOrder()
        {
            return orderDao.GetMostRecentOrder();
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

        public void UpdateOrderItemsStatus(List<OrderItem> orderItems)
        {
            orderDao.UpdateOrderItemsStatus(orderItems);
        }

        public void UpdateOrderFinishedStatus(Order order)
        {
            orderDao.UpdateOrderFinishedStatus(order);
        }

        public void UpdateAllOrderItemsStatus(Order order)
        {
            orderDao.UpdateAllOrderItemsStatus(order);
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

        public OrderItem GetLongestWaitingTime(List<Order> runningOrders)
        {
            List<OrderItem> allOrderItems = runningOrders.SelectMany(order => order.OrderItems).ToList();
            List<OrderItem> waitingOrderItems = allOrderItems.Where(orderItem => orderItem.ItemStatus != OrderStatus.Served).ToList();
            OrderItem orderItemLongestWaiting = waitingOrderItems.OrderBy(orderItem => orderItem.PlacementTime).FirstOrDefault();

            return orderItemLongestWaiting;
        }

        public void FinishAllOrders(List<Order> orders)
        {
            orders.ForEach(order => order.Finished = true);
            orders.ForEach(order => UpdateOrderFinishedStatus(order));
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
            UpdateOrderItemsStatus(servedOrderItems);
        }

        public decimal GetTotalPrice(ObservableCollection<OrderItem> orderItems)
        {
            decimal totalPrice = 0;
            foreach (OrderItem orderItem in orderItems)
                totalPrice += (decimal)orderItem.TotalPrice;
            return totalPrice;
        }
    }
}