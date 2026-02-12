using MusicNewsWatcher.Core.Models.Dtos;
using MusicNewsWatcher.Desktop.ViewModels.Items;

namespace MusicNewsWatcher.Desktop.ViewModels.Mappers;

public static class ArtistExtensions
{
    public static ArtistDto ToDto(this ArtistViewModel viewModel)
    {
        return new ArtistDto()
        {
            MusicProviderId = viewModel.ParentProvider.MusicProviderId,
            ArtistId = viewModel.ArtistId,
            Name = viewModel.Name,
            Uri = viewModel.Uri,
            Image = viewModel.Image,
        };
    }
}
