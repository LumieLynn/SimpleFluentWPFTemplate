using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Services;
using System.Reflection;

namespace SimpleTemplate.Infrastructure
{
    public static class DI
    {
        public static IServiceCollection AddAppComponents(this IServiceCollection services)
        {
            var allTypes = Assembly.GetExecutingAssembly().GetTypes();

            var pageMappings = new List<(Type VmType, Type ViewType)>();

            foreach (var type in allTypes)
            {
                if (!type.IsClass || type.IsAbstract) continue;

                var registerAttr = type.GetCustomAttribute<RegisterViewAttribute>();
                if (registerAttr != null)
                {
                    services.Add(new ServiceDescriptor(type, type, registerAttr.ViewModelLifetime));

                    services.Add(new ServiceDescriptor(
                        registerAttr.ViewType,
                        registerAttr.ViewType,
                        registerAttr.ViewLifetime));

                    pageMappings.Add((type, registerAttr.ViewType));
                }
            }

            services.AddSingleton<IPageService>(sp =>
            {
                var pageService = new PageService();
                foreach (var mapping in pageMappings)
                {
                    pageService.ConfigurePage(mapping.VmType, mapping.ViewType);
                }
                return pageService;
            });

            return services;
        }
    }
}
