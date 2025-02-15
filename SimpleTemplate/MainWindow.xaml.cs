using System.Windows;
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
            await InitializeAppAsync();
        }

        private async Task InitializeAppAsync()
        {
            _initializationCts = new CancellationTokenSource();
            await Task.Delay(2000, _initializationCts.Token);
            Content = new NavigationRootView();
        }

        protected override void OnClosed(EventArgs e)
        {
            _initializationCts?.Cancel();
            base.OnClosed(e);
        }
    }
}