using MusicNewsWatcher.Infrastructure.Helpers;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MusicNewsWatcher.Desktop.ViewModels;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public ICommand OpenInBrowserCommand { get; } = new LambdaCommand(OpenInBrowser);

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

    protected static void OpenInBrowser(object param)
    {
        if (param is string uri && Uri.IsWellFormedUriString(uri, UriKind.Absolute))
        {
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);
        }
    }

    protected string GetCachedImage(string originalSourceUri)
    {
        string[] VALID_EXT = new[] { ".jpg", ".png", ".jpeg" };
        const string PLACEHOLDER = @"D:\Programming\CSharp\Парсеры\BadicampWatcher\BadicampWatcher\Assets\image-placeholder.jpg";

        if (string.IsNullOrEmpty(originalSourceUri)) { return PLACEHOLDER; }
        if (!originalSourceUri.StartsWith("http")) { return PLACEHOLDER; }
        if (!Uri.IsWellFormedUriString(originalSourceUri, UriKind.Absolute)) { return PLACEHOLDER; }

        string clearName = new string(originalSourceUri.Except(Path.GetInvalidFileNameChars()).ToArray());
        string ext = Path.GetExtension(clearName);

        if (!VALID_EXT.Contains(ext.ToLower())) { ext = VALID_EXT[0]; }

        string cachedName = Path.Combine(FileBrowserHelper.CacheDirectory, originalSourceUri.GetMD5() + ext);
        if (!File.Exists(cachedName))
        {
            try
            {
                new WebClient().DownloadFile(originalSourceUri, cachedName);
            }
            catch(Exception ex)
            {
                File.Copy(PLACEHOLDER, cachedName);
                return PLACEHOLDER;
            }
        }
        return cachedName;
    }
}
