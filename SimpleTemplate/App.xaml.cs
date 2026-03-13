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
        private static List<Type> _discoveredViewModels = new();

        public App()
        {
            Services = ConfigureServices();
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            var pageService = new PageService();

            services
                // Services
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<INavigationViewService, NavigationViewService>()
                .AddSingleton<IPageService>(pageService)
                .AddSingleton<IMenuConfigurationService, MenuConfigurationService>();

            var vmResult = services.AddViewModels();
            var viewResult = services.AddViews();
            _discoveredViewModels = vmResult.vmTypes;

            foreach (var vmType in vmResult.vmTypes)
            {
                pageService.ConfigurePages(vmType.FullName!, vmType);
            }

            services.AddSingleton<MainWindow>();

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
                RegisterDataTemplates();
                var mainWindow = Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Startup failed: {ex}");
                Application.Current.Shutdown();
            }
        }

        private static void RegisterDataTemplates()
        {
            foreach (var vmType in _discoveredViewModels)
            {
                var viewTypeName = vmType.Name.Replace("ViewModel", "View");
                var viewNamespace = vmType.Namespace?.Replace("ViewModels", "Views");

                if (viewNamespace == null) continue;

                var viewType = vmType.Assembly.GetType($"{viewNamespace}.{viewTypeName}");

                if (viewType != null)
                {
                    var template = CreateDataTemplate(vmType, viewType);
                    Application.Current.Resources.Add(new DataTemplateKey(vmType), template);
                    Debug.WriteLine($"Registered DataTemplate: {vmType.Name} -> {viewType.Name}");
                }
                else
                {
                    Debug.WriteLine($"Warning: View for {vmType.Name} not found");
                }
            }
        }

        private static DataTemplate CreateDataTemplate(Type vmType, Type viewType)
        {
            return new DataTemplate(vmType)
            {
                VisualTree = new FrameworkElementFactory(viewType)
            };
        }
    }

}
