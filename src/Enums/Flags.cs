using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager.Enums {
  public enum Flags : long {
    None = 0,
    AutoHideOnOpen = 1,
    MinimizeToTray = 2,
    CloseToTray = 4,
    Foreground = 8,
    Background = 16,
    SupportsTransparency = 32,
    LoadAtStartup = 64
  }
}
