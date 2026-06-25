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

namespace SewingSystem.Forms
{
    public partial class XtraFormTafseel : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblCustomer CurCus;
        public XtraFormTafseel(tblCustomer cus)
        {
            InitializeComponent();
            CurCus = cus;
        }
      
        private void Save_Click(object sender, EventArgs e)
        {
            if (tblSellInvoiceBindingSource.Current != null)
            {
                tblSellInvoiceBindingSource.EndEdit();
                tblSellInvoice Sale = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                Sale.QuanRemin = Sale.TheQuantity;
                if (Sale.TheQuantity.Value <=0)
                {
                    XtraMessageBox.Show("عفوا لا يمكن ان تكون الكميه تساوي الصفر");
                    return;
                }
                if (Sale.TotalFinal <= 0)
                {
                    XtraMessageBox.Show("عفوا لا يمكن ان يكون المبلغ يساوي الصفر");
                    return;
                }
                if (Sale.MonyRemin < 0)
                {
                    XtraMessageBox.Show("عفوا لا يمكن ان يكون المبلغ المتبقي اقل من الصفر");
                    return;
                }
                using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    Sale.TaxAll = Sale.TotalFattInvoice + Sale.TaxMaden - Sale.TaxDayn;
                    Sale.TotalAll = ((Sale.TotalMony as double?) ?? 0) + ((Sale.TotalMaden as double?) ?? 0) - ((Sale.TotalDayn as double?) ?? 0)+ ((Sale.TaxAll as double?) ?? 0);
                    Sale.MonyRemin = ((Sale.TotalAll as double?) ?? 0) - ((Sale.MonyPay as double?) ?? 0) - ((Sale.MonyOrpon as double?) ?? 0);
                    db.tblSellInvoices.DeleteAllOnSubmit(db.tblSellInvoices.Where(m => m.ID == Sale.ID));
                    db.tblSellInvoices.InsertOnSubmit(Sale);
                    db.SubmitChanges();
                    if (Session.tblSellInvoice.Where(v => v.ID == Sale.ID).Count() <= 0)
                        Session.tblSellInvoice.Add(Sale);
                }
                MyFunaction.MessageBoxSave();
                Close();
            }
        }
        private void TblClasses_ListChanged(object sender, ListChangedEventArgs e)
        {
            tblClasseBindingSource.DataSource = Session.tblClasses.ToList();
        }
        private void XtraFormTafseel_Load(object sender, EventArgs e)
        {
            tblClasseBindingSource.DataSource = Session.tblClasses.ToList();
            tblFactorieBindingSource.DataSource = Session.tblFactorie.ToList();
            tblTailorBindingSource.DataSource = Session.tblTailor.ToList();
            Spn_PaidMeater.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidCash.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidNetwork.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidVisa.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidChik.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            spn_Net.EditValueChanging += Spn_Tax_EditValueChanging1;
            spn_BeforTax.EditValueChanging += Spn_Tax_EditValueChanging1;
            Session.tblClasses.ListChanged += TblClasses_ListChanged;
            tblSellInvoiceBindingSource.AddNew();
            tblSellInvoice tblSellInvoice = tblSellInvoiceBindingSource.Current as tblSellInvoice;
            tblSellInvoice.EnterTime = DateTime.Now;
            tblSellInvoice.SellDate = DateTime.Now;
            tblSellInvoice.UserID = Program.User.ID;
            tblSellInvoice.BranchID = Program.Branch.ID;
            tblSellInvoice.MonyPay =0;
            tblSellInvoice.MonyRemin = 0;
            tblSellInvoice.QuanRemin = 0;
            tblSellInvoice.TheQuantity = 0;
            tblSellInvoice.Cash = 0;
            tblSellInvoice.Chik = 0;
            tblSellInvoice.Network = 0;
            tblSellInvoice.Visa = 0;
            tblSellInvoice.Meater = 0;
            tblSellInvoice.Discount = 0;
            tblSellInvoice.TotalFinal = 0;
            tblSellInvoice.MonyOrpon = 0;
            tblSellInvoice.TaxAll = 0;
            tblSellInvoice.TaxDayn = 0;
            tblSellInvoice.TaxMaden = 0;
            tblSellInvoice.TotalAll = 0;
            tblSellInvoice.TotalDayn = 0;
            tblSellInvoice.TotalMaden = 0;
            tblSellInvoice.CusNumber = CurCus.CusNumber;
            txtCustomerName.Text = ((CurCus.CustomerName as string) ?? "");
            if (Session.tblSellInvoice.Where(s => s.BranchID == Program.User.BranchID).Count() <= 0)
                tblSellInvoice.InvoNumber = "1";
            else
            {
                try
                {
                    tblSellInvoice.InvoNumber = (Session.tblSellInvoice.Where(s => s.BranchID == Program.User.BranchID).Max(i => MyFunaction.GetlongValue((i.InvoNumber as string) ?? "0")) + 1).ToString();
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message + "|" + tblSellInvoice.InvoNumber);
                    return;
                }
            }
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
                sell.MonyRemin = ((sell.TotalFinal as double?) ?? 0)  - ((sell.MonyOrpon as double?) ?? 0) ;
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
                        if (e.NewValue.ToString() != "") {    if ((Program.Branch.UseTax as bool?) ?? false)
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
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkEdit11_CheckedChanged(object sender, EventArgs e)
        {
                layoutControlItemGN.Enabled = !checkEdit11.Checked;
                layoutControlItemGID.Enabled = !checkEdit11.Checked;
                layoutControlItemGE.Enabled = checkEdit11.Checked;
        }
    }
}