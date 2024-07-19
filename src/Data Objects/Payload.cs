using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameManager.Enums;
using static NtsLib.Utils;
using static GameManager.Base.Utils;
using System.Net;

namespace GameManager.Data_Objects {
  [Serializable]
  public class Payload {
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Port { get; set; } = 5012;
    public string ClientId { get; set; } = "";
    public string RecipientName { get; set; } = "";
    public string RecipientAddress { get; set; } = "";
    public string SenderName { get; set; } = "";
    public string SenderAddress { get; set; } = "";
    public string ServerPort { get; set; } = "";
    public string Game { get; set; } = "";
    public string JoinServer { get; set; } = "";
    public string ServerPassword { get; set; } = "";
    public SystemCommand Command { get; set; } = SystemCommand.None;
    public List<Game> Games { get; set; } = new List<Game>();

    public Payload(SystemCommand Command) {
      this.Command = Command;
      SenderName = cfg.Name.IsEmpty() ? Environment.MachineName : cfg.Name;
      SenderAddress = GetCurrentIP()?.ToString() ?? "";
    }
  }
}
