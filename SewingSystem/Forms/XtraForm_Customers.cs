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
using SewingSystem.LinqModel;
using System.Data.Entity;
using SewingSystem.Classes;
using System.Collections.ObjectModel;
using DevExpress.XtraEditors.Repository;
using System.Threading;
using System.Net;
using System.IO;
using DevExpress.XtraSplashScreen;
using static SewingSystem.Classes.WhatsappMessage;
using static SewingSystem.Classes.Master;

namespace SewingSystem.Forms
{
    public partial class XtraForm_Customers : DevExpress.XtraEditors.XtraForm
    {

        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();


        tblCustomer CurrCustomer => tblCustomerBindingSource.Current as tblCustomer;
        WhatsappMessage whatsappMessage = new WhatsappMessage();

        public XtraForm_Customers()
        {
            InitializeComponent();

            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "العملاء" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            btnAddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues ?? false;
            btnDelete.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Delete)?.TheValues ?? false;
            btnUpdate.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues ?? false;

          
            btnSms.Click += btnSearchByMobil_Click;
            BtnSendWhatsApp.Click += BtnSendWhatsApp_Click;
        }

        bool isNew = false;

        ComponentFlyoutDialog flyDialog = new ComponentFlyoutDialog();
        private async void btnRefreash_Click(object sender, EventArgs e)
        {
            flyDialog.WaitForm(this, 1);
            await Program.InitObjects();
            flyDialog.WaitForm(this, 0);
        }

        private void AddNew_Click(object sender, EventArgs e)
        {
            CurrCustomer.EnterTime = DateTime.Now;
            CurrCustomer.BranchID = Program.User.BranchID;
            CurrCustomer.UserID = Program.User.ID;
            isNew = true;
            EnableOrDisyble(false);
        }

