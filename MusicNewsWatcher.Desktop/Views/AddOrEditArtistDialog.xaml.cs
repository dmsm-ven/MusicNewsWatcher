using System.Windows;
using System.Windows.Input;

namespace MusicNewsWatcher.Desktop.Views;
/// <summary>
/// Interaction logic for AddNewArtistDialog.xaml
/// </summary>
public partial class AddOrEditArtistDialog : Window
{
    public AddOrEditArtistDialog()
    {
        InitializeComponent();
    }

    private void addNewArtistDialogWindow_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
    }
}
