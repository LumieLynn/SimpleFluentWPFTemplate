using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.ViewModels;

namespace SimpleTemplate.Views
{
    /// <summary>
    /// AppsPageView.xaml 的交互逻辑
    /// </summary>
    public partial class AppsPageView : Page
    {
        public AppsPageView()
        {
            InitializeComponent();
            DataContext = App.Current.Services.GetService<AppsPageViewModel>();
        }
    }
}
