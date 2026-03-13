using iNKORE.UI.WPF.Modern.Controls;
using SimpleTemplate.Contracts.Services;
using NavigationView = iNKORE.UI.WPF.Modern.Controls.NavigationView;

namespace SimpleTemplate.Services
{
    public class NavigationViewService(INavigationService navigationService, IPageService pageService, INavigationService _navigationService) : INavigationViewService
    {
        private NavigationView? _navigationView;

        //private readonly Dictionary<Type, NavigationViewItem> typeItemPairs = new();

        public void Initialize(NavigationView navigationView)
        {
            _navigationView = navigationView;
            _navigationView.BackRequested += OnBackRequested;
            _navigationView.ItemInvoked += OnItemInvoked;
            _navigationService.Navigated += OnNavigated;
        }

        private void OnBackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args) => navigationService.GoBack();

        private void OnItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                // 如果开启了自带的设置按钮，可以跳转到固定的 SettingsPageViewModel
                _navigationService.NavigateTo("SimpleTemplate.ViewModels.SettingsPageViewModel");
                return;
            }

            var item = args.InvokedItemContainer as NavigationViewItem;
            // 直接读取在 View 中绑定好的 Tag
            if (item?.Tag is string pageKey)
            {
                _navigationService.NavigateTo(pageKey);
            }
        }


        private void OnNavigated(object? sender, EventArgs e)
        {
            if (_navigationView == null) return;

            // 获取当前导航到的 ViewModel 的完整类名（即 PageKey）
            var currentVmType = _navigationService.GetCurrentViewModel()?.GetType().FullName;
            if (currentVmType == null) return;

            // 1. 尝试在主菜单树中查找匹配的 Item
            var selectedItem = GetSelectedItem(_navigationView.MenuItems, currentVmType);

            // 2. 如果主菜单没找到，尝试在底部菜单 (FooterMenuItems) 中查找
            if (selectedItem == null)
            {
                selectedItem = GetSelectedItem(_navigationView.FooterMenuItems, currentVmType);
            }

            // 3. 如果找到了对应的 Item，并且当前选中的不是它，则更新选中状态
            if (selectedItem != null && _navigationView.SelectedItem != selectedItem)
            {
                _navigationView.SelectedItem = selectedItem;
            }
        }

        private NavigationViewItem? GetSelectedItem(System.Collections.IList items, string targetPageKey)
        {
            foreach (var item in items)
            {
                if (item is NavigationViewItem navItem)
                {
                    // 检查当前节点是否匹配
                    if (navItem.Tag is string tag && tag == targetPageKey)
                    {
                        return navItem;
                    }

                    // 如果当前节点有子菜单，递归进入子菜单查找
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
    }
}
