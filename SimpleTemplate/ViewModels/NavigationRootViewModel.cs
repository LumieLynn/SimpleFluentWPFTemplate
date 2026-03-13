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

        public NavigationRootViewModel(INavigationService navigationService, INavigationViewService navigationViewService)
        {
            _navigationService = navigationService;
            _navigationService.Navigated += OnNavigated;
            _navigationViewService = navigationViewService;

            LoadMenuConfigurations();
        }

        private void LoadMenuConfigurations()
        {
            string json = File.ReadAllText("Assets/menu.json");

            using var doc = JsonDocument.Parse(json);
            MenuConfigs = JsonSerializer.Deserialize<ObservableCollection<MenuConfigItem>>(doc.RootElement.GetProperty("Main").GetRawText())!;
            FooterConfigs = JsonSerializer.Deserialize<ObservableCollection<MenuConfigItem>>(doc.RootElement.GetProperty("Footer").GetRawText())!;
        }

        private void OnNavigated(object? sender, EventArgs e)
        {
            IsBackEnabled = _navigationService.CanGoBack;
        }

    }
}
