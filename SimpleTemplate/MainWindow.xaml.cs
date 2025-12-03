using System.Diagnostics;
using System.Windows;
using SimpleTemplate.Contracts.Services;
using SimpleTemplate.ViewModels;
using SimpleTemplate.Views;

namespace SimpleTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly INavigationService _navService;
        private readonly INavigationViewService _navViewService;
        private readonly NavigationRootView _rootView;
        private readonly NavigationRootViewModel _rootVM;

        public MainWindow(
            INavigationService navService,
            INavigationViewService navViewService,
            NavigationRootView rootView,
            NavigationRootViewModel rootVM)
        {
            _navService = navService;
            _navViewService = navViewService;
            _rootView = rootView;
            _rootVM = rootVM;

            InitializeComponent();
            Loaded += async (s, e) => await OnLoadedAsync(s, e);
        }

        private async Task OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                await InitializeAppAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Initialize Failed:{ex.Message}");
            }
        }

        private async Task InitializeAppAsync()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(4));
            try
            {
                var initializationTask = InitializationNavigationAsync(cts.Token);
                var delayTask = Task.Delay(2000, cts.Token);

                await Task.WhenAll(initializationTask, delayTask);

                Content = _rootView;
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Initialize canceled");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Initialize failed: {ex}");
                throw;
            }
        }

        private async Task InitializationNavigationAsync(CancellationToken token)
        {
            _navViewService.Initialize(_rootView.NavigationViewControl, _rootVM.MenuItems, _rootVM.FooterItems);
            var pageType = typeof(HomePageViewModel);
            var pageKey = pageType.FullName!;
            _navService.Initialize(_rootView.Frame_Main, pageKey);
            await Task.Delay(1, token);
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }
    }
}