using System.ComponentModel;

namespace Model
{
    public class Order : INotifyPropertyChanged
    {
        public int DatabaseId { get; private set; }
        public Table Table { get; private set; }
        public Employee PlacedBy { get; private set; }
        public int OrderNumber { get; private set; }
        public int? ServingNumber { get; private set; }
        public bool Finished { get; set; }
        public decimal TotalPrice { get; private set; }
        public List<OrderItem> OrderItems { get; private set; }
        private OrderStatus? status;
        public OrderStatus? Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }

        public List<CategoryGroup> OrderItemsByCategory
        {
            get
            {
                return OrderItems
                    .GroupBy(item => item.Item.Category.CategoryType)
                    .Select(group => new CategoryGroup
                    {
                        Category = group.ToList().First().Item.Category,
                        CategoryStatus = GetCategoryStatus(group.ToList()),
                        Items = group.ToList()
                    })
                    .ToList();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Order(int databaseId, Table table, Employee placedBy, int orderNumber, int? servingNumber, bool finished, decimal totalPrice, OrderStatus? status = null)
        {
            DatabaseId = databaseId;
            Table = table;
            PlacedBy = placedBy;
            OrderNumber = orderNumber;
            ServingNumber = servingNumber;
            Finished = finished;
            TotalPrice = totalPrice;
            Status = status;
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

        private OrderStatus? GetCategoryStatus(List<OrderItem> items)
        {
            OrderStatus? status = null;
            bool isNotDone = false;

            foreach (OrderItem item in items)
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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}