using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace View
{
    /// <summary>
    /// Interaction logic for UserControlHeader.xaml
    /// </summary>
    public partial class UserControlHeader : UserControl
    {
        public ObservableCollection<Folder> Folders { get; set; }

        public UserControlHeader()
        {
            InitializeComponent();

            Folders = new ObservableCollection<Folder>
            {
                new Folder { Name = "Login View" },
                new Folder { Name = "Table View" },
                new Folder { Name = "Order View" },
                new Folder { Name = "Kitchen View" }
            };
            FoldersListBox.ItemsSource = Folders;
        }
    }
}
