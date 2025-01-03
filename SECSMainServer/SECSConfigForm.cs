using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using SECSEntity;
using SECSPublisher;

namespace SECSMainServer
{
    public partial class SecsConfigForm : Form
    {
        public SecsConfigForm()
        {
            InitializeComponent();
        }
        private void SecsConfigForm_Load(object sender, EventArgs e)
        {
            try
            {
                #region 初始化界面数据
                IPValue.Text = MainForm.SECSInfo.IP;
                PortValue.Text = MainForm.SECSInfo.Port.ToString();
                DeviceIDValue.Text = MainForm.SECSInfo.DeviceID.ToString();
                SECSModel.SelectedIndex = (int)MainForm.SECSInfo.connectionModel;
                #endregion
            }
            catch (Exception ex)
            {
                Logger.SECSLogger.Error($"Function {ex.TargetSite.Name} Error:{ex.Message}");
                throw;
            }
           
        }

        private void IsNumber(object sender, KeyPressEventArgs e)
        {
            //判断按键是不是要输入的类型。
            if (((int)e.KeyChar < 48 || (int)e.KeyChar > 57) && (int)e.KeyChar != 8 && (int)e.KeyChar != 46)
                e.Handled = true;

            //小数点的处理。
            if ((int)e.KeyChar == 46)                           //小数点
            {
                if (IPValue.Text.Length <= 0)
                    e.Handled = true;   //小数点不能在第一位
                else
                {
                    float f;
                    float oldf;
                    bool b1 = false, b2 = false;
                    b1 = float.TryParse(IPValue.Text, out oldf);
                    b2 = float.TryParse(IPValue.Text + e.KeyChar.ToString(), out f);
                    if (b2 == false)
                    {
                        if (b1 == true)
                            e.Handled = true;
                        else
                            e.Handled = false;
                    }
                }
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IPAddress.TryParse(IPValue.Text, out _))
                {
                    MessageBox.Show("IP is invalid", "Error", MessageBoxButtons.OK);
                    return;
                }
                #region Save
                MainForm.SECSInfo.IP = IPValue.Text;
                MainForm.SECSInfo.Port = Convert.ToInt32(PortValue.Text);
                MainForm.SECSInfo.DeviceID = Convert.ToInt32(DeviceIDValue.Text);
                MainForm.SECSInfo.connectionModel = (SECSEntity.ConnectionModel)SECSModel.SelectedIndex;
                #endregion
                MessageBox.Show("Save SECS Configuration OK", "Successful", MessageBoxButtons.OK);
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.SECSLogger.Error($"Function {ex.TargetSite.Name} Error:{ex.Message}");
                throw;
            }
            
        }
    }
}
