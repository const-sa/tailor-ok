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
using SewingSystem.LinqModel;
using SewingSystem.Classes;
using System.Threading;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Layout;
using System.Collections.ObjectModel;
using System.Globalization;
using SewingSystem.Report;

namespace SewingSystem.Forms
{
    public partial class XtraFormNote : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        tblPermission Permission2;
        List<tblPermission> tblPermissionWashType2 = new List<tblPermission>();
        public XtraFormNote()
        {
            InitializeComponent();
            this.Load += XtraFormInvoices_Load;
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "فاتورة جديدة" & p.UserGroupID == Program.User.UserGroupID);
            SubPermissen("Update");
        }
        private void XtraFormInvoices_Load(object sender, EventArgs e)
        {
            if ((Program.Branch.UseTax as bool?) ?? false)
                ItemForTheTax.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            else
                ItemForTheTax.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            Spn_PaidMeater.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidCash.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidNetwork.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidVisa.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidChik.EditValueChanging += Spn_PaidCash_EditValueChanging1;

            spn_Net.EditValueChanging += Spn_Tax_EditValueChanging1;
            spn_BeforTax.EditValueChanging += Spn_Tax_EditValueChanging1;

            tblSellCreditNoteBindingSource.CurrentChanged += tblSellCreditNoteBindingSource_CurrentChanged;
            Session.tblSellCreditNote.ListChanged += tblSellCreditNote_ListChanged;
            PrintDirect.Click += PrintDirect_Click;
            RefrashAll();
            PrintInvoice.Visible = PrintDirect.Visible;
            //AddNew.PerformClick();
        }
        private void Spn_PaidCash_EditValueChanging1(object sender, ChangingEventArgs e)
        {
            if (tblSellCreditNoteBindingSource.Current != null)
            {
                tblSellCreditNote sell = tblSellCreditNoteBindingSource.Current as tblSellCreditNote;
                switch (((TextEdit)sender).Name)
                {
                    case "Spn_PaidCash":
                        sell.MonyOrpon = ((sell.Chik as decimal?) ?? 0) + ((sell.Network as decimal?) ?? 0)
                       + ((sell.Visa as decimal?) ?? 0) + ((sell.Meater as decimal?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            sell.MonyOrpon += decimal.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidNetwork":
                        sell.MonyOrpon = ((sell.Chik as decimal?) ?? 0) + ((sell.Cash as decimal?) ?? 0)
                       + ((sell.Visa as decimal?) ?? 0) + ((sell.Meater as decimal?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            sell.MonyOrpon += decimal.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidChik":
                        sell.MonyOrpon = ((sell.Network as decimal?) ?? 0) + ((sell.Cash as decimal?) ?? 0)
                       + ((sell.Visa as decimal?) ?? 0) + ((sell.Meater as decimal?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            sell.MonyOrpon += decimal.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidMeater":
                        sell.MonyOrpon = ((sell.Network as decimal?) ?? 0) + ((sell.Cash as decimal?) ?? 0)
                       + ((sell.Visa as decimal?) ?? 0) + ((sell.Chik as decimal?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            sell.MonyOrpon += decimal.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidVisa":
                        sell.MonyOrpon = ((sell.Network as decimal?) ?? 0) + ((sell.Cash as decimal?) ?? 0)
                       + ((sell.Meater as decimal?) ?? 0) + ((sell.Chik as decimal?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            sell.MonyOrpon += decimal.Parse(e.NewValue.ToString());
                        break;
                    default:
                        break;
                }
                sell.MonyRemin = ((sell.TotalFinal as decimal?) ?? 0) - ((sell.MonyOrpon as decimal?) ?? 0);
            }
        }
        private void Spn_Tax_EditValueChanging1(object sender, ChangingEventArgs e)
        {
            if (tblSellCreditNoteBindingSource.Current != null)
            {
                tblSellCreditNote sell = tblSellCreditNoteBindingSource.Current as tblSellCreditNote;
                switch (((TextEdit)sender).Name)
                {
                    case "spn_Net":
                        if (e.NewValue.ToString() != "")
                        {
                            if ((Program.Branch.UseTax as bool?) ?? false)
                                sell.TotalFattCreditNote = ((decimal.Parse(e.NewValue.ToString()) / Session.Defualts.TaxOperator) * Session.Defualts.TaxRate);
                            else
                                sell.TotalFattCreditNote = 0;
                            sell.TotalMony = decimal.Parse(e.NewValue.ToString()) - sell.TotalFattCreditNote;
                            sell.MonyRemin = decimal.Parse(e.NewValue.ToString()) - ((sell.MonyOrpon as decimal?) ?? 0);
                        }
                        break;
                    case "spn_BeforTax":
                        if (e.NewValue.ToString() != "")
                        {
                            if ((Program.Branch.UseTax as bool?) ?? false)
                                sell.TotalFattCreditNote = (decimal.Parse(e.NewValue.ToString()) * Session.Defualts.TaxRate);
                            else
                                sell.TotalFattCreditNote = 0;
                            sell.TotalFinal = decimal.Parse(e.NewValue.ToString()) + sell.TotalFattCreditNote;
                            sell.MonyRemin = decimal.Parse(e.NewValue.ToString()) - ((sell.MonyOrpon as decimal?) ?? 0);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private void tblSellCreditNoteBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (this.IsActive)
            {
                if (tblSellCreditNoteBindingSource.Current != null)
                {
                    tblSellCreditNote currentsell = tblSellCreditNoteBindingSource.Current as tblSellCreditNote;
                    int v = Session.tblSellCreditNote.Where(i => i.ID == currentsell.ID & i.BranchID == Program.Branch.ID).Count();
                    if (v > 0)
                        SubPermissen("Update");
                    else if (v <= 0)
                        SubPermissen("Add");
                }
            }
        }
        bool per = false;
        public void SubPermissen(string TypeOperation)
        {
            //if (Permission != null)
            //{
            //    List<tblPermission> tblPermissionWashType = new List<tblPermission>();
            //    tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            //    AddNew.Visible = MyFunaction.ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add));
            //    DeleteInvoice.Visible = MyFunaction.ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Delete));
            //    PrintDirect.Visible = MyFunaction.ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print));
            //    if (TypeOperation == "Add")
            //        per = AddNew.Visible;
            //    else
            //        per = MyFunaction.ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update));
            //    if (per)
            //    {
            //        WashNumbertxt.ReadOnly = false;
            //        Carrier.ReadOnly = false;
            //        InvoiceNotsTextEdit.ReadOnly = false;
            //        InvoiceDateDateEdit.ReadOnly = false;
            //        MobilCust.ReadOnly = false;
            //        CustomerName.ReadOnly = false;
            //        NaylaIDLookUpEdit.ReadOnly = false;
            //        NashaIDLookUpEdit.ReadOnly = false;
            //        SearchInvoiceIDtxtedit.ReadOnly = false;
            //        IsDeliveryCheckEdit.ReadOnly = false;
            //        IsCleanedCheckEdit.ReadOnly = false;
            //        Save.Enabled = true;
            //        SaveAndPrint.Enabled = true;
            //        layoutViewInvoiceDetail.OptionsBehavior.ReadOnly = false;
            //    }
            //    else
            //    {
            //        Save.Enabled = false;
            //        SaveAndPrint.Enabled = false;
            //        WashNumbertxt.ReadOnly = true;
            //        Carrier.ReadOnly = true;
            //        InvoiceNotsTextEdit.ReadOnly = true;
            //        InvoiceDateDateEdit.ReadOnly = true;
            //        MobilCust.ReadOnly = true;
            //        CustomerName.ReadOnly = true;
            //        NaylaIDLookUpEdit.ReadOnly = true;
            //        NashaIDLookUpEdit.ReadOnly = true;
            //        SearchInvoiceIDtxtedit.ReadOnly = true;
            //        IsDeliveryCheckEdit.ReadOnly = true;
            //        IsCleanedCheckEdit.ReadOnly = true;
            //        layoutViewInvoiceDetail.OptionsBehavior.ReadOnly = true;
            //    }
            //}
        }
       
        private void tblSellCreditNote_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (this.IsActive & (!AddNew.Enabled | RefreachDeleteInvoice))
            {
                RefreachDeleteInvoice = false;
                if (Program.User.UserState == "Admin")
                {
                    tblSellCreditNoteBindingSource.DataSource = Session.tblSellCreditNote.Where(c => c.BranchID == Program.User.BranchID).ToList();
                }
            }
        }
            private void RefrechInvoice_Click(object sender, EventArgs e)
            {
                RefrashAll();
                //tblSellCreditNote_ListChanged(sender, null);
                AddNew.Enabled = true;
                SubPermissen("Update");
            }
            //private void TblCustomer_ListChanged(object sender, ListChangedEventArgs e)
            //{
            //    tblCustomerBindingSource.DataSource = Session.tblCustomer.Where(c => c.BranchID == Program.User.BranchID).ToList();
            //}
            //tblSellCreditNote PrintInvo;
        public void FunactionPrintInvoice()
        {
            try
            {
                if (tblSellCreditNoteBindingSource.Current != null)
                {
                    tblSellCreditNote sell = tblSellCreditNoteBindingSource.Current as tblSellCreditNote;
                    string CustomerName = "";
                    string Mobil = "";
                    if (((sell.CusNumber as string) ?? "") != "")
                    {
                        tblCustomer cus;
                        cus = Session.tblCustomer.SingleOrDefault(u => u.CusNumber == sell.CusNumber & u.BranchID == sell.BranchID);
                        CustomerName = (cus.CustomerName as string) ?? "";
                        Mobil = (cus.Mobil as string) ?? "";
                    }
                    TLVCls tlv = new TLVCls((Program.Branch.CompanyName as string) ?? "", (Program.Branch.TaxNumber as string) ?? "", sell.SellDate.Value, Convert.ToDouble(((sell.TotalFinal as decimal?) ?? 0)), Convert.ToDouble(((sell.TotalFattCreditNote as decimal?) ?? 0)));
                    var invoice = (from inv in Session.tblSellCreditNote
                                   where inv.ID == sell.ID & inv.BranchID == sell.BranchID
                                   select new
                                   {
                                       inv.InvoNumber,
                                       inv.CusNumber,
                                       QRCode = tlv.ToBase64(),
                                       TheName = CustomerName,
                                       user = Program.User.UserName,
                                       LogoImage = Program.Branch.LogoImage,
                                       Phone = Mobil,
                                       inv.SellDate,
                                       Discount = (inv.Discount as decimal?) ?? 0,
                                       TotalAfterDiscount = (inv.TotalAfterDiscount as decimal?) ?? 0,
                                       TotalFinal = (inv.TotalFinal as decimal?) ?? 0,
                                       inv.Notes,
                                       MonyPay = ((inv.MonyPay as decimal?) ?? 0) + ((inv.MonyOrpon as decimal?) ?? 0) + ((inv.Discount as decimal?) ?? 0),
                                       MonyRemin = (inv.MonyRemin as decimal?) ?? 0,
                                       TotalFattCreditNote = (inv.TotalFattCreditNote as decimal?) ?? 0,
                                       TotalMony = (inv.TotalMony as decimal?) ?? 0,
                                       TheQuantity = (inv.TheQuantity as int?) ?? 0,
                                   }).ToList();
                    if (invoice.Count() <= 0)
                    {
                        XtraMessageBox.Show("عفوا تاكد من حفظ الاشعار");
                        return;
                    }
                    Report.XtraReportPayAajel.Print(invoice, sell.ID, sell.ThePay, sell.SellDate.Value.ToString());
                }

            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }
            private void Save_Click(object sender, EventArgs e)
            {
            try
            {
                if (tblSellCreditNoteBindingSource.Current != null)
                {
                    CurrentInvoice = tblSellCreditNoteBindingSource.Current as tblSellCreditNote;
                    tblSellCreditNoteBindingSource.EndEdit();
                    CurrentInvoice.QuanRemin = CurrentInvoice.TheQuantity;
                    if (CurrentInvoice.TheQuantity.Value <= 0)
                    {
                        XtraMessageBox.Show("عفوا لا يمكن ان تكون الكميه تساوي الصفر");
                        return;
                    }
                    if (CurrentInvoice.TotalFinal <= 0)
                    {
                        XtraMessageBox.Show("عفوا لا يمكن ان يكون المبلغ يساوي الصفر");
                        return;
                    }
                    if (CurrentInvoice.MonyRemin < 0)
                    {
                        XtraMessageBox.Show("عفوا لا يمكن ان يكون المبلغ المتبقي اقل من الصفر");
                        return;
                    }
                    using (var db = new DataClasses1DataContext(Program.ConnectionString))
                    {
                        db.tblSellCreditNotes.DeleteAllOnSubmit(db.tblSellCreditNotes.Where(m => m.ID == CurrentInvoice.ID));
                        db.tblSellCreditNotes.InsertOnSubmit(CurrentInvoice);
                        db.SubmitChanges();
                    }
                    spn_Paid.ReadOnly = true;
                    SaveSuccess = true;
                    AddNew.Enabled = true;
                    if (((ToolStripButton)sender).Name == "SaveAndPrint")
                    {
                        PrintInvo = CurrentInvoice;
                        Program.PrintMode = "Direct";
                        ThreadPrintInvoice = new Thread(FunactionPrintInvoice);
                        ThreadPrintInvoice.IsBackground = true;
                        ThreadPrintInvoice.Start();
                    }
                    MyFunaction.MessageBoxSave();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }

        }
        Thread ThreadPrintInvoice;
            private void SaveAndPrint_Click(object sender, EventArgs e)
            {
              Save_Click(sender, e);
            if (SaveSuccess)
            {
                SaveSuccess = false;
                AddNew.PerformClick();
            }
        }
        public void ButtonPrint()
        {
            if (tblSellCreditNoteBindingSource.Current != null)
            {
                PrintInvo = tblSellCreditNoteBindingSource.Current as tblSellCreditNote;
                if (Session.tblSellCreditNote.Where(i => i.ID == PrintInvo.ID & i.BranchID == PrintInvo.BranchID).Count() > 0)
                {
                    ThreadPrintInvoice = new Thread(FunactionPrintInvoice);
                    ThreadPrintInvoice.IsBackground = true;
                    ThreadPrintInvoice.Start();
                }
                else
                {
                    if (Properties.Settings.Default.Language == "ar-SA")
                        XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "عفوا قم بحفظ الاشعار اولا !!!!!", "حفظ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    else if (Properties.Settings.Default.Language == "en-US")
                        XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "Sorry, Save the bill first!!!!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                }
            }
        }
        private void PrintInvoice_Click(object sender, EventArgs e)
            {
                Program.PrintMode = "ShowDialog";
                ButtonPrint();
            }

            private void AddNew_Click(object sender, EventArgs e)
            {
                SaveSuccess = false;
                FunctionAddNewInvoice();
            }
        public tblSellCreditNote CurrentInvoice;
        public tblCustomer CurrentCustomer;
        public tblDefaultSize CurrentDefaultSize;
        bool SaveSuccess = false;
        bool RefreachDeleteInvoice = false;
        public void FunctionAddNewInvoice()
        {
            tblSellCreditNoteBindingSource.AddNew();
            AddNew.Enabled = false;
            tblSellCreditNote tblSell = tblSellCreditNoteBindingSource.Current as tblSellCreditNote;
            tblSell.EnterTime = DateTime.Now;
            tblSell.SellDate = DateTime.Now;
            tblSell.UserID = Program.User.ID;
            tblSell.BranchID = Program.Branch.ID;
            tblSell.MonyPay = 0;
            tblSell.MonyRemin = 0;
            tblSell.QuanRemin = 0;
            tblSell.TheQuantity = 0;
            tblSell.Cash = 0;
            tblSell.Chik = 0;
            tblSell.Network = 0;
            tblSell.Visa = 0;
            tblSell.Meater = 0;
            tblSell.Discount = 0;
            tblSell.TotalFinal = 0;
            tblSell.MonyOrpon = 0;
            tblSell.TotalAfterDiscount = 0;
            tblSell.TotalFattCreditNote = 0;
            tblSell.TotalMony = 0;
            tblSell.DeliveryDate = DateTime.Now.AddDays(10);
            tblSell.ThePay = "Deferred آجل";
            tblSell.EnterTime = DateTime.Now;
            if (Session.tblSellCreditNote.Where(s => s.BranchID == Program.User.BranchID).Count() <= 0)
                tblSell.InvoNumber = "1";
            else
            {
                try
                {
                    tblSell.InvoNumber = (Session.tblSellCreditNote.Where(s => s.BranchID == Program.User.BranchID).Max(i => MyFunaction.GetlongValue((i.InvoNumber as string) ?? "0")) + 1).ToString();
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message + "|" + tblSell.InvoNumber);
                    return;
                }
            }
            spn_Paid.ReadOnly = false;
        }
        private void PrintDirect_Click(object sender, EventArgs e)
        {
            Program.PrintMode = "Direct";
            ButtonPrint();
        }
        public void RefrashAll()
        {
            tblBranchesBindingSource.DataSource = Session.tblBranche.ToList();
            tblUsersBindingSource.DataSource = Session.tblUser.ToList();
            tblSellCreditNoteBindingSource.DataSource = Session.tblSellCreditNote.ToList();
        }
        Forms.XtraFormReport frmReport = new Forms.XtraFormReport();

  

        private void SearchSaleInvoiceIDtxtedit_EditValueChanged(object sender, EventArgs e)
        {
                if (SearchSaleInvoiceIDtxtedit.EditValue != null)
                {
                    if (tblSellInvoiceBindingSource.Count > 0 & tblSellCreditNoteBindingSource.Current != null)
                    {
                        if (SearchSaleInvoiceIDtxtedit.GetSelectedDataRow() != null)
                        {
                            tblSellInvoice sale = SearchSaleInvoiceIDtxtedit.GetSelectedDataRow() as tblSellInvoice;
                            tblSellCreditNote saleReturn = tblSellCreditNoteBindingSource.Current as tblSellCreditNote;
                            saleReturn.TheQuantity = sale.TheQuantity;
                            saleReturn.ThePay = sale.ThePay;
                            saleReturn.CusNumber = sale.CusNumber;
                            saleReturn.MonyPay = sale.MonyPay;
                            saleReturn.MonyRemin = sale.MonyRemin;
                            saleReturn.Discount = sale.Discount;
                            saleReturn.TotalFattCreditNote = sale.TotalFattInvoice;
                            saleReturn.TotalMony = sale.TotalMony;
                            saleReturn.TotalAfterDiscount = sale.TotalAfterDiscount;
                            saleReturn.TotalFinal = sale.TotalFinal;
                            saleReturn.SaleID = sale.ID;
                        }
                    }
            }
        }
    }
}