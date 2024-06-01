using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

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

        private DispatcherTimer timer;

        public event RoutedEventHandler SelectedFolderChanged;

        public UserControlHeader()
        {
            InitializeComponent();
            InitializeTimer();
            DataContext = this;
        }

        private void InitializeTimer()
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            timer.Tick += Timer_Tick;
            timer.Start();

            UpdateDateTime();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }

        private void UpdateDateTime()
        {
            CurrentDateTextBlock.Text = DateTime.Now.ToString("ddd, dd MMMM yyyy");
            CurrentTimeTextBlock.Text = DateTime.Now.ToString("hh:mm:ss");
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
    }
}
