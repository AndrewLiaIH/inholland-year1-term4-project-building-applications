using Model;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// This class was created by Orest Pokotylenko. It is a user control that represents a table in the restaurant.
    /// </summary>
    public partial class UserControlTable : UserControl
    {
        private TableViewModel tableViewModel;

        private TableService tableService = new();
        private OrderService orderService = new();

        internal static readonly RoutedEvent OrderClickedEvent = EventManager.RegisterRoutedEvent(
        "OrderClicked", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserControlTable));

        internal event RoutedEventHandler OrderClicked
        {
            add { AddHandler(OrderClickedEvent, value); }
            remove { RemoveHandler(OrderClickedEvent, value); }
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
            ResetTable();
        }

        private void ButtonPay_Click(object sender, RoutedEventArgs e)
        {
            FinishAllOrders();
            UpdateRunningOrders();
        }

        private void ButtonReserve_Click(object sender, RoutedEventArgs e)
        {
            UpdateTableOccupiedStatus(true);
            tableViewModel.TableState = TableStatus.Reserved;
        }

        private void ButtonOrder_Click(object sender, RoutedEventArgs e)
        {
            RaiseEvent(new RoutedEventArgs(OrderClickedEvent));
        }

        private void ButtonServed_Click(object sender, RoutedEventArgs e)
        {
            SetOrderItemsToServed();
            UpdateRunningOrders();
            tableViewModel.UpdateWaitingTime();
        }

        private void OnRunningOrdersChanged()
        {
            UpdateRunningOrders();
        }

        private void OnTableOccupiedChanged()
        {
            Table updatedTable = tableService.GetTableById(tableViewModel.Table.DatabaseId);

            if (updatedTable != null)
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
            orderService.UpdateOrderFinishedStatus(order);
        }

        private void SetOrderItemsToServed()
        {
            orderService.SetOrderItemsToServed(tableViewModel.RunningOrders);
            UpdateRunningOrders();
        }

        private void FinishAllOrders()
        {
            orderService.FinishAllOrders(tableViewModel.RunningOrders);
        }

        private void ResetTable()
        {
            tableViewModel.TableState = TableStatus.Free;
            tableViewModel.RunningOrders.Clear();
            tableViewModel.UpdateWaitingTime();
        }

        private void UpdateRunningOrders()
        {
            tableViewModel.RunningOrders = orderService.GetAllRunningOrdersForTable(tableViewModel.Table);
            tableViewModel.SetTableState();
        }
    }
}