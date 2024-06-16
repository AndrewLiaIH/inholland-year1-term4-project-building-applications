using DAL;
using Model;
using Service;
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
        private UserControlNetworkError userControlNetworkError;
        private EmployeeService employeeService;

        internal static readonly RoutedEvent RemoveEventsEvent = EventManager.RegisterRoutedEvent(
        "RemoveEvents", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserControlTable));

        internal event RoutedEventHandler RemoveEvents
        {
            add { AddHandler(RemoveEventsEvent, value); }
            remove { RemoveHandler(RemoveEventsEvent, value); }
        }

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
            employeeService = new();
            employeeService.NetworkExceptionOccurred += NetworkExceptionOccurred;
        }

        private void NetworkExceptionOccurred()
        {
            Dispatcher.Invoke(() =>
            { 
            userControlNetworkError ??= new();
            MainContentControl.Content = userControlNetworkError;
            employeeService.RetryLogin += RetryLogin;
            });
        }

        private void RetryLogin()
        {
            Dispatcher.Invoke(() =>
            {
                userControlLoginView.Login();
                employeeService.RetryLogin -= RetryLogin;

                if (userControlLoginView.LoggedInEmployee == null) { }
                    ShowLoginView();
            });
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
            employeeService.NetworkExceptionOccurred -= NetworkExceptionOccurred;

            MainContentControl.Content = userControlTableView;
        }

        private void ShowKitchenView()
        {
            userControlKitchenView = new();

            SetHeader(userControlKitchenView);
            MainContentControl.Content = userControlKitchenView;
            bool forKitchen = userControlLoginView.LoggedInEmployee.Type == EmployeeType.Chef ? true : false;
            userControlKitchenView.userControlKitchenViewRunning.LoadOrders(forKitchen);
        }

        private void ShowOrderView()
        {
            if (userControlOrderView == null)
                userControlOrderView = new();

            // SetHeader(userControlOrderView);

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
            ShowOrderView();
        }

        private void UserControlHeader_Logout(object sender, EventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            employeeService.NetworkExceptionOccurred += NetworkExceptionOccurred;
            userControlLoginView.Refresh();
            ShowLoginView();

            if (userControlKitchenView != null)
                userControlKitchenView.userControlHeader.Logout -= UserControlHeader_Logout;

            if (userControlTableView != null)
            {
                userControlTableView.userControlHeader.Logout -= UserControlHeader_Logout;
            }

            if (userControlTableView != null)
            {
                userControlTableView.userControlHeader.Logout -= UserControlHeader_Logout;
            }
        }

        private void SetHeader(ILoggedInEmployeeHandler employeeHandler)
        {
            employeeHandler.SetLoggedInEmployee(userControlLoginView.LoggedInEmployee);
            employeeHandler.UserControlHeader.Logout += UserControlHeader_Logout;
        }
    }
}