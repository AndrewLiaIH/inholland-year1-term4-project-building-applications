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
        private OrderService orderService;
        private MenuService menuService;
        private Table orderingTable;
        public List<MenuItem> softDrinks, beers, wines, spirits, cofeeAndTea;
        public List<MenuItem> dinnerStarters, entremets, dinnerMains, dinnerDesserts;
        public List<MenuItem> lunchStarters, lunchMains, lunchDesserts;
        public ObservableCollection<OrderItem> orderItems;
        private const int temporaryNumber = 0;

        UserControlHeader ILoggedInEmployeeHandler.UserControlHeader => userControlHeader;

        public UserControlOrderView(Table table)
        {
            InitializeComponent();
            DataContext = this;
            orderService = new();
            orderingTable = table;
            menuService = new();
            orderItems = new();

            List<MenuItem> allMenuItems = menuService.GetAllMenuItems();
            List<MenuItem> drinkItems = new();
            List<MenuItem> dinnerItems = new();
            List<MenuItem> lunchItems = new();

            foreach (MenuItem item in allMenuItems)
                switch (item.Category.MenuCard.MenuType)
                {
                    case MenuType.Drinks: drinkItems.Add(item); break;
                    case MenuType.Dinner: dinnerItems.Add(item); break;
                    case MenuType.Lunch: lunchItems.Add(item); break;
                    default: break;
                }

            LoadDrinks(drinkItems);
            LoadDinner(dinnerItems);
            LoadLunch(lunchItems);
            DrinksSoftDrinksListBox.ItemsSource = softDrinks;
            DrinksBeersListBox.ItemsSource = beers;
            DrinksWinesListBox.ItemsSource = wines;
            DrinksSpiritsListBox.ItemsSource = spirits;
            DrinksCoffeeAndTeaListBox.ItemsSource = cofeeAndTea;
            DinnerStartersListBox.ItemsSource = dinnerStarters;
            DinnerEntremetsListBox.ItemsSource = entremets;
            DinnerMainsListBox.ItemsSource = dinnerMains;
            DinnerDessertsListBox.ItemsSource = dinnerDesserts;
            LunchStartersListBox.ItemsSource = lunchStarters;
            LunchMainsListBox.ItemsSource = lunchMains;
            LunchDessertsListBox.ItemsSource= lunchDesserts;
            OrderItemsControl.ItemsSource = orderItems;
        }

        private void LoadDrinks(List<MenuItem> drinkItems)
        {
            softDrinks = new(); beers = new(); wines = new(); spirits = new(); cofeeAndTea = new();
            foreach (MenuItem item in drinkItems)
                switch (item.Category.CategoryType)
                {
                    case CategoryType.SoftDrinks:
                        softDrinks.Add(item); break;
                    case CategoryType.BeersOnTap:
                        beers.Add(item); break;
                    case CategoryType.Wines:
                        wines.Add(item); break;
                    case CategoryType.SpiritDrinks:
                        spirits.Add(item); break;
                    case CategoryType.CoffeeTea:
                        cofeeAndTea.Add(item); break;
                    default: break;
                }
        }

        private void LoadDinner(List<MenuItem> dinnerItems)
        {
            dinnerStarters = new(); entremets = new(); dinnerMains = new(); dinnerDesserts = new();
            foreach (MenuItem item in dinnerItems)
                switch (item.Category.CategoryType)
                {
                    case CategoryType.Starters:
                        dinnerStarters.Add(item); break;
                    case CategoryType.Entremets:
                        entremets.Add(item); break;
                    case CategoryType.Mains:
                        dinnerMains.Add(item); break;
                    case CategoryType.Deserts:
                        dinnerDesserts.Add(item); break;
                    default: break;
                }
        }

        private void LoadLunch(List<MenuItem> lunchItems)
        {
            lunchStarters = new(); lunchMains = new(); lunchDesserts = new();
            foreach (MenuItem item in lunchItems)
                switch (item.Category.CategoryType)
                {
                    case CategoryType.Starters:
                        lunchStarters.Add(item); break;
                    case CategoryType.Mains:
                        lunchMains.Add(item); break;
                    case CategoryType.Deserts:
                        lunchDesserts.Add(item); break;
                    default: break;
                }
        }

        private void MenuItemButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItem newOrderItem = new(temporaryNumber, temporaryNumber, (MenuItem)(sender as Button).DataContext, DateTime.Now, OrderStatus.Waiting, DateTime.Now, 1, string.Empty);
            orderItems.Add(newOrderItem);
        }

        private void IncreaseQuantityButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (OrderItem)(sender as Button).DataContext;
            orderItem.IncreaseQuantity();
            UpdateOrderItemsControl();
        }

        private void DecreaseQuantityButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (OrderItem)(sender as Button).DataContext;
            orderItem.DecreaseQuantity();
            if (orderItem.Quantity == 0)
                orderItems.Remove(orderItem);
            UpdateOrderItemsControl();
        }

        private void EditCommentButton_Click(object sender, RoutedEventArgs e)
        {
            OrderItem orderItem = (OrderItem)(sender as Button).DataContext;
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter your comment:", "Edit Comment", orderItem.Comment);
            orderItem.Comment = input;
            UpdateOrderItemsControl();
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel the order?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                orderItems.Clear();
                Content = new UserControlTableView();
            }            
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            Order mostRecentOrder = orderService.GetMostRecentOrder();
            int newOrderNumber = mostRecentOrder.OrderNumber + 1;
            int? newServingNumber = mostRecentOrder.ServingNumber < 99 ? mostRecentOrder.ServingNumber + 1 : 1;
            decimal totalPrice = orderService.GetTotalPrice(orderItems);
            Order newOrder = new(temporaryNumber, orderingTable, userControlHeader.LoggedInEmployee, newOrderNumber, newServingNumber, false, totalPrice);
            orderService.CreateOrder(newOrder);
            menuService.UpdateStock(orderItems);
            Content = new UserControlTableView();
        }

        private void UpdateOrderItemsControl()
        {
            OrderItem[] itemsSource = orderItems.ToArray();
            orderItems.Clear();
            foreach (OrderItem item in itemsSource)
            {
                orderItems.Add(item);
            }
        }

        void ILoggedInEmployeeHandler.SetLoggedInEmployee(Employee employee)
        {
            userControlHeader.LoggedInEmployee = employee;
        }
    }
}
