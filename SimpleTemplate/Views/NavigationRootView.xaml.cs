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
            DataContext = viewModel;
            _navigationService = navigationService;
            Loaded += OnLoaded;
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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _navigationService.SetProperties(Frame_Main, NavigationViewControl);
            var page = _navigationService.GetOrCreatePage(typeof(HomePageViewModel));
            NavigationViewControl.Header = "Home";
            Frame_Main.Navigate(page);
        }

        private readonly NavigationService _navigationService;

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
