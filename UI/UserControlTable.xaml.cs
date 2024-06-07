using Model;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlTable.xaml
    /// </summary>
    public partial class UserControlTable : UserControl
    {
        private TableViewModel tableViewModel;
        private TableService tableService = new();
        private OrderService orderService = new();

        public static readonly RoutedEvent EditOrderClickedEvent = EventManager.RegisterRoutedEvent(
        "EditOrderClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserControlTable));

        public event RoutedEventHandler EditOrderClicked
        {
            add { AddHandler(EditOrderClickedEvent, value); }
            remove { RemoveHandler(EditOrderClickedEvent, value); }
        }

        public static readonly RoutedEvent AddOrderClickedEvent = EventManager.RegisterRoutedEvent(
        "AddOrderClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserControlTable));

        public event RoutedEventHandler AddOrderClicked
        {
            add { AddHandler(AddOrderClickedEvent, value); }
            remove { RemoveHandler(AddOrderClickedEvent, value); }
        }

        public UserControlTable()
        {
            InitializeComponent();
        }

        private void ButtonFree_Click(object sender, RoutedEventArgs e)
        {
            UpdateTableOccupiedStatus(false);
            tableViewModel.TableState = Status.Free;
        }

        private void ButtonReserve_Click(object sender, RoutedEventArgs e)
        {
            UpdateTableOccupiedStatus(true);
            tableViewModel.TableState = Status.Reserved;
        }

        private void ButtonOrder_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(AddOrderClickedEvent));
        }

        private void ButtonServed_Click(object sender, RoutedEventArgs e)
        {
            List<OrderItem> servedOrderItems = OrderItemsToServed();
            orderService.UpdateOrderCategoryStatus(servedOrderItems);
            tableViewModel.TableState = Status.Occupied;
        }

        private void ButtonEditOrder_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(EditOrderClickedEvent));
        }

        private void TableLoaded_Load(object sender, RoutedEventArgs e)
        {
            tableViewModel = DataContext as TableViewModel;
        }

        private void UpdateTableOccupiedStatus(bool status)
        {
            tableViewModel.Table.Occupied = status;
            tableService.UpdateTableStatus(tableViewModel.Table);
        }

        private List<OrderItem> OrderItemsToServed()
        {
            List<OrderItem> changedOrderItems = new();

            foreach (var order in tableViewModel.RunningOrders)
            {
                foreach (var orderItem in order.OrderItems)
                {
                    if (orderItem.ItemStatus == Status.ReadyToServe)
                    {
                        orderItem.ItemStatus = Status.Served;
                        changedOrderItems.Add(orderItem);
                    }
                }
            }

            return changedOrderItems;
        }
    }
}