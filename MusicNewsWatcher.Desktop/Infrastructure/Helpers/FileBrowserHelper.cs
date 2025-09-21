using System.Diagnostics;
using System.IO;

namespace MusicNewsWatcher.Desktop.Infrastructure.Helpers;

public static class FileBrowserHelper
{
    public static string CacheDirectory
    {
        get
        {
            string cacheDir = Path.Combine(Environment.CurrentDirectory, "cache");
            if (!Directory.Exists(cacheDir))
            {
                Directory.CreateDirectory(cacheDir);
            }
            return cacheDir;
        }
    }

    public static void OpenFolderInFileBrowser(string folder)
    {
        var ps = new ProcessStartInfo()
        {
            FileName = folder,
            UseShellExecute = true,
            Verb = "open"
        };
        Process.Start(ps);
    }
}

public static class ObservableCollectionExtensions
{
    public static void AddRange<T>(this System.Collections.ObjectModel.ObservableCollection<T> collection, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            collection.Add(item);
        }
    }
}



