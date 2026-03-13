using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.Services;
using System.Diagnostics;
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
                .AddSingleton<IPageService, PageService>()
                .AddSingleton<IMenuConfigurationService, MenuConfigurationService>();

            var vmResult = services.AddAppComponents();
            services.AddSingleton<MainWindow>();

            var provider = services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateScopes = true,
                ValidateOnBuild = true
            });
            var pageService = provider.GetRequiredService<IPageService>();
            foreach (var vmType in vmResult.vmTypes)
            {
                var vmFullName = vmType.FullName ?? string.Empty;
                var viewTypeName = vmFullName.Replace(".ViewModels.", ".Views.").Replace("ViewModel", "View");
                var viewType = vmType.Assembly.GetType(viewTypeName);

                if (viewType != null)
                {
                    pageService.ConfigurePage(vmFullName, vmType, viewType);
                }
                else
                {
                    Debug.WriteLine($"[Warning] Could not find the corresponding View for {vmFullName}. Please check the naming conventions.");
                }
            }

            return provider;
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
