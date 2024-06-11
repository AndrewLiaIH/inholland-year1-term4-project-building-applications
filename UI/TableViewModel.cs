using Microsoft.IdentityModel.Tokens;
using Model;
using System.ComponentModel;

namespace UI
{
    //Created by Orest Pokotylenko
    public class TableViewModel : INotifyPropertyChanged
    {
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
                    OnPropertyChanged("WaitingTime");
                }
            }
        }

        private Status tableState;
        public Status TableState
        {
            get { return tableState; }
            set
            {
                if (tableState != value)
                {
                    tableState = value;
                    OnPropertyChanged("TableState");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public TableViewModel(Table table,  int rowIndex, int columnIndex, List<Order> runningOrders)
        {
            Table = table;
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            RunningOrders = runningOrders;

            SetTableState();
        }

        internal void SetTableState()
        {
            if (ReadyToBeServed())
                TableState = Status.ReadyToServe;
            else if (TableHasRunningOrder())
                TableState = Status.Occupied;
            else if (Table.Occupied)
                TableState = Status.Reserved;
            else
                TableState = Status.Free;
        }

        private bool TableHasRunningOrder()
        {
            return !RunningOrders.IsNullOrEmpty();
        }

        private bool ReadyToBeServed()
        {
            List<OrderItem> orderItems = RunningOrders.SelectMany(order => order.OrderItems).ToList();
            return orderItems.Any(orderItem => orderItem.ItemStatus == Status.ReadyToServe);
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
            List<OrderItem> allOrderItems = RunningOrders.SelectMany(order => order.OrderItems).ToList();
            List<OrderItem> waitingOrderItems = allOrderItems.Where(orderItem => orderItem.ItemStatus != Status.Served).ToList();
            OrderItem orderItemLongestWaiting = waitingOrderItems.OrderBy(orderItem => orderItem.PlacementTime).FirstOrDefault();

            CalculateWaitingTime(orderItemLongestWaiting);
        }

        internal void UpdateWaitingTime()
        {
            if (RunningOrders.Count > 0)
                SetWaitingTime();
        }
    }
}