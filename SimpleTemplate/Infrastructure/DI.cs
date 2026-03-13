using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.ViewModels;
using SimpleTemplate.Views;
using System.Reflection;

namespace SimpleTemplate.Infrastructure
{
    public static class DI
    {
        public static (IServiceCollection services, List<Type> vmTypes) AddViewModels(this IServiceCollection services)
        {
            // Auto register all ViewModels
            var vmTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && t.Name.EndsWith("ViewModel"))
                .ToList();
            foreach (var type in vmTypes)
            {
                if (type == typeof(NavigationRootViewModel)) services.AddSingleton(type);
                else services.AddTransient(type);
            }
            return (services, vmTypes);
        }

        public static (IServiceCollection services, List<Type> vmTypes) AddViews(this IServiceCollection services)
        {
            // Auto register all Views
            var viewTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && t.Name.EndsWith("View"))
                .ToList();
            foreach (var type in viewTypes)
            {
                if (type == typeof(NavigationRootView)) services.AddSingleton(type);
                else services.AddTransient(type);
            }
            return (services, viewTypes);
        }
    }
}
