using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using iNKORE.UI.WPF.Modern.Common.IconKeys;
using iNKORE.UI.WPF.Modern.Controls;
using SimpleTemplate.Models;

namespace SimpleTemplate.ViewModels
{
    public partial class NavigationRootViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private bool isBackEnabled;

        [ObservableProperty]
        private ObservableCollection<NavigationViewItem> _menuItems = new();

        [ObservableProperty]
        private ObservableCollection<NavigationItem> _footerItems = new();

        [ObservableProperty]
        private string _appTitle = "SimpleTemplate";

        [ObservableProperty]
        private string _paneTitle = "Samples";

        [ObservableProperty]
        private object? selected;

        public NavigationRootViewModel()
        {
            InitializeNavigationItems();
            Selected = MenuItems.FirstOrDefault();
        }

        private void InitializeNavigationItems()
        {
            FooterItems.Add(new NavigationItem(
                "About",
                new FontIcon { Icon = SegoeFluentIcons.Info },
                typeof(AboutPageViewModel)));
            FooterItems.Add(new NavigationItem(
                "Settings",
                new FontIcon { Icon = SegoeFluentIcons.Settings },
                typeof(SettingsPageViewModel)));

            MenuItems.Add(new NavigationItem(
                "Home",
                new FontIcon { Icon = SegoeFluentIcons.Home },
                typeof(HomePageViewModel)));
            MenuItems.Add(new NavigationItem(
                "Apps",
                new FontIcon { Icon = SegoeFluentIcons.OEM },
                typeof(AppsPageViewModel)));

        }

    }
}
