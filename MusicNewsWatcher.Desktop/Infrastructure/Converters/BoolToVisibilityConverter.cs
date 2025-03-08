using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MusicNewsWatcher.Desktop.Infrastructure.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var result = Visibility.Collapsed;

        if (value is bool val)
        {
            if (parameter is string par && par == "inverse")
            {
                val = !val;
            }

            result = val ? Visibility.Visible : Visibility.Collapsed;
        }

        return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
