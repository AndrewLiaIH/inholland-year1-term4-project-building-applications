using Microsoft.IdentityModel.Tokens;
using Model;
using System.ComponentModel;

namespace UI
{
    //Created by Orest Pokotylenko
    /// <summary>
    /// TableViewModel class is used to represent the table from the Model layer in the TableViewTables user control.
    /// </summary>
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
            foreach (Order order in RunningOrders)
            {
                foreach (OrderItem item in order.OrderItems)
                {
                    if (item.ItemStatus == Status.ReadyToServe)
                        return true;
                }
            }

            return false;
        }

        private void CalculateWaitingTime(OrderItem orderItem)
        {
            if (orderItem != null)
                WaitingTime = DateTime.Now - orderItem.PlacementTime;
        }

        private void SetWaitingTime()
        {
            OrderItem orderItemLongestWaiting = null;

            foreach (Order order in RunningOrders)
            {
                for(int i = 0; i < order.OrderItems.Count; i++)
                {
                    if (order.OrderItems[i].ItemStatus != Status.Served)
                        if (orderItemLongestWaiting == null || order.OrderItems[i].PlacementTime < orderItemLongestWaiting.PlacementTime)
                            orderItemLongestWaiting = order.OrderItems[i];
                }
            }

            CalculateWaitingTime(orderItemLongestWaiting);
        }

        internal void UpdateWaitingTime()
        {
            if (RunningOrders.Count > 0)
                SetWaitingTime();
        }
    }
}