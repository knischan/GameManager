using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameManager.Data_Objects;
using static NtsLib.Utils;
using static GameManager.Base.Utils;

namespace GameManager.UI {
  public partial class ClientPanel : UserControl {
    public Client Client { get; set; } = null;
    public bool Selected { get; set; } = false;
    public event ClickEventHandler ClickEvent;
    public delegate void ClickEventHandler(object sender, SystemClickEventArgs e);

    public ClientPanel(Client c) {
      InitializeComponent();
      Client = c;
      Click += Control_Clicked;
      pcname.Click += Control_Clicked;
      ip.Click += Control_Clicked;
      games.Click += Control_Clicked;
      pcname.Text = c.Name;
      ip.Text = c.Address;
      BackColor = cfg.ColorProfiles.Inactive?.PanelBackground ?? Color.DarkSlateGray;
    }

    private void ClientPanel_Load(object sender, EventArgs e) {
      if (Client.Games.Count == 0) {

      } else {
        int flpRows = (Client.Games.Count + 16) / 17;
        games.Height = flpRows * 40;
        Height += (flpRows - 1) * 40;
      }

      foreach (Game game in Client.Games.OrderBy(x => x.Name)) {
        PictureBox pb = game.GetIcon();
        toolTip1.SetToolTip(pb, game.Name);
        games.Controls.Add(pb);
      }
    }

    private void Control_Clicked(object sender, EventArgs e) {
      SetSelected(!Selected);
    }

    public void SetSelected(bool status) {
      Selected = status;
      BackColor = Selected ? cfg.ColorProfiles.Active?.PanelBackground ?? Color.CornflowerBlue : cfg.ColorProfiles.Inactive?.PanelBackground ?? Color.DarkSlateGray;
      ip.ForeColor = Selected ? cfg.ColorProfiles.Active?.IpAddressBackground ?? Color.White : cfg.ColorProfiles.Inactive?.IpAddressBackground ?? Color.White;
      ip.BackColor = Selected ? cfg.ColorProfiles.Active?.IpAddressBackground ?? Color.Transparent : cfg.ColorProfiles.Inactive?.IpAddressBackground ?? Color.Transparent;
      pcname.ForeColor = Selected ? cfg.ColorProfiles.Active?.PcNameBackground ?? Color.Yellow : cfg.ColorProfiles.Inactive?.PcNameBackground ?? Color.Yellow;
      pcname.BackColor = Selected ? cfg.ColorProfiles.Active?.PcNameBackground ?? Color.Transparent : cfg.ColorProfiles.Inactive?.PcNameBackground ?? Color.Transparent;
      var args = new SystemClickEventArgs();
      args.Client = Client;
      args.Panel = this;
      ClickEvent?.Invoke(this, args);
    }

    public class SystemClickEventArgs : EventArgs {
      public ClientPanel Panel { get; set; } = null;
      public Client Client { get; set; } = null;
    }
  }
}
