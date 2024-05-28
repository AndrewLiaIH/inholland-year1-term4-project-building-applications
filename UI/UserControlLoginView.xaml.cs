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

        public UserControlLoginView()
        {
            InitializeComponent();
        }

        public static readonly RoutedEvent LoginSuccessfulEvent = EventManager.RegisterRoutedEvent(
            "LoginSuccessful", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(UserControlLoginView));

        // Provide CLR accessors for the event
        public event RoutedEventHandler LoginSuccessful
        {
            add { AddHandler(LoginSuccessfulEvent, value); }
            remove { RemoveHandler(LoginSuccessfulEvent, value); }
        }

        private void ClickHandler(object sender, RoutedEventArgs e)
        {
            LoggedInEmployee = GetEmployee();
            RaiseEvent(new RoutedEventArgs(LoginSuccessfulEvent));
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