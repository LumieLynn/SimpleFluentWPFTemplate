using iNKORE.UI.WPF.Modern.Controls;

namespace SimpleTemplate.Contracts.Services
{
    public interface INavigationService
    {
        event EventHandler Navigated;

        void Initialize(Frame frame, Type? pageType);

        bool CanGoBack
        {
            get;
        }

        bool NavigateTo(Type pageType, object? parameter = null);

        bool GoBack();

        object? GetCurrentViewModel();
    }
}
