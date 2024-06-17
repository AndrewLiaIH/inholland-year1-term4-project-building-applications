using Model;
using System.Windows;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<EmployeeType, Action> visibilityMap;

        private UserControlLoginView userControlLoginView;
        private UserControlTableView userControlTableView;
        private UserControlOrderView userControlOrderView;
        private UserControlKitchenView userControlKitchenView;

        public MainWindow()
        {
            InitializeComponent();

            visibilityMap = new()
            {
                { EmployeeType.NotSpecified, ShowLoginView },
                { EmployeeType.Chef, ShowKitchenView },
                { EmployeeType.Bartender, ShowKitchenView },
                { EmployeeType.Waiter, ShowTableView }
            };

            ShowLoginView();
        }

        private void ShowLoginView()
        {
            if (userControlLoginView == null)
            {
                userControlLoginView = new();
                userControlLoginView.LoginSuccessful += UserControlLoginView_LoginSuccessful;
            }

            MainContentControl.Content = userControlLoginView;
        }

        private void ShowTableView()
        {
            if (userControlTableView == null)
            {
                userControlTableView = new();
                userControlTableView.AddHandler(UserControlTable.OrderClickedEvent, new RoutedEventHandler(UserControlTable_OrderClicked));
            }

            SetHeader(userControlTableView);

            MainContentControl.Content = userControlTableView;
        }

        private void ShowKitchenView()
        {
            userControlKitchenView = new();

            SetHeader(userControlKitchenView);
            bool forKitchen = userControlLoginView.LoggedInEmployee.Type == EmployeeType.Chef ? true : false;
            userControlKitchenView.userControlKitchenViewRunning.LoadOrders(forKitchen);

            MainContentControl.Content = userControlKitchenView;
        }

        private void ShowOrderView(Table table)
        {
            if (userControlOrderView == null)
                userControlOrderView = new(table);

            SetHeader(userControlOrderView);

            MainContentControl.Content = userControlOrderView;
        }

        private void UserControlLoginView_LoginSuccessful(object sender, RoutedEventArgs e)
        {
            UpdateCurrentView(userControlLoginView.LoggedInEmployee);
        }

        public void UpdateCurrentView(Employee currentEmployee)
        {
            Action showView = visibilityMap[currentEmployee.Type];
            showView();
        }

        private void UserControlTable_OrderClicked(object sender, RoutedEventArgs e)
        {
            ShowOrderView((e as OrderClickedEventArgs).Table);
        }

        private void UserControlHeader_Logout(object sender, EventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            userControlLoginView.Refresh();
            ShowLoginView();

            if (userControlKitchenView != null)
                userControlKitchenView.userControlHeader.Logout -= UserControlHeader_Logout;

            if (userControlTableView != null)
                userControlTableView.userControlHeader.Logout -= UserControlHeader_Logout;
        }

        private void SetHeader(ILoggedInEmployeeHandler employeeHandler)
        {
            employeeHandler.SetLoggedInEmployee(userControlLoginView.LoggedInEmployee);
            employeeHandler.UserControlHeader.Logout += UserControlHeader_Logout;
        }
    }
}