namespace SimpleTemplate.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);
        Type GetViewType(string key);
        void ConfigurePage(string PageKey, Type vmType, Type viewType);

    }
}
