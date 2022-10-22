using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CSharpExamine.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string? propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public bool Set<T>(ref T param, T value, string? property = null)
        {
            if (Equals(param, value))
                return false;
            else
            {
                param = value;
                OnPropertyChanged(property);
                return true;
            }
        }
    }
}
