using System.Windows;
using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlKitchenView.xaml
    /// </summary>
    public partial class UserControlKitchenView : UserControl
    {
        public List<Folder> FoldersKitchen;

        public UserControlKitchenView()
        {
            InitializeComponent();

            FoldersKitchen = new()
            {
                new Folder("Running", userControlKitchenViewRunning),
                new Folder("Finished", userControlKitchenViewFinished)
            };

            userControlHeader.Folders = FoldersKitchen;
            userControlHeader.SelectedFolder = FoldersKitchen.First();
        }

        private void UserControlHeader_SelectedFolderChanged(object sender, RoutedEventArgs e)
        {
            Folder selectedFolder = userControlHeader.SelectedFolder;
            
            if (selectedFolder != null)
            {
                // Hide all views
                foreach (Folder folder in FoldersKitchen)
                    folder.UserControl.Visibility = Visibility.Hidden;

                // Show the selected folder
                selectedFolder.UserControl.Visibility = Visibility.Visible;
            }
        }
    }
}
