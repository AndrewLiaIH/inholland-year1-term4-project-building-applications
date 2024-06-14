namespace Model
{
    public class Order
    {
        public int DatabaseId { get; private set; }
        public Table Table { get; private set; }
        public Employee PlacedBy { get; private set; }
        public int OrderNumber { get; private set; }
        public int? ServingNumber { get; private set; }
        public bool Finished {get; private set; }
        public decimal TotalPrice { get; private set; }
        public List<OrderItem> OrderItems { get; private set; }
        private OrderStatus? orderStatus;
        public OrderStatus? Status
        {
            get
            {
                orderStatus = GetOrderStatus();
                return orderStatus;
            }

            set
            {
                orderStatus = value;
            }
        }
        public OrderStatus? StatusFromDB { get; private set; }

        public List<CategoryGroup> OrderItemsByCategory
        {
            get
            {
                return OrderItems
                    .GroupBy(item => item.Item.Category.CategoryType.ToString())
                    .Select(group => new CategoryGroup
                    {
                        Category = group.Key,
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
            StatusFromDB = status;
            OrderItems = new();
        }

        public override string ToString()
        {
            return $"#{OrderNumber}: ${TotalPrice}";
        }

        public void AddOrderItem(OrderItem item) 
        {
            if (OrderItems.Contains(item))
                item.IncreaseQuantity();
            else
                OrderItems.Add(item);
        }

        public void SetOrderItems(List<OrderItem> items)
        {
            OrderItems = items;
        }

        private OrderStatus? GetOrderStatus()
        {
            OrderStatus? status = null;
            bool isNotDone = false;

            foreach (OrderItem item in OrderItems)
            {
                if (item.ItemStatus != OrderStatus.Done)
                    isNotDone = true;

                if (status == null)
                    status = item.ItemStatus;
                else if (item.ItemStatus == OrderStatus.Preparing)
                    status = item.ItemStatus;
                else if (item.ItemStatus == OrderStatus.Waiting && status != OrderStatus.Preparing)
                    status = item.ItemStatus;
            }

            if (!isNotDone)
                status = OrderStatus.Done;

            return status;
        }

        public void SetFinished(bool finished)
        {
            Finished = finished;
        }
    }
}