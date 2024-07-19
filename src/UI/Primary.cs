using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameManager.UI;
using GameManager.Enums;
using GameManager.Data_Objects;
using static NtsLib.Utils;
using static GameManager.Base.Utils;
using GameManager.Base;
using System.Management;
using System.Net;
using System.IO;

namespace GameManager {
  public partial class Primary : Form {
    private Game activeGame = null;
    private FileSystemWatcher exitWatcher = new FileSystemWatcher();
    private bool isConnected = false;
    private bool isJoining = false;
    #pragma warning disable 4014

    public Primary() {
      if (cfg.Flags.HasFlag(Flags.AutoHideOnOpen)) Opacity = 0;
      InitializeComponent();
    }

    #region Form Stuff

    private void Primary_Load(object sender, EventArgs e) {
      formUI = this;
      SetConfiguration();
      Text = "GameManager - " + GetFileVersion();
      Icon = Properties.Resources.atari;
      notifyIcon1.Icon = Properties.Resources.atari;

      server.PayloadReceived += Server_PayloadReceived;
      server.TransmissionTimedOut += Server_TransmissionTimedOut;

      CheckClientStatusAsync();
      SetupForm();
      SetupExitMonitor();
    }

    private void Server_PayloadReceived(object sender, NetworkEventArgs e) {
      if (InvokeRequired) {
        Invoke(new MethodInvoker(() => Server_PayloadReceived(sender, e)));
        return;
      }

      PayloadHandler((Payload)sender, EventType.Payload, e.Client);
    }

    private void Server_TransmissionTimedOut(object sender, NetworkEventArgs e) {
      if (InvokeRequired) {
        Invoke(new MethodInvoker(() => Server_TransmissionTimedOut(sender, e)));
        return;
      }

      PayloadHandler((Payload)sender, EventType.TransmissionFailure, e.Client);
    }

    private void Primary_KeyDown(object sender, KeyEventArgs e) {
      switch (e.KeyCode) {
      case Keys.X:
        if ((e.Modifiers & Keys.Alt) == 0) return;
        Environment.Exit(0);
        break;
      }
    }

    private void Primary_Shown(object sender, EventArgs e) {
      if (cfg.Flags.HasFlag(Flags.AutoHideOnOpen)) {
        Hide();
        showprojectToolStripMenuItem.Text = "Sho&w GameManager";
      }
      Opacity = 100;
    }

    private void Primary_FormClosing(object sender, FormClosingEventArgs e) {
      if (cfg.Flags.HasFlag(Flags.CloseToTray)) {
        if (e.CloseReason != CloseReason.WindowsShutDown) e.Cancel = true;
        if (cfg.Flags.HasFlag(Flags.MinimizeToTray)) {
          showprojectToolStripMenuItem.Text = "Sho&w GameManager";
          Hide();
        } else {
          WindowState = FormWindowState.Minimized;
        }
      } else {
        Exit();
      }
    }

    private void Primary_Resize(object sender, EventArgs e) {
      if (WindowState == FormWindowState.Minimized) {
        showprojectToolStripMenuItem.Text = "Sho&w GameManager";
        if (cfg.Flags.HasFlag(Flags.MinimizeToTray)) Hide();
      }
      if (WindowState == FormWindowState.Normal) {
        showprojectToolStripMenuItem.Text = "&Hide GameManager";
        Show();
      }
    }

    private void ExitWatcher_Created(object sender, FileSystemEventArgs e) {
      if (InvokeRequired) {
        Invoke(new MethodInvoker(() => ExitWatcher_Created(sender, e)));
        return;
      }

      try {
        File.Delete(Path.Combine(exitWatcher.Path, exitWatcher.Filter));
      } catch {
      }

      Exit();
    }

    private void gameTSMI_Click(object sender, EventArgs e) {
      if (!(sender is ToolStripMenuItem)) return;

      ToolStripMenuItem tsmi = (ToolStripMenuItem)sender;
      if (!(tsmi.Tag is Game)) return;

      LaunchGame((Game)tsmi.Tag);
    }

    private void notifyIcon1_DoubleClick(object sender, EventArgs e) {
      ShowUI();
    }

    private void system_ClickEvent(object sender, ClientPanel.SystemClickEventArgs e) {
      ClientClicked(e.Client);
    }

    private void lblServer_Click(object sender, EventArgs e) {
      if (isConnected) return;

      ConnectServer();
    }

    #endregion

    #region Menus

    #region File

