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

namespace GameManager.UI {
  public partial class ClientDebug : UserControl {
    private Client clientObj = null;

    public ClientDebug(Client client) {
      InitializeComponent();
      clientObj = client;
    }

    private void ClientDebug_Load(object sender, EventArgs e) {
      client.Text = clientObj.Name;
      ip.Text = clientObj.Address;
      foreach (Game g in clientObj.Games) {
        PictureBox pb = new PictureBox();
        pb.Size = new Size(32, 32);
        pb.Image = g.Icon;
        games.Controls.Add(pb);
      }
    }
  }
}
