using SimpleTemplate.Models;

namespace SimpleTemplate.Contracts.Services
{
    /// <summary>
    /// Provides the navigation menu configuration consumed by the shell view.
    /// </summary>
    public interface IMenuConfigurationService
    {
        (IEnumerable<MenuConfigItem> Main, IEnumerable<MenuConfigItem> Footer) GetMenuConfig();
    }
}
