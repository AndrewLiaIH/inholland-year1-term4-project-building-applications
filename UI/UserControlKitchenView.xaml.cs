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
        private UserControlKitchenViewRunning userControlKitchenViewRunning;
        private UserControlKitchenViewFinished userControlKitchenViewFinished;

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
            userControlHeader.CounterWaiting.Visibility = Visibility.Visible;
            userControlHeader.CounterPreparing.Visibility = Visibility.Visible;
        }

        public void SetLoggedInEmployee(Employee employee)
        {
            userControlHeader.LoggedInEmployee = employee;
        }

        private void ShowKitchenViewRunning()
        {
            if (userControlKitchenViewRunning == null)
                userControlKitchenViewRunning = new();

            KitchenViewContentControl.Content = userControlKitchenViewRunning;
        }

        private void ShowKitchenViewFinished()
        {
            if (userControlKitchenViewFinished == null)
                userControlKitchenViewFinished = new();

            KitchenViewContentControl.Content = userControlKitchenViewFinished;
        }
    }
}
