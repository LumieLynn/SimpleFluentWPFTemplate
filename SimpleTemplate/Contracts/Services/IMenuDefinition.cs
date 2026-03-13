using SimpleTemplate.Models;
using System.Collections.Generic;

namespace SimpleTemplate.Contracts.Services
{
    public interface IMenuDefinition
    {
        (IEnumerable<MenuConfigItem> Main, IEnumerable<MenuConfigItem> Footer) Build();
    }
}