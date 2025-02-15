using System.Windows;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Infrastructure;

namespace SimpleTemplate
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider Services { get; }
        public static new App Current => (App)Application.Current;
        public App()
        {
            Services = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddViews()
                .AddViewModels()
                .AddSingleton<MainWindow>()
                .AddSingleton<NavigationService>()
                .BuildServiceProvider();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
