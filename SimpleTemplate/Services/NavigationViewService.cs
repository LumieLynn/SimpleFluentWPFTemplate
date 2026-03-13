using iNKORE.UI.WPF.Modern.Controls;
using SimpleTemplate.Contracts.Services;
using SimpleTemplate.ViewModels;
using NavigationView = iNKORE.UI.WPF.Modern.Controls.NavigationView;

namespace SimpleTemplate.Services
{
    public class NavigationViewService(INavigationService navigationService) : INavigationViewService, IDisposable
    {
        private NavigationView? _navigationView;

        public void Initialize(NavigationView navigationView)
        {
            _navigationView = navigationView;
            _navigationView.BackRequested += OnBackRequested;
            _navigationView.ItemInvoked += OnItemInvoked;
            navigationService.Navigated += OnNavigated;
        }

        private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => navigationService.GoBack();

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                navigationService.NavigateTo(typeof(SettingsPageViewModel).FullName);
                return;
            }

            var item = args.InvokedItemContainer as NavigationViewItem;
            if (item?.Tag is string pageKey)
            {
                navigationService.NavigateTo(pageKey);
            }
        }


        private void OnNavigated(object? sender, EventArgs e)
        {
            if (_navigationView == null) return;

            var currentVmType = navigationService.GetCurrentViewModel()?.GetType().FullName;
            if (currentVmType == null) return;

            _navigationView.Dispatcher.InvokeAsync(() =>
            {
                var selectedItem = GetSelectedItem(_navigationView.MenuItems, currentVmType);

                if (selectedItem == null)
                {
                    selectedItem = GetSelectedItem(_navigationView.FooterMenuItems, currentVmType);
                }

                if (selectedItem != null && _navigationView.SelectedItem != selectedItem)
                {
                    _navigationView.SelectedItem = selectedItem;
                }
            }, System.Windows.Threading.DispatcherPriority.Loaded);
        }

        private NavigationViewItem? GetSelectedItem(System.Collections.IList items, string targetPageKey)
        {
            foreach (var item in items)
            {
                if (item is NavigationViewItem navItem)
                {
                    if (navItem.Tag is string tag && tag == targetPageKey)
                    {
                        return navItem;
                    }

                    if (navItem.MenuItemsSource is System.Collections.IList children)
                    {
                        var childMatch = GetSelectedItem(children, targetPageKey);
                        if (childMatch != null)
                        {
                            return childMatch;
                        }
                    }
                }
            }
            return null;
        }

        public void Dispose()
        {
            if (_navigationView != null)
            {
                _navigationView.BackRequested -= OnBackRequested;
                _navigationView.ItemInvoked -= OnItemInvoked;

                _navigationView = null;
            }

            if (navigationService != null)
            {
                navigationService.Navigated -= OnNavigated;
            }

            GC.SuppressFinalize(this);
        }
    }
}
