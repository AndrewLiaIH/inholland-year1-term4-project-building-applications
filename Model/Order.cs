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
        private Status? orderStatus;
        public Status? OrderStatus
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
        public Status? StatusFromDB { get; private set; }

        public List<CategoryGroup> OrderItemsByCategory
        {
            get
            {
                return OrderItems
                    .GroupBy(item => item.Item.Category.CategoryType.ToString())
                    .Select(group => new CategoryGroup
                    {
                        Category = group.Key,
                        CategoryStatus = GetCategoryStatus(group.ToList()),
                        Items = group.ToList()

                    })
                    .ToList();
            }
        }

        public Order(int databaseId, Table table, Employee placedBy, int orderNumber, int? servingNumber, bool finished, decimal totalPrice, Status? status = null)
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

        private Status? GetOrderStatus()
        {
            Status? status = null;
            bool isNotDone = false;

            foreach (OrderItem item in OrderItems)
            {
                if (item.ItemStatus != Status.Done)
                    isNotDone = true;

                if (status == null)
                    status = item.ItemStatus;
                else if (item.ItemStatus == Status.Preparing)
                    status = item.ItemStatus;
                else if (item.ItemStatus == Status.Waiting && status != Status.Preparing)
                    status = item.ItemStatus;
            }

            if (!isNotDone)
                status = Status.Done;

            return status;
        }

        private Status? GetCategoryStatus(List<OrderItem> items)
        {
            Status? status = null;
            bool isNotDone = false;

            foreach (OrderItem item in items)
            {
                if (item.ItemStatus != Status.Done)
                    isNotDone = true;

                if (status == null)
                    status = item.ItemStatus;
                else if (item.ItemStatus == Status.Preparing)
                    status = item.ItemStatus;
                else if (item.ItemStatus == Status.Waiting && status != Status.Preparing)
                    status = item.ItemStatus;
            }

            if (!isNotDone)
                status = Status.Done;

            return status;
        }

        public void SetFinished(bool finished)
        {
            Finished = finished;
        }
    }
}
