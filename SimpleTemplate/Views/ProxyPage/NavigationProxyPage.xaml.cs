using System.Windows;
using System.Windows.Navigation;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace SimpleTemplate.Views.ProxyPage
{
    /// <summary>
    /// NavigationHostPage.xaml 的交互逻辑
    /// </summary>
    public partial class NavigationProxyPage : Page
    {
        public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register("ViewModel", typeof(object), typeof(NavigationProxyPage));

        public object ViewModel
        {
            get => GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public NavigationProxyPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (ViewModel is IDisposable disposable)
            {
                disposable.Dispose();
            }
            base.OnNavigatedFrom(e);
        }
    }
}
