using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GameManager.Data_Objects {
  [Serializable]
  public class Client {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public bool Online { get; set; } = false;
    public string Address { get; set; } = "";
    public List<Game> Games { get; set; } = new List<Game>();

    public override bool Equals(object obj) {
      return obj is Client client ? Id == client.Id : false;
    }

    public override int GetHashCode() {
      return Id.GetHashCode();
    }

    public Client Clone() {
      using (var ms = new MemoryStream()) {
        var formatter = new BinaryFormatter();
        formatter.Serialize(ms, this);
        ms.Position = 0;
        return (Client)formatter.Deserialize(ms);
      }
    }
  }
}
