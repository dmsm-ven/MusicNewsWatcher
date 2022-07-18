using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Models;

public class SettingsRoot
{
    public Rectangle MainWindowRectangle { get; set; }
    public DateTime? LastCheck { get; set; }
}
