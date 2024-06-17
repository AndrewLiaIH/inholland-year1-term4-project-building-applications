using Model;
using Service;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlKitchenView.xaml
    /// </summary>
    public partial class UserControlKitchenViewFinished : UserControl
    {
        private OrderService orderService = new();
        public List<Order> Orders { get; private set; }
        private bool forKitchen;

        public UserControlKitchenViewFinished()
        {
            InitializeComponent();
            Loaded += UserControlKitchenView_Loaded;
            orderService.OrdersChanged += UserControlKitchenView_OrdersChanged;
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

                            if (editButton != null)
                            {
                                editButton.Click -= EditStatus_Click;
                                editButton.Click += EditStatus_Click;
                            }
                        }
                    }
                }
            }
        }

        public void LoadOrders(bool forKitchen)
        {
            this.forKitchen = forKitchen;
            RefreshOrders(forKitchen, false);
            DataContext = this;
        }

        public void RefreshOrders(bool forKitchen, bool isRunning)
        {
            Orders = orderService.GetAllKitchenBarOrders(forKitchen, isRunning);
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

        private void ChangeStatus(OrderStatus newStatus, Button button)
        {
            CategoryGroup categoryGroup = button.DataContext as CategoryGroup;
            Order order = FindOrderForCategoryGroup(categoryGroup);

            if (categoryGroup != null)
            {
                foreach (OrderItem item in categoryGroup.Items)
                    item.SetItemStatus(newStatus);

                order.Status = GetOrderStatus(order);

                orderService.UpdateOrderItemsStatus(categoryGroup.Items);

                categoryGroup.CategoryStatus = newStatus;
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

        private Order FindOrderForCategoryGroup(CategoryGroup categoryGroup)
        {
            foreach (OrderItem item in categoryGroup.Items)
            {
                Order order = Orders.FirstOrDefault(o => o.OrderItems.Contains(item));

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
