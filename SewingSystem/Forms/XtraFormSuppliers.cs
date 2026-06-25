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
    public partial class XtraFormSuppliers : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();


        tblSupplier CurrCustomer => tblCustomerBindingSource.Current as tblSupplier;


        public XtraFormSuppliers()
        {
            InitializeComponent();

            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "الموردون" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            btnAddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues ?? false;
            btnDelete.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Delete)?.TheValues ?? false;
            btnUpdate.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues ?? false;


           
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
                if (CurrCustomer != null)
                {
                    flyDialog.WaitForm(this, 1);
                    string mssg = $"المورد: {CurrCustomer?.SupName} ";
                    using (var db = new DataClasses1DataContext(Program.ConnectionString))
                    {
                        int id = CurrCustomer.ID;
                        if (isNew)
                        {
                            if (!Session.tblSupplier.Where(s => s.BranchID == Program.User.BranchID).Any())
                                CurrCustomer.SupNumber = 1;
                            else
                                CurrCustomer.SupNumber = db.tblSuppliers.Where(s => s.BranchID == Program.User.BranchID).Max(i => i.SupNumber) + 1;
                        }
                        else
                            db.tblSuppliers.DeleteAllOnSubmit(db.tblSuppliers.Where(m => m.ID == CurrCustomer.ID));
                        db.tblSuppliers.InsertOnSubmit(CurrCustomer);
                        db.SubmitChanges();

                    }
                    //if (isNew && Program.Branch.SendMessOnPreparation)
                    //{
                    //    try
                    //    {
                    //        var cust = CurrCustomer;
                    //        if (cust.Mobile.Length >= 9)
                    //        {
                    //            switch ((MessageType)Program.Branch.MessageType)
                    //            {
                    //                case MessageType.Sms:
                    //                    if (string.IsNullOrWhiteSpace(Program.Branch.SmsUserName) ||
                    //                     string.IsNullOrWhiteSpace(Program.Branch.SmsPassword) ||
                    //                     string.IsNullOrWhiteSpace(Program.Branch.SmsSenderName) ||
                    //                        string.IsNullOrWhiteSpace(Program.Branch.WelcomeMessage)) { }
                    //                    else
                    //                        whatsappMessage.SendSMS(cust.Mobil, Program.Branch.WelcomeMessage);
                    //                    break;
                    //                case MessageType.WhatsApp:
                    //                    if (string.IsNullOrWhiteSpace(Program.Branch.access_token) ||
                    //                        string.IsNullOrWhiteSpace(Program.Branch.instance_id) ||
                    //                        string.IsNullOrWhiteSpace(Program.Branch.WelcomeMessage)) { }
                    //                    else
                    //                        Task.Run(() => whatsappMessage.SendJsonToUrl(whatsappMessage.Getwaclient(cust, null, "text", "", Program.Branch.WelcomeMessage)));
                    //                    break;
                    //            }
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //    }
                    //}
                    if (Session.tblSupplier.Any(v => v.ID == CurrCustomer.ID))
                    {
                        int index = Session.tblSupplier.IndexOf(Session.tblSupplier.Single(x => x.ID == CurrCustomer.ID));
                        Session.tblSupplier.Remove(Session.tblSupplier.Single(x => x.ID == CurrCustomer.ID));
                        Session.tblSupplier.Insert(index, CurrCustomer);
                    }
                    else
                        Session.tblSupplier.Add(CurrCustomer);
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
            tblCustomerBindingSource.DataSource = Session.tblSupplier.ToList();
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

     


    }
}