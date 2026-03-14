using SimpleTemplate.Models;

namespace SimpleTemplate.Contracts.Services
{
    public interface IMenuDefinition
    {
        (IEnumerable<MenuConfigItem> Main, IEnumerable<MenuConfigItem> Footer) Build();
    }
}