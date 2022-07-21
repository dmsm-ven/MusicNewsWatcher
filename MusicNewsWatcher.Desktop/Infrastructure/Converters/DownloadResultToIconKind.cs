using MahApps.Metro.IconPacks;
using MusicNewsWatcher.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MusicNewsWatcher.Infrastructure.Converters;

public class TrackDownloadResultToIconKind : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value is TrackDownloadResult d)
        {
            PackIconFontAwesomeKind result = d switch
            {
                TrackDownloadResult.Success => PackIconFontAwesomeKind.CheckSolid,
                TrackDownloadResult.Error => PackIconFontAwesomeKind.TimesSolid,
                TrackDownloadResult.Skipped => PackIconFontAwesomeKind.ArrowRightSolid,
                TrackDownloadResult.Cancelled => PackIconFontAwesomeKind.StopCircleSolid,
                _ => PackIconFontAwesomeKind.QuestionSolid
            };
            return result;
        }
        return PackIconFontAwesomeKind.QuestionSolid;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
