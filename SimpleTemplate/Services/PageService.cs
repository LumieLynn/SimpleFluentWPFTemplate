using SimpleTemplate.Contracts.Services;

namespace SimpleTemplate.Services
{
    public class PageService : IPageService
    {
        private readonly Dictionary<string, Type> _pages = new();

        public Type GetPageType(string key)
        {
            if (!_pages.TryGetValue(key, out Type? pageType))
            {
                throw new InvalidOperationException($"Page with key '{key}' not found.");
            }
            return pageType;
        }

        public void ConfigurePages(string PageKey, Type PageType)
        {
            _pages.Add(PageKey, PageType);
        }

    }
}
