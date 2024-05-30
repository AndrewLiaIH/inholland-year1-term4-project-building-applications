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
            userControlHeader.SelectedFolderChanged += UserControlHeader_SelectedFolderChanged;
        }

        private void UserControlHeader_SelectedFolderChanged(object sender, RoutedEventArgs e)
        {
            Folder selectedFolder = userControlHeader.SelectedFolder;
            
            if (selectedFolder != null)
            {
                // Hide all views and set IsActive to false
                foreach (Folder folder in FoldersKitchen)
                {
                    folder.UserControl.Visibility = Visibility.Hidden;
                    folder.IsActive = false;
                }

                // Show the selected folder and set IsActive to true
                selectedFolder.UserControl.Visibility = Visibility.Visible;
                selectedFolder.IsActive = true;
            }
        }
    }
}
