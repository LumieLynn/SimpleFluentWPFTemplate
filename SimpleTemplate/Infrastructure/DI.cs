using Microsoft.Extensions.DependencyInjection;
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

                var registerAttr = type.GetCustomAttribute<RegisterViewAttribute>();
                if (registerAttr != null)
                {
                    vmTypes.Add(type);
                    services.AddSingleton(type);

                    var viewDescriptor = new ServiceDescriptor(
                        registerAttr.ViewType,
                        registerAttr.ViewType,
                        registerAttr.ViewLifetime);

                    services.Add(viewDescriptor);
                }
            }

            return (services, vmTypes);
        }
    }
}
