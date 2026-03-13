using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using System.Windows.Navigation;
using Frame = iNKORE.UI.WPF.Modern.Controls.Frame;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace SimpleTemplate.Services
{
    public class NavigationService(IServiceProvider serviceProvider, IPageService pageService) : INavigationService
    {
        private Frame? _frame;

        public event EventHandler? Navigated;

        public void Initialize(Frame frame, string? pageKey = null)
        {
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

            var viewTypeName = viewModelType.FullName?.Replace("ViewModels", "Views").Replace("ViewModel", "View");
            var viewType = viewModelType.Assembly.GetType(viewTypeName);

            if (viewType != null && _frame != null)
            {
                var page = Activator.CreateInstance(viewType) as iNKORE.UI.WPF.Modern.Controls.Page;
                if (page != null)
                {
                    page.DataContext = serviceProvider.GetRequiredService(viewModelType);

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
    }
}
