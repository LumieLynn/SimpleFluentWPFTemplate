using iNKORE.UI.WPF.Modern.Controls;

namespace SimpleTemplate.Services
{
    public class PageService
    {
        private readonly Dictionary<string, Type> _pages = new();

        public Type GetPageType(string key)
        {
            Type? pageType;
            if (!_pages.TryGetValue(key, out pageType))
            {
                throw new InvalidOperationException($"Page with key '{key}' not found.");
            }
            return pageType;
        }

        public void ConfigurePages(IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                if (item is NavigationViewItem menuItem)
                {
                    if (menuItem.Tag is Type pageType)
                    {
                        _pages.Add(pageType.FullName!, pageType);
                    }
                    if (menuItem.MenuItemsSource is IEnumerable<object> child)
                        ConfigurePages(child);
                }
            }
        }
    }
}
