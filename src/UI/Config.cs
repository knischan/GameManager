using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameManager.Data_Objects;
using GameManager.Base;
using static NtsLib.Utils;
using static GameManager.Base.Utils;
using GameManager.Enums;
using System.Net;
using System.Net.Sockets;
using WindowsFirewallHelper;
using System.Diagnostics;

namespace GameManager.UI {
  public partial class Config : Form {
    public Configuration config;
    private Game activeGame = null;
    private Client activeClient = null;
    #pragma warning disable 0162

    public Config(Configuration config) {
      InitializeComponent();
      this.config = config;
    }

    #region Form Stuff

    private void Config_Load(object sender, EventArgs e) {
      Icon = Properties.Resources.atari;
      LoadGeneral();
      SetupForm();
      AddFirewallRule();
      ActiveControl = genName;
    }

    private void Config_KeyDown(object sender, KeyEventArgs e) {
      switch (e.KeyCode) {
      case Keys.G:
        if (!e.Alt) return;

        e.Handled = e.SuppressKeyPress = true;
        tabControl1.SelectedIndex = 1;
        break;

      case Keys.C:
        if (!e.Alt) return;

        e.Handled = e.SuppressKeyPress = true;
        tabControl1.SelectedIndex = 2;
        break;

      case Keys.S:
        if (e.Alt) {
          switch (tabControl1.SelectedIndex) {
          case 1:
            e.Handled = e.SuppressKeyPress = true;
            ScanDrives();
            break;
          }
          return;
        }

        if (!e.Control) return;
        e.Handled = e.SuppressKeyPress = true;

        switch (tabControl1.SelectedIndex) {
        case 1:
          if (activeGame == null) {
            SaveConfig();
            return;
          }

          SaveGame();
          break;

        case 2:
          if (activeClient == null) {
            SaveConfig();
            return;
          }

          SaveClient();
          break;

        default:
          SaveConfig();
          break;
        }
        break;

      case Keys.D:
        if (!e.Alt) return;
        e.Handled = e.SuppressKeyPress = true;

        switch (tabControl1.SelectedIndex) {
        case 1:
          if (activeGame == null) return;

          DeleteGame();
          break;

        case 2:
          if (activeClient == null) return;

          DeleteClient();
          break;
        }
        break;

      case Keys.Escape:
        games.SelectedIndex = -1;
        break;
      }
    }

    private void tabControl1_SelectedIndexChanged(object sender, EventArgs e) {
      switch (tabControl1.SelectedIndex) {
      case 0:
        break;
      }
    }

    private void saveConfigurationToolStripMenuItem_Click(object sender, EventArgs e) {
      SaveConfig();
    }

    private void cancelToolStripMenuItem_Click(object sender, EventArgs e) {
      DialogResult = DialogResult.Cancel;
      Close();
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
      Close();
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
      return TabCompleteHandler(this, msg, keyData) ? true : base.ProcessCmdKey(ref msg, keyData);
    }

    private void gameExe_Leave(object sender, EventArgs e) {
      LoadIcon();
    }

    private void genColMode_SelectedIndexChanged(object sender, EventArgs e) {
      PopulateColor();
    }

    private void colorSelectorChanged_SelectedIndexChanged(object sender, EventArgs e) {
      UpdateColorPanel();
    }

    private void btnGenColSave_Click(object sender, EventArgs e) {
      SaveColorConfig();
    }

    private void btnGenColCancel_Click(object sender, EventArgs e) {
      genColMode.SelectedIndex = -1;
    }

    #endregion

    #region Miscellaneous

    private void AddFirewallRule() {
      try {
        return;
        if (FirewallManager.Instance.Rules.Any(x => x.Name.Matches("NTS GameManager"))) return;

        var rule = FirewallManager.Instance.CreatePortRule(
          @"NTS GameManager",
          FirewallAction.Allow,
          5690,
          FirewallProtocol.TCP
        );
        FirewallManager.Instance.Rules.Add(rule);
      } catch (Exception ex) {
        Dialog("** Exception during AddFirewallRule:\n\n" + ex.Message);
      }
    }

