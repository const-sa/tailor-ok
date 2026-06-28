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

        // --- "نوع الاتصال" (داخلي / خارجي) + مؤشر الحالة (أحمر/أخضر) ---
        private DevExpress.XtraEditors.PanelControl pnlConnType;
        private RadioButton rbInternal;   // داخلي
        private RadioButton rbExternal;   // خارجي
        private DevExpress.XtraEditors.SimpleButton btnTest; // اختبار الاتصال
        private PictureBox picStatus;     // الدائرة الحمراء/الخضراء
        private Label lblStatus;          // متصل / غير متصل
        private bool _loadingConnType;    // يمنع تطبيق الإعداد المسبق أثناء التهيئة

        public XtraFormConnectionDB(string errMsg = "")
        {
            InitializeComponent();
            BuildConnTypeUi();
            txt_notes.Text = errMsg;
            PrefillCurrentSettings();
        }

        /// <summary>
        /// يبني شريط اختيار نوع الاتصال (داخلي/خارجي) مع زر اختبار الاتصال
        /// ومؤشر الحالة (دائرة حمراء = غير متصل، خضراء = متصل) أعلى تبويب الخادم.
        /// </summary>
        private void BuildConnTypeUi()
        {
            this.Height += 52; // مساحة إضافية للشريط الجديد

            pnlConnType = new DevExpress.XtraEditors.PanelControl();
            pnlConnType.Dock = DockStyle.Top;
            pnlConnType.Height = 92;
            pnlConnType.RightToLeft = RightToLeft.Yes;

            lblConnTitle = new Label
            {
                Text = "نوع الاتصال:",
                Font = new Font("Tahoma", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 0, 0),
                AutoSize = true
            };

            rbInternal = new RadioButton
            {
                Text = "داخلي (خادم محلي)",
                Font = new Font("Tahoma", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 0, 0),
                Size = new Size(200, 24),
                RightToLeft = RightToLeft.Yes
            };
            rbExternal = new RadioButton
            {
                Text = "خارجي (خادم بعيد)",
                Font = new Font("Tahoma", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(64, 0, 0),
                Size = new Size(200, 24),
                RightToLeft = RightToLeft.Yes
            };
            rbInternal.CheckedChanged += rbConnType_CheckedChanged;
            rbExternal.CheckedChanged += rbConnType_CheckedChanged;

            btnTest = new DevExpress.XtraEditors.SimpleButton
            {
                Text = "اختبار الاتصال",
                Size = new Size(130, 30)
            };
            btnTest.Click += btnTest_Click;

            picStatus = new PictureBox { SizeMode = PictureBoxSizeMode.AutoSize };
            lblStatus = new Label
            {
                Text = "غير متصل",
                Font = new Font("Tahoma", 8F, FontStyle.Bold),
                ForeColor = Color.FromArgb(214, 48, 48),
                AutoSize = true
            };

            pnlConnType.Controls.Add(lblConnTitle);
            pnlConnType.Controls.Add(rbInternal);
            pnlConnType.Controls.Add(rbExternal);
            pnlConnType.Controls.Add(btnTest);
            pnlConnType.Controls.Add(picStatus);
            pnlConnType.Controls.Add(lblStatus);

            // يوضع داخل تبويب "الاتصال بالخادم" أعلى عناصر النموذج.
            ServerConnect.Controls.Add(pnlConnType);
            pnlConnType.BringToFront();

            // التخطيط يدويًا حسب العرض الفعلي للوحة (يدعم RTL وتغيير الحجم).
            pnlConnType.SizeChanged += (s, e) => LayoutConnTypePanel();
            LayoutConnTypePanel();

            SetStatus(false, "غير متصل");
        }

        private Label lblConnTitle;
        private void LayoutConnTypePanel()
        {
            int w = pnlConnType.ClientSize.Width;
            // الجانب الأيمن: العنوان + خياري داخلي/خارجي.
            lblConnTitle.Location = new Point(w - lblConnTitle.Width - 12, 10);
            rbInternal.Location = new Point(w - rbInternal.Width - 12, 32);
            rbExternal.Location = new Point(w - rbExternal.Width - 12, 58);
            // الجانب الأيسر: زر الاختبار + مؤشر الحالة.
            btnTest.Location = new Point(12, 32);
            picStatus.Location = new Point(12, 66);
            lblStatus.Location = new Point(32, 66);
        }

        private void SetStatus(bool connected, string text)
        {
            picStatus.Image = Classes.ConnectionStatus.Dot(connected, 14);
            lblStatus.Text = text;
            lblStatus.ForeColor = connected
                ? Color.FromArgb(46, 184, 92)
                : Color.FromArgb(214, 48, 48);
        }

        /// <summary>عند اختيار المستخدم لنوع الاتصال يُعبّأ الإعداد المسبق المناسب.</summary>
        private void rbConnType_CheckedChanged(object sender, EventArgs e)
        {
            if (_loadingConnType) return;
            if (!((RadioButton)sender).Checked) return;

            if (rbExternal.Checked)
            {
                txtServerName.Text = Classes.ConnectionStatus.ExternalServer;
                txtDB_Name.Text = Classes.ConnectionStatus.ExternalDB;
                rbSQL.Checked = true;
                txtSqlUserName.Text = Classes.ConnectionStatus.ExternalUser;
                txtSqlPassword.Text = Classes.ConnectionStatus.ExternalPassword;
            }
            else
            {
                txtServerName.Text = Classes.ConnectionStatus.InternalServer;
                txtDB_Name.Text = Classes.ConnectionStatus.InternalDB;
                rbWindows.Checked = true;
            }
            SetStatus(false, "غير متصل");
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            this.UseWaitCursor = true;
            string err;
            bool ok = Classes.ConnectionStatus.Test(
                txtServerName.Text.Trim(),
                txtDB_Name.Text.Trim(),
                rbWindows.Checked,
                txtSqlUserName.Text.Trim(),
                txtSqlPassword.Text,
                out err);
            this.UseWaitCursor = false;
            SetStatus(ok, ok ? "متصل" : "غير متصل");
            if (!ok)
                MessageBox.Show("تعذّر الاتصال:\n" + err, "اختبار الاتصال",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Pre-fills the dialog with the connection that is currently in effect
        /// (decrypted into Program.* at startup) so the user just edits the values
        /// instead of retyping the whole connection by hand.
        /// </summary>
        private void PrefillCurrentSettings()
        {
            // تحديد نوع الاتصال المحفوظ دون استبدال القيم الحالية (يتم تطبيق
            // الإعداد المسبق فقط عند اختيار المستخدم لاحقًا).
            _loadingConnType = true;
            if (Program.ConnType == Classes.ConnectionStatus.External)
                rbExternal.Checked = true;
            else
                rbInternal.Checked = true;
            _loadingConnType = false;

            txtServerName.Text = Program.ServerName;
            txtDB_Name.Text = Program.DBName;
            if (Program.Mode == "SQL")
            {
                rbSQL.Checked = true;
                txtSqlUserName.Text = Program.SqlUserName;
                txtSqlPassword.Text = Program.SqlPassword;
            }
            else
            {
                rbWindows.Checked = true;
            }
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
            Properties.Settings.Default.ConnType = Classes.MyFunaction.Encryption(
                rbExternal.Checked ? Classes.ConnectionStatus.External : Classes.ConnectionStatus.Internal);
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

            // Use the DECRYPTED connection values (Program.* are refreshed by
            // MyFunaction.DecryptionSetting() inside SettingServer above). The
            // raw Properties.Settings values are AES-encrypted, so passing them
            // here previously wrote the encrypted base64 as the server/catalog
            // and forced SQL auth (Mode != "Windows"), producing a connection
            // string that EF could never open ("The underlying provider failed
            // on Open.").
            string newConnectionString = DatabaseHelper.GetConnectionString(
                Program.DBName,
                Program.ServerName,
                Program.SqlUserName,
                Program.SqlPassword,
                Program.Mode == "Windows"
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