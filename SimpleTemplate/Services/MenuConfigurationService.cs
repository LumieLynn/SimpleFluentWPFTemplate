using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Models;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace SimpleTemplate.Services
{
    public class MenuConfigurationService : IMenuConfigurationService
    {
        private static readonly string BaseDir = AppDomain.CurrentDomain.BaseDirectory;

        private static readonly string MenuConfigFilePath = Path.Combine(BaseDir, "Assets", "menu.json");

        public async Task<(IEnumerable<MenuConfigItem> Main, IEnumerable<MenuConfigItem> Footer)> GetMenuConfigAsync()
        {
            try
            {
                if (!File.Exists(MenuConfigFilePath))
                {
                    Debug.WriteLine($"[Configuration Error] Can't find Menu Configuration File: {MenuConfigFilePath}");
                    return (Enumerable.Empty<MenuConfigItem>(), Enumerable.Empty<MenuConfigItem>());
                }

                using var stream = File.OpenRead(MenuConfigFilePath);
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
