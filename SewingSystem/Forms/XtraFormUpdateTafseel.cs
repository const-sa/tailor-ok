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
    public partial class XtraFormUpdateTafseel : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblCustomer CurCus;
        tblSellInvoice sale;
        string CustName;
        public XtraFormUpdateTafseel(tblSellInvoice cus, string CustomerName)
        {
            InitializeComponent();
            sale = cus;
            CustName = CustomerName;
        }
        public void RefrechData()
        {
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                tblSellInvoiceBindingSource.DataSource = db.tblSellInvoices.Where(p => p.ID == sale.ID).ToList();
            }
        }
      
        private void Save_Click(object sender, EventArgs e)
        {
            if (tblSellInvoiceBindingSource.Current != null)
            {
                tblSellInvoiceBindingSource.EndEdit();
                tblSellInvoice Sale = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                if (Sale.TheQuantity.Value <= 0)
                {
                    XtraMessageBox.Show("عفوا لا يمكن ان تكون الكميه تساوي الصفر");
                    return;
                }
                if (Sale.TotalFinal <= 0)
                {
                    XtraMessageBox.Show("عفوا لا يمكن ان يكون المبلغ يساوي الصفر");
                    return;
                }
                if (Sale.QuanRemin.Value<0)
                {
                    XtraMessageBox.Show("عفوا لا يمكن ان تكون الكميه المتبقة اقل من الصفر");
                    return;
                }
                if (Sale.MonyRemin < 0)
                {
                    XtraMessageBox.Show("عفوا لا يمكن ان يكون المبلغ المتبقي اقل من الصفر");
                    return;
                }
                using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    db.tblSellInvoices.DeleteAllOnSubmit(db.tblSellInvoices.Where(m => m.ID == Sale.ID));
                    db.tblSellInvoices.InsertOnSubmit(Sale);
                    db.SubmitChanges();
                 
                }
                MyFunaction.MessageBoxSave();
                Close();
            }
        }
        private void XtraFormTafseel_Load(object sender, EventArgs e)
        {
            RefrechData();
            Spn_PaidMeater.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidCash.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidNetwork.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidVisa.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidChik.EditValueChanging += Spn_PaidCash_EditValueChanging1;

            spn_Net.EditValueChanging += Spn_Tax_EditValueChanging1;
            spn_Quantity.EditValueChanging += Spn_Quantity_EditValueChanging;
            var astlam = Session.tblPayment.Where(p=>p.InvoNumber== sale.InvoNumber & p.BranchID == sale.BranchID);
            if (astlam.Count()>0)
            {
                txtQuaDeliver.Text = astlam.Sum(s => (s.QuanDelivery as int?)??0).ToString();
                txtAllMonyPay.Text = astlam.Sum(s => (s.TheAmount as double?)??0).ToString();
                txtAllDiscount.Text = astlam.Sum(s => (s.Discount as double?) ?? 0).ToString();
            }
            txtCustomerName.Text = CustName;
            txtName.Focus();
        }

        private void Spn_Quantity_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (tblSellInvoiceBindingSource.Current != null)
            {
                tblSellInvoice sell = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                if (e.NewValue.ToString() != "")
                {
                    sell.TheQuantity = int.Parse(e.NewValue.ToString());
                    if (txtAllDiscount.Text != "")
                        sell.QuanRemin = sell.TheQuantity - int.Parse(txtQuaDeliver.Text);
                    else
                        sell.QuanRemin = sell.TheQuantity;
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
                if (txtAllMonyPay.EditValue.ToString() != "")
                    AllMonyPay = double.Parse(txtAllMonyPay.EditValue.ToString());
                else
                    AllMonyPay = 0;
                if (txtAllDiscount.EditValue.ToString() != "")
                    AllDiscount = double.Parse(txtAllDiscount.EditValue.ToString());
                else
                    AllDiscount = 0;
                sell.MonyRemin = ((sell.TotalFinal as double?) ?? 0) -
                    ((sell.MonyOrpon as double?) ?? 0) 
                 - AllMonyPay - AllDiscount;
            }
        }
        double AllMonyPay;
        double AllDiscount;
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
                            if (txtAllMonyPay.EditValue.ToString() != "")
                                AllMonyPay = double.Parse(txtAllMonyPay.EditValue.ToString());
                            else
                                AllMonyPay = 0;
                            if (txtAllDiscount.EditValue.ToString() != "")
                                AllDiscount = double.Parse(txtAllDiscount.EditValue.ToString());
                            else
                                AllDiscount = 0;
                            sell.MonyRemin = double.Parse(e.NewValue.ToString()) -
                                ((sell.MonyOrpon as double?) ?? 0)
                             - AllMonyPay - AllDiscount;
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
                            if (txtAllMonyPay.EditValue.ToString() != "")
                                AllMonyPay = double.Parse(txtAllMonyPay.EditValue.ToString());
                            else
                                AllMonyPay = 0;
                            if (txtAllDiscount.EditValue.ToString() != "")
                                AllDiscount = double.Parse(txtAllDiscount.EditValue.ToString());
                            else
                                AllDiscount = 0;
                            sell.MonyRemin = ((sell.TotalFinal as double?) ?? 0) -
                                ((sell.MonyOrpon as double?) ?? 0)
                             - AllMonyPay - AllDiscount;
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
    }
}