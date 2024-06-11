using Model;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    // This class is created by Orest Pokotylenko
    public partial class UserControlTable : UserControl
    {
        private TableViewModel tableViewModel;

        private TableService tableService = new();
        private OrderService orderService = new();

        internal static readonly RoutedEvent EditOrderClickedEvent = EventManager.RegisterRoutedEvent(
        "EditOrderClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserControlTable));

        internal event RoutedEventHandler EditOrderClicked
        {
            add { AddHandler(EditOrderClickedEvent, value); }
            remove { RemoveHandler(EditOrderClickedEvent, value); }
        }

        internal static readonly RoutedEvent AddOrderClickedEvent = EventManager.RegisterRoutedEvent(
        "AddOrderClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserControlTable));

        internal event RoutedEventHandler AddOrderClicked
        {
            add { AddHandler(AddOrderClickedEvent, value); }
            remove { RemoveHandler(AddOrderClickedEvent, value); }
        }

        public UserControlTable()
        {
            InitializeComponent();
            InitializeEvents();
        }

        private void InitializeEvents()
        {
            orderService.RunningOrdersChanged += OnRunningOrdersChanged;
            orderService.WaitingTimeChanged += OnWaitingTimeChanged;
            tableService.TableOccupiedChanged += OnTableOccupiedChanged;
        }

        //Event handlers
        private void TableLoaded_Load(object sender, RoutedEventArgs e)
        {
            tableViewModel = DataContext as TableViewModel;
        }

        private void ButtonFree_Click(object sender, RoutedEventArgs e)
        {
            UpdateTableOccupiedStatus(false);
            FinishAllOrders();
            ResetTable();
        }

        private void ButtonPay_Click(object sender, RoutedEventArgs e)
        {
            ButtonPay.Visibility = Visibility.Hidden;
            ButtonFree.Visibility = Visibility.Visible;
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
            SetOrderItemsToServed();
            tableViewModel.UpdateWaitingTime();
            tableViewModel.TableState = Status.Occupied;
        }

        private void ButtonEditOrder_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(EditOrderClickedEvent));
        }

        private void OnRunningOrdersChanged()
        {
            UpdateRunningOrders();
        }

        private void UpdateRunningOrders()
        {
            List<Order> ordersPerTable = orderService.GetAllRunningOrdersForTable(tableViewModel.Table);

            if (!orderService.EqualRunningOrders(ordersPerTable, tableViewModel.RunningOrders))
            {
                tableViewModel.RunningOrders = ordersPerTable;
                tableViewModel.SetTableState();
            }
        }

        private void OnTableOccupiedChanged()
        {
            Table updatedTable = tableService.GetTableById(tableViewModel.Table.DatabaseId);

            if (!tableService.EqualTableoccupation(updatedTable, tableViewModel.Table))
            {
                tableViewModel.Table.Occupied = updatedTable.Occupied;
                tableViewModel.SetTableState();
            }
        }

        private void OnWaitingTimeChanged()
        {
            tableViewModel.UpdateWaitingTime();
        }

        //Methods
        private void UpdateTableOccupiedStatus(bool status)
        {
            tableViewModel.Table.Occupied = status;
            tableService.UpdateTableStatus(tableViewModel.Table);
        }

        private void UpdateOrderStatus(Order order)
        {
            orderService.UpdateOrderStatus(order);
        }

        private List<OrderItem> OrderItemsToServed()
        {
            List<OrderItem> changedOrderItems = new();

            foreach (var order in tableViewModel.RunningOrders)
            {
                ProcessOrderItemsToServed(order, changedOrderItems);
            }

            return changedOrderItems;
        }

        private void ProcessOrderItemsToServed(Order order, List<OrderItem> changedOrderItems)
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

        private void SetOrderItemsToServed()
        {
            List<OrderItem> servedOrderItems = OrderItemsToServed();
            orderService.UpdateOrderCategoryStatus(servedOrderItems);
            UpdateRunningOrders();
        }

        private void FinishAllOrders()
        {
            foreach (Order order in tableViewModel.RunningOrders)
            {
                order.Finished = true;
                UpdateOrderStatus(order);
                orderService.UpdateAllOrderItemStatus(order);
            }
        }

        private void ResetTable()
        {
            tableViewModel.TableState = Status.Free;
            tableViewModel.RunningOrders.Clear();
            tableViewModel.UpdateWaitingTime();
        }
    }
}