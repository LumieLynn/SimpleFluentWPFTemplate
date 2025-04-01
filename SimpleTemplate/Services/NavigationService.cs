using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.ViewModels;
using SimpleTemplate.Views.ProxyPage;
using Frame = iNKORE.UI.WPF.Modern.Controls.Frame;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace SimpleTemplate.Services
{
    public class NavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly PageService _pageService;
        private readonly Dictionary<Type, WeakReference<Page>> _pageCache = new();
        private Frame? _frame;

        public event EventHandler Navigated;
        public event EventHandler NavigatedBack;

        public void Initialize(Frame frame)
        {
            _frame = frame;
            frame.Navigated += OnNavigated;
            var pageType = typeof(HomePageViewModel);
            var pageKey = pageType.FullName!;
            NavigateTo(pageKey);
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            Navigated?.Invoke(this, e);
        }

        public bool CanGoBack => _frame != null && _frame.CanGoBack;

        public bool GoBack()
        {
            bool navigated = false;
            if (CanGoBack)
            {
                _frame.GoBack();
                navigated = true;
            }

            return navigated;
        }

        public NavigationService(IServiceProvider serviceProvider, PageService pageService)
        {
            _serviceProvider = serviceProvider;
            _pageService = pageService;
        }

        public bool NavigateTo(string pageKey)
        {
            var pageType = _pageService.GetPageType(pageKey);
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

            var newPage = _serviceProvider.GetRequiredService<NavigationProxyPage>();
            newPage.ViewModel = _serviceProvider.GetRequiredService(viewModelType);

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
