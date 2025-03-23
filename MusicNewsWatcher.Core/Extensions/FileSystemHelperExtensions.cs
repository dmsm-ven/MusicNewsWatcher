namespace MusicNewsWatcher.Core.Extensions;

public static class FileSystemHelperExtensions
{
    public static void OpenFolderInExplorer(string filePath)
    {
        if (Uri.IsWellFormedUriString(filePath, UriKind.Absolute))
        {
            var psi = new System.Diagnostics.ProcessStartInfo();
            psi.UseShellExecute = true;
            psi.FileName = filePath;
            System.Diagnostics.Process.Start(psi);
        }
    }
}


