namespace Model
{
    public class Order
    {
        public int DatabaseId { get; private set; }
        public Table Table { get; private set; }
        public Employee PlacedBy { get; private set; }
        public int OrderNumber { get; private set; }
        public int? ServingNumber { get; private set; }
        public bool Finished { get; set; }
        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                foreach (OrderItem orderItem in OrderItems)
                    totalPrice += (decimal)orderItem.TotalPrice;
                return totalPrice;
            }
            private set { }
        }
        public List<OrderItem> OrderItems { get; private set; }
        public OrderStatus? OrderStatus { get; set; } //CONSIDER CALCULATED PROPERTY!!!!!!!!!!!!!!!!

        public List<CategoryGroup> OrderItemsByCategory
        {
            get
            {
                return OrderItems
                    .GroupBy(item => item.Item.Category)
                    .Select(group => new CategoryGroup
                    {
                        Category = group.Key,
                        CategoryStatus = GetCategoryStatus(group.ToList()),
                        Items = group.ToList()

                    })
                    .ToList();
            }
        }

        public Order(int databaseId, Table table, Employee placedBy, int orderNumber, int? servingNumber, bool finished, decimal totalPrice, OrderStatus? status = null)
        {
            DatabaseId = databaseId;
            Table = table;
            PlacedBy = placedBy;
            OrderNumber = orderNumber;
            ServingNumber = servingNumber;
            Finished = finished;
            TotalPrice = totalPrice;
            OrderStatus = status;
            OrderItems = new();
        }

        public Order(Table table, Employee placedBy, int orderNumber, int? servingNumber)
            : this(0, table, placedBy, orderNumber, servingNumber, false, 0)
        {
        }

        public Order(Table table, Employee employee)
            : this(0, table, employee, 0, 0, false, 0)
        { }

        public override string ToString()
        {
            return $"#{OrderNumber}: ${TotalPrice}";
        }

        public void AddOrderItem(OrderItem newOrderItem)
        {
            foreach (OrderItem orderItem in OrderItems)
            {
                if (orderItem.Item.ItemId == newOrderItem.Item.ItemId && orderItem.Comment == newOrderItem.Comment)
                {
                    IncreaseOrderItemQuantity(orderItem);
                    return;
                }
            }
            OrderItems.Add(newOrderItem);
        }

        public void IncreaseOrderItemQuantity(OrderItem increasingOrderItem)
        {
            int menuItemStock = (int)increasingOrderItem.Item.StockAmount;
            foreach (OrderItem orderItem in OrderItems)
            {
                if (orderItem.Item.ItemId == increasingOrderItem.Item.ItemId)
                    menuItemStock -= (int)orderItem.Quantity;
            }
            if (menuItemStock > 0)
                increasingOrderItem.IncreaseQuantity();
        }

        public void DecreaseOrderItemQuantity(OrderItem decreasingOrderItem)
        {
            decreasingOrderItem.DecreaseQuantity();
            if (decreasingOrderItem.Quantity == 0)
                OrderItems.Remove(decreasingOrderItem);
        }

        public void SetOrderItems(List<OrderItem> items)
        {
            OrderItems = items;
        }

        private OrderStatus? GetCategoryStatus(List<OrderItem> items)
        {
            OrderStatus? status = null;
            bool isNotDone = false;

            foreach (OrderItem item in items)
            {
                if (item.ItemStatus != Model.OrderStatus.Done)
                    isNotDone = true;

                if (status == null)
                    status = item.ItemStatus;
                else if (item.ItemStatus == Model.OrderStatus.Preparing)
                    status = item.ItemStatus;
                else if (item.ItemStatus == Model.OrderStatus.Waiting && status != Model.OrderStatus.Preparing)
                    status = item.ItemStatus;
            }

            if (!isNotDone)
                status = Model.OrderStatus.Done;

            return status;
        }
    }
}