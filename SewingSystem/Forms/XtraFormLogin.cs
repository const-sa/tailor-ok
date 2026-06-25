using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
//using SewingSystem.LinqModel;
using System;
using System.Threading;
using System.Globalization;
using DevExpress.XtraSplashScreen;
using SewingSystem.Classes;
using System.Windows.Input;
using System.Data.Entity;
using DevExpress.Utils;
using System.Diagnostics;

namespace SewingSystem.Forms
{
    public partial class XtraFormLogin : DevExpress.XtraEditors.XtraForm
    {
        public XtraFormLogin()
        {
            if (Keyboard.IsKeyDown(Key.LeftShift)) new XtraFormConnectionDB().ShowDialog();
            InitializeComponent();
            this.Load += XtraFormLogin_Load;
        }

        private void XtraFormLogin_Load(object sender, EventArgs e)
        {
            this.KeyDown += FormLogin_KeyDown;
            WindowsFormsSettings.DefaultFont =
               AppearanceObject.DefaultFont =
               AppearanceObject.DefaultMenuFont =
               WindowsFormsSettings.DefaultFont =
               AppearanceObject.DefaultFont = Properties.Settings.Default.SystemFont;
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInstallation()) return;
                //if (!CheckTrailVersion()) return;
                if (!Database.Exists(Program.ConnectionString))
                {
                    XtraMessageBox.Show("لا يوجد اتصال بقاعدة البيانات !!!! تاكد من الاتصال بالانترنت ؟", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (Properties.Settings.Default.Remember_me)
                {
                    Properties.Settings.Default.RememberUserName = UserNameTextEdit.Text.ToString();
                    Properties.Settings.Default.RememberLang = comboBoxEditLang.EditValue.ToString();
                    Properties.Settings.Default.RememberBranch = (BranchIDTextEdit.EditValue as short?) ?? 0;
                    Properties.Settings.Default.Remember_me = true;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.RememberUserName = "";
                    Properties.Settings.Default.RememberLang = "";
                    Properties.Settings.Default.Remember_me = false;
                    Properties.Settings.Default.Save();
                }
                if (comboBoxEditLang.Text == "اللغة العربية")
                {
                    Properties.Settings.Default.Language = "ar-SA";
                    Properties.Settings.Default.Save();
                }
                else if (comboBoxEditLang.Text == "English")
                {
                    Properties.Settings.Default.Language = "en-US";
                    Properties.Settings.Default.Save();
                }
                LinqModel.tblUser User = new LinqModel.tblUser();
                if (BranchIDTextEdit.EditValue != null)
                    User.BranchID = (BranchIDTextEdit.EditValue as short?) ?? 0;
                else if (Session.tblBranche != null)
                    User.BranchID = (Session.tblBranche.FirstOrDefault().ID as short?) ?? 0;
                if (UserNameTextEdit.Text.Trim() != string.Empty)
                    User.UserName = UserNameTextEdit.Text.Trim().ToString();
                if (UserPasswordTextEdit.Text.Trim() != string.Empty)
                    User.UserPassword = UserPasswordTextEdit.Text.Trim().ToString();
                var CurruntUser = Session.tblUser.Where(u => u.UserName == User.UserName & u.UserPassword == User.UserPassword & u.BranchID == User.BranchID);
                if (CurruntUser.Count() > 0)
                {
                    Program.User = CurruntUser.FirstOrDefault();
                    Program.Branch = Session.tblBranche.SingleOrDefault(b => b.ID == Program.User.BranchID);
                    Program.LogInUser = true;
                    Close();
                }
                else if (CurruntUser.Count() > 1)
                {
                    if (Properties.Settings.Default.Language == "ar-SA")
                        XtraMessageBox.Show("يوجد اكثر من مستخدم بنفس البيانات قم بمراجعة مدير النظام", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (Properties.Settings.Default.Language == "en-US")
                        XtraMessageBox.Show("There is more than one user with the same data, refer to the system administrator", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    if (Properties.Settings.Default.Language == "ar-SA")
                        XtraMessageBox.Show(/*BranchIDTextEdit.EditValue.ToString() + User.UserName.ToString() + User.UserPassword.ToString() +*/ "       اسم المستخدم او كلمة المرور غير صحيحة", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else if (Properties.Settings.Default.Language == "en-US")
                        XtraMessageBox.Show("Username or password is incorrect!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }

        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void comboBoxEditLang_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (e.NewValue.ToString() == "اللغة العربية")
            {
                Properties.Settings.Default.Language = "ar-SA";
                Properties.Settings.Default.Save();
            }
            else if (e.NewValue.ToString() == "English")
            {
                Properties.Settings.Default.Language = "en-US";
                Properties.Settings.Default.Save();
            }
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(Properties.Settings.Default.Language);
        }

        private void LogIn_Shown(object sender, EventArgs e)
        {
            SplashScreenManager.CloseForm();
        }
        private void LogIn_Load(object sender, EventArgs e)
        {
            tblBranchesBindingSource.DataSource = Session.tblBranche;
            UserNameTextEdit.EditValue = Properties.Settings.Default.RememberUserName;
            comboBoxEditLang.EditValue = Properties.Settings.Default.RememberLang;
            BranchIDTextEdit.EditValue = Properties.Settings.Default.RememberBranch;
            UserPasswordTextEdit.Focus();
        }
        private bool ValidateInstallation()
        {
            if (string.IsNullOrWhiteSpace(Properties.Settings.Default.accConFlag) ||
                Properties.Settings.Default.accConDt == new DateTime(0001, 1, 1))
            {
                if (Program.is5DaysTrail)
                {
                    DAL.Class1 cls = new DAL.Class1();
                    int value = cls.SettingGetValue();
                    if (value < Program.TargetValue)
                    {
                        return true;
                    }
                }



                XtraMessageBox.Show(this, "للمتابعة يرجى شراء النظام!");
                Application.Exit();
                return false;
            }
            return true;
        }

        string DateNow = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
        private ClsEncryption clsEncrp = new ClsEncryption();

        private bool CheckTrailVersion()
        {
            //if (Debugger.IsAttached)
            //    return true;
            string thisComputerSerail = ClsHardDriveSerial.HDDSerieal();
            string flagStr = this.clsEncrp.DecryptString(Properties.Settings.Default.accConFlag);

            string serial = flagStr.Substring(0, flagStr.Length - 1);
            string flag = flagStr.Substring(flagStr.Length - 1, 1);
            if (serial != thisComputerSerail)
            {
                XtraMessageBox.Show("عذرا لقد تم انتهاء مدة النسخة التجريبية! \n" + "يرجى شراء النظام للمتابعة.");
                Application.Exit();
                return false;
            }
            if (flag == "T") return true;

            int val = SetTrailVersionRemaingDays();

            if (val <= 0)
            {
                XtraMessageBox.Show("عذرا لقد تم انتهاء مدة النسخة التجريبية! \n" + "يرجى شراء النظام للمتابعة.");
                Application.Exit();
                return false;
            }
            return true;
        }

        private int SetTrailVersionRemaingDays()
        {
            DateTime dtOld = Properties.Settings.Default.accConDt;
            string conValStr = this.clsEncrp.DecryptString(Properties.Settings.Default.accConVal);
            int conVal = Convert.ToInt32(conValStr.Substring(0, conValStr.IndexOf("-")));

            if (dtOld.Date != DateTime.Now.Date)
            {
                Properties.Settings.Default.accConDt = DateTime.Now.Date;
                conVal -= 1;
                Properties.Settings.Default.accConVal = this.clsEncrp.EncryptString(conVal.ToString() + "-" + ClsHardDriveSerial.HDDSerieal());
                Properties.Settings.Default.Save();
            }
            return conVal;
        }

        private void FormLogin_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (UserPasswordTextEdit.Text == DateNow /*&& UserNameTextEdit.Text == string.Empty*/)
                    if (new FormTrailActivation().ShowDialog() == DialogResult.OK)
                        ClsXtraMssgBox.ShowInformation("Save Succefully");
                if (!dxValidationProvider1.Validate()) return;
                btnLogIn.PerformClick();
            }
        }
    }
   
}