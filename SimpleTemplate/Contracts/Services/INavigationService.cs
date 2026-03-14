using iNKORE.UI.WPF.Modern.Controls;

namespace SimpleTemplate.Contracts.Services
{
    public interface INavigationService
    {
        event EventHandler Navigated;

        void Initialize(Frame frame, string pageKey);

        bool CanGoBack
        {
            get;
        }

        bool NavigateTo(string pageKey, object? parameter = null);

        bool GoBack();

        object? GetCurrentViewModel();
    }
}
