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
        private TextBox activeTextBox;

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
            InitializeTextBox();
        }

        private void InitializeTextBox()
        {
            activeTextBox = UserLoginTextBox;
            activeTextBox.IsReadOnly = true;
        }

        private Employee GetEmployee()
        {
            int login = int.Parse(UserLoginTextBox.Text);
            string password = PasswordTextBox.Tag.ToString();
            EmployeeService employeeService = new();

            return employeeService.GetEmployeeByLoginAndPassword(login, password);
        }

        private void NumpadNumberClick(object sender, RoutedEventArgs e)
        {
            string input = (sender as Button).Tag.ToString();

            if (activeTextBox != null && activeTextBox.IsReadOnly)
                if (activeTextBox == UserLoginTextBox)
                {
                    activeTextBox.Text += input;
                }
                else
                {
                    activeTextBox.Text += "•";
                    activeTextBox.Tag += input;
                }            
        }

        /// <summary>
        /// Click events for all buttons of the numpad
        /// </summary>
        private void BackSpaceClick(object sender, RoutedEventArgs e)
        {
            if (activeTextBox != null && activeTextBox.Text.Length > 0)
            {
                activeTextBox.Text = activeTextBox.Text.Remove(activeTextBox.Text.Length - 1);

                if (activeTextBox == PasswordTextBox)
                    activeTextBox.Tag = activeTextBox.Tag.ToString().Remove(activeTextBox.Tag.ToString().Length - 1);
            }

        }

        private void ClearClick(object sender, RoutedEventArgs e)
        {
            if (activeTextBox != null)
            {
                activeTextBox.Text = string.Empty;

                if (activeTextBox == PasswordTextBox)
                    activeTextBox.Tag = string.Empty;
            }
        }

        /// <summary>
        /// Click events which change the active TextBox and make it possible to only use the numpad for input
        /// </summary>
        private void LoginFocused(object sender, RoutedEventArgs e)
        {
            activeTextBox = sender as TextBox;
            activeTextBox.IsReadOnly = true;

            PasswordTextBox.IsReadOnly = false;
        }

        private void PasswordFocused(object sender, RoutedEventArgs e)
        {
            activeTextBox = sender as TextBox;
            activeTextBox.IsReadOnly = true;

            UserLoginTextBox.IsReadOnly = false;
        }

        private void ClickHandler(object sender, RoutedEventArgs e)
        {
            LoggedInEmployee = GetEmployee();
            RaiseEvent(new RoutedEventArgs(LoginEvent));
        }
    }
}