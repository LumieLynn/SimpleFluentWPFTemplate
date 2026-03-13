using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.Services;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

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

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            //var pageService = new PageService();

            services
                // Services
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<INavigationViewService, NavigationViewService>()
                .AddSingleton<IMenuConfigurationService, MenuConfigurationService>()
                .AddSingleton<IMenuDefinition, Configurations.NavigationMenu>()
                .AddSingleton<IViewFactory, ViewFactory>()

                .AddAppComponents()
                .AddSingleton<MainWindow>();

            return services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateScopes = true,
                ValidateOnBuild = true
            });
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                var mainWindow = Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Startup failed: {ex}");
                Application.Current.Shutdown();
            }
        }

    }

}
