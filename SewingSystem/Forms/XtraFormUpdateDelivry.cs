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
    public partial class XtraFormUpdateDelivry : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblSellInvoice sale;
      tblPayment Updatepay;
        string CustName;
        public XtraFormUpdateDelivry(tblSellInvoice cus,string CustomerName,tblPayment CurrPayment)
        {
            InitializeComponent();
            sale = cus;
            CustName = CustomerName;
            Updatepay=CurrPayment;
        }
        public void RefrechData()
        {
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
                tblPaymentBindingSource.DataSource = db.tblPayments.Where(p => p.ID == Updatepay.ID).ToList();
        }
        tblSellInvoice CurrSale => tblSellInvoiceBindingSource.Current as tblSellInvoice;
        tblPayment CurrPayment => tblPaymentBindingSource.Current as tblPayment;
        bool ValidateData()
        {
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
        private void Save_Click(object sender, EventArgs e)
        {
            if (this.CurrPayment == null)
                return;
            if (!ValidateData())
                return;
            flyDialog.WaitForm(this, 1);
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                db.tblPayments.DeleteAllOnSubmit(db.tblPayments.Where(m => m.ID == CurrPayment.ID));
                db.tblPayments.InsertOnSubmit(CurrPayment);
                db.SubmitChanges();
                CurrSale.MonyRemin = CurrPayment.MonyRemin;
                CurrSale.QuanRemin = CurrPayment.QuanRemin;
                var paylist = db.tblPayments.Where(i => i.InvoNumber == CurrPayment.InvoNumber & i.BranchID == CurrPayment.BranchID).ToList();
                CurrSale.MonyPay = paylist?.Sum(s => s.TheAmount ?? 0);
                CurrSale.Discount = paylist?.Sum(s => s.Discount ?? 0);
                db.tblSellInvoices.DeleteAllOnSubmit(db.tblSellInvoices.Where(m => m.ID == CurrSale.ID));
                db.tblSellInvoices.InsertOnSubmit(CurrSale);
                db.SubmitChanges();
                Session.RefreshDatatSellInvoice(CurrSale);
                if (Session.tblPayment.Any(x => x.ID == CurrPayment.ID))
                {
                    int index = Session.tblPayment.IndexOf(Session.tblPayment.Single(x => x.ID == CurrPayment.ID));
                    Session.tblPayment.Remove(Session.tblPayment.Single(x => x.ID == CurrPayment.ID));
                    Session.tblPayment.Insert(index, CurrPayment);
                }
                else
                    Session.tblPayment.Add(CurrPayment);
            }
            flyDialog.WaitForm(this, 0);
            MyFunaction.MessageBoxSave();
            Close();
        }
        private void XtraFormDelivery_Load(object sender, EventArgs e)
        {
            tblSellInvoiceBindingSource.DataSource = sale;
            RefrechData();
            Spn_PaidMeater.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidCash.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidNetwork.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidVisa.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidChik.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_Discount.EditValueChanging += Spn_Discount_EditValueChanging;
            spn_Quantity.EditValueChanging += Spn_Quantity_EditValueChanging;
            txtCustomerName.Text = CustName;
            var delivery = Session.tblPayment.Where(s => s.InvoNumber == sale.InvoNumber& s.BranchID == sale.BranchID);
            if (delivery.Count() > 0)
            {
               txtAllMonyPay.Text= (delivery.Sum(s => s.TheAmount?? 0)- (Updatepay.TheAmount ?? 0)).ToString();
               txtAllDiscount.Text =( delivery.Sum(s => s.Discount ??0) - (Updatepay.Discount ?? 0)).ToString();
                txtAllQuantity.Text = (delivery.Sum(s =>s.QuanDelivery??0) - (Updatepay.QuanDelivery ?? 0)).ToString();
            }
        }
        private void Spn_Quantity_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (CurrSale == null)
                return;
            if (e.NewValue.ToString() == "")
                return;
            CurrPayment.QuanDelivery = int.Parse(e.NewValue.ToString());
            CurrPayment.QuanRemin = (CurrSale.TheQuantity ?? 0) - (CurrPayment.QuanDelivery ?? 0) - int.Parse(txtAllQuantity.Text.ToString());
        }
        private void Spn_Discount_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (CurrSale == null)
                return;
            if (e.NewValue.ToString() == "")
                return;
            CurrPayment.Discount = double.Parse(e.NewValue.ToString());
            if (txtAllMonyPay.EditValue.ToString() != "")
                AllMonyPay = double.Parse(txtAllMonyPay.EditValue.ToString());
            else
                AllMonyPay = 0;
            if (txtAllDiscount.EditValue.ToString() != "")
                AllDiscount = double.Parse(txtAllDiscount.EditValue.ToString());
            else
                AllDiscount = 0;
            CurrPayment.MonyRemin = (CurrSale.TotalFinal?? 0) -(CurrSale.MonyOrpon?? 0) - (CurrPayment.TheAmount?? 0) - (CurrPayment.Discount?? 0) - AllMonyPay - AllDiscount;
        }
        private void Spn_PaidCash_EditValueChanging1(object sender, ChangingEventArgs e)
        {
            if (CurrSale == null)
                return;
            switch (((TextEdit)sender).Name)
                {
                    case "Spn_PaidCash":
                        CurrPayment.TheAmount = (CurrPayment.Chik?? 0) + (CurrPayment.Network?? 0)+ (CurrPayment.Visa?? 0) + (CurrPayment.Meater?? 0);
                        if (e.NewValue.ToString() != "")
                            CurrPayment.TheAmount += double.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidNetwork":
                        CurrPayment.TheAmount = (CurrPayment.Chik?? 0) + (CurrPayment.Cash?? 0)+ (CurrPayment.Visa?? 0) + (CurrPayment.Meater?? 0);
                        if (e.NewValue.ToString() != "")
                            CurrPayment.TheAmount += double.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidChik":
                        CurrPayment.TheAmount = (CurrPayment.Network?? 0) + (CurrPayment.Cash?? 0)+ (CurrPayment.Visa?? 0) + (CurrPayment.Meater?? 0);
                        if (e.NewValue.ToString() != "")
                            CurrPayment.TheAmount += double.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidMeater":
                        CurrPayment.TheAmount = (CurrPayment.Network?? 0) + (CurrPayment.Cash?? 0)+ (CurrPayment.Visa?? 0) + (CurrPayment.Chik?? 0);
                        if (e.NewValue.ToString() != "")
                            CurrPayment.TheAmount += double.Parse(e.NewValue.ToString());
                        break;
                    case "Spn_PaidVisa":
                        CurrPayment.TheAmount = (CurrPayment.Network?? 0) + (CurrPayment.Cash?? 0)+ (CurrPayment.Meater?? 0) + (CurrPayment.Chik?? 0);
                        if (e.NewValue.ToString() != "")
                            CurrPayment.TheAmount += double.Parse(e.NewValue.ToString());
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
                CurrPayment.MonyRemin = (CurrSale.TotalFinal?? 0) -(CurrSale.MonyOrpon?? 0)- (CurrPayment.TheAmount?? 0) -(CurrPayment.Discount?? 0)- AllMonyPay - AllDiscount;
        }
        double AllMonyPay;
        double AllDiscount;
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}