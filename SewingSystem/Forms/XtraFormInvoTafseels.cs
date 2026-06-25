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
    public partial class XtraFormInvoTafseels : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        tblPermission Permission2;
        List<tblPermission> tblPermissionWashType2 = new List<tblPermission>();
        public XtraFormInvoTafseels()
        {
            InitializeComponent();
            this.Load += XtraFormInvoices_Load;
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "العملاء" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            AddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add).TheValues.Value;
            PrintDirect.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print).TheValues.Value;
            PrintInvoice.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print).TheValues.Value;
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update).TheValues.Value;
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

            tblSellInvoiceBindingSource.CurrentChanged += TblSellInvoiceBindingSource_CurrentChanged;
            Session.tblCustomer.ListChanged += TblCustomer_ListChanged;
            Session.tblSellInvoice.ListChanged += TblSellInvoice_ListChanged;
            PrintDirect.Click += PrintDirect_Click;
            RefrashAll();
            PrintInvoice.Visible = PrintDirect.Visible;
            AddNew.PerformClick();
        }
        private void Spn_PaidCash_EditValueChanging1(object sender, ChangingEventArgs e)
        {
            if (tblSellInvoiceBindingSource.Current != null)
            {
                tblSellInvoice sell = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                switch (((TextEdit)sender).Name)
                {
                    case "Spn_PaidCash":
                        sell.MonyOrpon = ((sell.Chik as double?) ?? 0) + ((sell.Network as double?) ?? 0)
                       + ((sell.Visa as double?) ?? 0) + ((sell.Meater as double?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            sell.MonyOrpon += double.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidNetwork":
                        sell.MonyOrpon = ((sell.Chik as double?) ?? 0) + ((sell.Cash as double?) ?? 0)
                       + ((sell.Visa as double?) ?? 0) + ((sell.Meater as double?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            sell.MonyOrpon += double.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidChik":
                        sell.MonyOrpon = ((sell.Network as double?) ?? 0) + ((sell.Cash as double?) ?? 0)
                       + ((sell.Visa as double?) ?? 0) + ((sell.Meater as double?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            sell.MonyOrpon += double.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidMeater":
                        sell.MonyOrpon = ((sell.Network as double?) ?? 0) + ((sell.Cash as double?) ?? 0)
                       + ((sell.Visa as double?) ?? 0) + ((sell.Chik as double?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            sell.MonyOrpon += double.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidVisa":
                        sell.MonyOrpon = ((sell.Network as double?) ?? 0) + ((sell.Cash as double?) ?? 0)
                       + ((sell.Meater as double?) ?? 0) + ((sell.Chik as double?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            sell.MonyOrpon += double.Parse(e.NewValue.ToString());
                        break;
                    default:
                        break;
                }
                sell.MonyRemin = ((sell.TotalFinal as double?) ?? 0) - ((sell.MonyOrpon as double?) ?? 0);
            }
        }
        private void Spn_Tax_EditValueChanging1(object sender, ChangingEventArgs e)
        {
            if (tblSellInvoiceBindingSource.Current != null)
            {
                tblSellInvoice sell = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                switch (((TextEdit)sender).Name)
                {
                    case "spn_Net":
                        if (e.NewValue.ToString() != "")
                        {
                            if ((Program.Branch.UseTax as bool?) ?? false)
                                sell.TotalFattInvoice = ((double.Parse(e.NewValue.ToString()) / Session.Defualts.TaxOperator) * Session.Defualts.TaxRate);
                            else
                                sell.TotalFattInvoice = 0;
                            sell.TotalMony = double.Parse(e.NewValue.ToString()) - sell.TotalFattInvoice;
                            sell.MonyRemin = double.Parse(e.NewValue.ToString()) - ((sell.MonyOrpon as double?) ?? 0);
                        }
                        break;
                    case "spn_BeforTax":
                        if (e.NewValue.ToString() != "")
                        {
                            if ((Program.Branch.UseTax as bool?) ?? false)
                                sell.TotalFattInvoice = (double.Parse(e.NewValue.ToString()) * Session.Defualts.TaxRate);
                            else
                                sell.TotalFattInvoice = 0;
                            sell.TotalFinal = double.Parse(e.NewValue.ToString()) + sell.TotalFattInvoice;
                            sell.MonyRemin = double.Parse(e.NewValue.ToString()) - ((sell.MonyOrpon as double?) ?? 0);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        private void TblSellInvoiceBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (this.IsActive)
            {
                if (tblSellInvoiceBindingSource.Current != null)
                {
                    tblSellInvoice currentInvo = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                    var s = Session.tblSellInvoice.Where(i => i.ID == currentInvo.ID & i.BranchID == currentInvo.BranchID).ToList();
                    int v = s.Count();
                    if (v > 0)
                    {
                        if (int.Parse((currentInvo.CusNumber as string) ?? "0") > 0)
                        {
                            tblCustomerBindingSource.DataSource = Session.tblCustomer.Where(c => c.CusNumber == currentInvo.CusNumber).ToList();
                            tblDefaultSizeBindingSource.DataSource = Session.tblDefaultSize.Where(c => c.ID == currentInvo.SizeID).ToList();
                        }
                        if (((s[0].HowPrint as short?) ?? 0) > 0)
                            DisaplyDeletAndEdit(false);
                        else
                            DisaplyDeletAndEdit(tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update).TheValues.Value);
                    }
                    else if (v <= 0)
                        DisaplyDeletAndEdit(tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add).TheValues.Value);
                }
            }
        }
     
      

        public void TblCustomerAddingNew()
        {
            //tblCustomerBindingSource.AddNew();
            tblCustomer tblCustomer =new tblCustomer();
            tblCustomer.EnterTime = DateTime.Now;
            tblCustomer.BranchID = Program.User.BranchID;
            tblCustomer.UserID = Program.User.ID;
            if (Session.tblCustomer.Where(s => s.BranchID == Program.User.BranchID).Count() <= 0)
                tblCustomer.CusNumber = "1";
            else
                tblCustomer.CusNumber = (Session.tblCustomer.Where(s => s.BranchID == Program.User.BranchID).Max(i => MyFunaction.GetlongValue((i.CusNumber as string) ?? "0")) + 1).ToString();
            tblCustomerBindingSource.DataSource = tblCustomer;
        }
        public void TblSizeAddingNew()
        {
            //tblDefaultSizeBindingSource.AddNew();
            tblDefaultSize tblDefaultSize =new tblDefaultSize();
            tblCustomer tblCustomer = tblCustomerBindingSource.Current as  tblCustomer;
            tblDefaultSize.EnterTime = DateTime.Now;
            tblDefaultSize.BranchID = Program.User.BranchID;
            tblDefaultSize.UserID = Program.User.ID;
            tblDefaultSize.J1 = false;
            tblDefaultSize.J2 = false;
            tblDefaultSize.J3= false;
            tblDefaultSize.J4 = false;

            tblDefaultSize.S1 = false;
            tblDefaultSize.S2 = false;
            tblDefaultSize.S3 = false;

            tblDefaultSize.Q1 = false;
            tblDefaultSize.Q2 = false;
            tblDefaultSize.Q3 = false;
            tblDefaultSize.Q4 = false;

            tblDefaultSize.K1 = false;
            tblDefaultSize.K2 = false;
            tblDefaultSize.K3 = false;
            tblDefaultSize.K4 = false;
            tblDefaultSize.CusNumber = tblCustomer.CusNumber;
            tblDefaultSizeBindingSource.DataSource = tblDefaultSize;
        }
        private void TblSellInvoice_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (this.IsActive & (!AddNew.Enabled | RefreachDeleteInvoice))
            {
                RefreachDeleteInvoice = false;
                if (Program.User.UserState == "Admin")
                {
                    tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(c => c.BranchID == Program.User.BranchID).ToList();
                    if (tblSellInvoiceBindingSource.Current != null)
                    {
                        tblSellInvoice currentsell = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                        if (int.Parse((currentsell.CusNumber as string) ?? "0") > 0)
                        {
                            tblCustomerBindingSource.DataSource = Session.tblCustomer.Where(c => c.CusNumber == currentsell.CusNumber).ToList();
                            tblDefaultSizeBindingSource.DataSource = Session.tblDefaultSize.Where(c => c.ID == currentsell.SizeID).ToList();
                        }
                    }
                }
            }
        }
            private void RefrechInvoice_Click(object sender, EventArgs e)
            {
                RefrashAll();
                //TblSellInvoice_ListChanged(sender, null);
                AddNew.Enabled = true;
            }
            private void TblCustomer_ListChanged(object sender, ListChangedEventArgs e)
        {
            searchLookUpEditCustomer.Properties.DataSource = Session.tblCustomer.ToList();
            //tblCustomerBindingSource.DataSource = Session.tblCustomer.Where(c => c.BranchID == Program.User.BranchID).ToList();
            }
            tblSellInvoice PrintInvo;
        public void FunactionPrintInvoice()
        {
            try
            {
                if (tblSellInvoiceBindingSource.Current != null)
                {
                    tblSellInvoice sell = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                    string CustomerName = "";
                    string Mobil = "";
                    if (((sell.CusNumber as string) ?? "") != "")
                    {
                        tblCustomer cus;
                        cus = Session.tblCustomer.SingleOrDefault(u => u.CusNumber == sell.CusNumber & u.BranchID == sell.BranchID);
                        CustomerName = (cus.CustomerName as string) ?? "";
                        Mobil = (cus.Mobil as string) ?? "";
                    }
                    TLVCls tlv = new TLVCls((Program.Branch.CompanyName as string) ?? "", (Program.Branch.TaxNumber as string) ?? "", sell.SellDate.Value, Convert.ToDecimal(((sell.TotalFinal as double?) ?? 0)), Convert.ToDecimal(((sell.TotalFattInvoice as double?) ?? 0)));
                    var invoice = (from inv in Session.tblSellInvoice
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
                                       Discount = (inv.Discount as double?) ?? 0,
                                       TotalAfterDiscount = (inv.TotalAfterDiscount as double?) ?? 0,
                                       TotalFinal = (inv.TotalFinal as double?) ?? 0,
                                       inv.Notes,
                                       MonyPay = ((inv.MonyPay as double?) ?? 0) + ((inv.MonyOrpon as double?) ?? 0) + ((inv.Discount as double?) ?? 0),
                                       MonyRemin = (inv.MonyRemin as double?) ?? 0,
                                       TotalFattInvoice = (inv.TotalFattInvoice as double?) ?? 0,
                                       TotalMony = (inv.TotalMony as double?) ?? 0,
                                       TheQuantity = (inv.TheQuantity as int?) ?? 0,
                                   }).ToList();
                    if (invoice.Count() <= 0)
                    {
                        XtraMessageBox.Show("عفوا تاكد من حفظ الفاتورة");
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
                if (tblSellInvoiceBindingSource.Current != null)
                {
                    CurrentInvoice = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                    tblSellInvoiceBindingSource.EndEdit();
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
                        if (tblCustomerBindingSource.Current != null)
                        {
                            CurrentCustomer = tblCustomerBindingSource.Current as tblCustomer;
                            tblCustomerBindingSource.EndEdit();
                            CurrentInvoice.CusNumber = CurrentCustomer.CusNumber;
                            db.tblCustomers.DeleteAllOnSubmit(db.tblCustomers.Where(m => m.ID == CurrentCustomer.ID));
                            db.tblCustomers.InsertOnSubmit(CurrentCustomer);
                            db.SubmitChanges();
                            if (Session.tblCustomer.Where(v => v.ID == CurrentCustomer.ID).Count() <= 0)
                                Session.tblCustomer.Add(CurrentCustomer);
                        }
                        if (tblDefaultSizeBindingSource.Current != null)
                        {
                            CurrentDefaultSize = tblDefaultSizeBindingSource.Current as tblDefaultSize;
                            tblDefaultSizeBindingSource.EndEdit();
                            db.tblDefaultSizes.DeleteAllOnSubmit(db.tblDefaultSizes.Where(m => m.ID == CurrentDefaultSize.ID));
                            db.tblDefaultSizes.InsertOnSubmit(CurrentDefaultSize);
                            db.SubmitChanges();
                            if (Session.tblDefaultSize.Where(v => v.ID == CurrentDefaultSize.ID).Count() <= 0)
                                Session.tblDefaultSize.Add(CurrentDefaultSize);
                            CurrentInvoice.SizeID = CurrentDefaultSize.ID;
                        }
                        CurrentInvoice.TaxAll = CurrentInvoice.TotalFattInvoice + CurrentInvoice.TaxMaden - CurrentInvoice.TaxDayn;
                        CurrentInvoice.TotalAll = ((CurrentInvoice.TotalFinal as double?) ?? 0) + ((CurrentInvoice.TotalMaden as double?) ?? 0)+ CurrentInvoice.TaxMaden - ((CurrentInvoice.TotalDayn as double?) ?? 0)-((CurrentInvoice.TaxDayn as double?) ?? 0);
                        CurrentInvoice.MonyRemin = ((CurrentInvoice.TotalAll as double?) ?? 0) - ((CurrentInvoice.MonyPay as double?) ?? 0) - ((CurrentInvoice.MonyOrpon as double?) ?? 0);
                        db.tblSellInvoices.DeleteAllOnSubmit(db.tblSellInvoices.Where(m => m.ID == CurrentInvoice.ID));
                        db.tblSellInvoices.InsertOnSubmit(CurrentInvoice);
                        db.SubmitChanges();
                        if (Session.tblSellInvoice.Where(v => v.ID == CurrentInvoice.ID).Count() <= 0)
                            Session.tblSellInvoice.Add(CurrentInvoice);
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
                    else
                        UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update).TheValues.Value;
                    MyFunaction.MessageBoxSave();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }

        }
        public void DisaplyDeletAndEdit(bool state)
        {
            UpdateRecord.Enabled = state;
            gridView1.OptionsBehavior.ReadOnly = !state;
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
            if (tblSellInvoiceBindingSource.Current != null)
            {
                PrintInvo = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                if (Session.tblSellInvoice.Where(i => i.ID == PrintInvo.ID & i.BranchID == PrintInvo.BranchID).Count() > 0)
                {
                    ThreadPrintInvoice = new Thread(FunactionPrintInvoice);
                    ThreadPrintInvoice.IsBackground = true;
                    ThreadPrintInvoice.Start();
                    DisaplyDeletAndEdit(false);
                }
                else
                {
                    if (Properties.Settings.Default.Language == "ar-SA")
                        XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "عفوا قم بحفظ الفاتورة اولا !!!!!", "حفظ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
        public tblSellInvoice CurrentInvoice;
        public tblCustomer CurrentCustomer;
        public tblDefaultSize CurrentDefaultSize;
        bool SaveSuccess = false;
        bool RefreachDeleteInvoice = false;
        public void FunctionAddNewInvoice()
        {
            tblSellInvoiceBindingSource.AddNew();
            AddNew.Enabled = false;
            tblSellInvoice tblSell = tblSellInvoiceBindingSource.Current as tblSellInvoice;
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
            tblSell.TotalFattInvoice = 0;
            tblSell.TotalMony = 0;
            tblSell.TaxAll = 0;
            tblSell.TaxDayn = 0;
            tblSell.TaxMaden = 0;
            tblSell.TotalAll = 0;
            tblSell.TotalDayn = 0;
            tblSell.TotalMaden = 0;
            tblSell.DeliveryDate = DateTime.Now.AddDays(10);
            tblSell.ThePay = "Deferred آجل";
            tblSell.EnterTime = DateTime.Now;
            if (Session.tblSellInvoice.Where(s => s.BranchID == Program.User.BranchID).Count() <= 0)
                tblSell.InvoNumber =   1;
            else
            {
                try
                {
                    tblSell.InvoNumber = (Session.tblSellInvoice.Where(s => s.BranchID == Program.User.BranchID).Max(i => MyFunaction.GetlongValue((i.InvoNumber as string) ?? "0")) + 1).ToString();
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message + "|" + tblSell.InvoNumber);
                    return;
                }
            }
            spn_Paid.ReadOnly = false;
          TblCustomerAddingNew();
            TblSizeAddingNew();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (tblSellInvoiceBindingSource.Current != null)
            {
                if (MyFunaction.MessageBoxDelete() == DialogResult.Yes)
                {
                    using (var db = new DataClasses1DataContext(Program.ConnectionString))
                    {
                        tblSellInvoice Payment = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                        tblSellInvoiceBindingSource.EndEdit();
                        db.tblSellInvoices.DeleteAllOnSubmit(db.tblSellInvoices.Where(m => m.ID == Payment.ID));
                        db.SubmitChanges();
                        RefreachDeleteInvoice = true;
                    }
                }
            }
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
            tblClassesBindingSource.DataSource = Session.tblClasses.ToList();
            tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.ToList();
            tblSellInvoice sale = tblSellInvoiceBindingSource.Current as tblSellInvoice;
            if (sale!=null)
            {
                tblCustomerBindingSource.DataSource = Session.tblCustomer.Where(s=>s.CusNumber==((sale.CusNumber as string)??"0")).ToList();
                tblDefaultSizeBindingSource.DataSource = Session.tblDefaultSize.Where(s => s.ID ==sale.SizeID).ToList();
            }
            searchLookUpEditDefaultSize.Properties.DataSource = Session.tblDefaultSize.ToList();
            searchLookUpEditCustomer.Properties.DataSource = Session.tblCustomer.ToList();
        }
        private void SearchCustomertxtedit_EditValueChanged(object sender, EventArgs e)
        {
            if (searchLookUpEditCustomer.EditValue != null)
            {
                tblCustomer Carr = searchLookUpEditCustomer.GetSelectedDataRow() as tblCustomer;
                if (Carr != null)
                {
                    tblCustomerBindingSource.DataSource = Carr;
                    searchLookUpEditCustomer.EditValue = null;
                }
            }
        }
        private void SearchDefaultSizetxtedit_EditValueChanged(object sender, EventArgs e)
        {
            if (searchLookUpEditDefaultSize.EditValue != null)
            {
                tblDefaultSize Carr = searchLookUpEditDefaultSize.GetSelectedDataRow() as tblDefaultSize;
                if (Carr != null)
                {
                    tblDefaultSizeBindingSource.DataSource = Carr;
                    searchLookUpEditDefaultSize.EditValue = null;
                }
            }
        }
        XtraReportSize ReportSize;
        private void BtnPrintSize_Click(object sender, EventArgs e)
        {
            if (tblSellInvoiceBindingSource.Current!=null)
            {
                tblSellInvoice sale = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                if (Session.tblSellInvoice.Where(i => i.ID == sale.ID & i.BranchID == sale.BranchID).Count() <= 0)
                {
                    if (Properties.Settings.Default.Language == "ar-SA")
                        XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "عفوا قم بحفظ الفاتورة اولا !!!!!", "حفظ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    else if (Properties.Settings.Default.Language == "en-US")
                        XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "Sorry, Save the bill first!!!!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    return;
                }
            ReportSize = new XtraReportSize();
            ReportSize.xrCheckBoxJ1.Checked = checkEditJ1.Checked;
            ReportSize.xrCheckBoxJ2.Checked = checkEditJ2.Checked;
            ReportSize.xrCheckBoxJ3.Checked = checkEditJ3.Checked;
            ReportSize.xrCheckBoxJ4.Checked = checkEditJ4.Checked;

                ReportSize.xrCheckBoxJ11.Checked = checkEditJ1.Checked;
                ReportSize.xrCheckBoxJ21.Checked = checkEditJ2.Checked;
                ReportSize.xrCheckBoxJ31.Checked = checkEditJ3.Checked;
                ReportSize.xrCheckBoxJ41.Checked = checkEditJ4.Checked;

                ReportSize.xrCheckBoxQ1.Checked = checkEditQ1.Checked;
                ReportSize.xrCheckBoxQ2.Checked = checkEditQ2.Checked;
                ReportSize.xrCheckBoxQ3.Checked = checkEditQ3.Checked;
                ReportSize.xrCheckBoxQ4.Checked = checkEditQ4.Checked;

                ReportSize.xrCheckBoxQ11.Checked = checkEditQ1.Checked;
                ReportSize.xrCheckBoxQ21.Checked = checkEditQ2.Checked;
                ReportSize.xrCheckBoxQ31.Checked = checkEditQ3.Checked;
                ReportSize.xrCheckBoxQ41.Checked = checkEditQ4.Checked;

                ReportSize.xrCheckBoxK1.Checked = checkEditK1.Checked;
                ReportSize.xrCheckBoxK2.Checked = checkEditK2.Checked;
                ReportSize.xrCheckBoxK3.Checked = checkEditK3.Checked;
                ReportSize.xrCheckBoxK4.Checked = checkEditK4.Checked;

                ReportSize.xrCheckBoxK11.Checked = checkEditK1.Checked;
                ReportSize.xrCheckBoxK21.Checked = checkEditK2.Checked;
                ReportSize.xrCheckBoxK31.Checked = checkEditK3.Checked;
                ReportSize.xrCheckBoxK41.Checked = checkEditK4.Checked;

                ReportSize.xrCheckBoxS1.Checked = checkEditS1.Checked;
                ReportSize.xrCheckBoxS2.Checked = checkEditS2.Checked;
                ReportSize.xrCheckBoxS3.Checked = checkEditS3.Checked;

                ReportSize.xrCheckBoxS11.Checked = checkEditS1.Checked;
                ReportSize.xrCheckBoxS21.Checked = checkEditS2.Checked;
                ReportSize.xrCheckBoxS31.Checked = checkEditS3.Checked;

                ReportSize.lbl_TotalFinal.Text = ((sale.TotalFinal as double?)??0).ToString();
                ReportSize.lbl_Total.Text = ((sale.TotalMony as double?) ?? 0).ToString();
                ReportSize.lbl_Tax.Text = ((sale.TotalFattInvoice as double?) ?? 0).ToString();
                ReportSize.lbl_Remining.Text = ((sale.MonyRemin as double?) ?? 0).ToString();
                ReportSize.lbl_Paid.Text = ((sale.MonyOrpon as double?) ?? 0).ToString();
                var size = Session.tblDefaultSize.Where(z => z.ID == sale.SizeID).ToList();
                if (size.Count()>0)
                {
                    ReportSize.tall.Text = ((size[0].tall as string) ?? "").ToString();
                    ReportSize.shoulder.Text = ((size[0].shoulder as string) ?? "").ToString();
                    ReportSize.hands.Text = ((size[0].hands as string) ?? "").ToString();
                    ReportSize.middle.Text = ((size[0].middle as string) ?? "").ToString();
                    ReportSize.kapak.Text = ((size[0].kapak as string) ?? "").ToString();
                    ReportSize.kom.Text = ((size[0].kom as string) ?? "").ToString();
                    ReportSize.neck.Text = ((size[0].neck as string) ?? "").ToString();
                    ReportSize.breast.Text = ((size[0].breast as string) ?? "").ToString();

                    ReportSize.tall1.Text = ((size[0].tall as string) ?? "").ToString();
                    ReportSize.shoulder1.Text = ((size[0].shoulder as string) ?? "").ToString();
                    ReportSize.hands1.Text = ((size[0].hands as string) ?? "").ToString();
                    ReportSize.middle1.Text = ((size[0].middle as string) ?? "").ToString();
                    ReportSize.kapak1.Text = ((size[0].kapak as string) ?? "").ToString();
                    ReportSize.kom1.Text = ((size[0].kom as string) ?? "").ToString();
                    ReportSize.neck1.Text = ((size[0].neck as string) ?? "").ToString();
                    ReportSize.breast1.Text = ((size[0].breast as string) ?? "").ToString();

                    ReportSize.A1.Text = ((size[0].F1 as string) ?? "").ToString();
                    ReportSize.A2.Text = ((size[0].F2 as string) ?? "").ToString();
                    ReportSize.A3.Text = ((size[0].F3 as string) ?? "").ToString();
                    ReportSize.A4.Text = ((size[0].F4 as string) ?? "").ToString();

                    ReportSize.A11.Text = ((size[0].F1 as string) ?? "").ToString();
                    ReportSize.A21.Text = ((size[0].F2 as string) ?? "").ToString();
                    ReportSize.A31.Text = ((size[0].F3 as string) ?? "").ToString();
                    ReportSize.A41.Text = ((size[0].F4 as string) ?? "").ToString();
                    ReportSize.Notes.Text = ((size[0].Notes as string) ?? "").ToString();
                    ReportSize.Notes1.Text = ((size[0].Notes as string) ?? "").ToString();
                }
                ReportSize.lbl_ID.Text = ((sale.InvoNumber as string) ?? "").ToString();
                ReportSize.lbl_CustomerPhone.Text = (((Session.tblCustomer.FirstOrDefault(c => c.CusNumber == sale.CusNumber).Mobil) as string) ?? "").ToString();
                ReportSize.lbl_TheName.Text = (((Session.tblCustomer.FirstOrDefault(c => c.CusNumber == sale.CusNumber).CustomerName) as string) ?? "").ToString();
                ReportSize.lbl_ID.Text = ((sale.InvoNumber as string) ?? "").ToString();
                ReportSize.lbl_SellDate.Text = sale.SellDate.Value.ToString();
                ReportSize.lbl_DeliveryDate.Text = sale.DeliveryDate.Value.ToString();

                ReportSize.lbl_TheQuantity.Text = ((sale.TheQuantity as int?) ?? 0).ToString();
                ReportSize.lbl_TheQuantity1.Text = ((sale.TheQuantity as int?) ?? 0).ToString();

                ReportSize.lbl_TheQuantity11.Text = ((sale.TheQuantity as int?) ?? 0).ToString();
             
                frmReport.documentViewer1.DocumentSource = ReportSize;
                frmReport.ShowDialog();
            }
        }
        Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
    }
}