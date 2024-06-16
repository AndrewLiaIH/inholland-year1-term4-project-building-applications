using Microsoft.IdentityModel.Tokens;
using Model;
using Service;
using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// This class is created by Orest Pokotylenko. It is used to login employees to the system.
    /// </summary>
    public partial class UserControlLoginView : UserControl
    {
        public Employee LoggedInEmployee { get; private set; }
        private TextBox activeTextBox;
        private EmployeeService employeeService = new();

        private const string EmptyBoxExceptionMessage = "Please, fill in all fields.";
        private const string WrongLoginOrPasswordMessage = "Wrong password or ID. Please, try again.";

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

        internal void Refresh()
        {
            ClearTextBoxes();
            SetAllTextBoxesToNormal();
            SetCurrentTextBox(LoginTextBox);
            LoggedInEmployee = null;
            LoginErrorMessage.Text = string.Empty;
        }

        /// <summary>
        /// Login methods
        /// </summary>
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        internal void Login()
        {
            ValidateLogin();

            if (LoggedInEmployee != null)
                RaiseEvent(new RoutedEventArgs(LoginEvent));
        }

        private void ValidateLogin()
        {
            if (InputEmptyValidation())
            {
                LoginUnsuccessful(EmptyBoxExceptionMessage);
                return;
            }

            if (InputToLongValidation())
            {
                LoginUnsuccessful(WrongLoginOrPasswordMessage);
                return;
            }

            LoggedInEmployee = GetEmployee();

            if (LoggedInEmployee == null)
                LoginUnsuccessful(WrongLoginOrPasswordMessage);
        }

        private void LoginUnsuccessful(string errorMessage)
        {
            SetTextBoxesToError();
            LoginErrorMessage.Text = errorMessage;
            activeTextBox = null;
        }

        private Employee GetEmployee()
        {
            int login = int.Parse(LoginTextBox.Text);
            string password = PasswordTextBox.Tag.ToString();

            return employeeService.GetEmployeeByLoginAndPassword(login, password);
        }

        /// <summary>
        /// Click events for all buttons of the numpad
        /// </summary>
        private void NumpadNumber_Click(object sender, RoutedEventArgs e)
        {
            string input = (sender as Button).Tag.ToString();

            if (activeTextBox != null && activeTextBox.IsReadOnly)
                FillTextBox(input);
        }

        private void FillTextBox(string input)
        {
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
            SetCurrentTextBox(sender as TextBox);
        }

        private void SetCurrentTextBox(TextBox currentTextBox)
        {
            activeTextBox = currentTextBox;
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

        private void SetAllTextBoxesToNormal()
        {
            var normalFieldState = (Style)FindResource("LoginTextBoxStyle");
            foreach (TextBox textBox in LoginStackPanel.Children.OfType<TextBox>())
                textBox.Style = normalFieldState;
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

        /// <summary>
        /// Validation methods
        /// </summary>
        private bool InputEmptyValidation()
        {
            return LoginTextBox.Text.IsNullOrEmpty() || PasswordTextBox.Text.IsNullOrEmpty();
        }

        private bool InputToLongValidation()
        {
            return LoginTextBox.Text.Length > 6 || PasswordTextBox.Text.Length > 6;
        }
    }
}