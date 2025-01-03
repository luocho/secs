namespace SECSMainServer
{
    partial class SecsConfigForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SECSModel = new System.Windows.Forms.ComboBox();
            this.IPValue = new System.Windows.Forms.TextBox();
            this.PortValue = new System.Windows.Forms.TextBox();
            this.DeviceIDValue = new System.Windows.Forms.TextBox();
            this.Save = new System.Windows.Forms.Button();
            this.Close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15F);
            this.label1.Location = new System.Drawing.Point(36, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15F);
            this.label2.Location = new System.Drawing.Point(36, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "DeviceID";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 15F);
            this.label3.Location = new System.Drawing.Point(36, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "PORT";
            // 
            // SECSModel
            // 
            this.SECSModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SECSModel.Font = new System.Drawing.Font("宋体", 15F);
            this.SECSModel.FormattingEnabled = true;
            this.SECSModel.Items.AddRange(new object[] {
            "Active",
            "Passive"});
            this.SECSModel.Location = new System.Drawing.Point(40, 162);
            this.SECSModel.Name = "SECSModel";
            this.SECSModel.Size = new System.Drawing.Size(112, 28);
            this.SECSModel.TabIndex = 3;
            // 
            // IPValue
            // 
            this.IPValue.Font = new System.Drawing.Font("宋体", 15F);
            this.IPValue.Location = new System.Drawing.Point(140, 35);
            this.IPValue.Name = "IPValue";
            this.IPValue.Size = new System.Drawing.Size(207, 30);
            this.IPValue.TabIndex = 4;
            // 
            // PortValue
            // 
            this.PortValue.Font = new System.Drawing.Font("宋体", 15F);
            this.PortValue.Location = new System.Drawing.Point(140, 75);
            this.PortValue.Name = "PortValue";
            this.PortValue.Size = new System.Drawing.Size(81, 30);
            this.PortValue.TabIndex = 5;
            this.PortValue.Text = "5000";
            this.PortValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IsNumber);
            // 
            // DeviceIDValue
            // 
            this.DeviceIDValue.Font = new System.Drawing.Font("宋体", 15F);
            this.DeviceIDValue.Location = new System.Drawing.Point(140, 118);
            this.DeviceIDValue.Name = "DeviceIDValue";
            this.DeviceIDValue.Size = new System.Drawing.Size(81, 30);
            this.DeviceIDValue.TabIndex = 6;
            this.DeviceIDValue.Text = "1";
            this.DeviceIDValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.IsNumber);
            // 
            // Save
            // 
            this.Save.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Save.Font = new System.Drawing.Font("宋体", 15F);
            this.Save.Location = new System.Drawing.Point(195, 264);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(82, 31);
            this.Save.TabIndex = 7;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Close
            // 
            this.Close.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.Close.Font = new System.Drawing.Font("宋体", 15F);
            this.Close.Location = new System.Drawing.Point(306, 264);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(82, 31);
            this.Close.TabIndex = 8;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // SecsConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 307);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.DeviceIDValue);
            this.Controls.Add(this.PortValue);
            this.Controls.Add(this.IPValue);
            this.Controls.Add(this.SECSModel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SecsConfigForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SECS Config";
            this.Load += new System.EventHandler(this.SecsConfigForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox SECSModel;
        private System.Windows.Forms.TextBox IPValue;
        private System.Windows.Forms.TextBox PortValue;
        private System.Windows.Forms.TextBox DeviceIDValue;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Close;
    }
}