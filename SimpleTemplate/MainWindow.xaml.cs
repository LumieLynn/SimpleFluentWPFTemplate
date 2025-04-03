using System.Diagnostics;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Views;

namespace SimpleTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _initializationCts;

        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
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
            _initializationCts = new CancellationTokenSource();
            try
            {
                await Task.Delay(2000, _initializationCts.Token)
                    .ConfigureAwait(true); // Ensure to return to the UI thread

                Debug.WriteLine("Initializing services...");

                // Check the container status before obtaining services
                if (App.Current.Services == null)
                {
                    throw new InvalidOperationException("ServiceProvider 未初始化");
                }

                //var navService = App.Current.Services.GetRequiredService<INavigationService>();
                //var navViewService = App.Current.Services.GetRequiredService<INavigationViewService>();
                //var rootV = App.Current.Services.GetRequiredService<NavigationRootView>();
                //var rootVM = App.Current.Services.GetRequiredService<NavigationRootViewModel>();

                //Debug.WriteLine($"Obtainings services success：navViewService={navViewService != null}, rootVM={rootVM != null}");

                //// Configure navigation relationships
                //navViewService.ConfigurePairs(rootVM.MenuItems);
                //navViewService.ConfigurePairs(rootVM.FooterItems);

                //// Service initialization
                //navViewService.Initialize(rootV.NavigationViewControl, rootVM.MenuItems, rootVM.FooterItems);
                //navService.Initialize(rootV.Frame_Main);
                //var source = rootV.NavigationViewControl.MenuItemsSource;


                // Ensure operation on the UI thread
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Content = App.Current.Services.GetRequiredService<NavigationRootView>();

                });
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Initialize cancled");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Initialize failed: {ex}");
                throw;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _initializationCts?.Cancel();
            base.OnClosed(e);
        }
    }
}