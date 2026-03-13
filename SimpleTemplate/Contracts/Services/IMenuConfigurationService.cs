using SimpleTemplate.Models;

namespace SimpleTemplate.Contracts.Services
{
    public interface IMenuConfigurationService
    {
        Task<(IEnumerable<MenuConfigItem> Main, IEnumerable<MenuConfigItem> Footer)> GetMenuConfigAsync();
    }
}
