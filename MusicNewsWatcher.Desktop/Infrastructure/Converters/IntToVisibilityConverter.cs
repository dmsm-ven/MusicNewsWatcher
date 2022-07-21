using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MusicNewsWatcher.Infrastructure.Converters;

public class IntToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        Visibility result = Visibility.Collapsed;

        if (value is int v)
        {
            result = (v == 0 ? Visibility.Collapsed : Visibility.Visible);
        }

        if(parameter is string par && par == "inverse")
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
