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

        private EmployeeService employeeService = new();
        private bool isNetworkExceptionSubscribed = false;

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

            SubscribeNetworkException();
            ShowLoginView();
        }

        /// <summary>
        /// Method that is called when network exception occurs. It creates a new UserControlNetworkError and sets it as the main content.
        /// After RetryLogin is called, it waits for the connection to be available and then calls the Login method.
        /// </summary>
        private void NetworkExceptionOccurred()
        {
            Dispatcher.Invoke(() =>
            {
                ShowNetworkErrorView();
                employeeService.RetryLogin += RetryLogin;
            });
        }

        private void RetryLogin()
        {
            Task.Run(async () =>
            {
                while (!employeeService.ConnectionAvalible<EmployeeDao>())
                {
                    await Task.Delay(4000);
                }

                Dispatcher.Invoke(() =>
                {
                    userControlLoginView.Login();
                    employeeService.RetryLogin -= RetryLogin;

                    if (userControlLoginView.LoggedInEmployee == null)
                    {
                        ShowLoginView();
                    }
                });
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
            SubscribeNetworkException();
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
            MainContentControl.Content = userControlKitchenView;
            bool forKitchen = userControlLoginView.LoggedInEmployee.Type == EmployeeType.Chef ? true : false;
            userControlKitchenView.userControlKitchenViewRunning.LoadOrders(forKitchen);
        }

        private void ShowOrderView()
        {
            userControlOrderView ??= new();
            MainContentControl.Content = userControlOrderView;
        }

        private void ShowNetworkErrorView()
        {
            userControlNetworkError ??= new();
            MainContentControl.Content = userControlNetworkError;
        }

        private void UserControlLoginView_LoginSuccessful(object sender, RoutedEventArgs e)
        {
            UpdateCurrentView(userControlLoginView.LoggedInEmployee);
            UnsubscribeNetworkException();
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
            userControlLoginView.Refresh();
            ShowLoginView();

            if (userControlKitchenView != null)
                userControlKitchenView.userControlHeader.Logout -= UserControlHeader_Logout;

            if (userControlTableView != null)
                userControlTableView.userControlHeader.Logout -= UserControlHeader_Logout;

            if (userControlTableView != null)
                userControlTableView.userControlHeader.Logout -= UserControlHeader_Logout;
        }

        private void SetHeader(ILoggedInEmployeeHandler employeeHandler)
        {
            employeeHandler.SetLoggedInEmployee(userControlLoginView.LoggedInEmployee);
            employeeHandler.UserControlHeader.Logout += UserControlHeader_Logout;
        }

        private void SubscribeNetworkException()
        {
            if (!isNetworkExceptionSubscribed)
            {
                employeeService.NetworkExceptionOccurred += NetworkExceptionOccurred;
                isNetworkExceptionSubscribed = true;
            }
        }

        private void UnsubscribeNetworkException()
        {
            if (isNetworkExceptionSubscribed)
            {
                employeeService.NetworkExceptionOccurred -= NetworkExceptionOccurred;
                isNetworkExceptionSubscribed = false;
            }
        }
    }
}