using System.Windows;
using System.Windows.Controls;

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

        public event RoutedEventHandler SelectedFolderChanged;

        public UserControlHeader()
        {
            InitializeComponent();
            DataContext = this;
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
