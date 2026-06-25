using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.Entity;
using System.Configuration;
using System.IO;
using System.Data.Common;
//using System.Data.EntityClient;
using System.Net;
using SewingSystem.LinqModel;
using SewingSystem.Classes;
using DevExpress.XtraReports.Native;

namespace SewingSystem.Forms
{
    public partial class XtraFormConnectionDB : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction myfunaction = new Classes.MyFunaction();
        public XtraFormConnectionDB(string errMsg = "")
        {
            InitializeComponent();
            txt_notes.Text = errMsg;
        }
        private void rbSQL_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                layoutControlGroup2.Enabled = true;
                //txtSqlPassword.Enabled = true;
                txtSqlUserName.Focus();
                //txtSqlUserName.Text = Program.SqlUserName;
                //txtSqlPassword.Text = Program.SqlPassword;
            }
            else
            {
                layoutControlGroup2.Enabled = false;
                //txtSqlPassword.Enabled = false;
                txtSqlUserName.ResetText();
                txtSqlPassword.ResetText();
                btnSave.Focus();
            }
        }
        public void SettingServer()
        {
            Properties.Settings.Default.ServerName = Classes.MyFunaction.Encryption(txtServerName.Text);
            Properties.Settings.Default.DBName = Classes.MyFunaction.Encryption(txtDB_Name.Text);
            Properties.Settings.Default.Mode = rbWindows.Checked == true ? Classes.MyFunaction.Encryption("Windows") : Classes.MyFunaction.Encryption("SQL");
            Properties.Settings.Default.SqlUserName = Classes.MyFunaction.Encryption(txtSqlUserName.Text);
            Properties.Settings.Default.SqlPassword = Classes.MyFunaction.Encryption(txtSqlPassword.Text);
            Properties.Settings.Default.Save();
            MyFunaction.DecryptionSetting();
            try
            {
                if (Database.Exists(myfunaction.ConnectionString_DB()))
                {
                    MessageBox.Show("تم حفظ الاعدادات بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Close();
                    using (DataClasses1DataContext db = new DataClasses1DataContext(myfunaction.ConnectionString_DB()))
                    {
                        bool e = db.ExecuteQuery<bool>(string.Format("Select is_broker_enabled from sys.databases where name='{0}'", txtDB_Name.Text.Trim())).ToList()[0];
                        while (!e)
                        {
                            db.ExecuteCommand(string.Format("alter database {0} set enable_broker with rollback immediate", txtDB_Name.Text.Trim()));
                            e = db.ExecuteQuery<bool>(string.Format("Select is_broker_enabled from sys.databases where name='{0}'", txtDB_Name.Text.Trim())).ToList()[0];
                        }
                    }
                    Application.Restart();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                txtServerName.Focus();
                txtServerName.SelectAll();
                return;
            }
        }
        public void SettingLocalDB()
        {
            Properties.Settings.Default.PathLoNaDB = txtPathDB.Text;
            Properties.Settings.Default.Save();
            try
            {
                this.UseWaitCursor = true;
                MessageBox.Show("تم حفظ الاعدادات بنجاح", "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign);
                Close();
                Application.Restart();
            }
            catch (Exception ex)
            {
                this.UseWaitCursor = false;
                MessageBox.Show(ex.Message);
                txtPathDB.ResetText();
                return;
            }
        }
        private void btnSavSetting_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.PathLocalDB = false;
            Properties.Settings.Default.Save();
            try
            {
                //txtServerName.Text = IPAddress.Parse(txtServerName.Text).ToString();
                SettingServer();
            }
            catch (Exception)
            {
                SettingServer();
            }


            string executablePath = AppDomain.CurrentDomain.BaseDirectory;
            string configPath = Path.Combine(executablePath, "SewingSystem.exe.config");
            FileFolderHelper.SetFilePermissions(configPath);

            string newConnectionString = DatabaseHelper.GetConnectionString(
                Properties.Settings.Default.DBName,
                Properties.Settings.Default.ServerName,
                Properties.Settings.Default.SqlUserName,
                Properties.Settings.Default.SqlPassword,
                Properties.Settings.Default.Mode == "Windows"
                );

            DatabaseHelper.UpdateConnectionString(Program.DBName_static + "Entities", newConnectionString);

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {

            xtraOpenFileDialog1.Filter = "DataBase Files|*.mdf";
            if (xtraOpenFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(xtraOpenFileDialog1.FileName))
                {
                    txtPathDB.Text = xtraOpenFileDialog1.FileName;
                    //btnSav.Enabled = true;
                    //btnRestor.Focus();
                }
            }
        }

        private void btnSave1_Click(object sender, EventArgs e)
        {
            bool valdition = true;
            if (txtPathDB.Text.Trim() == "")
            {
                MessageBox.Show("رجاء حدد مسار قاعدة البيانات اولا");
                valdition = false;
            }
            if (valdition)
            {
                Properties.Settings.Default.PathLocalDB = true;
                Properties.Settings.Default.Save();
                SettingLocalDB();
            }
        }

        private void btnCancel1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}