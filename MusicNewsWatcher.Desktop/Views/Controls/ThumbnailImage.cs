using MusicNewsWatcher.Desktop.Infrastructure.Helpers;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MusicNewsWatcher.Desktop.Views.Controls;

/// <summary>
/// Картинка-превью, которая загружается лениво: только когда оказывается рядом с видимой областью
/// родительского ScrollViewer, и освобождается, когда уходит далеко за её пределы.
/// Благодаря этому в памяти одновременно живут только изображения, близкие к экрану,
/// даже если в списке сотни исполнителей/альбомов.
/// </summary>
public sealed class ThumbnailImage : Image
{
    public static readonly DependencyProperty UrlProperty = DependencyProperty.Register(
        nameof(Url),
        typeof(string),
        typeof(ThumbnailImage),
        new PropertyMetadata(null, OnUrlChanged));

    /// <summary>Адрес оригинального изображения (из него считается имя файла в кэше)</summary>
    public string? Url
    {
        get => (string?)GetValue(UrlProperty);
        set => SetValue(UrlProperty, value);
    }

    /// <summary>Тип превью: определяет размер, до которого сжимается файл в кэше</summary>
    public ThumbnailSize Kind { get; set; } = ThumbnailSize.Album;

    /// <summary>Ширина в пикселях, до которой декодируется картинка в памяти (размер карточки с запасом под DPI)</summary>
    public int DecodeWidth { get; set; } = 200;

    // Картинка загружается, когда карточка не дальше N высот окна от видимой области,
    // и выгружается, когда уходит дальше M высот (разные пороги, чтобы не было «мигания»)
    private const double LoadDistanceInViewports = 1.0;
    private const double UnloadDistanceInViewports = 3.0;

    private ScrollViewer? scroller;
    private CancellationTokenSource? loadCts;
    private bool isLoading;

    public ThumbnailImage()
    {
        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
        IsVisibleChanged += (_, _) => Evaluate();
    }

    private static void OnUrlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var self = (ThumbnailImage)d;
        self.Release();
        self.Evaluate();
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        scroller = FindParentScrollViewer();
        if (scroller != null)
        {
            scroller.ScrollChanged += OnScrollChanged;
        }

        // Даём завершиться первому проходу layout, чтобы координаты карточки были корректны
        Dispatcher.BeginInvoke(new Action(Evaluate), DispatcherPriority.Background);
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (scroller != null)
        {
            scroller.ScrollChanged -= OnScrollChanged;
            scroller = null;
        }

        Release();
    }

    private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        Evaluate();
    }

    private void Evaluate()
    {
        if (!IsLoaded)
        {
            return;
        }

        if (!IsVisible)
        {
            Release();
            return;
        }

        if (Source is null)
        {
            if (!isLoading && IsWithinViewport(LoadDistanceInViewports))
            {
                _ = LoadAsync();
            }
        }
        else if (!IsWithinViewport(UnloadDistanceInViewports))
        {
            Release();
        }
    }

    private bool IsWithinViewport(double distanceInViewports)
    {
        if (scroller == null)
        {
            return true;
        }

        try
        {
            Rect bounds = TransformToAncestor(scroller).TransformBounds(new Rect(0, 0, ActualWidth, ActualHeight));

            double margin = scroller.ActualHeight * distanceInViewports;
            var viewport = new Rect(0, -margin, scroller.ActualWidth, scroller.ActualHeight + (2 * margin));

            return bounds.IntersectsWith(viewport);
        }
        catch (InvalidOperationException)
        {
            // Элемент уже не является потомком ScrollViewer
            return true;
        }
    }

    private async Task LoadAsync()
    {
        var service = App.HostContainer?.Services.GetService<IImageThumbnailCacheService>();
        if (service == null)
        {
            return;
        }

        loadCts?.Cancel();
        var cts = new CancellationTokenSource();
        loadCts = cts;
        isLoading = true;

        try
        {
            string path = await service.GetCachedImage(Url, Kind);
            if (cts.IsCancellationRequested)
            {
                return;
            }

            BitmapSource? bitmap = await ThumbnailBitmapLoader.LoadAsync(path, DecodeWidth);
            if (cts.IsCancellationRequested)
            {
                return;
            }

            if (bitmap != null)
            {
                Source = bitmap;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"ThumbnailImage: {ex.Message}");
        }
        finally
        {
            if (ReferenceEquals(loadCts, cts))
            {
                isLoading = false;
            }
        }
    }

    private void Release()
    {
        loadCts?.Cancel();
        loadCts = null;
        isLoading = false;

        if (Source is not null)
        {
            ClearValue(SourceProperty);
        }
    }

    private ScrollViewer? FindParentScrollViewer()
    {
        DependencyObject? current = VisualTreeHelper.GetParent(this);
        while (current != null)
        {
            if (current is ScrollViewer viewer)
            {
                return viewer;
            }
            current = VisualTreeHelper.GetParent(current);
        }
        return null;
    }
}
