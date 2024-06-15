using System.ComponentModel;

namespace Model
{
    public class CategoryGroup : INotifyPropertyChanged
    {
        private Category category;
        public Category Category
        {
            get { return category; }
            set
            {
                if (category != value)
                {
                    category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }

        private OrderStatus? categoryStatus;
        public OrderStatus? CategoryStatus
        {
            get { return categoryStatus; }
            set
            {
                if (categoryStatus != value)
                {
                    categoryStatus = value;
                    OnPropertyChanged(nameof(CategoryStatus));
                }
            }
        }

        private List<OrderItem> items;
        public List<OrderItem> Items
        {
            get { return items; }
            set
            {
                if (items != value)
                {
                    items = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
