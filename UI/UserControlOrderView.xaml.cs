using Model;
using Service;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using MenuItem = Model.MenuItem;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlOrderView.xaml
    /// </summary>
    public partial class UserControlOrderView : UserControl, ILoggedInEmployeeHandler
    {
        public UserControlHeader UserControlHeader => userControlHeader;
        private OrderService orderService;
        private MenuService menuService;
        private Order newOrder;
        private ObservableCollection<OrderItem> orderItems;
        private Action returnToTableOverview;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserControlOrderView"/> class.
        /// Sets up necessary services, creates a new order, and binds menu data to UI elements.
        /// </summary>
        /// <param name="table">The table associated with the order.</param>
        /// <param name="employee">The employee taking the order.</param>
        /// <param name="returnToTableOverview">The action to return to the table overview.</param>
        public UserControlOrderView(Table table, Employee employee, Action returnToTableOverview)
        {
            InitializeComponent();
            try
            {
                orderService = new();
                menuService = new();
                newOrder = new(table, employee);
                orderItems = new();
                this.returnToTableOverview = returnToTableOverview;

                Dictionary<MenuType, Dictionary<CategoryType, List<MenuItem>>> menu = menuService.GetFullMenu();
                AssignBindings(menu);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while initializing the order view: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Binds menu data to the ListBox controls in the UI.
        /// </summary>
        /// <param name="menu">The menu data to bind to the UI.</param>
        private void AssignBindings(Dictionary<MenuType, Dictionary<CategoryType, List<MenuItem>>> menu)
        {
            SoftDrinksListBox.ItemsSource = menu[MenuType.Drinks][CategoryType.SoftDrinks];
            BeersListBox.ItemsSource = menu[MenuType.Drinks][CategoryType.BeersOnTap];
            WinesListBox.ItemsSource = menu[MenuType.Drinks][CategoryType.Wines];
            SpiritsListBox.ItemsSource = menu[MenuType.Drinks][CategoryType.SpiritDrinks];
            CoffeeTeaListBox.ItemsSource = menu[MenuType.Drinks][CategoryType.CoffeeTea];
            DinnerStartersListBox.ItemsSource = menu[MenuType.Dinner][CategoryType.Starters];
            DinnerEntremetsListBox.ItemsSource = menu[MenuType.Dinner][CategoryType.Entremets];
            DinnerMainsListBox.ItemsSource = menu[MenuType.Dinner][CategoryType.Mains];
            DinnerDessertsListBox.ItemsSource = menu[MenuType.Dinner][CategoryType.Deserts];
            LunchStartersListBox.ItemsSource = menu[MenuType.Lunch][CategoryType.Starters];
            LunchMainsListBox.ItemsSource = menu[MenuType.Lunch][CategoryType.Mains];
            LunchDessertsListBox.ItemsSource = menu[MenuType.Lunch][CategoryType.Deserts];
            OrderItemsListBox.ItemsSource = orderItems;
        }

        /// <summary>
        /// Adds a new order item when a menu item button is clicked.
        /// </summary>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OrderItem newOrderItem = new((sender as Button).Tag as MenuItem, 1);
            newOrder.AddOrderItem(newOrderItem);
            UpdateOrderOverview();
        }

        /// <summary>
        /// Increases the quantity of the selected order item.
        /// </summary>
        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (sender as Button).Tag as OrderItem;
            newOrder.IncreaseOrderItemQuantity(orderItem);
            UpdateOrderOverview();
        }

        /// <summary>
        /// Decreases the quantity of the selected order item.
        /// </summary>
        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (sender as Button).Tag as OrderItem;
            newOrder.DecreaseOrderItemQuantity(orderItem);
            UpdateOrderOverview();
        }

        /// <summary>
        /// Edits the comment of the selected order item.
        /// </summary>
        private void EditComment_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (sender as Button).Tag as OrderItem;
            string comment = Microsoft.VisualBasic.Interaction.InputBox("Enter your comment:", "Edit Comment", orderItem.Comment);
            orderItem.SetComment(comment);
            UpdateOrderOverview();
        }

        /// <summary>
        /// Removes the selected order item.
        /// </summary>
        private void RemoveOrderItem_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (sender as Button).Tag as OrderItem;
            newOrder.RemoveOrderItem(orderItem);
            UpdateOrderOverview();
        }

        /// <summary>
        /// Cancels the current order and returns to the table overview.
        /// </summary>
        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel the order?", "Confirm cancelation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
                returnToTableOverview();
        }

        /// <summary>
        /// Places the current order and returns to the table overview.
        /// </summary>
        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to place the order?", "Confirm order", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    orderService.LoadNewOrder(newOrder);
                    MessageBox.Show("Order placed successfully!", "Success", MessageBoxButton.OK);
                    returnToTableOverview();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while placing the order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Updates the order overview to reflect the current state of the order.
        /// </summary>
        private void UpdateOrderOverview()
        {
            orderItems.Clear();
            foreach (OrderItem orderItem in newOrder.OrderItems)
                orderItems.Add(orderItem);
        }

        /// <summary>
        /// Sets the currently logged-in employee.
        /// </summary>
        /// <param name="employee">The employee to set as logged-in.</param>
        public void SetLoggedInEmployee(Employee employee)
        {
            userControlHeader.LoggedInEmployee = employee;
        }
    }
}
