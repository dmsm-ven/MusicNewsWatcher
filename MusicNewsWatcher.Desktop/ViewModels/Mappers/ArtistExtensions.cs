using MusicNewsWatcher.Core.Models.Dtos;
using MusicNewsWatcher.Desktop.ViewModels.Items;

namespace MusicNewsWatcher.Desktop.ViewModels.Mappers;

public static class ArtistExtensions
{
    public static ArtistDto ToDto(this ArtistViewModel viewModel)
    {
        return new ArtistDto(viewModel.ParentProvider.MusicProviderId, viewModel.ArtistId, viewModel.Name, viewModel.Uri, viewModel.Image);
    }
}
