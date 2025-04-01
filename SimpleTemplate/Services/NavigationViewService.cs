using iNKORE.UI.WPF.Modern.Controls;
using SimpleTemplate.Contracts.Services;
using NavigationView = iNKORE.UI.WPF.Modern.Controls.NavigationView;

namespace SimpleTemplate.Services
{
    public class NavigationViewService : INavigationViewService
    {
        private readonly INavigationService _navigationService;
        private NavigationView? _navigationView;

        public IEnumerable<object>? MenuItems;
        public IEnumerable<object>? FooterItems;

        public NavigationViewService(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public void Initialize(NavigationView navigationView, IEnumerable<object>? menuItems, IEnumerable<object>? footerItems)
        {
            _navigationView = navigationView;
            MenuItems = menuItems;
            FooterItems = footerItems;
            _navigationView.BackRequested += OnBackRequested;
            _navigationView.ItemInvoked += OnItemInvoked;
        }

        private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => _navigationService.GoBack();

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var selectedItem = args.InvokedItemContainer as NavigationViewItem;
            if (selectedItem != null && selectedItem.Tag is Type pageType)
            {
                var pageKey = pageType.FullName!;
                _navigationService.NavigateTo(pageKey);
            }
        }

        public NavigationViewItem? GetCurrentSelectedItem()
        {
            var currentViewModel = _navigationService.GetCurrentViewModel();
            if (currentViewModel == null)
                return null;
            var currentPageType = currentViewModel.GetType();
            return GetSelectedItem(MenuItems, currentPageType) ?? GetSelectedItem(FooterItems, currentPageType);
        }

        private NavigationViewItem? GetSelectedItem(IEnumerable<object> menuItems, Type pageType)
        {
            foreach (var item in menuItems.OfType<NavigationViewItem>())
            {
                if (item is NavigationViewItem menuItem)
                {
                    if (menuItem.Tag is Type tagType && tagType == pageType)
                        return menuItem;
                    if (menuItem.MenuItemsSource is IEnumerable<object> child)
                    {
                        var selectedItem = GetSelectedItem(child, pageType);
                        if (selectedItem != null)
                            return selectedItem;
                    }
                }
            }

            return null;
        }
    }
}
