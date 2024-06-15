using Model;
using Service;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Media;
using Microsoft.IdentityModel.Tokens;
using System.Windows;
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
        private DispatcherTimer timer;

        public UserControlKitchenViewRunning()
        {
            InitializeComponent();
            Loaded += UserControlKitchenView_Loaded;
            InitializeTimer();
            DataContext = this;
        }

        private void UserControlKitchenView_Loaded(object sender, RoutedEventArgs e)
        {
            AttachEventHandlers();
        }

        private void AttachEventHandlers()
        {
            // Find the ItemsControl that uses the CategoryTemplate
            ItemsControl itemsControl = FindName("OrdersItemsControl") as ItemsControl;
            if (itemsControl != null)
            {
                foreach (var item in itemsControl.Items)
                {
                    ContentPresenter container = itemsControl.ItemContainerGenerator.ContainerFromItem(item) as ContentPresenter;
                    if (container != null)
                    {
                        Button editButton = FindVisualChild<Button>(container, "editStatus");
                        Button changeButton = FindVisualChild<Button>(container, "changeStatus");

                        if (editButton != null)
                        {
                            editButton.Click += EditStatus_Click;
                        }
                        if (changeButton != null)
                        {
                            changeButton.Click += ChangeStatus_Click;
                        }
                    }
                }
            }
        }

        public void LoadOrders(bool forKitchen)
        {
            Orders = orderService.GetAllKitchenBarOrders(forKitchen, true);
        }

        private void InitializeTimer()
        {
            timer = new()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            foreach (Order order in OrdersItemsControl.Items)
            {
                ContentPresenter container = OrdersItemsControl.ItemContainerGenerator.ContainerFromItem(order) as ContentPresenter;
                TextBlock textBlock = container?.ContentTemplate.FindName("RunningTimeTextBlock", container) as TextBlock;
                Rectangle headerBackground = (Rectangle)container?.ContentTemplate.FindName("CardHeaderBackground", container);

                if (textBlock != null)
                {
                    TimeSpan runningTime = order.OrderItems.IsNullOrEmpty() ? TimeSpan.Zero : order.OrderItems[0].RunningTime;
                    textBlock.Text = $"{runningTime.Minutes:D2}:{runningTime.Seconds:D2}";

                    switch (runningTime.Minutes)
                    {
                        case > 10:
                            headerBackground.Fill = (SolidColorBrush)FindResource("Color6");
                            break;
                        case > 5:
                            headerBackground.Fill = (SolidColorBrush)FindResource("Color8");
                            break;
                        default:
                            headerBackground.Fill = (SolidColorBrush)FindResource("Color9");
                            break;
                    }
                }
            }
        }

        private void EditStatus_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                ContextMenu contextMenu = new ContextMenu();
                System.Windows.Controls.MenuItem waitingItem = new() { Header = "Waiting" };
                System.Windows.Controls.MenuItem preparingItem = new() { Header = "Preparing" };
                System.Windows.Controls.MenuItem doneItem = new() { Header = "Done" };

                waitingItem.Click += (s, args) => ChangeStatus(OrderStatus.Waiting, button);
                preparingItem.Click += (s, args) => ChangeStatus(OrderStatus.Preparing, button);
                doneItem.Click += (s, args) => ChangeStatus(OrderStatus.Done, button);

                contextMenu.Items.Add(waitingItem);
                contextMenu.Items.Add(preparingItem);
                contextMenu.Items.Add(doneItem);

                button.ContextMenu = contextMenu;
                contextMenu.IsOpen = true;
            }
        }

        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                CategoryGroup categoryGroup = button.DataContext as CategoryGroup;
                OrderStatus currentStatus = (OrderStatus)categoryGroup?.CategoryStatus;

                if (currentStatus != OrderStatus.Done)
                {
                    OrderStatus nextStatus = GetNextStatus(currentStatus);
                    ChangeStatus(nextStatus, button);
                }

                UpdateButtonStyles(button, currentStatus);
            }
        }

        private OrderStatus GetNextStatus(OrderStatus currentStatus)
        {
            if (currentStatus == OrderStatus.Waiting)
                return OrderStatus.Preparing;
            else
                return OrderStatus.Done;
        }

        private void ChangeStatus(OrderStatus newStatus, Button button)
        {
            CategoryGroup categoryGroup = button.DataContext as CategoryGroup;

            if (categoryGroup != null)
            {
                categoryGroup.CategoryStatus = newStatus;

                foreach (OrderItem item in categoryGroup.Items)
                    item.SetItemStatus(newStatus);

                UpdateStatusesInDatabase(categoryGroup.Items, newStatus);
            }
        }

        private void UpdateStatusesInDatabase(List<OrderItem> items, OrderStatus? newStatus)
        {
            orderService.UpdateOrderItemsStatus(items);
        }

        private void UpdateButtonStyles(Button button, OrderStatus newStatus)
        {
            button.Content = newStatus == OrderStatus.Done ? OrderStatus.Done : $"Finish {OrderStatus.Preparing.ToString().ToLower()}";
            button.Background = newStatus == OrderStatus.Done ? (SolidColorBrush)FindResource("Color11") : (SolidColorBrush)FindResource("Color12");

            if (newStatus == OrderStatus.Done)
                button.IsEnabled = false;
        }

        private static T FindVisualChild<T>(DependencyObject obj, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T tChild && ((FrameworkElement)child).Name == name)
                    return tChild;

                T childOfChild = FindVisualChild<T>(child, name);
                if (childOfChild != null)
                    return childOfChild;
            }
            return null;
        }
    }
}
