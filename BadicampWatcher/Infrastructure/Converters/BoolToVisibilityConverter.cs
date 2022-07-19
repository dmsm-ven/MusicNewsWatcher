using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MusicNewsWatcher.Infrastructure.Converters;

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var result = Visibility.Collapsed;

        if (value is bool? || value is bool)
        {
            result = (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        if (parameter is string par && par == "inverse")
        {
            result = (result == Visibility.Collapsed ? Visibility.Visible : Visibility.Collapsed);
        }

        return result;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
