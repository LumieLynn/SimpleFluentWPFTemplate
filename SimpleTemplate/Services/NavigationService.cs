using iNKORE.UI.WPF.Modern.Controls;
using SimpleTemplate.Views.ProxyPage;
using Frame = iNKORE.UI.WPF.Modern.Controls.Frame;

namespace SimpleTemplate.Services
{
    public class NavigationService
    {
        private readonly Dictionary<object, (Type ViewModelType, string Title)> _navigationMap = new();

        public void ConfigureNavigation(object menuItem, Type viewModelType, string title)
        {
            _navigationMap[menuItem] = (viewModelType, title);
        }

        public bool TryNavigate(object menuItem, Frame frame, NavigationView navigationView)
        {
            if (_navigationMap.TryGetValue(menuItem, out var config))
            {
                var viewModel = Activator.CreateInstance(config.ViewModelType);
                frame.Navigate(new NavigationProxyPage { ViewModel = viewModel });
                navigationView.Header = config.Title;
                return true;
            }
            return false;
        }
    }
}
