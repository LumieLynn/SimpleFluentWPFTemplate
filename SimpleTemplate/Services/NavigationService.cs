using SimpleTemplate.Contracts.Services;
using SimpleTemplate.Contracts.ViewModels;
using System.Windows;
using System.Windows.Navigation;
using Frame = iNKORE.UI.WPF.Modern.Controls.Frame;

namespace SimpleTemplate.Services
{
    public class NavigationService(IViewFactory viewFactory, IPageService pageService) : INavigationService, IDisposable
    {
        private Frame? _frame;
        private object? _pendingOldViewModel;
        private object? _pendingParameter;

        public event EventHandler? Navigated;

        public void Initialize(Frame frame, string? pageKey = null)
        {
            if (_frame != null)
            {
                DetachFrameEvents();
            }

            _frame = frame;
            frame.Navigating += OnFrameNavigating;
            frame.Navigated += OnFrameNavigated;

            if (pageKey != null)
            {
                NavigateTo(pageKey);
            }
        }

        private void DetachFrameEvents()
        {
            if (_frame != null)
            {
                _frame.Navigating -= OnFrameNavigating;
                _frame.Navigated -= OnFrameNavigated;
            }
        }

        public bool CanGoBack => _frame != null && _frame.CanGoBack;

        public bool GoBack()
        {
            if (!CanGoBack || _frame == null)
            {
                return false;
            }

            // Lifecycle callbacks are raised from the frame's Navigated event,
            // which fires for back navigation too - so the view model we return
            // to also receives OnNavigatedTo (with a null parameter).
            _frame.GoBack();
            return true;
        }

        public bool NavigateTo(string pageKey, object? parameter = null)
        {
            if (_frame == null)
            {
                return false;
            }

            var viewModelType = pageService.GetPageType(pageKey);
            if (viewModelType == GetCurrentViewModel()?.GetType())
            {
                // Already on this page - no-op, avoids duplicate journal entries.
                return true;
            }

            var viewType = pageService.GetViewType(pageKey);
            var page = viewFactory.CreateView(viewType);
            if (page == null)
            {
                throw new InvalidOperationException(
                    $"[Navigation Error] Navigation failed! Unable to instantiate View: {viewType.Name}.\n" +
                    $"Please check:\n" +
                    $"Is this View properly registered in DI?\n" +
                    $"Is its base class iNKORE.UI.WPF.Modern.Controls.Page?");
            }

            var viewModel = viewFactory.CreateViewModel(viewModelType);
            page.DataContext = viewModel;
            _pendingParameter = parameter;

            if (!_frame.Navigate(page))
            {
                _pendingParameter = null;
                return false;
            }

            return true;
        }

        private void OnFrameNavigating(object sender, NavigatingCancelEventArgs e)
        {
            // Capture the view model that is being left; the callback fires only
            // if the navigation actually succeeds (in OnFrameNavigated).
            _pendingOldViewModel = GetCurrentViewModel();
        }

        private void OnFrameNavigated(object sender, NavigationEventArgs e)
        {
            var oldViewModel = _pendingOldViewModel;
            _pendingOldViewModel = null;

            (oldViewModel as INavigationAware)?.OnNavigatedFrom();

            if (e.Content is FrameworkElement element)
            {
                (element.DataContext as INavigationAware)?.OnNavigatedTo(_pendingParameter);
            }

            _pendingParameter = null;

            Navigated?.Invoke(this, e);
        }

        public object? GetCurrentViewModel()
        {
            if (_frame?.Content is FrameworkElement element)
            {
                return element.DataContext;
            }

            return null;
        }

        public void Dispose()
        {
            DetachFrameEvents();
            _frame = null;
            GC.SuppressFinalize(this);
        }
    }
}
