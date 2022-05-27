using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace BandcampWatcher.Infrastructure.Converters;

public class BoolToForegroundColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var colors = parameter?.ToString()?.Split(';');
        if (parameter != null && colors?.Length == 2)
        {
            string pickedColor = colors[(bool)value ? 0 : 1];
            var brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(pickedColor));
            return brush;
        }
        return Brushes.Black;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}