        public void EnableOrDisyble(bool state)
        {
            btnAddNew.Enabled = btnDelete.Enabled = btnUpdate.Enabled = btnRefreash.Enabled =
                CustName.ReadOnly = CustNotes.ReadOnly = CustMobil.ReadOnly = CustTelephon.ReadOnly = CustEmail.ReadOnly = state;
            btnSave.Enabled = btnNoSave.Enabled = !state;
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                tblCustomerBindingSource.EndEdit();

                if (CurrCustomer != null)
                {
                    flyDialog.WaitForm(this, 1);
                    string mssg = $"العميل: {CurrCustomer?.CustomerName} ";
                    using (var db = new DataClasses1DataContext(Program.ConnectionString))
                    {
                        int id = CurrCustomer.ID;
                        if (isNew)
                        {
                            if (!Session.tblCustomer.Where(s => s.BranchID == Program.User.BranchID).Any())
                                CurrCustomer.CusNumber = 1;
                            else
                                CurrCustomer.CusNumber = db.tblCustomers.Where(s => s.BranchID == Program.User.BranchID).Max(i => i.CusNumber) + 1;
                        }
                        else
                            db.tblCustomers.DeleteAllOnSubmit(db.tblCustomers.Where(m => m.ID == CurrCustomer.ID));
                        db.tblCustomers.InsertOnSubmit(CurrCustomer);
                        db.SubmitChanges();

                    }
                    if (isNew && Program.Branch.SendMessOnPreparation)
                    {
                        try
                        {
                            var cust = CurrCustomer;
                            if (cust.Mobil.Length >= 9)
                            {
                                switch ((MessageType)Program.Branch.MessageType)
                                {
                                    case MessageType.Sms:
                                        if (string.IsNullOrWhiteSpace(Program.Branch.SmsUserName) ||
                                         string.IsNullOrWhiteSpace(Program.Branch.SmsPassword) ||
                                         string.IsNullOrWhiteSpace(Program.Branch.SmsSenderName) ||
                                            string.IsNullOrWhiteSpace(Program.Branch.WelcomeMessage)) { }
                                        else
                                            whatsappMessage.SendSMS(cust.Mobil, Program.Branch.WelcomeMessage);
                                        break;
                                    case MessageType.WhatsApp:
                                        if (string.IsNullOrWhiteSpace(Program.Branch.access_token) ||
                                            string.IsNullOrWhiteSpace(Program.Branch.instance_id) ||
                                            string.IsNullOrWhiteSpace(Program.Branch.WelcomeMessage)) { }
                                        else
                                            Task.Run(() => whatsappMessage.SendJsonToUrl(whatsappMessage.Getwaclient(cust, null, "text", "", Program.Branch.WelcomeMessage)));
                                        break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    if (Session.tblCustomer.Any(v => v.ID == CurrCustomer.ID))
                    {
                        int index = Session.tblCustomer.IndexOf(Session.tblCustomer.Single(x => x.ID == CurrCustomer.ID));
                        Session.tblCustomer.Remove(Session.tblCustomer.Single(x => x.ID == CurrCustomer.ID));
                        Session.tblCustomer.Insert(index, CurrCustomer);
                    }
                    else
                        Session.tblCustomer.Add(CurrCustomer);
                    flyDialog.WaitForm(this, 0);
                    flyDialog.ShowDialogForm(this, mssg, this.isNew);
                    isNew = false;
                }
                EnableOrDisyble(true);
            }
            catch (Exception ex)
            {
                flyDialog.WaitForm(this, 0);
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }

        private void XtraForm_Customers_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            RefrechData();
        }

        public void RefrechData()
        {
            tblCustomerBindingSource.DataSource = Session.tblCustomer.ToList();
            //tblBranchBindingSource.DataSource = Session.tblBranche.ToList();
            //tblUserBindingSource.DataSource = Session.tblUser.ToList();
            //searchLookUpEditInvo.Properties.DataSource = Session.tblSellInvoice.ToList();
            //if (CurrCustomer == null)
            //    return;
            //tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(c => c.CusNumber == CurrCustomer.CusNumber & c.BranchID == CurrCustomer.BranchID).ToList();
            //tblPaymentBindingSource.DataSource = Session.tblPayment.Where(c => c.CusNumber == CurrCustomer.CusNumber & c.BranchID == CurrCustomer.BranchID).ToList();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            isNew = false;
            EnableOrDisyble(false);
        }

        private void searchLookUpEditCustomer_EditValueChanged(object sender, EventArgs e)
        {

        }


        private void BtnSendWhatsApp_Click(object sender, EventArgs e)
        {
            if (CurrCustomer == null) return;
            Properties.Settings.Default.SMS_Message = txtMessageSMS.Text;
            Properties.Settings.Default.Save();
            if (CustEmail.Text == string.Empty)
            {
                XtraMessageBox.Show("قم باضافة رقم الجوال لهذا العميل", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                if (string.IsNullOrWhiteSpace(Program.Branch.access_token))
                    XtraMessageBox.Show("access_token غير موجود قم باضافة access_token من شاشة بيانات المؤسسة", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (string.IsNullOrWhiteSpace(Program.Branch.instance_id))
                    XtraMessageBox.Show("instance_id غير موجود قم باضافة instance_id من شاشة بيانات المؤسسة", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (string.IsNullOrWhiteSpace(txtMessageSMS.Text))
                    XtraMessageBox.Show("قم بكتابة الرسالة النصية!!!! ", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var cust = CurrCustomer;
                if (cust.Mobil.Length >= 9)
                    Task.Run(() => whatsappMessage.SendJsonToUrl(whatsappMessage.Getwaclient(CurrCustomer, null, "text", "", txtMessageSMS.Text)));
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private void btnSearchByMobil_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SMS_Message = txtMessageSMS.Text;
            Properties.Settings.Default.Save();
            if (CustEmail.Text == string.Empty)
            {
                XtraMessageBox.Show("قم باضافة رقم الجوال لهذا العميل", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            whatsappMessage.SendSMS(CustEmail.Text, txtMessageSMS.Text);
        }
    }
}