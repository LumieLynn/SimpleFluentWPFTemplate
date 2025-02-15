using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.ViewModels;
using SimpleTemplate.Views;

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
            var services = new ServiceCollection();

            services.AddSingleton<MainWindow>();

            services.AddSingleton<NavigationRootView>();
            services.AddSingleton<NavigationRootViewModel>();

            services.AddSingleton<HomePageView>();
            services.AddSingleton<HomePageViewModel>();

            services.AddSingleton<AppsPageView>();
            services.AddSingleton<AppsPageViewModel>();

            return services.BuildServiceProvider();
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            MainWindow = Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }
    }

}
