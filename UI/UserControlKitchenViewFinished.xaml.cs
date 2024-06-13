using Model;
using Service;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlKitchenView.xaml
    /// </summary>
    public partial class UserControlKitchenViewFinished : UserControl
    {
        private OrderService orderService = new();
        public List<Order> Orders { get; private set; }

        public UserControlKitchenViewFinished()
        {
            InitializeComponent();
            DataContext = this;
        }

        public void LoadOrders(bool forKitchen)
        {
            Orders = orderService.GetAllKitchenBarOrders(forKitchen, false);
        }
    }
}
