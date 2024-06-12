using System.ComponentModel;

namespace UI
{
    public class Folder : INotifyPropertyChanged
    {
        public string Name { get; }
        public Action ShowScreen { get; }
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

        public Folder(string name, Action showScreen, bool isActive = false)
        {
            Name = name;
            ShowScreen = showScreen;
            IsActive = isActive;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
