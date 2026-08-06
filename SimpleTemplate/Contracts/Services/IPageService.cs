namespace SimpleTemplate.Contracts.Services
{
    public interface IPageService
    {
        Type GetViewType(Type viewModelType);
        void ConfigurePage(Type viewModelType, Type viewType);
    }
}
