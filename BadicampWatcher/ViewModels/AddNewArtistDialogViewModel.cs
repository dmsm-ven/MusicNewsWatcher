using BandcampWatcher.DataAccess;
using BandcampWatcher.Models;
using System;
using System.Windows;
using System.Windows.Input;

namespace BandcampWatcher.ViewModels;

public class AddNewArtistDialogViewModel : ViewModelBase
{
    public ArtistModel NewArtist { get; private set; }

    public string SubmitCommandTitle { get; private set; }

    string name;
    public string Name { get => name; set => Set(ref name, value); }

    string image;
    public string Image 
    { 
        get => image;
        set => Set(ref image, value);
    }

    string uri;
    public string Uri { get => uri; set => Set(ref uri, value); }

    public ICommand SubmitCommand { get; }

    public AddNewArtistDialogViewModel(ArtistModel artist) : this()
    {
        Name = artist.Name;
        Uri = artist.Uri;
        Image = artist.Image;
        SubmitCommandTitle = "Изменить";
    }

    public AddNewArtistDialogViewModel()
    {
        SubmitCommand = new LambdaCommand(Submit, e =>
            !string.IsNullOrWhiteSpace(Name) &&
            !string.IsNullOrWhiteSpace(Image) &&
            !string.IsNullOrWhiteSpace(Uri));
        SubmitCommandTitle = "Добавить";
    }

    private void Submit(object obj)
    {
        NewArtist = new ArtistModel()
        {
            Name = Name,
            Uri = Uri,
            Image = Image
        };

        (obj as Window).DialogResult = true;
    }
}
