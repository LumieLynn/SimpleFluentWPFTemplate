using System.Windows;
using iNKORE.UI.WPF.Modern.Controls;
using SimpleTemplate.Services;
using SimpleTemplate.ViewModels;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace SimpleTemplate.Views
{
    /// <summary>
    /// NavigationRootView.xaml 的交互逻辑
    /// </summary>
    public partial class NavigationRootView : Page
    {
        public NavigationRootView(NavigationService navigationService, NavigationRootViewModel viewModel)
        {
            InitializeComponent();
            _navigationService = navigationService;
            DataContext = viewModel;
            _navigationService.SetProperties(Frame_Main, NavigationViewControl);
            _navigationService.ConfigureNavigation(viewModel.MenuItems);
            _navigationService.ConfigureNavigation(viewModel.FooterItems);
        }

        private readonly NavigationService _navigationService;

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            _navigationService.TryNavigate(sender.SelectedItem);
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
