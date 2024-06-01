using Model;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string WindowViewError = "An error occurred while displaying a window.";
        private Dictionary<EmployeeType, UserControl> visibilityMap;
        private UserControl currentView;

        public MainWindow()
        {
            InitializeComponent();

            visibilityMap = new()
            {
                { EmployeeType.NotSpecified, userControlLoginView },
                { EmployeeType.Chef, userControlKitchenView },
                { EmployeeType.Bartender, userControlKitchenView },
                { EmployeeType.Waiter, userControlTableView }
            };
        }

        private void UserControlLoginView_LoginSuccessful(object sender, RoutedEventArgs e)
        {
            UpdateCurrentView(userControlLoginView.LoggedInEmployee);

            if (currentView == userControlKitchenView)
            {
                userControlKitchenView.SetLoggedInEmployee(userControlLoginView.LoggedInEmployee);
                userControlKitchenView.userControlHeader.Logout += UserControlHeader_Logout;
            }
            else if (currentView == userControlTableView)
            {
                userControlTableView.SetLoggedInEmployee(userControlLoginView.LoggedInEmployee);
                userControlTableView.userControlHeader.Logout += UserControlHeader_Logout;
            }
        }

        public void UpdateCurrentView(Employee currentEmployee)
        {
            if (visibilityMap.TryGetValue(currentEmployee.Type, out UserControl currentView))
            {
                HideAllViews();
                ShowView(currentView);
                this.currentView = currentView;
            }
            else
            {
                throw new Exception(WindowViewError);
            }
        }

        private void UserControlHeader_Logout(object sender, EventArgs e)
        {
            Logout();
        }

        private void Logout()
        {
            HideAllViews();
            ShowView(userControlLoginView);
            userControlLoginView.Refresh();

            // Unsubscribe from logout event to avoid memory leaks
            userControlKitchenView.userControlHeader.Logout -= UserControlHeader_Logout;
        }

        private void HideAllViews()
        {
            foreach (KeyValuePair<EmployeeType, UserControl> visibility in visibilityMap)
                visibility.Value.Visibility = Visibility.Collapsed;
        }

        private void ShowView(UserControl view)
        {
            view.Visibility = Visibility.Visible;
        }
    }
}