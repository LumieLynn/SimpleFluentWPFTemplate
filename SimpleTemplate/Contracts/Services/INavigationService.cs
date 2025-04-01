using iNKORE.UI.WPF.Modern.Controls;

namespace SimpleTemplate.Contracts.Services
{
    public interface INavigationService
    {
        event EventHandler Navigated;

        void Initialize(Frame frame);

        bool CanGoBack
        {
            get;
        }

        bool NavigateTo(string pageKey);

        bool GoBack();

        object? GetCurrentViewModel();
    }
}
