using System.Windows;
using SimpleTemplate.Services;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.ViewModels;
using SimpleTemplate.Views;
using SimpleTemplate.Views.ProxyPage;

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
                .AddSingleton<NavigationService>()
                .AddSingleton<NavigationRootView>()
                .AddTransient<NavigationProxyPage>()
                .AddViewModels()
                .AddSingleton<MainWindow>()
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
