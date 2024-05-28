using Model;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlLogin.xaml
    /// </summary>
    public partial class UserControlLoginView : UserControl
    {
        public Employee LoggedInEmployee { get; private set; }

        public static readonly RoutedEvent LoginEvent = EventManager.RegisterRoutedEvent(
            "LoginSuccessful", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserControlLoginView));

        public event RoutedEventHandler LoginSuccessful
        {
            add { AddHandler(LoginEvent, value); }
            remove { RemoveHandler(LoginEvent, value); }
        }

        public UserControlLoginView()
        {
            InitializeComponent();
        }

        private void ClickHandler(object sender, RoutedEventArgs e)
        {
            LoggedInEmployee = GetEmployee();
            RaiseEvent(new RoutedEventArgs(LoginEvent));
        }

        private Employee GetEmployee()
        {
            int login = int.Parse(UserLoginTextBox.Text);
            string password = PasswordTextBox.Text;
            EmployeeService employeeService = new();

            return employeeService.GetEmployeeByLoginAndPassword(login, password);
        }
    }
}