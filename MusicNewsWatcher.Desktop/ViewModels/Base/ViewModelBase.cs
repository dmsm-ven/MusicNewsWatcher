using MusicNewsWatcher.Desktop.Infrastructure.Commands.Base;
using MusicNewsWatcher.Infrastructure.Helpers;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.ViewModels.Base;

public abstract class ViewModelBase : INotifyPropertyChanged
{
    private static readonly string PLACEHOLDER = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Assets", "image-placeholder.jpg");

    public event PropertyChangedEventHandler? PropertyChanged;

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

    private static void OpenInBrowser(object param)
    {
        if (param is string uri && Uri.IsWellFormedUriString(uri, UriKind.Absolute))
        {
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = uri;
            System.Diagnostics.Process.Start(psi);
        }
    }

    protected string GetCachedImageAndCreate(string originalSourceUri)
    {
        string[] VALID_EXT = new[] { ".jpg", ".png", ".jpeg" };

        if (string.IsNullOrEmpty(originalSourceUri)) { return PLACEHOLDER; }
        if (!originalSourceUri.StartsWith("http")) { return PLACEHOLDER; }
        if (!Uri.IsWellFormedUriString(originalSourceUri, UriKind.Absolute)) { return PLACEHOLDER; }

        string clearName = new(originalSourceUri.Except(Path.GetInvalidFileNameChars()).ToArray());
        string ext = Path.GetExtension(clearName);

        if (!VALID_EXT.Contains(ext.ToLower())) { ext = VALID_EXT[0]; }

        string cachedName = Path.Combine(FileBrowserHelper.CacheDirectory, originalSourceUri.GetMD5() + ext);
        if (!File.Exists(cachedName))
        {
            try
            {
                new WebClient().DownloadFile(originalSourceUri, cachedName);
            }
            catch (Exception ex)
            {
                File.Copy(PLACEHOLDER, cachedName);
                return PLACEHOLDER;
            }
        }
        return cachedName;
    }
}
