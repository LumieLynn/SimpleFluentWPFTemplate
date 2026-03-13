using Microsoft.Extensions.DependencyInjection;
using SimpleTemplate.Contracts.Services;
using Page = iNKORE.UI.WPF.Modern.Controls.Page;

namespace SimpleTemplate.Services
{
    public class ViewFactory(IServiceProvider serviceProvider) : IViewFactory
    {
        public Page CreateView(Type viewType)
        {
            return (Page)serviceProvider.GetRequiredService(viewType);
        }

        public object CreateViewModel(Type viewModelType)
        {
            return serviceProvider.GetRequiredService(viewModelType);
        }
    }
}