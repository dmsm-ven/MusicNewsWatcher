using BandcampWatcher.Infrastructure.Helpers;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

namespace BandcampWatcher.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool Set<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value)) return false;
        field = value;
        RaisePropertyChanged(propertyName);

        return true;
    }

    protected string GetCachedImage(string originalSourceUri)
    {
        string[] VALID_EXT = new[] { ".jpg", ".png", ".jpeg" };
        const string PLACEHOLDER = @"D:\Programming\CSharp\Парсеры\BadicampWatcher\BadicampWatcher\Assets\image-placeholder.jpg";

        if (!string.IsNullOrEmpty(originalSourceUri))
        {
            string clearName = new string(originalSourceUri.Except(Path.GetInvalidFileNameChars()).ToArray());
            string ext = Path.GetExtension(clearName);

            if (!VALID_EXT.Contains(ext.ToLower())) { ext = VALID_EXT[0]; }

            string cachedName = Path.Combine(App.CacheDirectory, originalSourceUri.GetMD5() + ext);

            if(!Uri.IsWellFormedUriString(originalSourceUri, UriKind.Absolute))
            {
                return PLACEHOLDER;
            }

            if (!File.Exists(cachedName))
            {
                try
                {
                    new WebClient().DownloadFile(originalSourceUri, cachedName);
                }
                catch
                {
                    return PLACEHOLDER;
                }
            }
            return cachedName;

        }
        return PLACEHOLDER;
    }
}
