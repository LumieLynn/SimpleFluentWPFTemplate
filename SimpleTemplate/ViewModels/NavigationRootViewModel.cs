using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using iNKORE.UI.WPF.Modern.Common.IconKeys;
using iNKORE.UI.WPF.Modern.Controls;
using SimpleTemplate.Services;

namespace SimpleTemplate.ViewModels
{
    public partial class NavigationRootViewModel : ObservableRecipient
    {
        private readonly NavigationService _navigationService;

        [ObservableProperty]
        private bool isBackEnabled;

        [ObservableProperty]
        private ObservableCollection<NavigationViewItem> _menuItems = [
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new FontIcon { Icon = SegoeFluentIcons.Home },
                Tag = "HomePageViewModel",
            },
            new NavigationViewItem()
            {
                Content = "Apps",
                Icon = new FontIcon { Icon = SegoeFluentIcons.OEM },
                Tag = "AppsPageViewModel",
            },
        ];

        [ObservableProperty]
        private ObservableCollection<NavigationViewItem> _footerItems = [
            new NavigationViewItem()
            {
                Content = "About",
                Icon = new FontIcon { Icon = SegoeFluentIcons.Info },
                Tag = "AboutPageViewModel"
            },
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new FontIcon { Icon = SegoeFluentIcons.Settings },
                Tag = "SettingsPageViewModel"
            }
        ];

        [ObservableProperty]
        private string _appTitle = "SimpleTemplate";

        [ObservableProperty]
        private string _paneTitle = "Samples";

        [ObservableProperty]
        private object? selected;

        public NavigationRootViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.ConfigureNavigation(MenuItems);
            _navigationService.ConfigureNavigation(FooterItems);
            Selected = MenuItems.FirstOrDefault();
        }

    }
}
