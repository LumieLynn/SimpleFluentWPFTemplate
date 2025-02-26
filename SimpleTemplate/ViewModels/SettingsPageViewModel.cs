using CommunityToolkit.Mvvm.ComponentModel;
using iNKORE.UI.WPF.Modern;

namespace SimpleTemplate.ViewModels
{
    public partial class SettingsPageViewModel : ObservableRecipient
    {
        [ObservableProperty]
        private ApplicationTheme? _currentTheme = ThemeManager.Current.ApplicationTheme;

        private ThemeManager _themeManager = ThemeManager.Current;

        public SettingsPageViewModel()
        {
            _themeManager.ActualApplicationThemeChanged += OnThemeChanged;
        }

        private void OnThemeChanged(ThemeManager sender, object args)
        {
            UpdateCurrentTheme();
        }

        public void Dispose()
        {
            _themeManager.ActualApplicationThemeChanged -= OnThemeChanged;
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
