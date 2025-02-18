using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleTemplate.Models
{
    public partial class NavigationItem : ObservableObject
    {
        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private object _icon;

        [ObservableProperty]
        private Type _targetViewModelType;

        public NavigationItem(string title, object icon, Type viewModelType)
        {
            Title = title;
            Icon = icon;
            TargetViewModelType = viewModelType;
        }
    }
}
