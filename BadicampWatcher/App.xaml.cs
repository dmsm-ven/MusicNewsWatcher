using BandcampWatcher.DataAccess;
using BandcampWatcher.Models;
using BandcampWatcher.Services;
using BandcampWatcher.ViewModels;
using Microsoft.Extensions.Hosting;
using System;
using System.Drawing;
using System.Windows;

namespace BandcampWatcher;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    ISettingsStorage settingsStorage = new JsonSettingsStorage("app_settings.json");
    protected override void OnStartup(StartupEventArgs e)
    {
        var settingsRoot = settingsStorage.Load();

        var vm = new MainWindowViewModel(settingsStorage);

        var mainWindow = new MainWindow();
        mainWindow.Top = settingsRoot.MainWindowRectangle.Y;
        mainWindow.Left = settingsRoot.MainWindowRectangle.X;
        mainWindow.Width = settingsRoot.MainWindowRectangle.Width;
        mainWindow.Height = settingsRoot.MainWindowRectangle.Height;
        mainWindow.DataContext = vm;
        mainWindow.Show();
    }

    protected override void OnDeactivated(EventArgs e)
    {
        var rect = new Rectangle()
        {
            Location = new System.Drawing.Point((int)MainWindow.Left, (int)MainWindow.Top),
            Size = new System.Drawing.Size((int)MainWindow.Width, (int)MainWindow.Height)
        };

        settingsStorage.SetValue(nameof(SettingsRoot.MainWindowRectangle), rect);
    }
}