    private void SetupForm() {
      foreach (ComboBox cb in new ComboBox[] { genColBgPnl, genColFgPcName, genColBgPcName, genColFgIpAddy, genColBgIpAddy }) {
        cb.Items.Clear();
        foreach (string colorName in Enum.GetNames(typeof(KnownColor)).OrderBy(name => name)) cb.Items.Add(colorName);
      }

      games.Items.Clear();
      games.Items.Add("[ NEW ]");
      foreach (Game game in config.Games.OrderBy(x => x.Name)) games.Items.Add(game.Name);

      clients.Items.Clear();
      clients.Items.Add("[ NEW ]");
      foreach (Client client in config.Clients.OrderBy(x => x.Name)) clients.Items.Add(client.Name);
    }

    public void SaveConfig() {
      if (!SaveGeneral()) return;

      new Dialog("Saved!").Show();
      DialogResult = DialogResult.OK;
      SetConfiguration(config);
      Close();
    }

    private void PopulateColor() {
      try {
        switch (genColMode.SelectedIndex) {
        case -1:
          ControlToggle(genColBgPnl, null);
          ControlToggle(genColFgIpAddy, null);
          ControlToggle(genColBgIpAddy, null);
          ControlToggle(genColFgPcName, null);
          ControlToggle(genColBgPcName, null);
          ControlToggle(btnGenColSave, null);
          ControlToggle(btnGenColCancel, null);
          break;

        default:
          ColorProfile cp = genColMode.SelectedIndex == 0 ? config.ColorProfiles.Active : config.ColorProfiles.Inactive;
          ControlToggle(genColBgPnl, cp.PanelBackground.Name, true);
          ControlToggle(genColFgIpAddy, cp.IpAddressForeground.Name, true);
          ControlToggle(genColBgIpAddy, cp.IpAddressBackground.Name, true);
          ControlToggle(genColFgPcName, cp.IpAddressForeground.Name, true);
          ControlToggle(genColBgPcName, cp.IpAddressBackground.Name, true);
          ControlToggle(btnGenColSave, null, true);
          ControlToggle(btnGenColCancel, null, true);
          break;
        }
        UpdateColorPanel();
      } catch (Exception ex) {
        Dialog("** Exception during PopulateColor:\n\n" + ex.Message);
      }
    }

    private void UpdateColorPanel() {
      try {
        genClientPanel.BackColor = GetColor(genColBgPnl.Text, Color.DarkSlateGray);
        genPanelPC.ForeColor = GetColor(genColFgPcName.Text, Color.Black);
        genPanelPC.BackColor = GetColor(genColBgPcName.Text, Color.Transparent);
        genPanelIp.ForeColor = GetColor(genColFgIpAddy.Text, Color.Black);
        genPanelIp.BackColor = GetColor(genColBgIpAddy.Text, Color.Transparent);
      } catch (Exception ex) {
        Dialog("** Exception during UpdateColorPanel:\n\n" + ex.Message);
      }
    }

    private void SaveColorConfig() {
      try {
        if (genColMode.SelectedIndex == -1) return;
        foreach (ComboBox cb in new ComboBox[] { genColBgPnl, genColFgIpAddy, genColBgIpAddy, genColFgPcName, genColBgPcName }) {
          if (cb.SelectedIndex > -1) continue;

          Dialog("All colors need to be selected!");
          return;
        }

        ColorProfile cp = genColMode.SelectedIndex == 0 ? config.ColorProfiles.Active: config.ColorProfiles.Inactive;
        cp.PanelBackground = GetColor(genColBgPnl.Text);
        cp.PcNameForeground = GetColor(genColFgPcName.Text);
        cp.PcNameBackground = GetColor(genColBgPcName.Text);
        cp.IpAddressForeground = GetColor(genColFgIpAddy.Text);
        cp.IpAddressBackground = GetColor(genColBgIpAddy.Text);
        genColMode.SelectedIndex = -1;
      } catch (Exception ex) {
        Dialog("** Exception during SaveColorConfig:\n\n" + ex.Message);
      }
    }

    #endregion

    #region Tab: General

