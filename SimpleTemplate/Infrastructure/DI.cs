using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.ViewModels;

namespace SimpleTemplate.Infrastructure
{
    public static class DI
    {
        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            // Auto register all ViewModels
            var vmTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && t.Name.EndsWith("ViewModel"));
            foreach (var type in vmTypes)
            {
                if (type == typeof(NavigationRootViewModel)) services.AddSingleton(type);
                else services.AddTransient(type);
            }
            return services;
        }
    }
}
