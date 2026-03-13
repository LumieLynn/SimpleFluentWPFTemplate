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
                // 1. 检查文件是否存在，防止程序启动直接崩溃
                if (!File.Exists(MenuConfigFile))
                {
                    Debug.WriteLine($"[Configuration Error] 找不到菜单配置文件: {MenuConfigFile}");
                    return (Enumerable.Empty<MenuConfigItem>(), Enumerable.Empty<MenuConfigItem>());
                }

                // 2. 异步读取并解析 JSON，避免阻塞 UI 线程
                using var stream = File.OpenRead(MenuConfigFile);
                using var doc = await JsonDocument.ParseAsync(stream);

                var mainJson = doc.RootElement.GetProperty("Main").GetRawText();
                var footerJson = doc.RootElement.GetProperty("Footer").GetRawText();

                var main = JsonSerializer.Deserialize<List<MenuConfigItem>>(mainJson);
                var footer = JsonSerializer.Deserialize<List<MenuConfigItem>>(footerJson);

                return (main ?? Enumerable.Empty<MenuConfigItem>(), footer ?? Enumerable.Empty<MenuConfigItem>());
            }
            catch (Exception ex)
            {
                // 捕获 JSON 格式错误或其他不可预知的 IO 异常
                Debug.WriteLine($"[Configuration Error] 解析菜单配置失败: {ex.Message}");
                return (Enumerable.Empty<MenuConfigItem>(), Enumerable.Empty<MenuConfigItem>());
            }
        }
    }
}
