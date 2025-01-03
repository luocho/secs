namespace SECSMainServer
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SecsConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseConnection = new System.Windows.Forms.ToolStripMenuItem();
            this.SMText = new System.Windows.Forms.TextBox();
            this.SendMessage = new System.Windows.Forms.Button();
            this.UiLog = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1003, 29);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // connectionToolStripMenuItem
            // 
            this.connectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SecsConfig,
            this.OpenConnection,
            this.CloseConnection});
            this.connectionToolStripMenuItem.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F);
            this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            this.connectionToolStripMenuItem.Size = new System.Drawing.Size(110, 25);
            this.connectionToolStripMenuItem.Text = "Connection";
            // 
            // SecsConfig
            // 
            this.SecsConfig.Name = "SecsConfig";
            this.SecsConfig.Size = new System.Drawing.Size(215, 26);
            this.SecsConfig.Text = "SECS Config";
            this.SecsConfig.Click += new System.EventHandler(this.SecsConfig_Click);
            // 
            // OpenConnection
            // 
            this.OpenConnection.Name = "OpenConnection";
            this.OpenConnection.Size = new System.Drawing.Size(215, 26);
            this.OpenConnection.Text = "Open Connection";
            this.OpenConnection.Click += new System.EventHandler(this.OpenConnection_Click);
            // 
            // CloseConnection
            // 
            this.CloseConnection.Name = "CloseConnection";
            this.CloseConnection.Size = new System.Drawing.Size(215, 26);
            this.CloseConnection.Text = "Close Connection";
            this.CloseConnection.Click += new System.EventHandler(this.CloseConnection_Click);
            // 
            // SMText
            // 
            this.SMText.Font = new System.Drawing.Font("宋体", 9F);
            this.SMText.Location = new System.Drawing.Point(34, 72);
            this.SMText.MaxLength = 10000000;
            this.SMText.Multiline = true;
            this.SMText.Name = "SMText";
            this.SMText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.SMText.Size = new System.Drawing.Size(944, 458);
            this.SMText.TabIndex = 1;
            // 
            // SendMessage
            // 
            this.SendMessage.Font = new System.Drawing.Font("宋体", 15F);
            this.SendMessage.Location = new System.Drawing.Point(34, 28);
            this.SendMessage.Name = "SendMessage";
            this.SendMessage.Size = new System.Drawing.Size(135, 38);
            this.SendMessage.TabIndex = 2;
            this.SendMessage.Text = "SendMessage";
            this.SendMessage.UseVisualStyleBackColor = true;
            this.SendMessage.Click += new System.EventHandler(this.SendMessage_Click);
            // 
            // UiLog
            // 
            this.UiLog.Font = new System.Drawing.Font("宋体", 15F);
            this.UiLog.Location = new System.Drawing.Point(34, 556);
            this.UiLog.Multiline = true;
            this.UiLog.Name = "UiLog";
            this.UiLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.UiLog.Size = new System.Drawing.Size(944, 130);
            this.UiLog.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 698);
            this.Controls.Add(this.UiLog);
            this.Controls.Add(this.SendMessage);
            this.Controls.Add(this.SMText);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mian";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SecsConfig;
        private System.Windows.Forms.ToolStripMenuItem OpenConnection;
        private System.Windows.Forms.ToolStripMenuItem CloseConnection;
        private System.Windows.Forms.TextBox SMText;
        private System.Windows.Forms.Button SendMessage;
        private System.Windows.Forms.TextBox UiLog;
    }
}

