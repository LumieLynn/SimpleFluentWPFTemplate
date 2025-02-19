using System.Globalization;
using System.Windows.Data;
using iNKORE.UI.WPF.Modern;

namespace SimpleTemplate.Helpers
{
    internal sealed class ThemeToIndexConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ApplicationTheme.Dark)
            {
                return 2;
            }
            if (ThemeManager.Current.ApplicationTheme == null)
            {
                return 0;
            }

            return 1;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is 2)
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            }
            else if (value is 1)
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            }
            else ThemeManager.Current.ApplicationTheme = null;
            return ThemeManager.Current.ApplicationTheme;
        }
    }
}
