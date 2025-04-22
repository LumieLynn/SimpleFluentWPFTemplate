using System.Diagnostics;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.Services;
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

        private static ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                // Services
                .AddSingleton<INavigationService, NavigationService>()
                .AddSingleton<INavigationViewService, NavigationViewService>()
                .AddSingleton<IPageService, PageService>()
                // Pages
                .AddSingleton<NavigationRootView>()
                .AddTransient<NavigationProxyPage>()
                .AddViewModels()
                // Windows
                .AddSingleton<MainWindow>()
                .BuildServiceProvider(new ServiceProviderOptions
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
                await RegisterAutoDataTemplatesAsync();
                var mainWindow = Services.GetRequiredService<MainWindow>();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Startup failed: {ex}");
                Application.Current.Shutdown();
            }
        }

        private async Task RegisterAutoDataTemplatesAsync()
        {
            var (assembly, viewTypes) = await Task.Run(() =>
            {
                var asm = Assembly.GetExecutingAssembly();
                var types = asm.GetTypes()
                    .Where(t => t.Name.EndsWith("View") && !t.IsAbstract)
                    .ToList();
                return (asm, types);
            });

            var templateTasks = viewTypes.Select(async viewType =>
            {
                var vmType = await Task.Run(() =>
                {
                    if (viewType.Namespace is null)
                        throw new InvalidOperationException($"View type {viewType.Name} has no namespace");

                    var vmTypeName = $"{viewType.Name}Model";
                    return assembly.GetType($"{viewType.Namespace.Replace("Views", "ViewModels")}.{vmTypeName}");
                });

                return (vmType, viewType);
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

        private DataTemplate CreateDataTemplate(Type vmType, Type viewType)
        {
            return new DataTemplate
            {
                DataType = vmType,
                VisualTree = new FrameworkElementFactory(viewType)
            };
        }
    }

}
