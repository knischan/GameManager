using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManager.Data_Objects {
  [Serializable]
  public class ColorProfile {
    public string Profile { get; set; } = "";
    public Color PanelBackground { get; set; } = Color.DarkSlateGray;
    public Color PcNameForeground { get; set; } = Color.Yellow;
    public Color PcNameBackground { get; set; } = Color.Transparent;
    public Color IpAddressForeground { get; set; } = Color.White;
    public Color IpAddressBackground { get; set; } = Color.Transparent;
  }
}
