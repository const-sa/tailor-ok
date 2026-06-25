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
    public partial class XtraFormPayment : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblSellInvoice sale;
        string CustName;
        public XtraFormPayment(tblSellInvoice cus,string CustomerName)
        {
            InitializeComponent();
            sale = cus;
            CustName = CustomerName;
        }
        private void Save_Click(object sender, EventArgs e)
        {
            if (tblPaymentBindingSource.Current != null)
            {
                tblPaymentBindingSource.EndEdit();
                tblPayment pay = tblPaymentBindingSource.Current as tblPayment;
                tblSellInvoice Sale = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                if (pay.QuanDelivery.Value <= 0& pay.TheAmount <= 0)
                {
                    XtraMessageBox.Show("عفوا لا يمكن ان تكون الكميه المستلمه والمبلغ المستلم تساوي الصفر");
                    return;
                }
                if (pay.QuanRemin < 0)
                {
                    XtraMessageBox.Show("عفوا لا يمكن ان تكون الكميه المتبقية اقل من الصفر");
                    return;
                }
                if (pay.MonyRemin < 0)
                {
                    XtraMessageBox.Show("عفوا لا يمكن ان يكون المبلغ المتبقي اقل من الصفر");
                    return;
                }
                using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    db.tblPayments.DeleteAllOnSubmit(db.tblPayments.Where(m => m.ID == pay.ID));
                    db.tblPayments.InsertOnSubmit(pay);
                    db.SubmitChanges();
                    Sale.MonyRemin = pay.MonyRemin;
                    Sale.QuanRemin = pay.QuanRemin;
                    Sale.MonyPay = db.tblPayments.Where(i => i.InvoNumber == pay.InvoNumber & i.BranchID == pay.BranchID).Sum(s => ((s.TheAmount as decimal?) ?? 0));
                    Sale.Discount = db.tblPayments.Where(i => i.InvoNumber == pay.InvoNumber & i.BranchID == pay.BranchID).Sum(s => ((s.Discount as decimal?) ?? 0));
                    db.tblSellInvoices.DeleteAllOnSubmit(db.tblSellInvoices.Where(m => m.ID == Sale.ID));
                    db.tblSellInvoices.InsertOnSubmit(Sale);
                    db.SubmitChanges();
                }
                MyFunaction.MessageBoxSave();
                Close();
            }
        }
        private void XtraFormDelivery_Load(object sender, EventArgs e)
        {
            tblSellInvoiceBindingSource.DataSource = sale;
            Spn_PaidMeater.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidCash.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidNetwork.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidVisa.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidChik.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_Discount.EditValueChanging += Spn_Discount_EditValueChanging;
            spn_Quantity.EditValueChanging += Spn_Quantity_EditValueChanging;
            tblPaymentBindingSource.AddNew();
            tblPayment tblPayment = tblPaymentBindingSource.Current as tblPayment;
            tblPayment.EnterTime = DateTime.Now;
            tblPayment.PayDate = DateTime.Now;
            tblPayment.UserID = Program.User.ID;
            tblPayment.BranchID = Program.Branch.ID;
            tblPayment.CusNumber = sale.CusNumber;
            tblPayment.MonyPay =0;
            tblPayment.TheAmount = 0;
            tblPayment.Cash = 0;
            tblPayment.Chik = 0;
            tblPayment.Visa = 0;
            tblPayment.Meater = 0;
            tblPayment.Network = 0;
            tblPayment.QuanDelivery = 0;
            tblPayment.Discount = 0;
            tblPayment.InvoNumber = sale.InvoNumber;
            txtCustomerName.Text = CustName;
            tblPayment.MonyRemin = sale.MonyRemin;
            tblPayment.QuanRemin = sale.QuanRemin;
            var delivery = Session.tblPayment.Where(s => s.InvoNumber == sale.InvoNumber& s.BranchID == sale.BranchID);
            if (delivery.Count() > 0)
            {
                sale.MonyPay = (delivery.Sum(s => s.TheAmount.Value) as decimal?) ?? 0;
                sale.Discount = (delivery.Sum(s => s.Discount.Value) as decimal?) ?? 0;
                txtAllMonyPay.Text = (sale.MonyPay.Value + sale.Discount.Value).ToString();// (delivery.Sum(s=>s.TheAmount.Value) + delivery.Sum(s => s.Discount.Value)).ToString();
               txtAllDiscount.Text =(sale.Discount.Value).ToString();
            }
        }
        private void Spn_Quantity_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (tblSellInvoiceBindingSource.Current != null)
            {
                tblSellInvoice sell = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                tblPayment pay = tblPaymentBindingSource.Current as tblPayment;
                if (e.NewValue.ToString() != "")
                {
                    pay.QuanDelivery = int.Parse(e.NewValue.ToString());
                    pay.QuanRemin = ((sell.QuanRemin as int?) ?? 0) - ((pay.QuanDelivery as int?) ?? 0);
                }
            }
        }
        private void Spn_Discount_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (tblSellInvoiceBindingSource.Current != null)
            {
                tblSellInvoice sell = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                tblPayment pay = tblPaymentBindingSource.Current as tblPayment;
                if (e.NewValue.ToString() != "")
                {
                    pay.Discount = decimal.Parse(e.NewValue.ToString());
                    pay.MonyRemin = ((sell.MonyRemin as decimal?) ?? 0) - ((pay.TheAmount as decimal?) ?? 0) - ((pay.Discount as decimal?) ?? 0);
                }
            }
        }
        private void Spn_PaidCash_EditValueChanging1(object sender, ChangingEventArgs e)
        {
            if (tblSellInvoiceBindingSource.Current != null)
            {
                tblSellInvoice sell = tblSellInvoiceBindingSource.Current as tblSellInvoice;
                tblPayment pay = tblPaymentBindingSource.Current as tblPayment;
                switch (((TextEdit)sender).Name)
                {
                    case "Spn_PaidCash":
                        pay.TheAmount = ((pay.Chik as decimal?) ?? 0) + ((pay.Network as decimal?) ?? 0)
                       + ((pay.Visa as decimal?) ?? 0) + ((pay.Meater as decimal?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            pay.TheAmount += decimal.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidNetwork":
                        pay.TheAmount = ((pay.Chik as decimal?) ?? 0) + ((pay.Cash as decimal?) ?? 0)
                       + ((pay.Visa as decimal?) ?? 0) + ((pay.Meater as decimal?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            pay.TheAmount += decimal.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidChik":
                        pay.TheAmount = ((pay.Network as decimal?) ?? 0) + ((pay.Cash as decimal?) ?? 0)
                       + ((pay.Visa as decimal?) ?? 0) + ((pay.Meater as decimal?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            pay.TheAmount += decimal.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidMeater":
                        pay.TheAmount = ((pay.Network as decimal?) ?? 0) + ((pay.Cash as decimal?) ?? 0)
                       + ((pay.Visa as decimal?) ?? 0) + ((pay.Chik as decimal?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            pay.TheAmount += decimal.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidVisa":
                        pay.TheAmount = ((pay.Network as decimal?) ?? 0) + ((pay.Cash as decimal?) ?? 0)
                       + ((pay.Meater as decimal?) ?? 0) + ((pay.Chik as decimal?) ?? 0);
                        if (e.NewValue.ToString() != "")
                            pay.TheAmount += decimal.Parse(e.NewValue.ToString());
                        break;
                    default:
                        break;
                }
                pay.MonyRemin = ((sell.MonyRemin as decimal?) ?? 0) - ((pay.TheAmount as decimal?) ?? 0) - ((pay.Discount as decimal?) ?? 0);
            }
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            tblPayment tblPayment = tblPaymentBindingSource.Current as tblPayment;

            Close();
        }
    }
}