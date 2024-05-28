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
            DependencyProperty.Register("Folders", typeof(List<Folder>), typeof(UserControlHeader), new PropertyMetadata(new List<Folder>(), OnFoldersChanged));

        public static DependencyProperty TemporaryFoldersProperty =
            DependencyProperty.Register("TemporaryFolders", typeof(List<Folder>), typeof(UserControlHeader), new PropertyMetadata(new List<Folder>(), OnFoldersChanged));

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

        public List<Folder> TemporaryFolders
        {
            get { return (List<Folder>)GetValue(TemporaryFoldersProperty); }
            set { SetValue(TemporaryFoldersProperty, value); }
        }

        public event RoutedEventHandler SelectedFolderChanged;

        public UserControlHeader()
        {
            InitializeComponent();
            DataContext = this;
        }

        private static void OnFoldersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UserControlHeader;
            control.OnFoldersChanged(e);
        }

        private void OnFoldersChanged(DependencyPropertyChangedEventArgs e)
        {
            // You might need to do something when the folders change
            // For example, you can manually refresh the ListBox's ItemsSource
            FoldersListBox.ItemsSource = null;
            FoldersListBox.ItemsSource = Folders;
            TemporaryFoldersListBox.ItemsSource = null;
            TemporaryFoldersListBox.ItemsSource = TemporaryFolders;
        }

        private static void OnSelectedFolderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as UserControlHeader;
            control.SelectedFolderChanged?.Invoke(control, new RoutedEventArgs());
        }

        private void TemporaryFoldersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedFolder = (Folder)TemporaryFoldersListBox.SelectedItem;
        }
    }
}
