using System.Windows.Controls;

namespace UI
{
    public class Folder
    {
        public string Name { get; }
        public UserControl UserControl { get; }

        public Folder(string name, UserControl userControl)
        {
            Name = name;
            UserControl = userControl;
        }
    }
}
