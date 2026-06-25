using DevExpress.XtraEditors;
using SewingSystem.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace SewingSystem.Forms
{
    public partial class XtraFormBackupSett : DevExpress.XtraEditors.XtraForm
    {
        public XtraFormBackupSett()
        {
            InitializeComponent();
        }

        private void XtraFormBackupSett_Load(object sender, EventArgs e)
        {
            txt_days.Text = Properties.Settings.Default.BackupFrequencyDays.ToString();
            txt_Hours.Text = Properties.Settings.Default.BackupFrequencyHours.ToString();
            txt_path.Text = Properties.Settings.Default.BackupPath.ToString();

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            //CustNotes.Text = ConfigurationManager.AppSettings["BackupFrequencyDays"];

            //string newValue = CustName.Text;
            //UpdateAppConfig("BackupFrequencyDays", newValue); 
            //MessageBox.Show("Value saved successfully!");


            if (int.TryParse(txt_days.Text, out int days)
                && int.TryParse(txt_Hours.Text, out int Hours))
            {
                if (days < 0 || Hours < 0)
                {
                    MessageBox.Show("يرجى ادخال قيمة غير سالبة");
                    return;
                }

                if (days == 0 && Hours == 0)
                {
                    MessageBox.Show("لا يمكن لكلا القيمتين أن تساويا الصفر");
                    return;
                }

                Properties.Settings.Default.BackupFrequencyDays = days;
                Properties.Settings.Default.BackupFrequencyHours = Hours;
                Properties.Settings.Default.BackupPath = txt_path.Text;
                Properties.Settings.Default.Save();

                MessageBox.Show("تمت العملية بنجاح");
            }
            else
            {
                MessageBox.Show("يرجى ادخال قيمة صحيحة");
            }
        }

        private void btn_backup_Click(object sender, EventArgs e)
        {
            string dbName = Properties.Settings.Default.DBName.ToString();

            string ConString = Program.ConnectionString;

            string backupDirectory = Properties.Settings.Default.BackupPath.ToString();

            bool b = MyFunaction.BackupDatabase(ConString, backupDirectory);

            if (b)
            {
                MessageBox.Show("تمت العملية بنجاح");
            }

        }

        private void btnBrows_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txt_path.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        //private void UpdateAppConfig(string key, string value)
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

        //    XmlNode appSettingsNode = xmlDoc.SelectSingleNode("configuration/appSettings");
        //    if (appSettingsNode != null)
        //    {
        //        XmlElement element = (XmlElement)appSettingsNode.SelectSingleNode($"//add[@key='{key}']");
        //        if (element != null)
        //        {
        //            element.SetAttribute("value", value);
        //        }
        //        else
        //        {
        //            XmlElement newElement = xmlDoc.CreateElement("add");
        //            newElement.SetAttribute("key", key);
        //            newElement.SetAttribute("value", value);
        //            appSettingsNode.AppendChild(newElement);
        //        }
        //        xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
        //        ConfigurationManager.RefreshSection("appSettings");
        //    }
        //}
    }
}