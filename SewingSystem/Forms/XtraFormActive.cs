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
using System.Management;

namespace SewingSystem.Forms
{
    public partial class XtraFormActive : DevExpress.XtraEditors.XtraForm
    {
        public XtraFormActive()
        {
            InitializeComponent();
        }
        public static string get_cpu()
        {
            ManagementClass mc = new ManagementClass("win32_processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                return mo.Properties["processorID"].Value.ToString();
            }
            return string.Empty;
        }
        private void XtraFormActive_Load(object sender, EventArgs e)
        {
            serial_tb.Text= get_cpu();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            
            int date = DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day;
            string the_serial = serial_tb.Text;
            string code = the_serial + date.ToString();
            if (code == activate_tb.Text)
            {
                // حفظ التفعيل في قاعدة البيانات (مرتبط برقم المعالج) بدل ملف App.config المحلي
                try
                {
                    new Classes.ActivationConfig
                    {
                        IsActivated = true,
                        MachineSerial = the_serial,
                        ActivatedAt = DateTime.Now
                    }.Save();
                }
                catch (Exception ex) { XtraMessageBox.Show("تعذّر حفظ التفعيل في قاعدة البيانات: " + ex.Message); }

                Properties.Settings.Default.is_activated = true; // إبقاء التوافق مع الإعداد القديم
                Properties.Settings.Default.Save();
                XtraMessageBox.Show("تم التفعيل بنجاح\nيرجى أعادة تشغيل البرنامج");
                Application.Restart();
            }
            else
                XtraMessageBox.Show("كود التنشيط غير صحيح","خطاء",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }

        private void BtnCopySerail_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(serial_tb.Text);
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}