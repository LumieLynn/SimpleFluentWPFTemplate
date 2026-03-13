using iNKORE.UI.WPF.Modern.Common.IconKeys;
using iNKORE.UI.WPF.Modern.Controls;
using SimpleTemplate.Models;
using SimpleTemplate.ViewModels;
using System.Collections;
using System.Windows;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace SimpleTemplate.Views
{
    /// <summary>
    /// NavigationRootView.xaml 的交互逻辑
    /// </summary>
    public partial class NavigationRootView : Page
    {
        public NavigationRootView(NavigationRootViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            Loaded += OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is NavigationRootViewModel vm)
            {
                // 动态构建主菜单和底部菜单
                BuildNavigationMenu(vm.MenuConfigs, NavigationViewControl.MenuItems);
                BuildNavigationMenu(vm.FooterConfigs, NavigationViewControl.FooterMenuItems);

                NavigationViewControl.SelectionChanged += (s, args) =>
                {
                    if (args.SelectedItem is NavigationViewItem selectedItem)
                    {
                        vm.Header = selectedItem.Content?.ToString();
                    }
                };
            }
        }

        private void BuildNavigationMenu(IEnumerable<MenuConfigItem> configs, IList targetCollection)
        {
            targetCollection.Clear();
            foreach (var config in configs)
            {
                targetCollection.Add(CreateMenuItem(config));
            }
        }

        private object CreateMenuItem(MenuConfigItem config)
        {
            if (config.Type == "Separator") return new NavigationViewItemSeparator();
            if (config.Type == "Header") return new NavigationViewItemHeader { Content = config.Title };

            var item = new NavigationViewItem
            {
                Content = config.Title,
                Tag = config.TargetPage, // save PageKey to Tag
                IsExpanded = config.IsExpanded,
                SelectsOnInvoked = config.Type == "Item"
            };

            if (!string.IsNullOrEmpty(config.Icon))
            {
                var iconType = typeof(SegoeFluentIcons);

                // 尝试获取静态属性 (Property)
                var propInfo = iconType.GetProperty(config.Icon, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                if (propInfo != null)
                {
                    // 使用 dynamic 让编译器在运行时自动匹配 Icon 属性的真实类型（通常是 IconKey）
                    item.Icon = new FontIcon { Icon = (dynamic)propInfo.GetValue(null)! };
                }
                else
                {
                    // 以防库的某些版本是用 const 或 static readonly 字段 (Field) 定义的
                    var fieldInfo = iconType.GetField(config.Icon, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
                    if (fieldInfo != null)
                    {
                        item.Icon = new FontIcon { Icon = (dynamic)fieldInfo.GetValue(null)! };
                    }
                }
            }

            // 递归处理子菜单
            if (config.Children != null && config.Children.Count > 0)
            {
                item.MenuItemsSource = config.Children.Select(CreateMenuItem).ToList();
            }

            return item;
        }

        private void NavigationViewControl_PaneClosing(NavigationView sender, NavigationViewPaneClosingEventArgs args)
        {
            if (sender.DisplayMode == NavigationViewDisplayMode.Minimal)
            {
                AppTitleBar.Visibility = Visibility.Visible;
            }
        }

        private void NavigationViewControl_PaneOpening(NavigationView sender, object args)
        {
            if (sender.DisplayMode == NavigationViewDisplayMode.Minimal)
            {
                AppTitleBar.Visibility = Visibility.Collapsed;
            }
        }

        private void NavigationViewControl_DisplayModeChanged(NavigationView sender, NavigationViewDisplayModeChangedEventArgs args)
        {
            Thickness currMargin = AppTitleBar.Margin;
            if (sender.DisplayMode == NavigationViewDisplayMode.Minimal)
            {
                AppTitleBar.Margin = new Thickness((sender.CompactPaneLength * 2), currMargin.Top, currMargin.Right, currMargin.Bottom);
            }
            else
            {
                AppTitleBar.Margin = new Thickness(sender.CompactPaneLength, currMargin.Top, currMargin.Right, currMargin.Bottom);
            }
            AppTitleBar.Visibility = sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top ? Visibility.Collapsed : Visibility.Visible;
            AppTitle.Margin = new Thickness(2, currMargin.Top, currMargin.Right, currMargin.Bottom);
        }
    }
}
