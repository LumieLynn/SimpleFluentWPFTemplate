using iNKORE.UI.WPF.Modern;
using System.Globalization;
using System.Windows.Data;

namespace SimpleTemplate.Helpers
{
    internal sealed class ThemeToIndexConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                ApplicationTheme.Dark => 2,
                ApplicationTheme.Light => 1,
                _ => 0
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value switch
            {
                2 => ApplicationTheme.Dark,
                1 => ApplicationTheme.Light,
                _ => null
            };
        }
    }
}
