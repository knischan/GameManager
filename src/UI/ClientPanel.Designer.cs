namespace GameManager.UI {
  partial class ClientPanel {
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.components = new System.ComponentModel.Container();
      this.pcname = new System.Windows.Forms.Label();
      this.ip = new System.Windows.Forms.Label();
      this.games = new System.Windows.Forms.FlowLayoutPanel();
      this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
      this.SuspendLayout();
      // 
      // pcname
      // 
      this.pcname.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.pcname.ForeColor = System.Drawing.Color.Yellow;
      this.pcname.Location = new System.Drawing.Point(8, 8);
      this.pcname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.pcname.Name = "pcname";
      this.pcname.Size = new System.Drawing.Size(493, 40);
      this.pcname.TabIndex = 0;
      this.pcname.Text = "label1";
      this.pcname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // ip
      // 
      this.ip.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ip.ForeColor = System.Drawing.Color.White;
      this.ip.Location = new System.Drawing.Point(509, 7);
      this.ip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.ip.Name = "ip";
      this.ip.Size = new System.Drawing.Size(148, 40);
      this.ip.TabIndex = 1;
      this.ip.Text = "999.999.999.999";
      this.ip.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
      // 
      // games
      // 
      this.games.AutoScroll = true;
      this.games.Location = new System.Drawing.Point(8, 52);
      this.games.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.games.Name = "games";
      this.games.Size = new System.Drawing.Size(649, 40);
      this.games.TabIndex = 2;
      // 
      // ClientPanel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.DarkSlateGray;
      this.Controls.Add(this.games);
      this.Controls.Add(this.ip);
      this.Controls.Add(this.pcname);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "ClientPanel";
      this.Size = new System.Drawing.Size(667, 105);
      this.Load += new System.EventHandler(this.ClientPanel_Load);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label pcname;
    private System.Windows.Forms.Label ip;
    private System.Windows.Forms.FlowLayoutPanel games;
    private System.Windows.Forms.ToolTip toolTip1;
  }
}
