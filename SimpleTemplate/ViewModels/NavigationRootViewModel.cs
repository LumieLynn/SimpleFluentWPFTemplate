using CommunityToolkit.Mvvm.ComponentModel;
using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Models;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SimpleTemplate.ViewModels
{
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

            _ = LoadMenuConfigurationsAsync();
        }

        private async Task LoadMenuConfigurationsAsync()
        {
            var (main, footer) = await _menuConfigurationService.GetMenuConfigAsync();

            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                MenuConfigs.Clear();
                foreach (var item in main) MenuConfigs.Add(item);

                FooterConfigs.Clear();
                foreach (var item in footer) FooterConfigs.Add(item);

                MenuLoaded?.Invoke();
            });
        }

        private void OnNavigated(object? sender, EventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack;
        }

    }
}