    private void LoadGeneral() {
      genAutoHide.Checked = config.Flags.HasFlag(Flags.AutoHideOnOpen);
      genCloseTray.Checked = config.Flags.HasFlag(Flags.CloseToTray);
      genMinTray.Checked = config.Flags.HasFlag(Flags.MinimizeToTray);
      genName.Text = config.Name.IsEmpty() ? Environment.MachineName.ToUpper() : config.Name;
      genStartup.Checked = config.Flags.HasFlag(Flags.LoadAtStartup);
      LoadBindIPs();
      genClientPanel.BackColor = config.ColorProfiles.Inactive?.PanelBackground ?? Color.DarkSlateGray;
      genPanelPC.Text = Environment.MachineName.ToUpper();
      foreach (Game game in config.Games.OrderBy(x => x.Name).Take(15)) genPanelFLP.Controls.Add(game.GetIcon());
    }

    private bool SaveGeneral() {
      config.Flags = 0;
      config.Flags = config.Flags.SetFlag(Flags.AutoHideOnOpen, genAutoHide.Checked);
      config.Flags = config.Flags.SetFlag(Flags.CloseToTray, genCloseTray.Checked);
      config.Flags = config.Flags.SetFlag(Flags.MinimizeToTray, genMinTray.Checked);
      config.Flags = config.Flags.SetFlag(Flags.LoadAtStartup, genStartup.Checked);
      config.Name = genName.Text.Trim().ToUpper();
      if (bindIP.SelectedIndex > -1) {
        foreach (IPAddress ip in GetIPs(AddressFamily.InterNetwork)) {
          if (!bindIP.Text.Matches(ip.ToString())) continue;

          config.IP = ip;
          break;
        }
      }

      SetConfiguration(config);

      if (genStartup.Checked) {
        var result = AddApplicationToStartup("GameManager", Process.GetCurrentProcess().MainModule.FileName);
        if (result.Failed) Dialog("Failed to add GameManager to the startup!\n\n" + result.Message);
      }
      return true;
    }

    #endregion

    #region Tab: Games

    private void games_SelectedIndexChanged(object sender, EventArgs e) {
      switch (games.SelectedIndex) {
      case -1:
        activeGame = null;
        ActiveControl = games;
        break;

      case 0:
        activeGame = new Game();
        break;

      default:
        activeGame = config.GetGame(games.Text);
        if (activeGame == null) {
          Dialog("Could not load this game record!");
          games.SelectedIndex = -1;
          return;
        }
        break;
      }

      PopulateGame();
    }

    private void btnGameSave_Click(object sender, EventArgs e) {
      SaveGame();
    }

    private void btnPush_Click(object sender, EventArgs e) {
      PushSettings();
    }

    private void btnGameDelete_Click(object sender, EventArgs e) {
      DeleteGame();
    }

    private void btnGameCancel_Click(object sender, EventArgs e) {
      games.SelectedIndex = -1;
    }

    private void gameIconOverride_Leave(object sender, EventArgs e) {
      LoadIcon();
    }

    private void btnGameScan_Click(object sender, EventArgs e) {
      ScanDrives();
    }

    private void PopulateGame() {
      ControlToggle(gameName, activeGame?.Name ?? "", activeGame != null);
      ControlToggle(gameExe, activeGame?.Executable ?? "", activeGame != null);
      ControlToggle(gameProcess, activeGame?.ProcessName ?? "", activeGame != null);
      ControlToggle(gameArgs, activeGame?.Arguments ?? "", activeGame != null);
      ControlToggle(gameJoinServer, activeGame?.ServerJoinArguments ?? "", activeGame != null);
      ControlToggle(gameIconOverride, "", activeGame != null);
      ControlToggle(gameWorkDir, activeGame?.WorkingDirectory ?? "", activeGame != null);
      ControlToggle(gameIcon, activeGame?.Icon ?? null, activeGame != null);
      btnGameSave.Enabled = activeGame != null;
      btnGameDelete.Enabled = activeGame != null && !activeGame.Name.IsEmpty();
      btnGameCancel.Enabled = activeGame != null;
      ActiveControl = gameName;
    }

