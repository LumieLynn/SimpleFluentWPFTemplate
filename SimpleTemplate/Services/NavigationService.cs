using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Views.ProxyPage;
using Frame = iNKORE.UI.WPF.Modern.Controls.Frame;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace SimpleTemplate.Services
{
    public class NavigationService(IServiceProvider serviceProvider, IPageService pageService) : INavigationService
    {
        private readonly Dictionary<Type, WeakReference<Page>> _pageCache = new();
        private Frame? _frame;

        public event EventHandler? Navigated;

        public void Initialize(Frame frame, string? pageKey = null)
        {
            _frame = frame;
            frame.Navigated += OnNavigated;
            if (pageKey != null)
            {
                NavigateTo(pageKey);
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            Navigated?.Invoke(this, e);
        }

        public bool CanGoBack => _frame != null && _frame.CanGoBack;

        public bool GoBack()
        {
            bool navigated = false;
            if (CanGoBack && _frame != null)
            {
                _frame.GoBack();
                navigated = true;
            }

            return navigated;
        }

        public bool NavigateTo(string pageKey)
        {
            var pageType = pageService.GetPageType(pageKey);
            if (_frame != null)
            {
                var navigated = _frame.Navigate(GetOrCreatePage(pageType));
                return navigated;
            }

            return false;
        }

        private Page GetOrCreatePage(Type viewModelType)
        {
            if (_pageCache.TryGetValue(viewModelType, out var weakref) && weakref.TryGetTarget(out var cachedPage))
            {
                return cachedPage;
            }

            var newPage = serviceProvider.GetRequiredService<NavigationProxyPage>();
            newPage.ViewModel = serviceProvider.GetRequiredService(viewModelType);

            _pageCache[viewModelType] = new WeakReference<Page>(newPage);
            return newPage;
        }

        public object? GetCurrentViewModel()
        {
            if (_frame != null && _frame.Content is NavigationProxyPage proxyPage)
            {
                return proxyPage.ViewModel;
            }

            return null;
        }
    }
}
