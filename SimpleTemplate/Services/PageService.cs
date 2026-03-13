using SimpleTemplate.Contracts.Services;

namespace SimpleTemplate.Services
{
    public class PageService : IPageService
    {
        private readonly Dictionary<string, (Type VmType, Type ViewType)> _pages = new();

        public Type GetPageType(string key)
        {
            if (!_pages.TryGetValue(key, out var mapping))
                throw new InvalidOperationException($"Page with key '{key}' not found.");
            return mapping.VmType;
        }

        public Type GetViewType(string key)
        {
            if (!_pages.TryGetValue(key, out var mapping))
                throw new InvalidOperationException($"Page with key '{key}' not found.");
            return mapping.ViewType;
        }

        public void ConfigurePage(string key, Type vmType, Type viewType)
        {
            _pages.TryAdd(key, (vmType, viewType));
        }

    }
}
