using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Models;

namespace SimpleTemplate.Services
{
    public class MenuConfigurationService(IMenuDefinition menuDefinition) : IMenuConfigurationService
    {
        public (IEnumerable<MenuConfigItem> Main, IEnumerable<MenuConfigItem> Footer) GetMenuConfig()
        {
            return menuDefinition.Build();
        }
    }
}
