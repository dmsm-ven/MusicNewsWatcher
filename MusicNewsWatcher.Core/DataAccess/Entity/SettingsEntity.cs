using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicNewsWatcher.Core;

public class SettingsEntity
{
    [Key]
    public string Name { get; set; }

    public string Value { get; set; } = string.Empty;
}
