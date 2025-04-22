using CommunityToolkit.Mvvm.ComponentModel;
using iNKORE.UI.WPF.Modern;

namespace SimpleTemplate.ViewModels
{
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
