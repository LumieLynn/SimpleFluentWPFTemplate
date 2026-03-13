using iNKORE.UI.WPF.Modern.Controls;

namespace SimpleTemplate.Contracts.Services
{
    public interface INavigationViewService : IDisposable
    {
        void Initialize(NavigationView navigationView);

    }
}
