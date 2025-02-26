using System.Collections.ObjectModel;
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

        [ObservableProperty]
        private ObservableCollection<NavigationItem> _children = [];
        public NavigationItem(string title, object icon, Type viewModelType)
        {
            Title = title;
            Icon = icon;
            TargetViewModelType = viewModelType;
        }
    }
}