    private void connectToServerToolStripMenuItem_Click(object sender, EventArgs e) {
      if (isConnected) {
        DisconnectServer();
        return;
      }

      ConnectServer();
    }

    private async void configurationToolStripMenuItem_Click(object sender, EventArgs e) {
      using (Config c = new Config(cfg.Clone())) {
        if (c.ShowDialog() != DialogResult.OK) return;

        CheckClientStatusAsync();
        SetupForm();
        if (!isConnected) return;

        Payload payload = new Payload(SystemCommand.GamesList);
        payload.SenderName = Environment.MachineName;
        payload.RecipientName = cfg.Server;
        payload.RecipientAddress = cfg.Server;
        payload.Games = cfg.Games;

        await server.TransmitAsync(payload, cfg.Server);
      }
    }

    private void debugModeToolStripMenuItem_Click(object sender, EventArgs e) {
      debugModeToolStripMenuItem.Checked = !debugModeToolStripMenuItem.Checked;
      debugMode = debugModeToolStripMenuItem.Checked;
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
      Exit();
    }

    #endregion

    #region Context

    private void showprojectToolStripMenuItem_Click(object sender, EventArgs e) {
      ShowUI();
    }

    private void contextExitToolStripMenuItem_Click(object sender, EventArgs e) {
      Exit();
    }

    #endregion

    #endregion

    #region Tab: Client

    private void game_SelectedIndexChanged(object sender, EventArgs e) {
      if (game.SelectedIndex == -1) {
        activeGame = null;
      } else {
        ClientPanel selectedClient = systems.Controls.OfType<ClientPanel>().FirstOrDefault(cp => cp.BackColor == (cfg.ColorProfiles.Active?.PanelBackground ?? Color.CornflowerBlue));
        activeGame = selectedClient.Client.Games.Where(x => x.Name.Matches(game.Text)).FirstOrDefault();
      }

      PopulateGame();
      if (activeGame != null) ActiveControl = srvAddress;
    }

    private async void ConnectServer() {
      Dialog dlg = new Dialog("Server address:", "OK,Cancel", "input.default::" + cfg.Server);
      DialogReturn dlr = dlg.Show();
      if (dlr.Button.Matches("cancel")) return;
      if (dlr.Input.IsEmpty()) return;

      if (!IsAlive(dlr.Input)) {
        Dialog("The specified server is not pinging!");
        return;
      }

      Payload payload = new Payload(SystemCommand.JoinNetwork);
      payload.SenderName = Environment.MachineName;
      payload.RecipientName = dlr.Input;
      payload.RecipientAddress = dlr.Input;
      payload.Games = cfg.Games;

      isJoining = true;
      await server.TransmitAsync(payload, dlr.Input);
    }

    private void DisconnectServer() {
      if (!cfg.Server.IsEmpty()) new Payload(SystemCommand.LeaveNetwork).Transmit(cfg.Server);

      ServerConnectionToggle("");
    }

    private void cltGame_SelectedIndexChanged(object sender, EventArgs e) {
      ControlToggle(cltAddress, "", cltGame.SelectedIndex > -1);
      ControlToggle(cltPort, "", cltGame.SelectedIndex > -1);
      ControlToggle(cltPassword, "", cltGame.SelectedIndex > -1);
      btnCltStart.Enabled = cltGame.SelectedIndex > -1;
    }

    private void btnCltStart_Click(object sender, EventArgs e) {
      if (activeGame == null) {
        Dialog("Load a game first!");
        return;
      }

      int port = GetInt(cltPort.Text);
      if (!ValidateControl(cltPort, "!val", "int", "min::1", "max::65535", "msg.int::That is not a valid number!", "msg.min::That is outside the valid port range!", "msg.max::That is outside the valid port range!")) { 
        ActiveControl = cltPort;
        return;
      }

      GameControl.Launch(activeGame, cltAddress.Text.Trim(), cltPassword.Text.Trim(), cltPort.Text.Trim());
    }

    public void LaunchGame(Game g) {
      try {
        if (g == null) {
          Dialog("Attempted to launch a game, but the game data is invalid!");
          return;
        }

        GameControl.Launch(g);
      } catch (Exception ex) {
        Dialog("** Exception during LaunchGame:\n\n" + ex.Message);
      }
    }

    #endregion

    #region Tab: Server

