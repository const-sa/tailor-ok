using DevExpress.XtraEditors;
using SewingSystem.Classes;
using System;
using System.Windows.Forms;

namespace SewingSystem.Forms
{
    public partial class FormTrailActivation : XtraForm
    {
        private ClsEncryption clsEncryp = new ClsEncryption();

        public FormTrailActivation()
        {
            InitializeComponent();
        }

        private void FormTrailActivation_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            SaveSettings();
        }

        private void SaveSettings()
        {
            if (!ValidateData()) return;
            if (!ValidatePassword()) return;

            SaveNewSettings();
            DialogResult = DialogResult.OK;
        }

        private void SaveNewSettings()
        {
         Properties.Settings.Default.accConFlag = this.clsEncryp.EncryptString(ClsHardDriveSerial.HDDSerieal() + textEditIsPurchased.Text);
         Properties.Settings.Default.accConDt = DateTime.Now;
            if (!String.IsNullOrEmpty(textEditTrailDays.Text))
             Properties.Settings.Default.accConVal = this.clsEncryp.EncryptString(textEditTrailDays.Text + "-" + ClsHardDriveSerial.HDDSerieal());
            Properties.Settings.Default.Save();
        }

        private bool ValidateData()
        {
            if (dxValidationProvider1.Validate()) return true;
            textEditPassword.Focus();
            return false;
        }
        string DateNow = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}";
        private bool ValidatePassword()
        {
            if (textEditPassword.Text == DateNow) return true;

            ClsXtraMssgBox.ShowError("PasswordError!");
            textEditPassword.Focus();
            return false;
        }
    }
}