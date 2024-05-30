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
        }

        public void UpdateCurrentView(Employee currentEmployee)
        {
            if (visibilityMap.TryGetValue(currentEmployee.Type, out UserControl currentView))
            {
                foreach (KeyValuePair<EmployeeType, UserControl> visibility in visibilityMap)
                    visibility.Value.Visibility = Visibility.Hidden;

                currentView.Visibility = Visibility.Visible;
            }
            else
            {
                throw new Exception(WindowViewError);
            }
        }
    }
}