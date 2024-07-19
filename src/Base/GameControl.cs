using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameManager.Data_Objects;
using static NtsLib.Utils;
using static GameManager.Base.Utils;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace GameManager.Base {
  public static class GameControl {

    public static void Launch(Game game, string server = "", string password = "", string port = "") {
      try {
        if (game == null) {
          Dialog("Cannot launch game; no game was passed to the Launch method!");
          return;
        }

        string exe = game.Executable;
        string dir = game.WorkingDirectory.IsEmpty() ? Path.GetDirectoryName(game.Executable) : game.WorkingDirectory;
        string baseargs = game.Arguments;
        string joinargs = game.ServerJoinArguments;

        baseargs = baseargs.Replace("%client%", Environment.MachineName);
        joinargs = joinargs.Replace("%server%", server);
        joinargs = joinargs.Replace("%ip%", server);
        joinargs = joinargs.Replace("%port%", port);
        joinargs = joinargs.Replace("%base%", game.Arguments);

        if (password.IsEmpty()) {
          joinargs = Regex.Replace(joinargs, @"\[\[.+?\]\]", "");
        } else {
          joinargs = Regex.Replace(joinargs, @"\[\[(.+?)\]\]", "$1");
          joinargs = joinargs.Replace("%password%", password);
        }

        if (port.IsEmpty()) {
          joinargs = Regex.Replace(joinargs, @"\(\(.+?\)\)", "");
        } else {
          joinargs = Regex.Replace(joinargs, @"\(\((.+?)\)\)", "$1");
        }

        if (!File.Exists(exe)) {
          Dialog("Cannot launch game; executable is missing!\n\n[" + exe + "]");
          return;
        }
        if (!Directory.Exists(dir)) {
          Dialog("Cannot launch game; working directory is missing!\n\n[" + dir + "]");
          return;
        }

        Process proc = new Process();
        proc.StartInfo.FileName = exe;
        proc.StartInfo.WorkingDirectory = dir;
        proc.StartInfo.Arguments = server.IsEmpty() ? baseargs : joinargs;
        proc.Start();
      } catch (Exception ex) {
        Dialog("** Exception during Launch:\n\n" + ex.Message);
      }
    }

    public static void Terminate(Game game) {
      try {
        if (game == null) return;

        foreach (Process process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(game.Executable))) {
          try {
            process.Kill();
          } catch {
          }
        }
      } catch (Exception ex) {
        Dialog("** Exception during Terminate:\n\n" + ex.Message);
      }
    }
  }
}
