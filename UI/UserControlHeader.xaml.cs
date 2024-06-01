using Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlHeader.xaml
    /// </summary>
    public partial class UserControlHeader : UserControl
    {
        public static DependencyProperty FoldersProperty =
            DependencyProperty.Register("Folders", typeof(List<Folder>), typeof(UserControlHeader), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedFolderProperty =
            DependencyProperty.Register("SelectedFolder", typeof(Folder), typeof(UserControlHeader), new PropertyMetadata(null, OnSelectedFolderChanged));

        public static DependencyProperty LoggedInEmployeeProperty =
            DependencyProperty.Register("LoggedInEmployee", typeof(Employee), typeof(UserControlHeader), new PropertyMetadata(null));

        public List<Folder> Folders
        {
            get { return (List<Folder>)GetValue(FoldersProperty); }
            set { SetValue(FoldersProperty, value); }
        }

        public Folder SelectedFolder
        {
            get { return (Folder)GetValue(SelectedFolderProperty); }
            set { SetValue(SelectedFolderProperty, value); }
        }

        public Employee LoggedInEmployee
        {
            get { return (Employee)GetValue(LoggedInEmployeeProperty); }
            set { SetValue(LoggedInEmployeeProperty, value); }
        }

        private DispatcherTimer timer;

        public event RoutedEventHandler SelectedFolderChanged;
        public event EventHandler Logout;

        public UserControlHeader()
        {
            InitializeComponent();
            InitializeTimer();
            DataContext = this;
        }

        private void InitializeTimer()
        {
            timer = new()
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateTime();
            CurrentDateTextBlock.Text = DateTime.Now.ToString("ddd, dd MMMM yyyy");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            CurrentTimeTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private static void OnSelectedFolderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserControlHeader control = (UserControlHeader)d;
            control.SelectedFolderChanged?.Invoke(control, new RoutedEventArgs());
        }

        private void FoldersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedFolder = (Folder)FoldersListBox.SelectedItem;
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            Logout?.Invoke(this, EventArgs.Empty);
        }
    }
}
