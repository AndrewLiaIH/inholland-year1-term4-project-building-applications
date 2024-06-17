using Model;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlKitchenView.xaml
    /// </summary>
    public partial class UserControlKitchenView : UserControl, ILoggedInEmployeeHandler
    {
        public List<Folder> FoldersKitchen;
        public UserControlHeader UserControlHeader => userControlHeader;
        public UserControlKitchenViewRunning userControlKitchenViewRunning;
        public UserControlKitchenViewFinished userControlKitchenViewFinished;

        public UserControlKitchenView()
        {
            InitializeComponent();

            FoldersKitchen = new()
            {
                new Folder("Running", ShowKitchenViewRunning),
                new Folder("Finished", ShowKitchenViewFinished)
            };

            userControlHeader.Folders = FoldersKitchen;
            userControlHeader.SelectedFolder = FoldersKitchen.First();
        }

        public void SetLoggedInEmployee(Employee employee)
        {
            userControlHeader.LoggedInEmployee = employee;
        }

        private void ShowKitchenViewRunning()
        {
            userControlKitchenViewRunning = new();

            KitchenViewContentControl.Content = userControlKitchenViewRunning;

            if (userControlHeader.LoggedInEmployee != null)
            {
                bool forKitchen = userControlHeader.LoggedInEmployee.Type == EmployeeType.Chef ? true : false;
                userControlKitchenViewRunning.LoadOrders(forKitchen);
            }
        }

        private void ShowKitchenViewFinished()
        {
            userControlKitchenViewFinished = new();

            KitchenViewContentControl.Content = userControlKitchenViewFinished;

            bool forKitchen = userControlHeader.LoggedInEmployee.Type == EmployeeType.Chef ? true : false;
            userControlKitchenViewFinished.LoadOrders(forKitchen);
        }
    }
}
