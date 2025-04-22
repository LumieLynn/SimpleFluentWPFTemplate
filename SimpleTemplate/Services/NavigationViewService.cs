using System.Diagnostics;
using iNKORE.UI.WPF.Modern.Controls;
using SimpleTemplate.Contracts.Services;
using NavigationView = iNKORE.UI.WPF.Modern.Controls.NavigationView;

namespace SimpleTemplate.Services
{
    public class NavigationViewService(INavigationService navigationService, IPageService pageService) : INavigationViewService
    {
        private NavigationView? _navigationView;

        private readonly Dictionary<Type, NavigationViewItem> typeItemPairs = new();

        public void Initialize(NavigationView navigationView, IEnumerable<object>? menuItems = null, IEnumerable<object>? footerItems = null)
        {
            _navigationView = navigationView;
            if (menuItems != null)
                ConfigurePairs(menuItems);
            if (footerItems != null)
                ConfigurePairs(footerItems);
            _navigationView.BackRequested += OnBackRequested;
            _navigationView.ItemInvoked += OnItemInvoked;
        }

        private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => navigationService.GoBack();

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItemContainer is NavigationViewItem selectedItem && selectedItem.Tag is Type pageType)
            {
                var pageKey = pageType.FullName!;
                navigationService.NavigateTo(pageKey);
            }
        }

        public void ConfigurePairs(IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                if (item is NavigationViewItem menuItem)
                {
                    if (menuItem.Tag is Type pageType)
                    {
                        typeItemPairs.Add(pageType, menuItem);
                        var pageKey = pageType.FullName!;
                        pageService.ConfigurePages(pageKey, pageType);
                    }
                    if (menuItem.MenuItemsSource is IEnumerable<object> child)
                        ConfigurePairs(child);
                }
            }
        }

        public NavigationViewItem? GetCurrentSelectedItem()
        {
            var currentViewModel = navigationService.GetCurrentViewModel();
            if (currentViewModel == null)
                return null;
            var currentPageType = currentViewModel.GetType();
            var selectedItem = GetSelectedItem(currentPageType);
            if (_navigationView != null)
                Debug.WriteLine($"CurrentSelectedItem: {_navigationView.SelectedItem}");
            return selectedItem;
        }

        private NavigationViewItem? GetSelectedItem(Type pageType)
        {
            if (typeItemPairs.TryGetValue(pageType, out var selectedItem))
            {
                return selectedItem;
            }
            return null;
        }
    }
}
