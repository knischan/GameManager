using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GameManager.Data_Objects;
using static NtsLib.Utils;
using static GameManager.Base.Utils;
using Newtonsoft.Json;

namespace GameManager.Base {
  public class GameDetector {
    public List<Game> Games = new List<Game>();
    public List<string> SkipDrives = new List<string>();
    public List<string> RootFolders = new List<string>();
    public dynamic JsonData = null;

    public GameDetector(string jsonfile) {
      try {
        string json = File.ReadAllText(jsonfile);
        JsonData = JsonConvert.DeserializeObject<dynamic>(json);
        if (JsonData == null) return;

        if (JsonData.config?.rootfolders != null) {
          foreach (string folder in JsonData.config.rootfolders) RootFolders.Add(folder);
        }
        if (JsonData.config?.skipdrives != null) {
          foreach (string drive in JsonData.config.skipdrives) SkipDrives.Add(drive.Standardize());
        }
      } catch (Exception ex) {
        Dialog("** Exception during GameDetector initialization:\n\n" + ex.Message);
      }
    }

    public List<string> Scan() {
      List<string> newGames = new List<string>();
      try {
        if (JsonData?.games == null) return newGames;

        foreach (DriveInfo drive in GetLocalDrives()) {
          if (SkipDrives.Contains(drive.Name.Standardize())) continue;

          foreach (string folder in RootFolders) {
            string fullpath = Path.Combine(drive.Name, folder);
            if (!Directory.Exists(fullpath)) continue;

            foreach (var data in JsonData.games) {
              if (data.executable == null) continue;
              if (data.name == null) continue;
              if (!DoesGameMatch(data, fullpath)) continue;

              Game game = new Game();
              game.Name = data.name.ToString();
              if (Games.Any(x => x.Name.Matches(game.Name))) continue;

              game.Executable = Path.Combine(fullpath, data.executable.ToString());
              game.WorkingDirectory = data.workingdirectory?.ToString() ?? "";
              game.Arguments = data.launch?.ToString() ?? "";
              game.ProcessName = data.process?.ToString() ?? "";
              game.ServerJoinArguments = data.join?.ToString() ?? "";
              game.WorkingDirectory = data.folder?.ToString() ?? "";

              string iconfile = data.icon?.ToString() ?? "";
              if (iconfile.IsEmpty()) {
                game.Icon = GetImageFromFile(game.Executable);
              } else { 
                string checkpath = Path.IsPathRooted(iconfile) ? iconfile : Path.Combine(Path.GetDirectoryName(game.Executable), iconfile);
                if (File.Exists(checkpath)) game.Icon = GetImageFromFile(checkpath);
              }

              Games.Add(game);
              newGames.Add(game.Name);
            }
          }
        }
      } catch (Exception ex) {
        Dialog("** Exception during Scan:\n\n" + ex.Message);
      }
      return newGames;
    }

    private bool DoesGameMatch(dynamic game, string gamepath) {
      try {
        if (game?.executable == null) return false;
        if (!File.Exists(Path.Combine(gamepath, game.executable.ToString()))) return false;
        if (game.signature == null) return true;

        List<string> sigFiles = new List<string>();
        List<string> sigFolders = new List<string>();

        foreach (var signature in game.signature) {
          if (signature.file != null) sigFiles.Add(Path.Combine(gamepath, signature.file.ToString()));
          if (signature.folder != null) sigFolders.Add(Path.Combine(gamepath, signature.folder.ToString()));
        }

        foreach (string file in sigFiles) if (!File.Exists(file)) return false;
        foreach (string folder in sigFolders) if (!Directory.Exists(folder)) return false;
        return true;
      } catch (Exception ex) {
        Dialog("** Exception during DoesGameMatch:\n\n" + ex.Message);
        return false;
      }
    }
  }
}
