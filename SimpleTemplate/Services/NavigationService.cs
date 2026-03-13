using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using System.Diagnostics;
using System.Windows.Navigation;
using Frame = iNKORE.UI.WPF.Modern.Controls.Frame;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace SimpleTemplate.Services
{
    public class NavigationService(IServiceProvider serviceProvider, IPageService pageService) : INavigationService, IDisposable
    {
        private Frame? _frame;

        public event EventHandler? Navigated;

        public void Initialize(Frame frame, string? pageKey = null)
        {
            if (_frame != null)
            {
                _frame.Navigated -= OnNavigated;
            }

            _frame = frame;
            frame.Navigated += OnNavigated;
            if (pageKey != null)
            {
                NavigateTo(pageKey);
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            Navigated?.Invoke(this, e);
        }

        public bool CanGoBack => _frame != null && _frame.CanGoBack;

        public bool GoBack()
        {
            bool navigated = false;
            if (CanGoBack && _frame != null)
            {
                _frame.GoBack();
                navigated = true;
            }

            return navigated;
        }

        public bool NavigateTo(string pageKey)
        {
            var viewModelType = pageService.GetPageType(pageKey);

            var vmFullName = viewModelType.FullName ?? string.Empty;
            var viewTypeName = vmFullName
                .Replace(".ViewModels.", ".Views.")
                .Replace("ViewModel", "View");

            var viewType = viewModelType.Assembly.GetType(viewTypeName);

            if (viewType == null)
            {
                Debug.WriteLine($"[Navigation Error] 无法找到 View 类型: {viewTypeName} (对应的 ViewModel: {vmFullName})");
                return false;
            }

            if (_frame != null)
            {
                var page = serviceProvider.GetService(viewType) as Page;

                if (page != null)
                {
                    page.DataContext = serviceProvider.GetRequiredService(viewModelType);
                    return _frame.Navigate(page);
                }
                else
                {
                    Debug.WriteLine($"[Navigation Error] 无法实例化 View: {viewTypeName}，请检查是否在 DI 中注册或它是否继承自 Page。");
                }
            }

            return false;
        }

        public object? GetCurrentViewModel()
        {
            if (_frame?.Content is System.Windows.FrameworkElement element)
            {
                return element.DataContext;
            }

            return null;
        }

        public void Dispose()
        {
            if (_frame != null)
            {
                _frame.Navigated -= OnNavigated;
                _frame = null;
            }
            GC.SuppressFinalize(this);
        }
    }
}
