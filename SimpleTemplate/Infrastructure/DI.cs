using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace SimpleTemplate.Infrastructure
{
    public static class DI
    {
        //public static IServiceCollection AddViews(this IServiceCollection services)
        //{
        //    // Auto register all PageViews
        //    var viewTypes = Assembly.GetExecutingAssembly()
        //        .GetTypes()
        //        .Where(t => t.Name.EndsWith("PageView"));
        //    foreach (var type in viewTypes)
        //    {
        //        services.AddTransient(type);
        //    }
        //    return services;
        //}

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            // Auto register all ViewModels
            var vmTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && t.Name.EndsWith("ViewModel"));
            foreach (var type in vmTypes)
            {
                services.AddSingleton(type);
            }
            return services;
        }
    }
}
