using Model;
using Service;
using System.ComponentModel;

namespace UI
{
    //Created by Orest Pokotylenko
    public class TableViewModel : INotifyPropertyChanged
    {
        private OrderService orderService = new();

        public Table Table { get; set; }
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }
        public List<Order> RunningOrders;

        private TimeSpan? waitingTime;
        public TimeSpan? WaitingTime
        {
            get { return waitingTime; }
            set
            {
                if (waitingTime != value)
                {
                    waitingTime = value;
                    OnPropertyChanged(nameof(WaitingTime));
                }
            }
        }

        private TableStatus tableState;
        public TableStatus TableState
        {
            get { return tableState; }
            set
            {
                if (tableState != value)
                {
                    tableState = value;
                    OnPropertyChanged(nameof(TableState));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TableViewModel(Table table, int rowIndex, int columnIndex, List<Order> runningOrders)
        {
            Table = table;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            RunningOrders = runningOrders;

            SetTableState();
        }

        internal void SetTableState()
        {
            if (orderService.HasRunningOrders(RunningOrders))
                UpdateTableStatusReadyToServe(orderService.Paid(RunningOrders));
            else
                TableState = Table.Occupied ? TableStatus.Reserved : TableStatus.Free;
        }

        private void UpdateTableStatusReadyToServe(bool paid)
        {
            TableState = paid ? TableStatus.OccupiedPaid : TableStatus.Occupied;
            TableStatusReadyToServe(paid);
        }

        private HashSet<MenuType> GetDoneMenuTypes()
        {
            HashSet<MenuType> statuses = new();

            foreach (Order order in RunningOrders)
            {
                List<OrderItem> doneOrderItems = order.OrderItems.Where(orderItem => orderItem.ItemStatus == OrderStatus.Done).ToList();
                statuses.UnionWith(doneOrderItems.Select(orderItem => orderItem.Item.Category.MenuCard.MenuType));
            }

            return statuses;
        }

        private void TableStatusReadyToServe(bool paid)
        {
            HashSet<MenuType> statuses = GetDoneMenuTypes();

            bool containsDrinks = statuses.Contains(MenuType.Drinks);
            bool containsDinner = statuses.Contains(MenuType.Dinner);
            bool containsLunch = statuses.Contains(MenuType.Lunch);

            if (containsDrinks && (containsDinner || containsLunch))
                TableState = !paid ? TableStatus.ReadyToServeAll : TableStatus.ReadyToServeAllPaid;
            else if (containsDinner || containsLunch)
                TableState = !paid ? TableStatus.ReadyToServeFood : TableStatus.ReadyToServeFoodPaid;
            else if (containsDrinks)
                TableState = !paid ? TableStatus.ReadyToServeDrinks : TableStatus.ReadyToServeDrinksPaid;
        }

        private void CalculateWaitingTime(OrderItem orderItem)
        {
            if (orderItem != null)
                WaitingTime = DateTime.Now - orderItem.PlacementTime;
            else
                WaitingTime = null;
        }

        private void SetWaitingTime()
        {
            OrderItem longestWaitingTimeItem = orderService.GetLongestWaitingTime(RunningOrders);
            CalculateWaitingTime(longestWaitingTimeItem);
        }

        internal void UpdateWaitingTime()
        {
            if (RunningOrders.Count > 0)
                SetWaitingTime();
        }
    }
}