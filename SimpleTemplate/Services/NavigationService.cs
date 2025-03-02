using System.Windows.Navigation;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Views.ProxyPage;
using Frame = iNKORE.UI.WPF.Modern.Controls.Frame;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace SimpleTemplate.Services
{
    public class NavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<object, (Type ViewModelType, string Title)> _navigationMap = new();
        private readonly Dictionary<Type, object> _selectionMap = new();
        private readonly Dictionary<Type, WeakReference<Page>> _pageCache = new();
        private Frame _frame;
        private NavigationView _navView;

        public void SetProperties(Frame frame, NavigationView navigationView)
        {
            _frame = frame;
            _frame.Navigated += (s, e) =>
            {
                UpdateBackButton();
            };
            _navView = navigationView;
            _navView.BackRequested += NavView_BackRequested;
            _navView.ItemInvoked += _navView_ItemInvoked;
        }

        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        private void _navView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            TryNavigate(sender.SelectedItem);
        }

        private bool TryGoBack()
        {
            if (_navView.IsPaneOpen && (_navView.DisplayMode == NavigationViewDisplayMode.Compact || _navView.DisplayMode == NavigationViewDisplayMode.Minimal))
            {
                return false;
            }

            bool navigated = false;
            if (_frame.CanGoBack)
            {
                _frame.Navigated += OnNavigatedBack;
                _frame.GoBack();
                navigated = true;
            }

            return navigated;
        }

        private void OnNavigatedBack(object sender, NavigationEventArgs e)
        {
            _frame.Navigated -= OnNavigatedBack;

            NavigationProxyPage currentPage = (NavigationProxyPage)_frame.Content;
            var viewModelType = currentPage.ViewModel.GetType();

            if (currentPage != null && viewModelType != null)
            {
                _navView.SelectedItem = _selectionMap[viewModelType];
                _navView.Header = _navigationMap[_navView.SelectedItem].Title;
            }
        }

        private void UpdateBackButton()
        {
            _navView.IsBackEnabled = _frame.CanGoBack ? true : false;
        }

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ConfigureNavigation(IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                if (item is NavigationViewItem menuItem)
                {
                    RegisterMenuItem(menuItem);
                    if (menuItem.MenuItemsSource != null && menuItem.MenuItemsSource is IEnumerable<object> child)
                        ConfigureNavigation(child);
                }
            }
        }

        private void RegisterMenuItem(NavigationViewItem item)
        {
            if (item.Tag != null && item.Tag is Type type)
            {
                _navigationMap[item] = (type, item.Content.ToString());
                _selectionMap[type] = item;
            }
        }

        public bool TryNavigate(object menuItem)
        {
            if (_navigationMap.TryGetValue(menuItem, out var config))
            {
                var page = GetOrCreatePage(config.ViewModelType);
                _frame.Navigate(page);
                _navView.Header = config.Title;
                return true;
            }
            return false;
        }

        public Page GetOrCreatePage(Type viewModelType)
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
    }
}