    private void DeleteGame() {
      if (activeGame == null) return;
      if (GetDialogButton("Are you sure?", "Yes,No").Matches("no")) return;

      config.DeleteGame(activeGame);
      games.SelectedIndex = -1;
      SetupForm();
    }

    private void SaveGame() {
      if (!ValidateControl(gameName, "val")) {
        Dialog("Name must be provided!");
        ActiveControl = gameName;
        return;
      }
      if (!ValidateControl(gameExe, "val")) {
        Dialog("Executable must be provided!");
        ActiveControl = gameExe;
        return;
      }

      Game tmp = config.GetGame(gameName.Text);
      if (tmp != null && !tmp.Id.Matches(activeGame.Id)) {
        Dialog("Cannot save; that name is in use!");
        ActiveControl = gameName;
        return;
      }

      if (!gameIconOverride.IsEmpty() && !File.Exists(gameIconOverride.Text)) {
        Dialog("The specified icon is not valid!");
        ActiveControl = gameIconOverride;
        return;
      }

      activeGame.Name = gameName.Text.Trim();
      activeGame.Executable = gameExe.Text.Trim();
      activeGame.ProcessName = gameProcess.Text.Trim();
      activeGame.Arguments = gameArgs.Text.Trim();
      activeGame.ServerJoinArguments = gameJoinServer.Text.Trim();
      activeGame.Icon = gameIcon.Image;
      activeGame.WorkingDirectory = gameWorkDir.Text.Trim();
      config.SetGame(activeGame);
      games.SelectedIndex = -1;
      SetupForm();
    }

    private void LoadIcon() {
      gameIcon.Image = null;
      if (activeGame == null) return;

      if (!gameIconOverride.Text.IsEmpty()) {
        Image tmp = ExtractIconToImage(gameIconOverride.Text.Trim());
        if (tmp == null) {
          Dialog("Could not import icon from override file!");
          return;
        }

        activeGame.Icon = tmp;
      }

      if (activeGame.Icon != null) {
        gameIcon.Image = activeGame.Icon;
        return;
      }
      if (gameExe.IsEmpty()) return;

      gameIcon.Image = ExtractIconToImage(gameExe.Text.Trim());
    }

    private void LoadBindIPs() {
      try {
        bindIP.Items.Clear();
        foreach (IPAddress ip in GetIPs(AddressFamily.InterNetwork)) {
          bindIP.Items.Add(ip.ToString());
        }
        bindIP.SelectedIndex = config.IP == null ? -1 : bindIP.FindString(config.IP.ToString());
      } catch (Exception ex) {
        Dialog("** Exception during LoadBindIPs:\n\n" + ex.Message);
      }
    }

    private void ScanDrives() {
      try {
        string detectorFile = Path.Combine(Application.StartupPath, "gamedetector.json");
        if (!File.Exists(detectorFile)) {
          if (GetDialogButton("Cannot scan; the game signature file is not present!\n\nShould I generate a default one?", "Yes,No").Matches("no")) return;

          try {
            File.WriteAllText(detectorFile, Properties.Resources.gamedetection);
          } catch (Exception ex) {
            Dialog("Could not generate file!\n\n" + ex.Message);
            return;
          }
        }

        Dialog("OK, I'll scan your drives to search for games.\n\nThis may take a minute, please wait for the next dialog to pop up.");
        GameDetector gd = new GameDetector(detectorFile);
        List<string> results = gd.Scan();
        string newgames = "";
        int added = 0;
        foreach (Game game in gd.Games) {
          if (config.Games.Any(x => x.Name.Matches(game.Name))) continue;

          added++;
          newgames += (newgames.IsEmpty() ? "" : Environment.NewLine) + game.Name;
          config.Games.Add(game);
        }
        if (added == 0) {
          Dialog(Pluralize("!# game!s !ww detected, but none were new.", results.Count));
          return;
        }

        Dialog(Pluralize("Detected !# game!s, ", results.Count) + Pluralize("!# !ww new:", added) + Environment.NewLine + Environment.NewLine + newgames);
        SetupForm();
      } catch (Exception ex) {
        Dialog("** Exception during ScanDrives:\n\n" + ex.Message);
      }
    }

