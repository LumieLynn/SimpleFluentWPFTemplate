using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using System.Windows.Navigation;
using Frame = iNKORE.UI.WPF.Modern.Controls.Frame;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace SimpleTemplate.Services
{
    public class NavigationService(IViewFactory viewFactory, IPageService pageService) : INavigationService, IDisposable
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
            var viewType = pageService.GetViewType(pageKey);

            if (_frame != null)
            {
                var page = viewFactory.CreateView(viewType);
                var viewModel = viewFactory.CreateViewModel(viewModelType);

                if (page != null)
                {
                    page.DataContext = viewModel;
                    return _frame.Navigate(page);
                }
                else
                {
                    throw new InvalidOperationException(
                        $"[Navigation Error] Navigation failed! Unable to instantiate View: {viewType.Name}。" +
                        $"Please check：\n" +
                        $"Is this View properly registered in DI?\n" +
                        $"Is its base class iNKORE.UI.WPF.Modern.Controls.Page?");
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
