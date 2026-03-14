using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Contracts.ViewModels;
using System.Windows.Navigation;
using Frame = iNKORE.UI.WPF.Modern.Controls.Frame;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace SimpleTemplate.Services
{
    public class NavigationService(IViewFactory viewFactory, IPageService pageService) : INavigationService, IDisposable
    {
        private Frame? _frame;
        private string? _currentPageKey;
        private readonly Stack<string> _backStack = new();

        public event EventHandler? Navigated;

        public void Initialize(Frame frame, string? pageKey = null)
        {
            if (_frame != null)
            {
                _frame.Navigated -= OnNavigated;
            }

            _frame = frame;
            frame.Navigated += OnNavigated;
            _backStack.Clear();

            if (pageKey != null)
            {
                NavigateTo(pageKey);
            }
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is System.Windows.FrameworkElement element)
            {
                if (element.DataContext is INavigationAware newVm)
                {
                    newVm.OnNavigatedTo(element.Tag);
                }
            }
            Navigated?.Invoke(this, e);
        }

        public bool CanGoBack => _frame != null && _backStack.Count > 0;

        public bool GoBack()
        {
            if (CanGoBack && _frame != null)
            {
                var previousPageKey = _backStack.Pop();

                return NavigateInternal(previousPageKey, null, isBackNavigation: true);
            }
            return false;
        }

        public bool NavigateTo(string pageKey, object? parameter = null)
        {
            if (_currentPageKey == pageKey) return false;

            return NavigateInternal(pageKey, parameter, isBackNavigation: false);
        }

        private bool NavigateInternal(string pageKey, object? parameter, bool isBackNavigation)
        {
            var viewModelType = pageService.GetPageType(pageKey);
            var viewType = pageService.GetViewType(pageKey);

            if (_frame != null)
            {
                if (GetCurrentViewModel() is INavigationAware oldVm)
                {
                    oldVm.OnNavigatedFrom();
                }

                if (!isBackNavigation && _currentPageKey != null)
                {
                    _backStack.Push(_currentPageKey);
                }

                var page = viewFactory.CreateView(viewType);
                var viewModel = viewFactory.CreateViewModel(viewModelType);

                if (page != null)
                {
                    page.DataContext = viewModel;
                    _currentPageKey = pageKey;

                    if (viewModel is INavigationAware newVm)
                    {
                        newVm.OnNavigatedTo(parameter);
                    }

                    return _frame.Navigate(page);
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
