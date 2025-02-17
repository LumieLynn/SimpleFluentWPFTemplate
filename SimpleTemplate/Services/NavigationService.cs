﻿using System.Windows.Navigation;
using iNKORE.UI.WPF.Modern.Controls;
using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Views.ProxyPage;
using Frame = iNKORE.UI.WPF.Modern.Controls.Frame;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace SimpleTemplate.Services
{
    public class NavigationService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<object, (Type ViewModelType, string Title)> _navigationMap = new();
        private readonly Dictionary<Type, WeakReference<Page>> _pageCache = new();
        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ConfigureNavigation(object menuItem, Type viewModelType, string title)
        {
            _navigationMap[menuItem] = (viewModelType, title);
        }

        public bool TryNavigate(object menuItem, Frame frame, NavigationView navigationView)
        {
            if (_navigationMap.TryGetValue(menuItem, out var config))
            {
                var page = GetOrCreatePage(config.ViewModelType);
                frame.Navigate(page);
                navigationView.Header = config.Title;
                return true;
            }
            return false;
        }

        private Page GetOrCreatePage(Type viewModelType)
        {
            if (_pageCache.TryGetValue(viewModelType, out var weakref) && weakref.TryGetTarget(out var cachedPage))
            {
                return cachedPage;
            }

            var newPage = _serviceProvider.GetRequiredService<NavigationProxyPage>();
            newPage.ViewModel = _serviceProvider.GetRequiredService(viewModelType);

            _pageCache[viewModelType] = new WeakReference<Page>(newPage);
            return newPage;
        }
    }
}
