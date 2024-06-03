using Model;
using Service;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlKitchenView.xaml
    /// </summary>
    public partial class UserControlKitchenViewRunning : UserControl
    {
        private OrderService orderService = new();
        public List<Order> Orders { get; private set; }

        public UserControlKitchenViewRunning()
        {
            InitializeComponent();
            LoadOrders();
            DataContext = this;
        }

        private void LoadOrders()
        {
            Orders = orderService.GetAllRunningOrders();
        }
    }
}
