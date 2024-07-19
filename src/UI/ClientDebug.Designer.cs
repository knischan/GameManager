namespace GameManager.UI {
  partial class ClientDebug {
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
      this.label1 = new System.Windows.Forms.Label();
      this.client = new System.Windows.Forms.Label();
      this.ip = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.games = new System.Windows.Forms.FlowLayoutPanel();
      this.SuspendLayout();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.ForeColor = System.Drawing.Color.Yellow;
      this.label1.Location = new System.Drawing.Point(14, 17);
      this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 10);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(60, 20);
      this.label1.TabIndex = 0;
      this.label1.Text = "Client:";
      // 
      // client
      // 
      this.client.AutoSize = true;
      this.client.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.client.ForeColor = System.Drawing.Color.Yellow;
      this.client.Location = new System.Drawing.Point(110, 17);
      this.client.Margin = new System.Windows.Forms.Padding(4, 0, 4, 10);
      this.client.Name = "client";
      this.client.Size = new System.Drawing.Size(60, 20);
      this.client.TabIndex = 1;
      this.client.Text = "Client:";
      // 
      // ip
      // 
      this.ip.AutoSize = true;
      this.ip.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.ip.ForeColor = System.Drawing.Color.Yellow;
      this.ip.Location = new System.Drawing.Point(110, 47);
      this.ip.Margin = new System.Windows.Forms.Padding(4, 0, 4, 10);
      this.ip.Name = "ip";
      this.ip.Size = new System.Drawing.Size(60, 20);
      this.ip.TabIndex = 3;
      this.ip.Text = "Client:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.ForeColor = System.Drawing.Color.Yellow;
      this.label4.Location = new System.Drawing.Point(14, 47);
      this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 10);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(31, 20);
      this.label4.TabIndex = 2;
      this.label4.Text = "IP:";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label6.ForeColor = System.Drawing.Color.Yellow;
      this.label6.Location = new System.Drawing.Point(14, 77);
      this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 10);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(71, 20);
      this.label6.TabIndex = 4;
      this.label6.Text = "Games:";
      // 
      // games
      // 
      this.games.Location = new System.Drawing.Point(114, 80);
      this.games.Name = "games";
      this.games.Size = new System.Drawing.Size(579, 189);
      this.games.TabIndex = 5;
      // 
      // ClientDebug
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.AutoScroll = true;
      this.BackColor = System.Drawing.Color.Teal;
      this.Controls.Add(this.games);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.ip);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.client);
      this.Controls.Add(this.label1);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
      this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
      this.Name = "ClientDebug";
      this.Size = new System.Drawing.Size(715, 295);
      this.Load += new System.EventHandler(this.ClientDebug_Load);
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label client;
    private System.Windows.Forms.Label ip;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.FlowLayoutPanel games;
  }
}
