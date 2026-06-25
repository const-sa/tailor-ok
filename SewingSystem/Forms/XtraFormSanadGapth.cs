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
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraRichEdit.API.Native;

namespace SewingSystem.Forms
{
    public partial class XtraFormPayment : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionTransfer = new List<tblPermission>();
        public XtraFormPayment()
        {

            InitializeComponent();
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "سندات القبض" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionTransfer.Clear();

            tblPermissionTransfer = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            AddNew.Visible = tblPermissionTransfer.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues ?? false;
            Delete.Visible = tblPermissionTransfer.FirstOrDefault(p => p.PermissionName == Program.Delete)?.TheValues ?? false;
            Print.Visible = tblPermissionTransfer.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues ?? false;
            UpdateRecord.Enabled = tblPermissionTransfer.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues ?? false;
        }
        public void RefrechData()
        {
            tblCustomerBindingSource.DataSource = Session.tblCustomer.ToList();
            tblBranchesBindingSource.DataSource = Session.tblBranche;
            tblUsersBindingSource.DataSource = Session.tblUser;
            tblPaymentBindingSource.DataSource = Session.tblPayment.ToList();
            searchLookUpEditInvo.Properties.DataSource = Session.tblSellInvoice.ToList();
            GridControlPayment.DataSource = Session.tblPayment.Where(p => p.InvoNumber == ((searchLookUpEditInvo.EditValue as long?)?? 0) & p.BranchID == Program.Branch.ID).ToList();
            UpdateRecord.Enabled = tblPermissionTransfer.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues ?? false;
        }
        private void searchLookUpEditInvo_EditValueChanged(object sender, EventArgs e)
        {
            if (searchLookUpEditInvo.EditValue == null)
                return;
            tblSellInvoice sale = searchLookUpEditInvo.GetSelectedDataRow() as tblSellInvoice;
            if (sale == null)
                return;
            tblSellInvoiceBindingSource.DataSource = sale;
            if (CurrPayment != null)
            {
                CurrPayment.CusNumber = sale.CusNumber;
                CurrPayment.InvoNumber = sale.InvoNumber;
                CurrPayment.MonyRemin = sale.MonyRemin;
                CurrPayment.QuanRemin = sale.QuanRemin;
            }
            GridControlPayment.DataSource = Session.tblPayment.Where(p => p.InvoNumber == sale.InvoNumber & p.BranchID == sale.BranchID).ToList();
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            RefrechData();
        }
        tblSellInvoice CurrSale => tblSellInvoiceBindingSource.Current as tblSellInvoice;
        tblPayment CurrPayment => tblPaymentBindingSource.Current as tblPayment;
        private void AddNew_Click(object sender, EventArgs e)
        {
            tblPaymentBindingSource.AddNew();
            CurrPayment.EnterTime = DateTime.Now;
            CurrPayment.PayDate = DateTime.Now;
            CurrPayment.UserID = Program.User.ID;
            CurrPayment.BranchID = Program.Branch.ID;
            CurrPayment.MonyPay = 0;
            CurrPayment.TheAmount = 0;
            CurrPayment.Cash = 0;
            CurrPayment.Chik = 0;
            CurrPayment.Visa = 0;
            CurrPayment.Meater = 0;
            CurrPayment.Network = 0;
            CurrPayment.QuanDelivery = 0;
            CurrPayment.Discount = 0;
            isNew = true;
        }
        bool ValidateData()
        {
            if ((CurrPayment.CusNumber ?? 0) == 0 | (CurrPayment.InvoNumber ?? 0) == 0)
            {
                XtraMessageBox.Show("قم باختيار الفاتورة واسم العميل اولا");
                return false;
            }
            if ((CurrPayment.QuanDelivery ?? 0) <= 0 & (CurrPayment.TheAmount ?? 0) <= 0)
            {
                XtraMessageBox.Show("عفوا لا يمكن ان تكون الكميه المستلمه والمبلغ المستلم تساوي الصفر");
                return false;
            }
            if (CurrPayment.QuanRemin < 0)
            {
                XtraMessageBox.Show("عفوا لا يمكن ان تكون الكميه المتبقية اقل من الصفر");
                return false;
            }
            if (CurrPayment.MonyRemin < 0)
            {
                XtraMessageBox.Show("عفوا لا يمكن ان يكون المبلغ المتبقي اقل من الصفر");
                return false;
            }
            return true;
        }
        ComponentFlyoutDialog flyDialog = new ComponentFlyoutDialog();
        bool isNew = false;
        private void Save_Click(object sender, EventArgs e)
        {
            if (this.CurrPayment == null)
                return;
            if (!ValidateData())
                return; 
            flyDialog.WaitForm(this, 1);
            string mssg = $"سند قبض رقم : {CurrPayment?.ID} ";
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                if (!isNew)
                    db.tblPayments.DeleteAllOnSubmit(db.tblPayments.Where(m => m.ID == CurrPayment.ID));
                db.tblPayments.InsertOnSubmit(CurrPayment);
                db.SubmitChanges();
                if (Session.tblPayment.Any(x => x.ID == CurrPayment.ID))
                {
                    int index = Session.tblPayment.IndexOf(Session.tblPayment.Single(x => x.ID == CurrPayment.ID));
                    Session.tblPayment.Remove(Session.tblPayment.Single(x => x.ID == CurrPayment.ID));
                    Session.tblPayment.Insert(index, CurrPayment);
                }
                else 
                    Session.tblPayment.Add(CurrPayment);
                CurrSale.MonyRemin = CurrPayment.MonyRemin;
                CurrSale.QuanRemin = CurrPayment.QuanRemin;
                var paylist = db.tblPayments.Where(i => i.InvoNumber == CurrPayment.InvoNumber & i.BranchID == CurrPayment.BranchID).ToList();
                CurrSale.MonyPay = paylist?.Sum(s => s.TheAmount?? 0);
                CurrSale.Discount = paylist?.Sum(s => s.Discount?? 0);
                db.tblSellInvoices.DeleteAllOnSubmit(db.tblSellInvoices.Where(m => m.ID == CurrSale.ID));
                db.tblSellInvoices.InsertOnSubmit(CurrSale);
                db.SubmitChanges();
            }
            Session.RefreshDatatSellInvoice(CurrSale);
            RefrechData();
            flyDialog.WaitForm(this, 0);
            flyDialog.ShowDialogForm(this, mssg, this.isNew);
            isNew = false;
            UpdateRecord.Enabled = tblPermissionTransfer.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues ?? false;
        }

        private void Spn_Quantity_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (this.CurrSale == null)
                return;
            if (e.NewValue.ToString() == "")
                return;
            CurrPayment.QuanDelivery = int.Parse(e.NewValue.ToString());
            CurrPayment.QuanRemin = (CurrSale.QuanRemin?? 0) - (CurrPayment.QuanDelivery?? 0);
        }
        private void Spn_Discount_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (this.CurrSale == null)
                return;
            if (e.NewValue.ToString() == "")
                return;
            CurrPayment.Discount = double.Parse(e.NewValue.ToString());
            CurrPayment.MonyRemin = (CurrSale.MonyRemin ?? 0) - (CurrPayment.TheAmount ?? 0) - (CurrPayment.Discount ?? 0);
        }
        private void Spn_PaidCash_EditValueChanging1(object sender, ChangingEventArgs e)
        {
            if (this.CurrSale == null)
                return;
            switch (((TextEdit)sender).Name)
                {
                    case "Spn_PaidCash":
                        CurrPayment.TheAmount = (CurrPayment.Chik?? 0) + (CurrPayment.Network?? 0)
                       + (CurrPayment.Visa?? 0) + (CurrPayment.Meater?? 0);
                        if (e.NewValue.ToString() != "")
                            CurrPayment.TheAmount += double.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidNetwork":
                        CurrPayment.TheAmount = (CurrPayment.Chik?? 0) + (CurrPayment.Cash?? 0)
                       + (CurrPayment.Visa?? 0) + (CurrPayment.Meater?? 0);
                        if (e.NewValue.ToString() != "")
                            CurrPayment.TheAmount += double.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidChik":
                        CurrPayment.TheAmount = (CurrPayment.Network?? 0) + (CurrPayment.Cash?? 0)
                       + (CurrPayment.Visa?? 0) + (CurrPayment.Meater?? 0);
                        if (e.NewValue.ToString() != "")
                            CurrPayment.TheAmount += double.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidMeater":
                        CurrPayment.TheAmount = (CurrPayment.Network?? 0) + (CurrPayment.Cash?? 0)
                       + (CurrPayment.Visa?? 0) + (CurrPayment.Chik?? 0);
                        if (e.NewValue.ToString() != "")
                            CurrPayment.TheAmount += double.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidVisa":
                        CurrPayment.TheAmount = (CurrPayment.Network?? 0) + (CurrPayment.Cash?? 0)
                       + (CurrPayment.Meater?? 0) + (CurrPayment.Chik?? 0);
                        if (e.NewValue.ToString() != "")
                            CurrPayment.TheAmount += double.Parse(e.NewValue.ToString());
                        break;
                    default:
                        break;
                }
                CurrPayment.MonyRemin = (CurrSale.MonyRemin?? 0) - (CurrPayment.TheAmount?? 0) - (CurrPayment.Discount?? 0);
        }
        private void tblPaymentBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (this.CurrPayment == null)
                return;

            tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(p => p.InvoNumber == CurrPayment.InvoNumber).ToList();
            GridControlPayment.DataSource = Session.tblPayment.Where(p => p.InvoNumber == CurrPayment.InvoNumber).ToList();
           
            if (Session.tblPayment.Any(i => i.ID == CurrPayment.ID & i.BranchID == CurrPayment.BranchID))
                UpdateRecord.Enabled = tblPermissionTransfer.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues ?? false;
            else
                UpdateRecord.Enabled = tblPermissionTransfer.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues ?? false;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (this.CurrPayment == null || MyFunaction.MessageBoxDelete() != DialogResult.Yes)
                return;
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                db.tblPayments.DeleteAllOnSubmit(db.tblPayments.Where(m => m.ID == CurrPayment.ID));
                db.SubmitChanges();
                Session.tblPayment.Remove(Session.tblPayment.Single(x => x.ID == CurrPayment.ID));
                RefrechData();
            }
        }
        //Report.XtraReportCategorie rptCategorie;
        private void Print_Click(object sender, EventArgs e)
        {
            //rptCategorie = new Report.XtraReportCategorie();
            //rptCategorie.DataSource = Session.tblPayment;
            //PrintReport(rptCategorie);
        }

        //Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
        public void PrintReport(DevExpress.XtraReports.UI.XtraReport Report)
        {
            //frmReport.documentViewer1.DocumentSource = Report;
            //frmReport.ShowDialog();
        }

        private void XtraFormPayment_Load(object sender, EventArgs e)
        {
            RefrechData();
            Spn_PaidMeater.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidCash.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidNetwork.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidVisa.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidChik.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_Discount.EditValueChanging += Spn_Discount_EditValueChanging;
            spn_Quantity.EditValueChanging += Spn_Quantity_EditValueChanging;
        }

    }
}