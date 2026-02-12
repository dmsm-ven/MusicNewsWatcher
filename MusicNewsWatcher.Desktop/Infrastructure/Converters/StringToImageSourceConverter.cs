using Microsoft.Extensions.Options;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MusicNewsWatcher.Desktop.Infrastructure.Converters;

[ValueConversion(typeof(string), typeof(ImageSource))]
public class StringToImageSourceConverter : IValueConverter
{
    private static Lazy<string> defaultPath;
    static StringToImageSourceConverter()
    {
        defaultPath = new Lazy<string>(() =>
        {
            var thumbnailOptions = App.HostContainer.Services.GetRequiredService<IOptions<ImageThumbnailCacheServiceOptions>>();

            return thumbnailOptions.Value.PlaceholderFilePath;
        });
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (!(value is string valueString) || !File.Exists(valueString))
        {
            return defaultPath.Value;
        }

        try
        {
            ImageSource image = BitmapFrame.Create(new Uri(valueString), BitmapCreateOptions.IgnoreImageCache, BitmapCacheOption.OnLoad);
            return image;
        }
        catch
        {
            return defaultPath.Value;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
