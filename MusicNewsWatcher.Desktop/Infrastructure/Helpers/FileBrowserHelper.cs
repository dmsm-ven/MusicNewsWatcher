using System;
using System.Diagnostics;
using System.IO;

namespace MusicNewsWatcher;

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

    public static string DownloadDirectory
    {
        get
        {
            string downloadDir = Path.Combine(Environment.CurrentDirectory, "downloads");
            if (!Directory.Exists(downloadDir))
            {
                Directory.CreateDirectory(downloadDir);
            }
            return downloadDir;
        }
    }

    public static void OpenFolderInFileBrowser(string downloadedFilesDirectory)
    {
        var ps = new ProcessStartInfo()
        {
            FileName = downloadedFilesDirectory,
            UseShellExecute = true,
            Verb = "open"
        };
        Process.Start(ps);
    }

    public static void OpenDownloadsFolder()
    {
        OpenFolderInFileBrowser(DownloadDirectory);
    }
}



