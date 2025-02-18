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
            await Task.Delay(2000, _initializationCts.Token)
                .ConfigureAwait(true);
            Content = App.Current.Services.GetRequiredService<NavigationRootView>();
        }

        protected override void OnClosed(EventArgs e)
        {
            _initializationCts?.Cancel();
            base.OnClosed(e);
        }
    }
}