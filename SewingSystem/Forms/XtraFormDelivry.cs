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
using ECRPaymentsAPI;
using static SewingSystem.Classes.Master;
using CSHARPDLL;
using DevExpress.Utils.Extensions;

namespace SewingSystem.Forms
{
    public partial class XtraFormDelivry : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblSellInvoice sale;
        string CustName;
        public XtraFormDelivry(tblSellInvoice cus,string CustomerName)
        {
            InitializeComponent();
            sale = cus;
            CustName = CustomerName;
        }
        ComponentFlyoutDialog flyDialog = new ComponentFlyoutDialog();
        tblPayment CurrPay => tblPaymentBindingSource.Current as tblPayment;
        tblSellInvoice CurrInvoice => tblSellInvoiceBindingSource.Current as tblSellInvoice;
        private void Save_Click(object sender, EventArgs e)
        {
            if (CurrPay == null)
                return;
            if (CurrPay.QuanDelivery.Value <= 0 & CurrPay.TheAmount <= 0)
            {
                XtraMessageBox.Show("عفوا لا يمكن ان تكون الكميه المستلمه والمبلغ المستلم تساوي الصفر");
                return;
            }
            if (CurrPay.QuanRemin < 0)
            {
                XtraMessageBox.Show("عفوا لا يمكن ان تكون الكميه المتبقية اقل من الصفر");
                return;
            }
            if (CurrPay.MonyRemin < 0)
            {
                XtraMessageBox.Show("عفوا لا يمكن ان يكون المبلغ المتبقي اقل من الصفر");
                return;
            }
            flyDialog.WaitForm(this, 1);
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                db.tblPayments.InsertOnSubmit(CurrPay);
                db.SubmitChanges();
                CurrInvoice.MonyRemin = CurrPay.MonyRemin;
                CurrInvoice.QuanRemin = CurrPay.QuanRemin;
                var pay = db.tblPayments.Where(i => i.InvoNumber == CurrPay.InvoNumber & i.BranchID == CurrPay.BranchID).ToList();
                if (pay.Any())
                {
                    CurrInvoice.MonyPay = pay.Sum(s => s.TheAmount ?? 0);
                    CurrInvoice.Discount = pay.Sum(s => s.Discount ?? 0);
                }
                db.tblSellInvoices.DeleteAllOnSubmit(db.tblSellInvoices.Where(m => m.ID == CurrInvoice.ID));
                db.tblSellInvoices.InsertOnSubmit(CurrInvoice);
                db.SubmitChanges();
                Session.RefreshDatatSellInvoice(CurrInvoice);
                if (Session.tblPayment.Where(v => v.ID == CurrPay.ID).Count() <= 0)
                    Session.tblPayment.Add(CurrPay);
            }
            flyDialog.WaitForm(this,0);
            MyFunaction.MessageBoxSave();
            Close();
        }
        private void XtraFormDelivery_Load(object sender, EventArgs e)
        {
            tblSellInvoiceBindingSource.DataSource = sale;
            Spn_PaidMeater.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidCash.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            textEditPaidCreditCard.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidVisa.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_PaidChik.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            Spn_Discount.EditValueChanging += Spn_Discount_EditValueChanging;
            spn_Quantity.EditValueChanging += Spn_Quantity_EditValueChanging;
            ecrTimer.Tick += EcrTimer_Tick;
            textEditPaidCreditCard.Properties.Buttons.ForEach(x =>
            {
                x.Visible = Program.Branch.SendToECR;
                switch (x.Kind)
                {
                    case ButtonPredefines.Delete:
                        x.Visible = !((byte)NetworkPaymentType.Geidea920 == Program.Branch.NetworkPaymentType);
                        break;
                    case ButtonPredefines.OK:
                        break;
                }
            });
            tblPaymentBindingSource.AddNew();
            CurrPay.EnterTime = DateTime.Now;
            CurrPay.PayDate = DateTime.Now;
            CurrPay.UserID = Program.User.ID;
            CurrPay.BranchID = Program.Branch.ID;
            CurrPay.CusNumber = sale.CusNumber;
            CurrPay.MonyPay =0;
            CurrPay.TheAmount = 0;
            CurrPay.Cash = 0;
            CurrPay.Chik = 0;
            CurrPay.Visa = 0;
            CurrPay.Meater = 0;
            CurrPay.Network = 0;
            CurrPay.QuanDelivery = 0;
            CurrPay.Discount = 0;
            CurrPay.InvoNumber = sale.InvoNumber;
            txtCustomerName.Text = CustName;
            CurrPay.MonyRemin = sale.MonyRemin;
            CurrPay.QuanRemin = sale.QuanRemin;
            var delivery = Session.tblPayment.Where(s => s.InvoNumber == sale.InvoNumber& s.BranchID == sale.BranchID);
            if (delivery.Count() > 0)
            {
                sale.MonyPay = delivery.Sum(s => s.TheAmount??0);
                sale.Discount =delivery.Sum(s => s.Discount??0);
                txtAllMonyPay.Text = ((sale.MonyPay??0) +(sale.Discount??0)).ToString();// (delivery.Sum(s=>s.TheAmount.Value) + delivery.Sum(s => s.Discount.Value)).ToString();
               txtAllDiscount.Text =(sale.Discount??0).ToString();
            }
        }
        int numPort = 0;
        public bool checkDevice(string valueIPOrPort)
        {
            try
            {
                if (valueIPOrPort.Contains("."))
                {
                    string[] array = valueIPOrPort.Split('.');
                    numPort = int.Parse(array[3]) & 0xFF;
                    numPort <<= 8;
                    numPort |= int.Parse(array[2]) & 0xFF;
                    numPort <<= 8;
                    numPort |= int.Parse(array[1]) & 0xFF;
                    numPort <<= 8;
                    numPort |= int.Parse(array[0]) & 0xFF;
                }
                else
                    numPort = int.Parse(valueIPOrPort);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        PaymentResponseViewModel TaskpaymentResponseViewModel;
        private void SendECR(string ecrAmount)
        {
            string ecrPort = Program.Branch.EcrPort;
            byte[] MSIGD = Encoding.ASCII.GetBytes("PUR");
            byte[] ECRno = Encoding.ASCII.GetBytes("123");
            byte[] Rcptno = Encoding.ASCII.GetBytes((CurrInvoice?.InvoNumber ?? 0).ToString());
            byte[] Amount = Encoding.ASCII.GetBytes(ecrAmount);
            byte[] Addfield1 = Encoding.ASCII.GetBytes("E");
            byte[] Addfield2 = Encoding.ASCII.GetBytes(" ");
            byte[] Addfield3 = Encoding.ASCII.GetBytes(" ");
            byte[] Addfield4 = Encoding.ASCII.GetBytes(" ");
            byte[] Addfield5 = Encoding.ASCII.GetBytes(" ");

            this.isProcessing = true;
            UpdateECRresponse();

            try
            {
                Task.Run(() =>
                {
                    byte[] ecr = CUSTOMDLLNET.StartAUCECRTran(MSIGD, ECRno, Rcptno, Amount, Addfield1,
                        Addfield2, Addfield3, Addfield4, Addfield5, ecrPort, 1);

                    string result = CUSTOMDLLNET.mirrorMessage();
                    this.isProcessing = false;
                });
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private bool ValidateECR()
        {
            bool isValid = true;

            try
            {
                isValid = CUSTOMDLLNET.checkDevice(Program.Branch.EcrPort);
                if (!isValid) ClsXtraMssgBox.ShowError(Session.LangEng ? "Failed to connect to ATM" : "فشل الإتصال بالصرافة الآلية");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }

            return isValid;
        }

        private bool ValidateECRamount(out string ecrAmountString)
        {
            ecrAmountString = null;
            bool isValid = !decimal.TryParse(textEditPaidCreditCard.Text, out decimal ecrAmount) || ecrAmount == 0 ? false : true;
            if (!isValid)
            {
                ClsXtraMssgBox.ShowError(Session.LangEng ? "You must enter a credit card payment first!" : "يجب إدخال المدفوع شبكه أولاً!");
                textEditPaidCreditCard.Focus();
            }
            else //if ((NetworkPaymentType)Program.Branch.NetworkPaymentType == NetworkPaymentType.alhamrani)
            {
                int ecrAmountInt = (int)ecrAmount;
                int ecrAmountCount = ecrAmountInt.ToString().Length;
                while (ecrAmountCount < 10)
                {
                    ecrAmountCount++;
                    ecrAmountString += "0";
                }
                ecrAmountString += ecrAmountInt.ToString();
                if (ecrAmount % 1 != 0)
                {
                    int ecrAmountFraction = Convert.ToInt32((ecrAmount % 1) * 100m);
                    ecrAmountString += ecrAmountFraction.ToString();
                }
                while (ecrAmountString.Length < 12) ecrAmountString += "0";
            }
            return isValid;
        }

        private void UpdateECRresponse()
        {
            this.ecrTimer.Interval = 500;
            this.ecrTimer.Start();
        }
        bool isProcessing;
        private void EcrTimer_Tick(object sender, EventArgs e)
        {
            if (Program.Branch.NetworkPaymentType == (byte)NetworkPaymentType.alhamrani)
            {
                string result = CUSTOMDLLNET.mirrorMessage();
                labelECR.Text = $"الحالة: {result}";
                if (!this.isProcessing)
                    this.ecrTimer.Stop();
            }
            else if (Program.Branch.NetworkPaymentType == (byte)NetworkPaymentType.Geidea920)
            {
                labelECR.Text = $"الحالة: {TaskpaymentResponseViewModel?.ResposeMsg}";
                if (!this.isProcessing)
                    this.ecrTimer.Stop();
            }
        }
        Timer ecrTimer = new Timer();
        private async void textEditPaidCreditCard_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                switch (e.Button.Kind)
                {
                    case ButtonPredefines.Delete:
                        if ((NetworkPaymentType)Program.Branch.NetworkPaymentType == NetworkPaymentType.alhamrani)
                            CUSTOMDLLNET.sendCancel();
                        break;
                    case ButtonPredefines.OK:
                        bool isValid;
                        textEditPaidCreditCard.Focus();
                        if (!Program.Branch.SendToECR) return;
                        switch ((NetworkPaymentType)Program.Branch.NetworkPaymentType)
                        {
                            case NetworkPaymentType.alhamrani:
                                if (!ValidateECR()) return;
                                if (!ValidateECRamount(out string ecrAmount)) return;
                                SendECR(ecrAmount);
                                break;
                            case NetworkPaymentType.Geidea920:
                                try
                                {
                                    isValid = checkDevice(Program.Branch.EcrPort);
                                    if (!isValid) ClsXtraMssgBox.ShowError(Session.LangEng ? "Failed to connect to ATM" : "فشل الإتصال بالصرافة الآلية");
                                    isValid = !double.TryParse(textEditPaidCreditCard.Text, out double ecrAmount2) || ecrAmount2 == 0 ? false : true;
                                    if (!isValid)
                                    {
                                        ClsXtraMssgBox.ShowError(Session.LangEng ? "You must enter a credit card payment first!" : "يجب إدخال المدفوع بطاقة إئتمان أولاً!");
                                        textEditPaidCreditCard.Focus();
                                        return;
                                    }
                                    PaymentRequestViewModel RequestModel = new ECRPaymentsAPI.PaymentRequestViewModel()
                                    {
                                        Mode = 1,
                                        BaudRate = 38400,
                                        Parity = 0,
                                        DataBits = 8,
                                        StopBits = 0,
                                        ECRReffrence = 1,
                                        EnableReceiptPrint = true,
                                        Amount = ecrAmount2,
                                        ComPort = numPort,
                                    };
                                    e.Button.Enabled = false;
                                    this.isProcessing = true;
                                    UpdateECRresponse();
                                    TaskpaymentResponseViewModel = await GeideaPament.Pay(RequestModel);
                                    this.isProcessing = false;
                                    e.Button.Enabled = true;
                                }
                                catch (Exception ex)
                                {
                                    e.Button.Enabled = true;
                                    XtraMessageBox.Show(ex.Message);
                                }
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ClsXtraMssgBox.ShowError(ex.Message);
                return;
            }
        }

        private void Spn_Quantity_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (CurrInvoice == null)
                return;
            if (e.NewValue.ToString() == "")
                return;
            CurrPay.QuanDelivery = int.Parse(e.NewValue.ToString());
            CurrPay.QuanRemin = (CurrInvoice.QuanRemin ?? 0) - (CurrPay.QuanDelivery?? 0);
        }
        private void Spn_Discount_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (CurrInvoice == null)
                return;
            if (e.NewValue.ToString() == "")
                return;
            CurrPay.Discount = double.Parse(e.NewValue.ToString());
            CurrPay.MonyRemin = (CurrInvoice.MonyRemin ?? 0) - (CurrPay.TheAmount ?? 0) - (CurrPay.Discount ?? 0);
        }
        private void Spn_PaidCash_EditValueChanging1(object sender, ChangingEventArgs e)
        {
            if (CurrInvoice == null)
                return;
            switch (((TextEdit)sender).Name)
            {
                case "Spn_PaidCash":
                    CurrPay.TheAmount = (CurrPay.Chik?? 0) + (CurrPay.Network?? 0)+ (CurrPay.Visa?? 0) + (CurrPay.Meater?? 0);
                    if (e.NewValue.ToString() != "")
                        CurrPay.TheAmount += double.Parse(e.NewValue.ToString());
                    break;
                case "textEditPaidCreditCard":
                    CurrPay.TheAmount = (CurrPay.Chik?? 0) + (CurrPay.Cash?? 0)+ (CurrPay.Visa?? 0) + (CurrPay.Meater?? 0);
                    if (e.NewValue.ToString() != "")
                        CurrPay.TheAmount += double.Parse(e.NewValue.ToString());
                    break;
                case "Spn_PaidChik":
                    CurrPay.TheAmount = (CurrPay.Network?? 0) + (CurrPay.Cash?? 0)+ (CurrPay.Visa?? 0) + (CurrPay.Meater?? 0);
                    if (e.NewValue.ToString() != "")
                        CurrPay.TheAmount += double.Parse(e.NewValue.ToString());
                    break;
                case "Spn_PaidMeater":
                    CurrPay.TheAmount = (CurrPay.Network?? 0) + (CurrPay.Cash?? 0) + (CurrPay.Visa?? 0) + (CurrPay.Chik?? 0);
                    if (e.NewValue.ToString() != "")
                        CurrPay.TheAmount += double.Parse(e.NewValue.ToString());
                    break;
                case "Spn_PaidVisa":
                    CurrPay.TheAmount = (CurrPay.Network?? 0) + (CurrPay.Cash?? 0) + (CurrPay.Meater?? 0) + (CurrPay.Chik?? 0);
                    if (e.NewValue.ToString() != "")
                        CurrPay.TheAmount += double.Parse(e.NewValue.ToString());
                    break;
                default:
                    break;
            }
            CurrPay.MonyRemin = (CurrInvoice.MonyRemin?? 0) - (CurrPay.TheAmount?? 0) - (CurrPay.Discount?? 0);
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}