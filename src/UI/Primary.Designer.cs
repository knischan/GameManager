namespace GameManager {
  partial class Primary {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.timer1 = new System.Windows.Forms.Timer(this.components);
      this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
      this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.showprojectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tsmiGames = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
      this.panel1 = new System.Windows.Forms.Panel();
      this.lblPolling = new System.Windows.Forms.Label();
      this.panel3 = new System.Windows.Forms.Panel();
      this.label10 = new System.Windows.Forms.Label();
      this.srvAddress = new System.Windows.Forms.TextBox();
      this.btnConnect = new System.Windows.Forms.Button();
      this.srvPort = new System.Windows.Forms.TextBox();
      this.btnTerminate = new System.Windows.Forms.Button();
      this.btnStart = new System.Windows.Forms.Button();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label9 = new System.Windows.Forms.Label();
      this.game = new System.Windows.Forms.ComboBox();
      this.srvPassword = new System.Windows.Forms.TextBox();
      this.btnSelectAll = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.systems = new System.Windows.Forms.FlowLayoutPanel();
      this.activity = new System.Windows.Forms.TextBox();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.connectToServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.debugModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.tabControl1 = new System.Windows.Forms.TabControl();
      this.tabClient = new System.Windows.Forms.TabPage();
      this.panel2 = new System.Windows.Forms.Panel();
      this.panel4 = new System.Windows.Forms.Panel();
      this.label8 = new System.Windows.Forms.Label();
      this.cltGame = new System.Windows.Forms.ComboBox();
      this.btnCltStart = new System.Windows.Forms.Button();
      this.cltPort = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.cltPassword = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.cltAddress = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.lblServer = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.tabServer = new System.Windows.Forms.TabPage();
      this.panel5 = new System.Windows.Forms.Panel();
      this.panel6 = new System.Windows.Forms.Panel();
      this.connectClient = new System.Windows.Forms.ComboBox();
      this.btnClearSelection = new System.Windows.Forms.Button();
      this.contextMenuStrip1.SuspendLayout();
      this.panel1.SuspendLayout();
      this.panel3.SuspendLayout();
      this.menuStrip1.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabClient.SuspendLayout();
      this.panel2.SuspendLayout();
      this.panel4.SuspendLayout();
      this.tabServer.SuspendLayout();
      this.panel5.SuspendLayout();
      this.panel6.SuspendLayout();
      this.SuspendLayout();
      // 
      // timer1
      // 
      this.timer1.Interval = 1000;
      // 
      // notifyIcon1
      // 
      this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
      this.notifyIcon1.Text = "GameManager";
      this.notifyIcon1.Visible = true;
      this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
      // 
      // contextMenuStrip1
      // 
      this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showprojectToolStripMenuItem,
            this.tsmiGames,
            this.exitToolStripMenuItem1});
      this.contextMenuStrip1.Name = "contextMenuStrip1";
      this.contextMenuStrip1.Size = new System.Drawing.Size(185, 70);
      // 
      // showprojectToolStripMenuItem
      // 
      this.showprojectToolStripMenuItem.Name = "showprojectToolStripMenuItem";
      this.showprojectToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
      this.showprojectToolStripMenuItem.Text = "Sho&w GameManager";
      this.showprojectToolStripMenuItem.Click += new System.EventHandler(this.showprojectToolStripMenuItem_Click);
      // 
      // tsmiGames
      // 
      this.tsmiGames.Name = "tsmiGames";
      this.tsmiGames.Size = new System.Drawing.Size(184, 22);
      this.tsmiGames.Text = "&Games";
      // 
      // exitToolStripMenuItem1
      // 
      this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
      this.exitToolStripMenuItem1.Size = new System.Drawing.Size(184, 22);
      this.exitToolStripMenuItem1.Text = "E&xit";
      this.exitToolStripMenuItem1.Click += new System.EventHandler(this.contextExitToolStripMenuItem_Click);
      // 
      // panel1
      // 
      this.panel1.BackColor = System.Drawing.Color.AliceBlue;
      this.panel1.Controls.Add(this.btnClearSelection);
      this.panel1.Controls.Add(this.lblPolling);
      this.panel1.Controls.Add(this.panel3);
      this.panel1.Controls.Add(this.btnSelectAll);
      this.panel1.Controls.Add(this.label1);
      this.panel1.Controls.Add(this.systems);
      this.panel1.Location = new System.Drawing.Point(6, 6);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(730, 784);
      this.panel1.TabIndex = 1;
      // 
      // lblPolling
      // 
      this.lblPolling.AutoSize = true;
      this.lblPolling.ForeColor = System.Drawing.Color.Blue;
      this.lblPolling.Location = new System.Drawing.Point(503, 16);
      this.lblPolling.Name = "lblPolling";
      this.lblPolling.Size = new System.Drawing.Size(207, 20);
      this.lblPolling.TabIndex = 9;
      this.lblPolling.Text = "Polling clients, please wait ...";
      this.lblPolling.Visible = false;
      // 
      // panel3
      // 
      this.panel3.BackColor = System.Drawing.Color.Lavender;
      this.panel3.Controls.Add(this.connectClient);
      this.panel3.Controls.Add(this.label10);
      this.panel3.Controls.Add(this.srvAddress);
      this.panel3.Controls.Add(this.btnConnect);
      this.panel3.Controls.Add(this.srvPort);
      this.panel3.Controls.Add(this.btnTerminate);
      this.panel3.Controls.Add(this.btnStart);
      this.panel3.Controls.Add(this.label2);
      this.panel3.Controls.Add(this.label3);
      this.panel3.Controls.Add(this.label9);
      this.panel3.Controls.Add(this.game);
      this.panel3.Controls.Add(this.srvPassword);
      this.panel3.Location = new System.Drawing.Point(85, 439);
      this.panel3.Name = "panel3";
      this.panel3.Size = new System.Drawing.Size(531, 319);
      this.panel3.TabIndex = 8;
      // 
      // label10
      // 
      this.label10.AutoSize = true;
      this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label10.Location = new System.Drawing.Point(405, 122);
      this.label10.Name = "label10";
      this.label10.Size = new System.Drawing.Size(14, 20);
      this.label10.TabIndex = 45;
      this.label10.Text = ":";
      // 
      // srvAddress
      // 
      this.srvAddress.BackColor = System.Drawing.Color.Gainsboro;
      this.srvAddress.Enabled = false;
      this.srvAddress.Location = new System.Drawing.Point(269, 119);
      this.srvAddress.Name = "srvAddress";
      this.srvAddress.Size = new System.Drawing.Size(130, 26);
      this.srvAddress.TabIndex = 11;
      this.srvAddress.TextChanged += new System.EventHandler(this.srvAddress_TextChanged);
      // 
      // btnConnect
      // 
      this.btnConnect.Enabled = false;
      this.btnConnect.Location = new System.Drawing.Point(194, 246);
      this.btnConnect.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
      this.btnConnect.Name = "btnConnect";
      this.btnConnect.Size = new System.Drawing.Size(140, 49);
      this.btnConnect.TabIndex = 51;
      this.btnConnect.Text = "Start && Connect";
      this.btnConnect.UseVisualStyleBackColor = true;
      this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
      // 
      // srvPort
      // 
      this.srvPort.BackColor = System.Drawing.Color.Gainsboro;
      this.srvPort.Enabled = false;
      this.srvPort.Location = new System.Drawing.Point(425, 119);
      this.srvPort.Name = "srvPort";
      this.srvPort.Size = new System.Drawing.Size(84, 26);
      this.srvPort.TabIndex = 21;
      // 
      // btnTerminate
      // 
      this.btnTerminate.Enabled = false;
      this.btnTerminate.Location = new System.Drawing.Point(340, 246);
      this.btnTerminate.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
      this.btnTerminate.Name = "btnTerminate";
      this.btnTerminate.Size = new System.Drawing.Size(140, 49);
      this.btnTerminate.TabIndex = 61;
      this.btnTerminate.Text = "Terminate";
      this.btnTerminate.UseVisualStyleBackColor = true;
      this.btnTerminate.Click += new System.EventHandler(this.btnTerminate_Click);
      // 
      // btnStart
      // 
      this.btnStart.Enabled = false;
      this.btnStart.Location = new System.Drawing.Point(48, 246);
      this.btnStart.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
      this.btnStart.Name = "btnStart";
      this.btnStart.Size = new System.Drawing.Size(140, 49);
      this.btnStart.TabIndex = 41;
      this.btnStart.Text = "Start Game";
      this.btnStart.UseVisualStyleBackColor = true;
      this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(15, 16);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(57, 20);
      this.label2.TabIndex = 2;
      this.label2.Text = "Game:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(15, 179);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(179, 20);
      this.label3.TabIndex = 8;
      this.label3.Text = "Password (if applicable):";
      // 
      // label9
      // 
      this.label9.AutoSize = true;
      this.label9.Location = new System.Drawing.Point(15, 96);
      this.label9.Name = "label9";
      this.label9.Size = new System.Drawing.Size(95, 20);
      this.label9.TabIndex = 42;
      this.label9.Text = "Connect To:";
      // 
      // game
      // 
      this.game.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.game.FormattingEnabled = true;
      this.game.Location = new System.Drawing.Point(19, 39);
      this.game.Name = "game";
      this.game.Size = new System.Drawing.Size(490, 28);
      this.game.TabIndex = 1;
      this.game.SelectedIndexChanged += new System.EventHandler(this.game_SelectedIndexChanged);
      // 
      // srvPassword
      // 
      this.srvPassword.BackColor = System.Drawing.Color.Gainsboro;
      this.srvPassword.Enabled = false;
      this.srvPassword.Location = new System.Drawing.Point(19, 202);
      this.srvPassword.Name = "srvPassword";
      this.srvPassword.Size = new System.Drawing.Size(490, 26);
      this.srvPassword.TabIndex = 31;
      // 
      // btnSelectAll
      // 
      this.btnSelectAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
      this.btnSelectAll.Location = new System.Drawing.Point(17, 386);
      this.btnSelectAll.Name = "btnSelectAll";
      this.btnSelectAll.Size = new System.Drawing.Size(545, 25);
      this.btnSelectAll.TabIndex = 7;
      this.btnSelectAll.Text = "Select All";
      this.btnSelectAll.UseVisualStyleBackColor = true;
      this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(13, 16);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(156, 20);
      this.label1.TabIndex = 1;
      this.label1.Text = "Connected Systems:";
      // 
      // systems
      // 
      this.systems.AutoScroll = true;
      this.systems.BackColor = System.Drawing.Color.LightGray;
      this.systems.Location = new System.Drawing.Point(17, 39);
      this.systems.Name = "systems";
      this.systems.Size = new System.Drawing.Size(693, 341);
      this.systems.TabIndex = 0;
      // 
      // activity
      // 
      this.activity.BackColor = System.Drawing.Color.Ivory;
      this.activity.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
      this.activity.Location = new System.Drawing.Point(6, 7);
      this.activity.Multiline = true;
      this.activity.Name = "activity";
      this.activity.ScrollBars = System.Windows.Forms.ScrollBars.Both;
      this.activity.Size = new System.Drawing.Size(609, 783);
      this.activity.TabIndex = 62;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 10F);
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(1406, 27);
      this.menuStrip1.TabIndex = 2;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // fileToolStripMenuItem
      // 
      this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToServerToolStripMenuItem,
            this.configurationToolStripMenuItem,
            this.debugModeToolStripMenuItem,
            this.exitToolStripMenuItem});
      this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
      this.fileToolStripMenuItem.Size = new System.Drawing.Size(41, 23);
      this.fileToolStripMenuItem.Text = "&File";
      // 
      // connectToServerToolStripMenuItem
      // 
      this.connectToServerToolStripMenuItem.Name = "connectToServerToolStripMenuItem";
      this.connectToServerToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
      this.connectToServerToolStripMenuItem.Text = "&Connect to Server";
      this.connectToServerToolStripMenuItem.Click += new System.EventHandler(this.connectToServerToolStripMenuItem_Click);
      // 
      // configurationToolStripMenuItem
      // 
      this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
      this.configurationToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
      this.configurationToolStripMenuItem.Text = "Confi&guration";
      this.configurationToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripMenuItem_Click);
      // 
      // debugModeToolStripMenuItem
      // 
      this.debugModeToolStripMenuItem.Name = "debugModeToolStripMenuItem";
      this.debugModeToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
      this.debugModeToolStripMenuItem.Text = "De&bug Mode";
      this.debugModeToolStripMenuItem.Click += new System.EventHandler(this.debugModeToolStripMenuItem_Click);
      // 
      // exitToolStripMenuItem
      // 
      this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
      this.exitToolStripMenuItem.Size = new System.Drawing.Size(188, 24);
      this.exitToolStripMenuItem.Text = "E&xit";
      this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
      // 
      // tabControl1
      // 
      this.tabControl1.Controls.Add(this.tabClient);
      this.tabControl1.Controls.Add(this.tabServer);
      this.tabControl1.Location = new System.Drawing.Point(12, 43);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(749, 831);
      this.tabControl1.TabIndex = 3;
      // 
      // tabClient
      // 
      this.tabClient.BackColor = System.Drawing.Color.DarkSlateBlue;
      this.tabClient.Controls.Add(this.panel2);
      this.tabClient.Location = new System.Drawing.Point(4, 29);
      this.tabClient.Name = "tabClient";
      this.tabClient.Padding = new System.Windows.Forms.Padding(3);
      this.tabClient.Size = new System.Drawing.Size(741, 798);
      this.tabClient.TabIndex = 1;
      this.tabClient.Text = "  Client  ";
      // 
      // panel2
      // 
      this.panel2.BackColor = System.Drawing.Color.AliceBlue;
      this.panel2.Controls.Add(this.panel4);
      this.panel2.Controls.Add(this.lblServer);
      this.panel2.Controls.Add(this.label4);
      this.panel2.Location = new System.Drawing.Point(6, 7);
      this.panel2.Name = "panel2";
      this.panel2.Size = new System.Drawing.Size(729, 784);
      this.panel2.TabIndex = 2;
      // 
      // panel4
      // 
      this.panel4.BackColor = System.Drawing.Color.Lavender;
      this.panel4.Controls.Add(this.label8);
      this.panel4.Controls.Add(this.cltGame);
      this.panel4.Controls.Add(this.btnCltStart);
      this.panel4.Controls.Add(this.cltPort);
      this.panel4.Controls.Add(this.label5);
      this.panel4.Controls.Add(this.cltPassword);
      this.panel4.Controls.Add(this.label6);
      this.panel4.Controls.Add(this.cltAddress);
      this.panel4.Controls.Add(this.label7);
      this.panel4.Location = new System.Drawing.Point(121, 194);
      this.panel4.Name = "panel4";
      this.panel4.Size = new System.Drawing.Size(488, 396);
      this.panel4.TabIndex = 10;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label8.Location = new System.Drawing.Point(392, 155);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(14, 20);
      this.label8.TabIndex = 33;
      this.label8.Text = ":";
      // 
      // cltGame
      // 
      this.cltGame.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cltGame.FormattingEnabled = true;
      this.cltGame.Location = new System.Drawing.Point(18, 43);
      this.cltGame.Name = "cltGame";
      this.cltGame.Size = new System.Drawing.Size(450, 28);
      this.cltGame.TabIndex = 1;
      this.cltGame.SelectedIndexChanged += new System.EventHandler(this.cltGame_SelectedIndexChanged);
      // 
      // btnCltStart
      // 
      this.btnCltStart.Enabled = false;
      this.btnCltStart.Location = new System.Drawing.Point(153, 322);
      this.btnCltStart.Margin = new System.Windows.Forms.Padding(3, 3, 3, 15);
      this.btnCltStart.Name = "btnCltStart";
      this.btnCltStart.Size = new System.Drawing.Size(182, 49);
      this.btnCltStart.TabIndex = 31;
      this.btnCltStart.Text = "Start Game";
      this.btnCltStart.UseVisualStyleBackColor = true;
      this.btnCltStart.Click += new System.EventHandler(this.btnCltStart_Click);
      // 
      // cltPort
      // 
      this.cltPort.BackColor = System.Drawing.Color.Gainsboro;
      this.cltPort.Enabled = false;
      this.cltPort.Location = new System.Drawing.Point(412, 152);
      this.cltPort.Name = "cltPort";
      this.cltPort.Size = new System.Drawing.Size(56, 26);
      this.cltPort.TabIndex = 32;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(14, 20);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(57, 20);
      this.label5.TabIndex = 2;
      this.label5.Text = "Game:";
      // 
      // cltPassword
      // 
      this.cltPassword.BackColor = System.Drawing.Color.Gainsboro;
      this.cltPassword.Enabled = false;
      this.cltPassword.Location = new System.Drawing.Point(18, 225);
      this.cltPassword.Name = "cltPassword";
      this.cltPassword.Size = new System.Drawing.Size(450, 26);
      this.cltPassword.TabIndex = 21;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(14, 129);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(122, 20);
      this.label6.TabIndex = 10;
      this.label6.Text = "Server Address:";
      // 
      // cltAddress
      // 
      this.cltAddress.BackColor = System.Drawing.Color.Gainsboro;
      this.cltAddress.Enabled = false;
      this.cltAddress.Location = new System.Drawing.Point(18, 152);
      this.cltAddress.Name = "cltAddress";
      this.cltAddress.Size = new System.Drawing.Size(368, 26);
      this.cltAddress.TabIndex = 11;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(14, 202);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(179, 20);
      this.label7.TabIndex = 12;
      this.label7.Text = "Password (if applicable):";
      // 
      // lblServer
      // 
      this.lblServer.AutoSize = true;
      this.lblServer.ForeColor = System.Drawing.Color.Blue;
      this.lblServer.Location = new System.Drawing.Point(259, 18);
      this.lblServer.Name = "lblServer";
      this.lblServer.Size = new System.Drawing.Size(107, 20);
      this.lblServer.TabIndex = 9;
      this.lblServer.Text = "Disconnected";
      this.lblServer.Click += new System.EventHandler(this.lblServer_Click);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(13, 18);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(240, 20);
      this.label4.TabIndex = 8;
      this.label4.Text = "Current Management Server:";
      // 
      // tabServer
      // 
      this.tabServer.BackColor = System.Drawing.Color.DarkSlateBlue;
      this.tabServer.Controls.Add(this.panel1);
      this.tabServer.Location = new System.Drawing.Point(4, 29);
      this.tabServer.Name = "tabServer";
      this.tabServer.Padding = new System.Windows.Forms.Padding(3);
      this.tabServer.Size = new System.Drawing.Size(741, 798);
      this.tabServer.TabIndex = 0;
      this.tabServer.Text = "  Server  ";
      // 
      // panel5
      // 
      this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
      this.panel5.Controls.Add(this.panel6);
      this.panel5.Location = new System.Drawing.Point(767, 70);
      this.panel5.Name = "panel5";
      this.panel5.Size = new System.Drawing.Size(627, 804);
      this.panel5.TabIndex = 63;
      // 
      // panel6
      // 
      this.panel6.BackColor = System.Drawing.Color.DarkSlateBlue;
      this.panel6.Controls.Add(this.activity);
      this.panel6.Location = new System.Drawing.Point(3, 3);
      this.panel6.Name = "panel6";
      this.panel6.Size = new System.Drawing.Size(621, 798);
      this.panel6.TabIndex = 0;
      // 
      // connectClient
      // 
      this.connectClient.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.connectClient.Enabled = false;
      this.connectClient.FormattingEnabled = true;
      this.connectClient.Location = new System.Drawing.Point(19, 119);
      this.connectClient.Name = "connectClient";
      this.connectClient.Size = new System.Drawing.Size(244, 28);
      this.connectClient.TabIndex = 62;
      this.connectClient.SelectedIndexChanged += new System.EventHandler(this.connectClient_SelectedIndexChanged);
      // 
      // btnClearSelection
      // 
      this.btnClearSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
      this.btnClearSelection.Location = new System.Drawing.Point(568, 386);
      this.btnClearSelection.Name = "btnClearSelection";
      this.btnClearSelection.Size = new System.Drawing.Size(142, 25);
      this.btnClearSelection.TabIndex = 10;
      this.btnClearSelection.Text = "Clear";
      this.btnClearSelection.UseVisualStyleBackColor = true;
      this.btnClearSelection.Click += new System.EventHandler(this.btnClearSelection_Click);
      // 
      // Primary
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.DarkSlateGray;
      this.ClientSize = new System.Drawing.Size(1406, 886);
      this.Controls.Add(this.panel5);
      this.Controls.Add(this.tabControl1);
      this.Controls.Add(this.menuStrip1);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
      this.KeyPreview = true;
      this.MainMenuStrip = this.menuStrip1;
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "Primary";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "GameManager";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Primary_FormClosing);
      this.Load += new System.EventHandler(this.Primary_Load);
      this.Shown += new System.EventHandler(this.Primary_Shown);
      this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Primary_KeyDown);
      this.Resize += new System.EventHandler(this.Primary_Resize);
      this.contextMenuStrip1.ResumeLayout(false);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.panel3.ResumeLayout(false);
      this.panel3.PerformLayout();
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.tabControl1.ResumeLayout(false);
      this.tabClient.ResumeLayout(false);
      this.panel2.ResumeLayout(false);
      this.panel2.PerformLayout();
      this.panel4.ResumeLayout(false);
      this.panel4.PerformLayout();
      this.tabServer.ResumeLayout(false);
      this.panel5.ResumeLayout(false);
      this.panel6.ResumeLayout(false);
      this.panel6.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.ToolTip toolTip1;
    private System.Windows.Forms.Timer timer1;
    private System.Windows.Forms.NotifyIcon notifyIcon1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
    private System.Windows.Forms.ToolStripMenuItem showprojectToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem tsmiGames;
    private System.Windows.Forms.TextBox srvPassword;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btnSelectAll;
    private System.Windows.Forms.Button btnTerminate;
    private System.Windows.Forms.Button btnConnect;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.ComboBox game;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.FlowLayoutPanel systems;
    private System.Windows.Forms.ToolStripMenuItem connectToServerToolStripMenuItem;
    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabServer;
    private System.Windows.Forms.TabPage tabClient;
    private System.Windows.Forms.Panel panel2;
    private System.Windows.Forms.Label lblServer;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button btnCltStart;
    private System.Windows.Forms.ComboBox cltGame;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox cltPassword;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox cltAddress;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.TextBox cltPort;
    private System.Windows.Forms.TextBox srvAddress;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.Label label10;
    private System.Windows.Forms.TextBox srvPort;
    private System.Windows.Forms.TextBox activity;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.Panel panel4;
    private System.Windows.Forms.Panel panel5;
    private System.Windows.Forms.Panel panel6;
    private System.Windows.Forms.ToolStripMenuItem debugModeToolStripMenuItem;
    private System.Windows.Forms.Label lblPolling;
    private System.Windows.Forms.ComboBox connectClient;
    private System.Windows.Forms.Button btnClearSelection;
  }
}
