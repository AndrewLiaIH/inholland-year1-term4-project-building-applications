using Microsoft.IdentityModel.Tokens;
using Model;
using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace UI
{
    /// <summary>
    /// </summary>
    public partial class UserControlKitchenViewRunning : UserControl
    {
        private OrderService orderService = new();
        public List<Order> Orders { get; private set; }
        private DispatcherTimer timer;
        private bool forKitchen;

        public UserControlKitchenViewRunning()
        {
            InitializeComponent();
            Loaded += UserControlKitchenView_Loaded;
            orderService.OrdersChanged += UserControlKitchenView_OrdersChanged;
            InitializeTimer();
        }

        private void UserControlKitchenView_OrdersChanged()
        {
            RefreshOrders(forKitchen, true);
        }

        private void UserControlKitchenView_Loaded(object sender, RoutedEventArgs e)
        {
            AttachEventHandlers();
        }

        private void AttachEventHandlers()
        {
            foreach (Order item in OrdersItemsControl.Items)
            {
                ContentPresenter container = OrdersItemsControl.ItemContainerGenerator.ContainerFromItem(item) as ContentPresenter;
                AttachHandlersToContainer(container);
            }
        }

        private void AttachHandlersToContainer(ContentPresenter container)
        {
            if (container != null)
            {
                ItemsControl categoriesItemsControl = FindVisualChild<ItemsControl>(container, "CategoriesItemsControl");

                if (categoriesItemsControl != null)
                {
                    foreach (CategoryGroup categoryItem in categoriesItemsControl.Items)
                    {
                        ContentPresenter categoryContainer = categoriesItemsControl.ItemContainerGenerator.ContainerFromItem(categoryItem) as ContentPresenter;

                        if (categoryContainer != null)
                        {
                            Button editButton = FindVisualChild<Button>(categoryContainer, "editStatus");
                            Button changeButton = FindVisualChild<Button>(categoryContainer, "changeStatus");

                            if (editButton != null)
                            {
                                editButton.Click -= EditStatus_Click;
                                editButton.Click += EditStatus_Click;
                            }
                            if (changeButton != null)
                            {
                                changeButton.Click -= ChangeStatus_Click;
                                changeButton.Click += ChangeStatus_Click;
                            }
                        }
                    }
                }
            }
        }

        public void LoadOrders(bool forKitchen)
        {
            this.forKitchen = forKitchen;
            RefreshOrders(forKitchen, true);
            DataContext = this;
        }

        public void RefreshOrders(bool forKitchen, bool isRunning)
        {
            Orders = orderService.GetAllKitchenBarOrders(forKitchen, isRunning);
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
                OrderStatus nextStatus = GetNextStatus(currentStatus);

                if (currentStatus != OrderStatus.Done)
                    ChangeStatus(nextStatus, button);

                UpdateButtonStyles(button, nextStatus);
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
            Order order = FindOrderForCategoryGroup(categoryGroup);

            if (categoryGroup != null)
            {
                categoryGroup.CategoryStatus = newStatus;

                foreach (OrderItem item in categoryGroup.Items)
                    item.SetItemStatus(newStatus);

                order.Status = GetOrderStatus(order);

                orderService.UpdateOrderItemsStatus(categoryGroup.Items);

                // Traverse the visual tree to find the parent container
                DependencyObject parent = button;
                while (parent != null && !(parent is ContentPresenter))
                {
                    parent = VisualTreeHelper.GetParent(parent);
                }

                ContentPresenter container = parent as ContentPresenter;
                Button changeStatusButton = FindVisualChild<Button>(container, "changeStatus");

                if (changeStatusButton != null)
                {
                    UpdateButtonStyles(changeStatusButton, newStatus);
                }
            }
        }

        private OrderStatus? GetOrderStatus(Order order)
        {
            OrderStatus? status = null;
            bool isNotDone = false;

            foreach (OrderItem item in order.OrderItems)
            {
                if (item.ItemStatus != OrderStatus.Done)
                    isNotDone = true;

                if (status == null)
                    status = item.ItemStatus;
                else if (item.ItemStatus == OrderStatus.Preparing)
                    status = item.ItemStatus;
                else if (item.ItemStatus == OrderStatus.Waiting && status != OrderStatus.Preparing)
                    status = item.ItemStatus;
            }

            if (!isNotDone)
                status = OrderStatus.Done;

            return status;
        }

        private void UpdateButtonStyles(Button button, OrderStatus newStatus)
        {
            button.Content = newStatus switch
            {
                OrderStatus.Done => OrderStatus.Done,
                OrderStatus.Preparing => "Finish preparing",
                _ => "Start preparing"

            };

            button.Background = newStatus switch
            {
                OrderStatus.Done => (SolidColorBrush)FindResource("Color11"),
                OrderStatus.Preparing => (SolidColorBrush)FindResource("Color12"),
                _ => (SolidColorBrush)FindResource("Color2")

            };

            button.IsEnabled = newStatus switch
            {
                OrderStatus.Done => false,
                OrderStatus.Preparing => true,
                _ => true

            };
        }

        private Order FindOrderForCategoryGroup(CategoryGroup categoryGroup)
        {
            foreach (OrderItem item in categoryGroup.Items)
            {
                Order order = Orders.FirstOrDefault(o => o.OrderItems.Any(oi => oi.DatabaseId == item.DatabaseId));

                if (order != null)
                    return order;
            }

            return null;
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
