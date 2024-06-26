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
        public ObservableCollection<OrderItem> orderItems;
        Action returnToTableOverview;


        public UserControlOrderView(Table table, Employee employee, Action returnToTableOverview)
        {
            InitializeComponent();
            DataContext = this;
            orderService = new();
            menuService = new();
            newOrder = new(table, employee);
            orderItems = new();
            this.returnToTableOverview = returnToTableOverview;

            List<MenuItem> allMenuItems = menuService.GetAllMenuItems();
            Dictionary<MenuType, Dictionary<CategoryType, List<MenuItem>>> menu = LoadMenu(allMenuItems);
            AssignBindings(menu);
        }

        private Dictionary<MenuType, Dictionary<CategoryType, List<MenuItem>>> LoadMenu(List<MenuItem> allMenuItems)
        {
            Dictionary<MenuType, Dictionary<CategoryType, List<MenuItem>>> menu = new Dictionary<MenuType, Dictionary<CategoryType, List<MenuItem>>>();
            foreach (MenuItem item in allMenuItems)
            {
                if (!menu.ContainsKey(item.Category.MenuCard.MenuType))
                {
                    menu.Add(item.Category.MenuCard.MenuType, new Dictionary<CategoryType, List<MenuItem>>());
                    menu[item.Category.MenuCard.MenuType].Add(item.Category.CategoryType, new List<MenuItem>());
                }
                else if (!menu[item.Category.MenuCard.MenuType].ContainsKey(item.Category.CategoryType))
                    menu[item.Category.MenuCard.MenuType].Add(item.Category.CategoryType, new List<MenuItem>());
                menu[item.Category.MenuCard.MenuType][item.Category.CategoryType].Add(item);
            }
            return menu;
        }

        public void AssignBindings(Dictionary<MenuType, Dictionary<CategoryType, List<MenuItem>>> menu)
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            OrderItem newOrderItem = new((MenuItem)(sender as Button).DataContext, 1);
            newOrder.AddOrderItem(newOrderItem);
            UpdateOrderOverview();
        }

        private void IncreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (sender as Button).Tag as OrderItem;
            newOrder.IncreaseOrderItemQuantity(orderItem);
            UpdateOrderOverview();
        }

        private void DecreaseQuantity_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (sender as Button).Tag as OrderItem;
            newOrder.DecreaseOrderItemQuantity(orderItem);
            UpdateOrderOverview();
        }

        private void EditComment_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (sender as Button).Tag as OrderItem;
            orderItem.Comment = Microsoft.VisualBasic.Interaction.InputBox("Enter your comment:", "Edit Comment", orderItem.Comment);
            UpdateOrderOverview();
        }

        private void RemoveOrderItem_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (sender as Button).Tag as OrderItem;
            newOrder.OrderItems.Remove(orderItem);
            UpdateOrderOverview();
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel the order?", "Confirm cancelation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                newOrder.OrderItems.Clear();
                UpdateOrderOverview();
                returnToTableOverview();
            }
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to place the order?", "Confirm order", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                orderService.LoadNewOrder(newOrder);
                MessageBox.Show("Order placed successfully!");
                returnToTableOverview();
            }
        }

        private void UpdateOrderOverview()
        {
            orderItems.Clear();
            foreach (OrderItem item in newOrder.OrderItems)
                orderItems.Add(item);
        }

        public void SetLoggedInEmployee(Employee employee)
        {
            userControlHeader.LoggedInEmployee = employee;
        }
    }
}
