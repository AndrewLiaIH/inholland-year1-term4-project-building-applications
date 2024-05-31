using Microsoft.IdentityModel.Tokens;
using Model;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    // This class is created by Orest Pokotylenko
    /// <summary>
    /// Interaction logic for UserControlLogin.xaml
    /// </summary>
    public partial class UserControlLoginView : UserControl
    {
        public Employee LoggedInEmployee { get; private set; }
        private TextBox activeTextBox;

        private const string EmptyBoxExceptionMessage = "Please fill in all fields";
        private const string WrongLoginOrPasswordMessage = "The user id or password is incorrect";

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
            activeTextBox = LoginTextBox;
            activeTextBox.IsReadOnly = true;
        }

        /// <summary>
        /// Login methods
        /// </summary>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();

            if (LoggedInEmployee != null)
                RaiseEvent(new RoutedEventArgs(LoginEvent));
        }

        private void Login()
        {
            try
            {
                activeTextBox = null;
                ValidateLogin();
            }
            catch (Exception ex)
            {
                SetTextBoxesToError();
                LoginErrorMessage.Text = ex.Message;
            }
        }

        private void ValidateLogin()
        {
            if (!InputEmptyValidation())
            {
                LoggedInEmployee = GetEmployee();

                if (LoggedInEmployee == null)
                {
                    ClearTextBoxes();
                    throw new Exception(WrongLoginOrPasswordMessage);
                }
            }
            else
            {
                throw new Exception(EmptyBoxExceptionMessage);
            }
        }

        private bool InputEmptyValidation()
        {
            return LoginTextBox.Text.IsNullOrEmpty() || PasswordTextBox.Text.IsNullOrEmpty();
        }

        private Employee GetEmployee()
        {
            int login = int.Parse(LoginTextBox.Text);
            string password = PasswordTextBox.Tag.ToString();
            EmployeeService employeeService = new();

            return employeeService.GetEmployeeByLoginAndPassword(login, password);
        }

        /// <summary>
        /// Click events for all buttons of the numpad
        /// </summary>
        private void NumpadNumber_Click(object sender, RoutedEventArgs e)
        {
            string input = (sender as Button).Tag.ToString();

            if (activeTextBox != null && activeTextBox.IsReadOnly)
                if (activeTextBox == LoginTextBox)
                {
                    activeTextBox.Text += input;
                }
                else
                {
                    activeTextBox.Text += "•";
                    activeTextBox.Tag += input;
                }
        }

        private void NumPadBackSpace_Click(object sender, RoutedEventArgs e)
        {
            if (activeTextBox != null && activeTextBox.Text.Length > 0)
            {
                activeTextBox.Text = activeTextBox.Text.Remove(activeTextBox.Text.Length - 1);

                if (activeTextBox == PasswordTextBox)
                    activeTextBox.Tag = activeTextBox.Tag.ToString().Remove(activeTextBox.Tag.ToString().Length - 1);
            }

        }

        private void NumPadClear_Click(object sender, RoutedEventArgs e)
        {
            if (activeTextBox != null)
            {
                activeTextBox.Text = string.Empty;

                if (activeTextBox == PasswordTextBox)
                    activeTextBox.Tag = string.Empty;
            }
        }

        /// <summary>
        /// TextBox states including standard, focus and error and states
        /// </summary>
        private void LoginTextBox_Focused(object sender, RoutedEventArgs e)
        {
            activeTextBox = sender as TextBox;
            activeTextBox.IsReadOnly = true;

            foreach (TextBox textBox in LoginStackPanel.Children.OfType<TextBox>())
            {
                if (textBox != activeTextBox)
                {
                    textBox.IsReadOnly = false;
                }
            }

            SetTextBoxToNormal();
        }

        private void SetTextBoxesToError()
        {
            var errorFieldState = (Style)FindResource("LoginTextBoxErrorStyle");

            LoginTextBox.Style = errorFieldState;
            PasswordTextBox.Style = errorFieldState;
        }

        private void SetTextBoxToNormal()
        {
            var normalFieldState = (Style)FindResource("LoginTextBoxStyle");
            activeTextBox.Style = normalFieldState;
        }

        private void ClearTextBoxes()
        {
            foreach (TextBox textBox in LoginStackPanel.Children.OfType<TextBox>())
            {
                textBox.Text = string.Empty;
                textBox.Tag = string.Empty;
            }
        }
    }
}