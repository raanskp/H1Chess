namespace H1Chess
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chessImageHolder = new System.Windows.Forms.PictureBox();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.connectionStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startServerMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.stopServerMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.connectToServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localhostConnectMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.connectIPMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.chessImageHolder)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chessImageHolder
            // 
            this.chessImageHolder.Location = new System.Drawing.Point(12, 44);
            this.chessImageHolder.Name = "chessImageHolder";
            this.chessImageHolder.Size = new System.Drawing.Size(512, 512);
            this.chessImageHolder.TabIndex = 0;
            this.chessImageHolder.TabStop = false;
            this.chessImageHolder.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chessImageHolder_MouseClick);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 559);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(538, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // connectionStatusLabel
            // 
            this.connectionStatusLabel.Name = "connectionStatusLabel";
            this.connectionStatusLabel.Size = new System.Drawing.Size(67, 17);
            this.connectionStatusLabel.Text = "localIP:port";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(538, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startServerMenu,
            this.stopServerMenu,
            this.connectToServerToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(64, 20);
            this.testToolStripMenuItem.Text = "Connect";
            // 
            // startServerMenu
            // 
            this.startServerMenu.Name = "startServerMenu";
            this.startServerMenu.Size = new System.Drawing.Size(167, 22);
            this.startServerMenu.Text = "Start server";
            this.startServerMenu.Click += new System.EventHandler(this.startServerMenu_ClickAsync);
            // 
            // stopServerMenu
            // 
            this.stopServerMenu.Name = "stopServerMenu";
            this.stopServerMenu.Size = new System.Drawing.Size(167, 22);
            this.stopServerMenu.Text = "Stop server";
            this.stopServerMenu.Visible = false;
            this.stopServerMenu.Click += new System.EventHandler(this.stopServerMenu_Click);
            // 
            // connectToServerToolStripMenuItem
            // 
            this.connectToServerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.localhostConnectMenu,
            this.connectIPMenu});
            this.connectToServerToolStripMenuItem.Name = "connectToServerToolStripMenuItem";
            this.connectToServerToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.connectToServerToolStripMenuItem.Text = "Connect to server";
            // 
            // localhostConnectMenu
            // 
            this.localhostConnectMenu.Name = "localhostConnectMenu";
            this.localhostConnectMenu.Size = new System.Drawing.Size(125, 22);
            this.localhostConnectMenu.Text = "Localhost";
            this.localhostConnectMenu.Visible = false;
            this.localhostConnectMenu.Click += new System.EventHandler(this.localhostConnectMenu_Click);
            // 
            // connectIPMenu
            // 
            this.connectIPMenu.Name = "connectIPMenu";
            this.connectIPMenu.Size = new System.Drawing.Size(125, 22);
            this.connectIPMenu.Text = "Enter IP";
            this.connectIPMenu.Click += new System.EventHandler(this.enterIPToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(164, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 581);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.chessImageHolder);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Chess game v0.1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chessImageHolder)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox chessImageHolder;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel connectionStatusLabel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startServerMenu;
        private System.Windows.Forms.ToolStripMenuItem stopServerMenu;
        private System.Windows.Forms.ToolStripMenuItem connectToServerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localhostConnectMenu;
        private System.Windows.Forms.ToolStripMenuItem connectIPMenu;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}