    private void PushSettings() {
      Game game = activeGame.Clone();
      game.Name = gameName.Text.Trim();
      game.Executable = gameExe.Text.Trim();
      game.WorkingDirectory = gameWorkDir.Text.Trim();
      game.Arguments = gameArgs.Text.Trim();
      game.ServerJoinArguments = gameJoinServer.Text.Trim();
      game.ProcessName = gameProcess.Text.Trim();

      string sentTo = "";
      foreach (Client client in config.Clients) {
        if (!client.Games.Any(x => x.Name.Matches(game.Name))) continue;

        Payload payload = new Payload(SystemCommand.GameUpdate);
        payload.RecipientAddress = client.Address;
        payload.RecipientName = client.Name;
        payload.Games.Add(game);
        payload.Transmit(client.Address);
        sentTo += (sentTo.IsEmpty() ? "" : Environment.NewLine) + client.Name;
      }

      Dialog("This game has been pushed to the following clients for update:" + Environment.NewLine + Environment.NewLine + sentTo);
    }

    #endregion

    #region Tab: Clients

    private void clients_SelectedIndexChanged(object sender, EventArgs e) {
      switch (clients.SelectedIndex) {
      case -1:
        activeClient = null;
        break;

      case 0:
        activeClient = new Client();
        break;

      default:
        activeClient = config.GetClient(clients.Text);
        if (activeClient == null) {
          Dialog("Could not load client!");
          clients.SelectedIndex = -1;
          return;
        }

        break;
      }
      PopulateClient();
    }

    private void btnCltSave_Click(object sender, EventArgs e) {
      SaveClient();
    }

    private void btnCltDelete_Click(object sender, EventArgs e) {
      DeleteClient();
    }

    private void btnCltCancel_Click(object sender, EventArgs e) {
      clients.SelectedIndex = -1;
    }

    private void cltGames_SelectedIndexChanged(object sender, EventArgs e) {
      LoadClientGameIcon();
    }

    private void SaveClient() {
      if (!ValidateControl(cltName, "val")) {
        Dialog("There must be a system name!");
        ActiveControl = cltName;
        return;
      }
      if (!ValidateControl(cltAddress, "val")) {
        Dialog("Client address cannot be empty!");
        ActiveControl = cltAddress;
        return;
      }

      IPAddress ip = GetIPAddress(cltAddress.Text);
      if (ip == null) {
        Dialog("That doesn't appear to be a valid IP!");
        ActiveControl = cltAddress;
        return;
      }

      activeClient.Name = cltName.Text.Trim();
      activeClient.Address = cltAddress.Text.Trim();
      config.SetClient(activeClient);
      clients.SelectedIndex = -1;
    }

    private void DeleteClient() {
      if (activeClient == null) return;
      if (GetDialogButton("Are you sure?", "Yes,No").Matches("no")) return;

      config.DeleteClient(activeClient);
      clients.SelectedIndex = -1;
      SetupForm();
    }

    private void PopulateClient() {
      ControlToggle(cltName, activeClient?.Name ?? "", activeClient != null);
      ControlToggle(cltAddress, activeClient?.Address ?? "", activeClient != null);
      ControlToggle(cltGames, activeClient?.Games.Select(x => x.Name).ToList() ?? new List<string>(), activeClient != null);
      cltOnline.Image = activeClient == null ? Properties.Resources.chk_off : (activeClient.Online ? Properties.Resources.chk_1 : Properties.Resources.chk_0);
      btnCltSave.Enabled = activeClient != null;
      btnCltDelete.Enabled = activeClient != null && !activeClient.Name.IsEmpty();
      btnCltCancel.Enabled = activeClient != null;
    }

    private void LoadClientGameIcon() {
      cltGameIcon.Image = null;
      if (activeClient == null) return;
      if (cltGames.SelectedIndex == -1) return;

      Game tmpGame = activeClient.Games.FirstOrDefault(x => x.Name.Matches(cltGames.Text));
      if (tmpGame == null) return;
      cltGameIcon.Image = tmpGame.Icon;
    }

    #endregion

  }
}