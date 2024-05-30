using System.ComponentModel;
using System.Windows.Controls;

namespace UI
{
    public class Folder : INotifyPropertyChanged
    {
        public string Name { get; }
        public UserControl UserControl { get; }
        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (isActive != value)
                {
                    isActive = value;
                    OnPropertyChanged(nameof(IsActive));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Folder(string name, UserControl userControl, bool isActive = false)
        {
            Name = name;
            UserControl = userControl;
            IsActive = isActive;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
