using Newtonsoft.Json;
using SECSEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using SECSPublisher;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using HSMSHandler;
using System.Threading;
using SecsMessageEntity;
using static SECSPublisher.Logger;
namespace SECSMainServer
{
    public partial class MainForm : Form
    {
        #region 初始化
        public static SETEntity SECSInfo = new SETEntity();
        string SECSConfigPath = ReadOptions("SECSConfigPath");
        HsmsConnection hsms = HsmsConnection.Instance;
        #endregion
        public MainForm()
        {
            InitializeComponent();
        }

        private void SecsConfig_Click(object sender, EventArgs e)
        {
            SecsConfigForm secsConfig = new SecsConfigForm();
            secsConfig.ShowDialog();
        }

        private void OpenConnection_Click(object sender, EventArgs e)
        {
            try
            {
                hsms.OpenConnect();
               
            }
            catch (Exception ex)
            {
                Logger.SECSLogger.Error($"Function {ex.TargetSite.Name} Error:{ex.Message}");
                throw ex;
            }
        }
        private static string ReadOptions(string KeyName)
        {
            return ConfigurationManager.AppSettings["SECSConfigPath"];
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            SECSInfo = JosnHandler.ReadJosn<SETEntity>(SECSConfigPath);
            SECSInfo.Init(SECSInfo);
            Logger.LogWritten += OnLogWritten;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //保存信息到文件中
            JosnHandler.WriteJosn(SECSInfo, SECSConfigPath);
        }

        private void SendMessage_Click(object sender, EventArgs e)
        {
            if (SMText.Text!="")
            {
                if (hsms.IsConnection)
                {
                    SecsMessage sm = new SecsMessage();
                    sm.SecsHead.DeviceID = (ushort)SECSInfo.DeviceID;
                    sm.StringToSecsMessage(SMText.Text);
                    hsms.SendMessage(sm.ToBytes());
                }
                else
                {
                    MessageBox.Show("No One Connection!","错误",MessageBoxButtons.OK);
                }
            }
        }
        private void OnLogWritten(object sender, LogEventArgs e)
        {
            if (UiLog.InvokeRequired)
            {
                UiLog.Invoke((Action)(() => UiLog.AppendText(e.LogMessage + Environment.NewLine)));
            }
            else
            {
                // 更新界面上的日志输出
                UiLog.AppendText(e.LogMessage + Environment.NewLine);
            }
            
        }

        private void CloseConnection_Click(object sender, EventArgs e)
        {
            hsms.Disconnect();
        }
    }
}
