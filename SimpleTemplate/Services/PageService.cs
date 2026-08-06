using SimpleTemplate.Contracts.Services;

namespace SimpleTemplate.Services
{
    public class PageService : IPageService
    {
        private readonly Dictionary<Type, Type> _views = new();

        public Type GetViewType(Type viewModelType)
        {
            if (!_views.TryGetValue(viewModelType, out var viewType))
                throw new InvalidOperationException($"No view is registered for ViewModel '{viewModelType.Name}'.");
            return viewType;
        }

        public void ConfigurePage(Type viewModelType, Type viewType)
        {
            _views.TryAdd(viewModelType, viewType);
        }
    }
}