    private void TestKill() {
      string machineName = "carenath";
      string processName = "cmd.exe";
      string username = "strahan";
      string password = "fbY4p7r2";

      ConnectionOptions options = new ConnectionOptions {
        Username = username,
        Password = password,
        Authority = "ntlmdomain:NISCHAN.COM",
        Impersonation = ImpersonationLevel.Impersonate,
        EnablePrivileges = true
      };

      ManagementScope scope = new ManagementScope($"\\\\{machineName}\\root\\cimv2", options);
      scope.Connect();

      ObjectQuery query = new ObjectQuery($"SELECT * FROM Win32_Process WHERE Name = '{processName}'");
      ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

      foreach (ManagementObject process in searcher.Get()) {
        process.InvokeMethod("Terminate", null);
        Dialog($"Process {process["ProcessId"]} has been killed.");
      }
    }

    private void btnSelectAll_Click(object sender, EventArgs e) {
      BulkSelect();
    }

    private void btnClearSelection_Click(object sender, EventArgs e) {
      BulkSelect(false);
    }

    private void btnStart_Click(object sender, EventArgs e) {
      LaunchGame();
    }

    private void btnConnect_Click(object sender, EventArgs e) {
      LaunchGame(true);
    }

    private void btnTerminate_Click(object sender, EventArgs e) {
      TerminateGame();
    }

    private void connectClient_SelectedIndexChanged(object sender, EventArgs e) {
      LoadClientIp();
    }

    private void srvAddress_TextChanged(object sender, EventArgs e) {
      connectClient.SelectedIndex = -1;
    }

    private void BulkSelect(bool selectStatus = true) {
      foreach (Control ctrl in systems.Controls) {
        ClientPanel cp = (ClientPanel)ctrl;
        cp.SetSelected(selectStatus);
      }
    }

    private void PopulateGame() {
      ControlToggle(srvPassword, "", activeGame != null && !activeGame.ServerJoinArguments.IsEmpty());
      ControlToggle(connectClient, "", activeGame != null && !activeGame.ServerJoinArguments.IsEmpty());
      ControlToggle(srvAddress, GetCurrentIP().ToString(), activeGame != null && !activeGame.ServerJoinArguments.IsEmpty());
      ControlToggle(srvPort, "", activeGame != null && !activeGame.ServerJoinArguments.IsEmpty());
      btnConnect.Enabled = activeGame != null && !activeGame.ServerJoinArguments.IsEmpty();
      btnStart.Enabled = activeGame != null;
      btnTerminate.Enabled = activeGame != null;
    }

    private async void LaunchGame(bool connect = false) {
      List<Client> selectedClients = GetSelectedClients();
      if (selectedClients.Count == 0) {
        Dialog("You haven't selected any clients!");
        return;
      }
      if (connect && srvAddress.IsEmpty()) {
        Dialog("You cannot send a connection request if the address has not been specified!");
        return;
      }

      if (!srvPort.IsEmpty() && !ValidateControl(srvPort, "int", "min::1", "max::65535", "msg::Invalid port value!")) {
        ActiveControl = srvPort;
        return;
      }

      foreach (Client client in selectedClients) {
        Payload payload = new Payload(SystemCommand.LaunchGame);
        payload.Game = activeGame.Name;
        if (connect) {
          payload.JoinServer = GetCurrentIP().ToString();
          payload.ServerPassword = srvPassword.Text;
          payload.ServerPort = srvAddress.Text.Trim();
        }
        LogUI("Sending " + activeGame.Name + " start command to " + client.Name);
        await server.TransmitAsync(payload, client.Address);
      }
    }

    private async void TerminateGame() {
      try {
        List<Client> selectedClients = GetSelectedClients();
        if (selectedClients.Count == 0) {
          Dialog("You haven't selected any clients!");
          return;
        }

        foreach (Client client in selectedClients) {
          Payload payload = new Payload(SystemCommand.TerminateGame);
          payload.Game = activeGame.Name;
          await server.TransmitAsync(payload, client.Address);
        }
      } catch (Exception ex) {
        Dialog("** Exception during TerminateGame:\n\n" + ex.Message);
      }
    }

    public void ClientClicked(Client client) {
      try {
        var selectedPanels = systems.Controls.OfType<ClientPanel>()
                              .Where(cp => cp.BackColor == (cfg.ColorProfiles.Active?.PanelBackground ?? Color.CornflowerBlue))
                              .Select(cp => cp.Client?.Games.OrderBy(x => x.Name).Select(x => x.Name).ToList())
                              .ToList();

        var commonGames = selectedPanels
                          .Skip(1)
                          .Aggregate(new HashSet<string>(selectedPanels.FirstOrDefault() ?? new List<string>()),
                                     (h, e) => { h.IntersectWith(e); return h; });

        game.Items.Clear();
        foreach (var g in commonGames) game.Items.Add(g);
      } catch (Exception ex) {
        Dialog("** Exception during ClientClicked:\n\n" + ex.Message);
      }
    }

