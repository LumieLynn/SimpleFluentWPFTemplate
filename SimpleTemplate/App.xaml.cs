using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.Services;
using SimpleTemplate.Views;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;

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
            services
                // Services
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<INavigationViewService, NavigationViewService>()
                .AddSingleton<IPageService, PageService>()
                // Pages
                .AddSingleton<NavigationRootView>();

            var result = services.AddViewModels();
            _discoveredViewModels = result.vmTypes;
            result.services.AddSingleton<MainWindow>();

            return result.services.BuildServiceProvider(new ServiceProviderOptions
            {
                ValidateScopes = true,
                ValidateOnBuild = true
            });
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                await RegisterDataTemplatesAsync();
                var mainWindow = Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Startup failed: {ex}");
                Application.Current.Shutdown();
            }
        }

        private static async Task RegisterDataTemplatesAsync()
        {
            var templateTasks = _discoveredViewModels.Select(async vmType =>
            {
                return await Task.Run(() =>
                {
                    var viewTypeName = vmType.Name.Replace("ViewModel", "View");
                    var viewNamespace = vmType.Namespace?.Replace("ViewModels", "Views");

                    if (viewNamespace == null) return (vmType, null);

                    var viewType = vmType.Assembly.GetType($"{viewNamespace}.{viewTypeName}");
                    return (vmType, viewType);
                });
            });

            var results = await Task.WhenAll(templateTasks);

            foreach (var (vmType, viewType) in results)
            {
                if (vmType != null)
                {
                    var template = CreateDataTemplate(vmType, viewType);
                    Application.Current.Resources.Add(new DataTemplateKey(vmType), template);
                    Debug.WriteLine($"Registered DataTemplate: {vmType.Name} -> {viewType.Name}");
                }
                else
                {
                    Debug.WriteLine($"Warning：ViewModel for {viewType.Name} not found");
                }
            }
        }

        private static DataTemplate CreateDataTemplate(Type vmType, Type viewType)
        {
            string xaml = $@"
            <DataTemplate DataType=""{{x:Type vm:{vmType.Name}}}""
                          xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                          xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
                          xmlns:vm=""clr-namespace:{vmType.Namespace};assembly={vmType.Assembly.GetName().Name}""
                          xmlns:v=""clr-namespace:{viewType.Namespace};assembly={viewType.Assembly.GetName().Name}"">
                <v:{viewType.Name} />
            </DataTemplate>";

            var template = (DataTemplate)XamlReader.Parse(xaml);
            return template;
        }
    }

}
