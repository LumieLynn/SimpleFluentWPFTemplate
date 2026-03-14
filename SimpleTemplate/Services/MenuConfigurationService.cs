using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Models;

namespace SimpleTemplate.Services
{
    public class MenuConfigurationService(IMenuDefinition menuDefinition) : IMenuConfigurationService
    {
        public Task<(IEnumerable<MenuConfigItem> Main, IEnumerable<MenuConfigItem> Footer)> GetMenuConfigAsync()
        {
            var (main, footer) = menuDefinition.Build();
            return Task.FromResult((main, footer));
        }
    }
}
