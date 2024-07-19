using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameManager.Base;

namespace GameManager {
  static class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args) {
      if (Utils.IsRunning()) {
        NtsLib.Utils.Dialog("You cannot have more than one instance of GameManager running!");
        Environment.Exit(1);
        return;
      }

      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);

      Utils.Init(args);
      Application.Run(new Primary());
    }
  }
}
