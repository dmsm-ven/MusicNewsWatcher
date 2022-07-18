using MusicNewsWatcher.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Services;

public class JsonSettingsStorage : ISettingsStorage
{
    private readonly string fileName;

    public JsonSettingsStorage(string fileName)
    {
        this.fileName = fileName;
    }

    public SettingsRoot Load()
    {
        if (File.Exists(fileName))
        {
            return JsonConvert.DeserializeObject<SettingsRoot>(File.ReadAllText(fileName));
        }
        return new SettingsRoot() { MainWindowRectangle = new System.Drawing.Rectangle() { X = 100, Y = 100, Width = 500, Height = 500 } };
    }



    public void SetValue<T>(string propertyName, T value)
    {
        var settings = Load();

        settings.GetType().GetProperty(propertyName).SetValue(settings, value);


        File.WriteAllText(fileName, JsonConvert.SerializeObject(settings));
    }
}
public interface ISettingsStorage
{
    SettingsRoot Load();

    void SetValue<T>(string propertyName, T value);
}