    private void LoadClientIp() {
      if (connectClient.SelectedIndex == -1) return;

      if (connectClient.SelectedIndex == 0) {
        srvAddress.Text = GetLocalIpAddress()?.ToString() ?? "";
        return;
      }

      Client client = cfg.GetClient(connectClient.Text);
      if (client == null) {
        Dialog("I couldn't load the record for that client!");
        return;
      }

      srvAddress.Text = client.Address;
    }

    #endregion

    #region Miscellaneous

    private void PayloadHandler(Payload payload, EventType etype, Client client = null) {
      try {
        client = client == null ? cfg.GetClient(payload.RecipientName) : client;

        switch (payload.Command) {
        case SystemCommand.LaunchGame:
          if (payload.Game.IsEmpty()) return;

          LogUI("Received command to start " + payload.Game);
          break;

        case SystemCommand.ShuttingDown:
          if (!isConnected) return;

          ServerConnectionToggle("");
          break;

        case SystemCommand.JoinNetwork:
        case SystemCommand.Pong:
          if (etype == EventType.TransmissionFailure) {
            if (isJoining) {
              ActivityLog("Failed to join network!");
              isJoining = false;
            }
            return;
          }

          ActivityLog(payload.SenderName + " is online");
          if (payload.Games.Count > 0 && client != null) {
            ActivityLog(payload.SenderName + " sent games");
            client.Games = payload.Games;
            SetConfiguration();
          }
          SetupForm();
          break;

        case SystemCommand.GameUpdate:
          if (client == null) {
            ActivityLog(payload.SenderName + " received a game update for an invalid client!");
            return;
          }

          client.Games = payload.Games;
          SetConfiguration();
          break;

        case SystemCommand.GamesList:
          if (client == null) return;

          client.Games = payload.Games;
          SetConfiguration();
          ActivityLog(payload.SenderName + Pluralize(" transmitted their game!s list (!#)", payload.Games.Count));
          SetupForm();
          break;

        case SystemCommand.JoinedNetwork:
          cfg.Server = payload.SenderAddress;
          ServerConnectionToggle(payload.SenderAddress);
          ActivityLog("Joined server " + payload.SenderAddress);
          SetConfiguration();
          isJoining = true;
          break;

        case SystemCommand.LeaveNetwork:
          if (etype == EventType.TransmissionFailure) {
            ActivityLog(payload.SenderName + " left the network");
            return;
          }

          if (cfg.Server.Matches(payload.SenderAddress)) ServerConnectionToggle("");
          ActivityLog(payload.SenderName + " went offline");
          SetupForm();
          break;

        case SystemCommand.Ping:
          switch (etype) {
          case EventType.Payload:
            if (!cfg.Server.Matches(payload.SenderAddress)) return;
            if (isConnected) return;

            ServerConnectionToggle(payload.SenderAddress);
            ActivityLog("Server " + payload.SenderAddress + " came online");
            cfg.Server = payload.SenderAddress;
            SetConfiguration();
            break;

          case EventType.TransmissionFailure:
            if (client == null) return;

            foreach (ClientPanel cp in systems.Controls.OfType<ClientPanel>()) {
              if (!cp.Client.Name.Matches(client.Name)) continue;

              systems.Controls.Remove(cp);
              break;
            }
            break;
          }
          break;
        }
      } catch (Exception ex) {
        Dialog("** Exception during PayloadHandler:\n\n" + ex.Message);
      }
    }

    private void SetupExitMonitor() {
      try {
        exitWatcher.Path = AppDomain.CurrentDomain.BaseDirectory;
        exitWatcher.Filter = "exit.dat";
        exitWatcher.NotifyFilter = NotifyFilters.FileName;
        exitWatcher.Created += ExitWatcher_Created;
        exitWatcher.EnableRaisingEvents = true;
      } catch (Exception ex) {
        Dialog("** Exception during SetupExitMonitor:\n\n" + ex.Message);
      }
    }

    public void ActivityLog(string msg) {
      activity.Text += "[" + DateTime.Now.ToString("MM/dd/yyyy @ hh:mm tt") + "]: " + msg + Environment.NewLine;
      activity.SelectionStart = activity.Text.Length;
      activity.ScrollToCaret();
    }

