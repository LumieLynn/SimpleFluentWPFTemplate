using iNKORE.UI.WPF.Modern.Controls;

namespace SimpleTemplate.Contracts.Services
{
    public interface INavigationViewService
    {
        void Initialize(NavigationView navigationView, IEnumerable<object>? menuItems, IEnumerable<object>? footerItems);

        void ConfigurePairs(IEnumerable<object> items);

        NavigationViewItem? GetCurrentSelectedItem();
    }
}
