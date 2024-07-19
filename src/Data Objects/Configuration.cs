using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using GameManager.Data_Objects;
using GameManager.Enums;
using static NtsLib.Utils;
using static GameManager.Base.Utils;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameManager.Data_Objects {
  [Serializable]
  public class Configuration {
    public long Flags { get; set; } = 0;
    public int Port { get; set; } = 5012;
    public int ConnectionTimeout { get; set; } = 1000;
    public string Server { get; set; } = "";
    public string Name { get; set; } = "";
    public List<AppColor> Colors { get; set; } = new List<AppColor>();
    public List<Game> Games { get; set; } = new List<Game>();
    public List<Client> Clients { get; set; } = new List<Client>();
    public IPAddress IP { get; set; } = null;
    public (ColorProfile Active, ColorProfile Inactive) ColorProfiles { get; set; } = (new ColorProfile(), new ColorProfile());

    public AppColor GetAppColor(Control ctrl) {
      if (ctrl == null || ctrl.Tag == null) return null;

      return Colors.FirstOrDefault(x => x.Name.Matches(ctrl.Tag.ToString()));
    }

    public AppColor GetAppColor(string name) {
      return name.IsEmpty() ? null : Colors.FirstOrDefault(x => x.Name.Matches(name));
    }

    public bool HasFlag(Flags flag) {
      return ((long)flag & Flags) == (long)flag;
    }

    public void SetFlag(Flags flag, bool status = true) {
      if (status) {
        if (HasFlag(flag)) return;

        Flags += (long)flag;
        SetConfiguration();
        return;
      }

      if (!HasFlag(flag)) return;

      Flags -= (long)flag;
      SetConfiguration();
    }

    public Game GetGame(string name) {
      try {
        return Games.Where(x => x.Name.Matches(name)).FirstOrDefault();
      } catch (Exception ex) {
        Dialog("** Exception during GetGame:\n\n" + ex.Message);
        return null;
      }
    }

    public void DeleteGame(Game game) {
      try {
        Games.Remove(game);
      } catch (Exception ex) {
        Dialog("** Exception during DeleteGame:\n\n" + ex.Message);
      }
    }

    public void SetGame(Game game) {
      try {
        int index = Games.FindIndex(x => x.Id.Matches(game.Id));
        if (index == -1) {
          Games.Add(game);
          return;
        }

        Games[index] = game;
      } catch (Exception ex) {
        Dialog("** Exception during SetGame:\n\n" + ex.Message);
      }
    }

    public void SetDefaultIP() {
      try {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        IP = host.AddressList.FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
      } catch (Exception ex) {
        Dialog("** Exception during SetDefaultIP:\n\n" + ex.Message);
      }
    }

    public Client GetClient(string name) {
      return Clients.FirstOrDefault(x => x.Name.Matches(name));
    }

    public bool GetClientStatus(string clientname) {
      Client client = GetClient(clientname);
      return client != null ? GetClientStatus(client) : false;
    }

    public bool GetClientStatus(Client client) {
      if (client == null) return false;

      Client clt = Clients.FirstOrDefault(x => x.Name.Matches(client.Name));
      return clt?.Online ?? client.Online;
    }

    public void SetClientStatus(string clientname, bool status) {
      Client client = GetClient(clientname);
      if (client != null) SetClientStatus(client, status);
    }

    public void SetClientStatus(Client client, bool status) {
      if (client == null) return;

      Client clt = Clients.FirstOrDefault(x => x.Name.Matches(client.Name));
      if (clt == null) return;

      clt.Online = status;
    }

    public void DeleteClient(Client client) {
      try {
        Clients.Remove(client);
      } catch (Exception ex) {
        Dialog("** Exception during DeleteClient:\n\n" + ex.Message);
      }
    }

    public void SetClient(Client client) {
      try {
        int index = Clients.FindIndex(x => x.Name.Matches(client.Name));
        if (index == -1) {
          Clients.Add(client);
          return;
        }

        Clients[index] = client;
      } catch (Exception ex) {
        Dialog("** Exception during SetClient:\n\n" + ex.Message);
      }
    }

    public Configuration Clone() {
      using (var ms = new MemoryStream()) {
        var formatter = new BinaryFormatter();
        formatter.Serialize(ms, this);
        ms.Position = 0;
        return (Configuration)formatter.Deserialize(ms);
      }
    }
  }
}
