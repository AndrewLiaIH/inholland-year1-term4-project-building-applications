using System.Windows.Controls;

namespace UI
{
    /// <summary>
    /// Interaction logic for UserControlHeader.xaml
    /// </summary>
    public partial class UserControlHeader : UserControl
    {
        public List<Folder> Folders { get; set; }
        public List<Folder> TemporaryFolders { get; set; }

        public UserControlHeader()
        {
            InitializeComponent();

            FoldersListBox.ItemsSource = Folders;

            TemporaryFolders = new List<Folder>
            {
                new Folder { Name = "Login View" },
                new Folder { Name = "Table View" },
                new Folder { Name = "Order View" },
                new Folder { Name = "Kitchen View" }
            };
            TemporaryFoldersListBox.ItemsSource = Folders;
        }
    }
}
