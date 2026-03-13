using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.Models;
using SimpleTemplate.Views;
using System.Collections.ObjectModel;

namespace SimpleTemplate.ViewModels
{
    [RegisterView(typeof(NavigationRootView), viewLifetime: ServiceLifetime.Singleton, viewModelLifetime:ServiceLifetime.Singleton)]
    public partial class NavigationRootViewModel : ObservableRecipient
    {
        private readonly INavigationService _navigationService;
        private readonly INavigationViewService _navigationViewService;
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

        public event Action? MenuLoaded;

        public NavigationRootViewModel(
            INavigationService navigationService,
            INavigationViewService navigationViewService,
            IMenuConfigurationService menuConfigurationService)
        {
            _navigationService = navigationService;
            _navigationService.Navigated += OnNavigated;
            _navigationViewService = navigationViewService;
            _menuConfigurationService = menuConfigurationService;

        }

        public async Task InitializeAsync()
        {
            await LoadMenuConfigurationsAsync();
        }

        private async Task LoadMenuConfigurationsAsync()
        {
            var (main, footer) = await _menuConfigurationService.GetMenuConfigAsync();

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                MenuConfigs = new ObservableCollection<MenuConfigItem>(main);
                FooterConfigs = new ObservableCollection<MenuConfigItem>(footer);

                MenuLoaded?.Invoke();
            });
        }

        private void OnNavigated(object? sender, EventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack;
        }

    }
}
