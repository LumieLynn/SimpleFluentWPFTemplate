using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.ViewModels;
using SimpleTemplate.Views;
using System.Reflection;

namespace SimpleTemplate.Infrastructure
{
    public static class DI
    {
        public static (IServiceCollection services, List<Type> vmTypes) AddAppComponents(this IServiceCollection services)
        {
            var vmTypes = new List<Type>();

            var allTypes = Assembly.GetExecutingAssembly().GetTypes();

            foreach (var type in allTypes)
            {
                if (!type.IsClass || type.IsAbstract) continue;
                
                if (type.Name.EndsWith("ViewModel"))
                {
                    vmTypes.Add(type);
                    if (type == typeof(NavigationRootViewModel))
                        services.AddSingleton(type);
                    else
                        services.AddTransient(type);
                }
                else if (type.Name.EndsWith("View"))
                {
                    if (type == typeof(NavigationRootView))
                        services.AddSingleton(type);
                    else
                        services.AddTransient(type);
                }
            }
            
            return (services, vmTypes);
        }
    }
}
