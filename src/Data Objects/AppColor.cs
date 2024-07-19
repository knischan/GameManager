using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NtsLib.Utils;
using GameManager.Enums;
using System.Windows.Forms;

namespace GameManager.Data_Objects {
  [Serializable]
  public class AppColor {
    public Guid Id;
    public string Name { get; set; } = "";
    public long Flags { get; set; } = 0;
    public Color Foreground { get; set; } = Color.Black;
    public Color Background { get; set; } = Color.Transparent;

    public override bool Equals(object obj) {
      return obj is AppColor? ((AppColor)obj).Id == Id : false;
    }

    public override int GetHashCode() {
      return Id.GetHashCode();
    }

    public AppColor() {
      Flags = Flags.SetFlag(Enums.Flags.Foreground, true);
      Flags = Flags.SetFlag(Enums.Flags.Background, true);
    }

    public AppColor(string Name) {
      this.Name = Name;
      Flags = Flags.SetFlag(Enums.Flags.Foreground, true);
      Flags = Flags.SetFlag(Enums.Flags.Background, true);
    }

    public AppColor(string Name, Color Foreground, Color Background, bool supportsTransparency = true) {
      Id = Guid.NewGuid();
      this.Name = Name;
      this.Foreground = Foreground;
      this.Background = Background;
      Flags = Flags.SetFlag(Enums.Flags.Foreground, true);
      Flags = Flags.SetFlag(Enums.Flags.Background, true);
      Flags = Flags.SetFlag(Enums.Flags.SupportsTransparency, supportsTransparency);
    }

    public void Apply(Control ctrl) {
      if (Flags.HasFlag(Enums.Flags.Foreground)) ctrl.ForeColor = Foreground;
      if (!Flags.HasFlag(Enums.Flags.Background)) return;

      ctrl.BackColor = Flags.HasFlag(Enums.Flags.SupportsTransparency) ? Background : (Background == Color.Transparent ? Color.FromKnownColor(KnownColor.Control) : Background);
    }
  }
}
