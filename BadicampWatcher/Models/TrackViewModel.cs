﻿using MusicNewsWatcher.Infrastructure.Helpers;
using MusicNewsWatcher.Services;
using System.Web;

namespace MusicNewsWatcher.ViewModels;

public class TrackViewModel : ViewModelBase
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string Name { get; set; }
    public string DisplayName => Name.ToDisplayName();
    public string? DownloadUri { get; set; }


    public bool IsDownloaded => DownloadResult != TrackDownloadResult.None;

    TrackDownloadResult downloadResult;
    public TrackDownloadResult DownloadResult
    {
        get => downloadResult;
        set
        {
            if(Set(ref downloadResult, value))
            {
                RaisePropertyChanged(nameof(IsDownloaded));
            }
        }
    }

    bool isDownloading;
    public bool IsDownloading
    {
        get => isDownloading;
        set => Set(ref isDownloading, value);
    }
}