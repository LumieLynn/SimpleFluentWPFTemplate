using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.ViewModels;
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
                services.AddSingleton(type);
            }
            return (services, vmTypes);
        }
    }
}
