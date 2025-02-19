using CommunityToolkit.Mvvm.ComponentModel;
using iNKORE.UI.WPF.Modern;

namespace SimpleTemplate.ViewModels
{
    public partial class SettingsPageViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private ApplicationTheme? _currentTheme = ThemeManager.Current.ApplicationTheme;

        public SettingsPageViewModel()
        {
            ThemeManager.Current.ActualApplicationThemeChanged += (s, e) =>
            {
                UpdateCurrentTheme();
            };
        }

        private void UpdateCurrentTheme()
        {
            if (CurrentTheme != ThemeManager.Current.ApplicationTheme)
            {
                CurrentTheme = ThemeManager.Current.ApplicationTheme;
            }
        }
    }
}
