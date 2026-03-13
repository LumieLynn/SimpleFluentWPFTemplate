using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleTemplate.Services
{
    public class MenuConfigurationService : IMenuConfigurationService
    {
        private const string MenuConfigFile = "Assets/menu.json";

        public async Task<(IEnumerable<MenuConfigItem> Main, IEnumerable<MenuConfigItem> Footer)> GetMenuConfigAsync()
        {
            try
            {
                if (!File.Exists(MenuConfigFile))
                {
                    Debug.WriteLine($"[Configuration Error] Can't find Menu Configuration File: {MenuConfigFile}");
                    return (Enumerable.Empty<MenuConfigItem>(), Enumerable.Empty<MenuConfigItem>());
                }

                using var stream = File.OpenRead(MenuConfigFile);
                using var doc = await JsonDocument.ParseAsync(stream);

                var main = doc.RootElement.GetProperty("Main").Deserialize<List<MenuConfigItem>>();
                var footer = doc.RootElement.GetProperty("Footer").Deserialize<List<MenuConfigItem>>();

                return (main ?? Enumerable.Empty<MenuConfigItem>(), footer ?? Enumerable.Empty<MenuConfigItem>());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[Configuration Error] Failed to parse menu configurations: {ex.Message}");
                return (Enumerable.Empty<MenuConfigItem>(), Enumerable.Empty<MenuConfigItem>());
            }
        }
    }
}
