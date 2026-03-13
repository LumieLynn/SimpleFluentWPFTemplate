using CommunityToolkit.Mvvm.ComponentModel;
using iNKORE.UI.WPF.Modern;
using SimpleTemplate.Infrastructure;
using SimpleTemplate.Views;

namespace SimpleTemplate.ViewModels
{
    [RegisterView(typeof(SettingsPageView))]
    public partial class SettingsPageViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private ApplicationTheme? _currentTheme = ThemeManager.Current.ApplicationTheme;

        partial void OnCurrentThemeChanging(ApplicationTheme? oldValue, ApplicationTheme? newValue)
        {
            if (ThemeManager.Current.ApplicationTheme != newValue)
            {
                ThemeManager.Current.ApplicationTheme = newValue;
            }
        }

    }
}
