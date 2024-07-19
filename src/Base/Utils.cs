using System;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Json;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Net;
using System.Reflection;
using System.Media;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using GameManager.Data_Objects;
using GameManager.Enums;
using GameManager.UI;
using static NtsLib.Utils;
using static GameManager.Base.Utils;

namespace GameManager.Base {
  public static class Utils {
    public static Configuration cfg;
    public static Primary formUI = null;
    public static SocketServer server = null;
    public static string activeConfigPath = "";
    public static string activeConfig = "GameManager.cfg";
    public static bool debugMode = false;

    public static void Init(string[] args) {
      try {
        activeConfigPath = Path.Combine(Environment.GetEnvironmentVariable("APPDATA"), "Nischan Technology Solutions", "GameManager");
        for (int x = 0; x < args.Length; x++) {
          switch (args[x].ToLower()) {
          case "/cfg":
            if (x == args.Length - 1) {
              new Dialog("You need to pass the config file after /cfg!").Show();
              Environment.Exit(1);
              return;
            }

            x++; activeConfigPath = args[x];
            activeConfig = Path.GetFileName(args[x]);
            if (!Path.GetDirectoryName(args[x]).IsEmpty()) activeConfigPath = Path.GetDirectoryName(args[x]);
            break;
          }
        }

        try {
          if (!Directory.Exists(activeConfigPath)) Directory.CreateDirectory(activeConfigPath);
        } catch (Exception ex) {
          Dialog("I ran into a problem creating the application data folder (" + activeConfigPath + "):" + Environment.NewLine + Environment.NewLine + ex.Message);
          Environment.Exit(1);
          return;
        }

        if (!File.Exists(Path.Combine(activeConfigPath, activeConfig))) {
          new Dialog("No configuration file was found.  Please configure the product before use.").Show();
          using (Config config = new Config(GetDefaultConfiguration())) {
            if (config.ShowDialog() == DialogResult.Cancel) ExitError("Configuration was aborted; cancelling");

            SetConfiguration(config.config);
            if (!File.Exists(Path.Combine(activeConfigPath, activeConfig))) ExitError("The configuration file failed to generate!");
          }
        }
        cfg = Deserialize<Configuration>(Path.Combine(activeConfigPath, activeConfig));
        try {
          server = new SocketServer(5012);
          server.Start();
        } catch (Exception ex) {
          Dialog("** Exception during server initialization: " + ex.Message);
          Environment.Exit(1);
        }
      } catch (Exception ex) {
        Dialog("Exception during Init: " + ex.Message);
      }
    }

    public static Configuration GetConfiguration() {
      try {
        if (!File.Exists(Path.Combine(activeConfigPath, activeConfig))) {
          return new Configuration();
        }

        return Deserialize<Configuration>(Path.Combine(activeConfigPath, activeConfig));
      } catch (Exception ex) {
        new Dialog("Exception during GetConfiguration: " + ex.Message).Show();
        return null;
      }
    }

    public static void SetConfiguration() {
      SetConfiguration(cfg);
    }

    public static void SetConfiguration(Configuration config) {
      try {
        Serialize(Path.Combine(activeConfigPath, activeConfig), config);
        cfg = config.Clone();
      } catch (Exception ex) {
        new Dialog("Exception during SetConfiguration:" + ex.Message).Show();
      }
    }

    public static async void Transmit(this Payload payload, string ipaddr = null) {
      try {
        await server.TransmitAsync(payload, ipaddr);
      } catch (Exception ex) {
        Dialog("** Exception during Transmit:\n\n" + ex.Message);
      }
    }

    public static Payload CreatePayload(SystemCommand command) {
      Payload payload = new Payload(command);
      payload.Port = cfg.Port;
      payload.SenderName = Environment.MachineName;
      return payload;
    }

    public static void PlayResource(string resourceName, int SoundFlags = 0) {
      try {
        Assembly executingAssembly = Assembly.GetExecutingAssembly();
        string resourceNamespace = executingAssembly.GetName().Name;
        System.Resources.ResourceManager myManager = new System.Resources.ResourceManager(resourceNamespace + ".Properties.Resources", executingAssembly);
        var resource = myManager.GetObject("copied");
        if (resource is Stream stream) {
          using (SoundPlayer player = new SoundPlayer(stream)) {
            player.Play();
          }
        }
      } catch (Exception ex) {
        Form myform = new Form();
        TextBox tb = new TextBox();
        tb.Text = ex.Message;
        myform.Controls.Add(tb);
        myform.ShowDialog();
      }
    }

    public static string GetVersionNumber(bool includeInsignificants = false) {
      Assembly assembly = Assembly.GetExecutingAssembly();
      FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
      string fileVersion = versionInfo.FileVersion;

      if (!includeInsignificants) {
        Version version = new Version(fileVersion);
        fileVersion = $"{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
      }

      return fileVersion;
    }

    public static IPAddress GetCurrentIP() {
      if (cfg.IP != null) return cfg.IP;

      IPAddress[] ips = GetIPFromHostname(Environment.MachineName);
      foreach (IPAddress ip in ips) {
        string[] tmp = ip.ToString().Split('.');
        if (tmp[0] == "10") return ip;
      }
      return null;
    }

    public static void ExitError(string msg, int exitLevel = 1) {
      Dialog(msg);
      Environment.Exit(exitLevel);
      Application.Exit();
    }

    public static Configuration GetDefaultConfiguration() {
      Configuration config = SetupInitialColorScheme(new Configuration());
      return config;
    }

    public static Configuration SetupInitialColorScheme(Configuration config) {
      AppColor formprimary = new AppColor("Primary: Form", Color.Black, Color.DarkSlateBlue, false); formprimary.Flags = (long)Flags.Background;
      AppColor menuprimary = new AppColor("Primary: Menu", Color.Black, Color.FromKnownColor(KnownColor.Control));
      AppColor panelprimary = new AppColor("Primary: Panel", Color.Black, Color.AliceBlue); panelprimary.Flags = (long)Flags.Background;
      AppColor activity_label = new AppColor("Primary: Activity Label", Color.Black, Color.Transparent);
      AppColor activity = new AppColor("Primary: Activity", Color.Black, Color.Ivory, false);
      AppColor canvas = new AppColor("Image Canvas Background", Color.Black, Color.Black, false); canvas.Flags = (long)Flags.Background;
      config.Colors.Clear();
      config.Colors.AddRange(new AppColor[] { formprimary, menuprimary, panelprimary, activity_label, activity, canvas });
      return config;
    }
    
    public static bool IsRunning() {
      try {
        return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Count(p => p.Id != Process.GetCurrentProcess().Id) > 0;
      } catch (Exception ex) {
        Dialog("** Exception during IsRunning:\n\n" + ex.Message);
        return false;
      }
    }

    public static void LogUI(string msg) {
      try {
        if (formUI == null) return;

        if (formUI.InvokeRequired) {
          formUI.Invoke(new Action(() => {
            formUI.ActivityLog(msg);
          }));
        } else {
          formUI.ActivityLog(msg);
        }
      } catch (Exception ex) {
        Dialog("** Exception during LogUI:\n\n" + ex.Message);
      }
    }
  }
}
