using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using iNKORE.UI.WPF.Modern.Common.IconKeys;
using iNKORE.UI.WPF.Modern.Controls;
using SimpleTemplate.Contracts.Services;

namespace SimpleTemplate.ViewModels
{
    public partial class NavigationRootViewModel : ObservableRecipient
    {
        public readonly INavigationService _navigationService;
        public readonly INavigationViewService _navigationViewService;
        public readonly IPageService _pageService;

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = [
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new FontIcon { Icon = SegoeFluentIcons.Home },
                Tag = typeof(HomePageViewModel),
            },
            new NavigationViewItemSeparator(),
            new NavigationViewItemHeader()
            {
                Content = "Hierarchical Items"
            },
            new NavigationViewItem()
            {
                Content = "Example",
                Icon = new FontIcon { Icon = SegoeFluentIcons.Calendar },
                SelectsOnInvoked = false,
                IsExpanded = true,
                MenuItemsSource = new ObservableCollection<object>
                {
                    new NavigationViewItem()
                    {
                        Content = "Apps",
                        Icon = new FontIcon { Icon = SegoeFluentIcons.OEM },
                        Tag = typeof(AppsPageViewModel),
                    },
                }
            },
        ];

        [ObservableProperty]
        private ObservableCollection<object> _footerItems = [
            new NavigationViewItem()
            {
                Content = "About",
                Icon = new FontIcon { Icon = SegoeFluentIcons.Info },
                Tag = typeof(AboutPageViewModel)
            },
            new NavigationViewItem()
            {
                Content = "Settings",
                Icon = new FontIcon { Icon = SegoeFluentIcons.Settings },
                Tag = typeof(SettingsPageViewModel)
            }
        ];

        [ObservableProperty]
        private bool isBackEnabled;

        [ObservableProperty]
        private string _appTitle = "SimpleTemplate";

        [ObservableProperty]
        private string _paneTitle = "Samples";

        [ObservableProperty]
        private object? selected;

        [ObservableProperty]
        private object? header;

        public NavigationRootViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
        {
            _navigationService = navigationService;
            _navigationService.Navigated += OnNavigated;
            _navigationViewService = navigationViewService;
        }

        private void OnNavigated(object? sender, EventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack;
            Selected = _navigationViewService.GetCurrentSelectedItem();
            if (Selected is NavigationViewItem item)
            {
                Header = item.Content as string;
            }
        }

        public void SetProperties(NavigationView navigationView, Frame frame)
        {
            _navigationViewService.Initialize(navigationView, MenuItems, FooterItems);
            _navigationService.Initialize(frame);
        }
    }
}
