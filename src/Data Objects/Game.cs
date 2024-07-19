using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static NtsLib.Utils;

namespace GameManager.Data_Objects {
  [Serializable]
  public class Game {
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "";
    public string Executable { get; set; } = "";
    public string Arguments { get; set; } = "";
    public string ProcessName { get; set; } = "";
    public string ServerJoinArguments { get; set; } = "";
    public string WorkingDirectory { get; set; } = "";
    public Image Icon { get; set; } = null;

    public bool IsRunning() {
      foreach (Process process in Process.GetProcesses()) {
        if (process.ProcessName.ContainsStd(ProcessName)) return true;
      }
      return false;
    }

    public string Kill(bool waitForExit = false) {
      string ret = "";
      try {
        foreach (Process process in Process.GetProcessesByName(ProcessName)) {
          try {
            process.Kill();
            if (waitForExit) process.WaitForExit();
          } catch (Exception ex) {
            ret += "Exception: " + ex.Message + Environment.NewLine;
          }
        }
      } catch (Exception ex) {
        ret += "Exception: " + ex.Message + Environment.NewLine;
      }
      return ret;
    }

    public Image GetIconImage() {
      return Icon == null ? Properties.Resources.unknown : Icon;
    }

    public PictureBox GetIcon() {
      try {
        PictureBox pb = new PictureBox();
        pb.Size = new Size(32, 32);
        pb.Image = Icon;
        pb.BorderStyle = BorderStyle.FixedSingle;
        return pb;
      } catch (Exception ex) {
        Dialog("** Exception during GetIcon:\n\n" + ex.Message);
        return null;
      }
    }

    public Game Clone() {
      using (var ms = new MemoryStream()) {
        var formatter = new BinaryFormatter();
        formatter.Serialize(ms, this);
        ms.Position = 0;
        return (Game)formatter.Deserialize(ms);
      }
    }
  }
}
