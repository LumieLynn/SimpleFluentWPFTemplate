using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.Models;
using SimpleTemplate.Views;
using System.Collections.ObjectModel;

namespace SimpleTemplate.ViewModels
{
    [RegisterView(typeof(NavigationRootView), viewLifetime: ServiceLifetime.Singleton, viewModelLifetime: ServiceLifetime.Singleton)]
    public partial class NavigationRootViewModel : ObservableRecipient
    {
        private readonly INavigationService _navigationService;
        private readonly IMenuConfigurationService _menuConfigurationService;

        [ObservableProperty]
        private ObservableCollection<MenuConfigItem> _menuConfigs = new();

        [ObservableProperty]
        private ObservableCollection<MenuConfigItem> _footerConfigs = new();

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

        public NavigationRootViewModel(
            INavigationService navigationService,
            IMenuConfigurationService menuConfigurationService)
        {
            _navigationService = navigationService;
            _navigationService.Navigated += OnNavigated;
            _menuConfigurationService = menuConfigurationService;
        }

        /// <summary>
        /// Loads the menu configuration. Called once by the shell view when it loads.
        /// </summary>
        public void Initialize()
        {
            var (main, footer) = _menuConfigurationService.GetMenuConfig();
            MenuConfigs = new ObservableCollection<MenuConfigItem>(main);
            FooterConfigs = new ObservableCollection<MenuConfigItem>(footer);
        }

        private void OnNavigated(object? sender, EventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack;
        }
    }
}
