using System.Diagnostics;
using System.Reflection;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
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

        private static IServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<NavigationService>()
                .AddTransient<NavigationRootView>()
                .AddTransient<NavigationProxyPage>()
                .AddViewModels()
                .AddSingleton<MainWindow>()
                .BuildServiceProvider();
        }
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RegisterAutoDataTemplates();
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        private void RegisterAutoDataTemplates()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var viewTypes = assembly.GetTypes()
                .Where(t => t.Name.EndsWith("View") && !t.IsAbstract);

            foreach (var viewType in viewTypes)
            {
                var vmTypeName = $"{viewType.Name}Model";
                var vmType = assembly.GetType($"{viewType.Namespace.Replace("Views", "ViewModels")}.{vmTypeName}");

                if (vmType != null)
                {
                    var template = CreateDataTemplate(vmType, viewType);
                    Application.Current.Resources.Add(new DataTemplateKey(vmType), template);
                    Debug.WriteLine($"Registered DataTemplate: {vmType.Name} → {viewType.Name}");
                }
                else
                {
                    Debug.WriteLine($"Warning：The ViewModel of {viewType.Name} not found. Check :{vmTypeName}, {$"{viewType.Namespace}.{vmTypeName}"}");
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
