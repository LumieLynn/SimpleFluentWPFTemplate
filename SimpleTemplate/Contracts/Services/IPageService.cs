namespace SimpleTemplate.Contracts.Services
{
    public interface IPageService
    {
        Type GetPageType(string key);

        void ConfigurePages(string PageKey, Type PageType);

    }
}
