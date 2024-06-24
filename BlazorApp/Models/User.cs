using System.ComponentModel;

namespace BlazorApp.Models
{
    public class User : INotifyPropertyChanged
    {
        public string Username { get; private set; } = "Đang chờ ...";
        public bool IsEditable { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Set(string username, bool isEditable)
        {
            Username = username;
            IsEditable = isEditable;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEditable)));
        }

        public void Reset()
        {
            Username = "Đang chờ ...";
            IsEditable = false;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Username)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEditable)));
        }
    }
}