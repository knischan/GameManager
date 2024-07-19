using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using GameManager.Enums;
using static GameManager.Base.Utils;
using static NtsLib.Utils;

namespace GameManager.Data_Objects {
  public class Packet {
    public string Name { get; set; } = "";
    public IPAddress IP { get; set; } = null;
    public SystemCommand Command { get; set; } = SystemCommand.None;
    public string Payload { get; set; } = "";

    public Packet() {
      Name = Environment.MachineName;
      IP = cfg.IP;
    }
  }
}