    private async Task CheckClientStatusAsync() {
      if (cfg.Clients.Count == 0) return;

      lblPolling.Location = new Point(228, 159);
      lblPolling.Visible = true;
      systems.Visible = false;
      btnSelectAll.Visible = false;
      btnClearSelection.Visible = false;
      cfg.Clients.ForEach(x => x.Online = false);
      ActivityLog("Polling client machines ...");
      foreach (Client client in cfg.Clients) {
        if (client.Address.IsEmpty()) continue;

        Payload payload = new Payload(SystemCommand.Ping);
        payload.RecipientName = client.Name;
        payload.RecipientAddress = client.Address;
        payload.Transmit(client.Address);
      }

      await Task.Delay(1200);

      lblPolling.Visible = false;
      systems.Visible = true;
      btnSelectAll.Visible = true;
      btnClearSelection.Visible = true;
    }

    private void SetupForm() {
      systems.Controls.Clear();
      connectClient.Items.Clear();
      connectClient.Items.Add("[ THIS PC ]");
      foreach (Client client in cfg.Clients.OrderBy(x => x.Name)) {
        if (!client.Online) continue;

        ClientPanel cp = new ClientPanel(client);
        cp.ClickEvent += system_ClickEvent;
        systems.Controls.Add(cp);
        if (client.Name.Matches("Carenath")) foreach (Game g in client.Games) ActivityLog(client.Name + ": " + g.Name);


        connectClient.Items.Add(client.Name);
      }

      tsmiGames.DropDownItems.Clear();
      game.Items.Clear();
      cltGame.Items.Clear();
      foreach (Game g in cfg.Games.OrderBy(x => x.Name)) {
        cltGame.Items.Add(g.Name);
        ToolStripMenuItem tsmi = new ToolStripMenuItem();
        tsmi.Text = g.Name;
        tsmi.Tag = g;
        tsmi.Click += (sender, e) => { LaunchGame(g); };
        tsmiGames.DropDownItems.Add(tsmi);
      }

      systems.Refresh();
      if (cfg.Clients.Count > 0) tabControl1.SelectedIndex = 1;
      if (cfg.Server.IsEmpty() || isConnected) return;

      server.JoinNetwork(cfg.Server);
    }

    private async void Exit() {
      ActivityLog("Closing down ...");
      if (notifyIcon1 != null) {
        notifyIcon1.Visible = false;
        notifyIcon1.Dispose();
        notifyIcon1 = null;
      }

      try {
        foreach (Client clt in cfg.Clients.Where(x => x.Online)) {
          Payload shitface = new Payload(SystemCommand.LeaveNetwork);
          await server.TransmitAsync(shitface, clt.Address);
        }
      } catch {
      }

      if (isConnected && !cfg.Server.IsEmpty()) {
        try {
          Payload exiting = new Payload(SystemCommand.LeaveNetwork);
          await server.TransmitAsync(exiting, cfg.Server);
        } catch {
        }
      }

      Environment.Exit(0);
    }

    public void ShowUI() {
      WindowState = FormWindowState.Normal;
      Show();
      BringToFront();
      Focus();
      int screenWidth = Screen.PrimaryScreen.Bounds.Width;
      int screenHeight = Screen.PrimaryScreen.Bounds.Height;
      int windowWidth = this.Width;
      int windowHeight = this.Height;
      int centerX = (screenWidth - windowWidth) / 2;
      int centerY = (screenHeight - windowHeight) / 2;

      StartPosition = FormStartPosition.Manual;
      Location = new Point(centerX, centerY);
      Size = new Size(1422, 925);
    }

    private List<Client> GetSelectedClients() {
      List<Client> ret = new List<Client>();
      foreach (ClientPanel cp in systems.Controls.OfType<ClientPanel>()) {
        if (cp.BackColor != (cfg.ColorProfiles.Active?.PanelBackground ?? Color.DarkSlateGray)) continue;

        ret.Add(cp.Client);
      }
      return ret;
    }

    private void ServerConnectionToggle(string server) {
      try {
        isConnected = !server.IsEmpty();
        lblServer.Text = isConnected ? server : "Disconnected";
        connectToServerToolStripMenuItem.Text = isConnected ? "&Disconnect from Server" : "&Connect to Server";
      } catch (Exception ex) {
        Dialog("** Exception during ServerConnectionToggle:\n\n" + ex.Message);
      }
    }

    #endregion

  }
}
