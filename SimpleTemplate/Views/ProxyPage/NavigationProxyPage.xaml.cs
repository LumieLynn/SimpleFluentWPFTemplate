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

        public string SendViewModelName()
        {
            if (ViewModel == null) return string.Empty;
            return ViewModel.GetType().FullName;
        }

        public NavigationProxyPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (DataContext is IDisposable disposable)
            {
                disposable.Dispose();
            }
            base.OnNavigatedFrom(e);
        }
    }
}
