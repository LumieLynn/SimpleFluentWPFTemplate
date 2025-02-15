using System.Windows;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Extensions.DependencyInjection;
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
        public NavigationRootView()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetService<NavigationRootViewModel>();
            _navigationService.ConfigureNavigation(NavigationItem_Home, typeof(HomePageViewModel), "Home");
            _navigationService.ConfigureNavigation(NavigationItem_Apps, typeof(AppsPageViewModel), "Apps");
        }

        private readonly NavigationService _navigationService = new();

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            _navigationService.TryNavigate(sender.SelectedItem, Frame_Main, NavigationViewControl);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            NavigationViewControl.SelectedItem = NavigationItem_Home;
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
