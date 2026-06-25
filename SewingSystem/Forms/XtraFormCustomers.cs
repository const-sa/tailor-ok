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
using ClosedXML.Excel;

namespace SewingSystem.Forms
{
    public partial class XtraFormCustomers : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        public XtraFormCustomers()
        {
            InitializeComponent();
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "العملاء" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            btnAddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            btnDelete.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Delete)?.TheValues??false;
            btnUpdate.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;

            btnTafseel.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == "تفصيل")?.TheValues??false;
            btnUpdateTafseel.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == "تعديل تفصيل")?.TheValues??false;
            btnDelivry.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == "استلام")?.TheValues??false;
            btnUpdateDilvery.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == "تعديل استلام")?.TheValues??false;
            btnDeleteInvoice.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == "حذف تفصيل")?.TheValues??false;
            btnOkDelivry.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == "مستلم")?.TheValues??false;
            btnNoDelivry.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == "غير مستلم")?.TheValues??false;
            btnPrintInvoice.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == "طباعة فاتورة التفصيل")?.TheValues??false;
            btnGomashType.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == "نوع القماش")?.TheValues??false;
            btnSms.Click += btnSearchByMobil_Click;
            BtnSendWhatsApp.Click += BtnSendWhatsApp_Click;
        }

       
        bool isNew = false;
        private void btnTafseel_Click(object sender, EventArgs e)
        {
            if (CurrCustomer == null)
                return;
            if (btnSave.Enabled)
            {
                XtraMessageBox.Show("قم بحفظ بيانات العميل اولا");
                return;
            }
            tblSellInvoice tblSell = new tblSellInvoice
            {
                EnterTime = DateTime.Now,
                SellDate = DateTime.Now,
                UserID = Program.User.ID,
                BranchID = Program.Branch.ID,
                MonyPay = 0,
                MonyRemin = 0,
                QuanRemin = 0,
                TheQuantity = 1,
                Cash = 0,
                Chik = 0,
                Network = 0,
                Visa = 0,
                Meater = 0,
                Discount = 0,
                TotalFinal = 0,
                MonyOrpon = 0,
                TotalAfterDiscount = 0,
                TotalFattInvoice = 0,
                TotalMony = 0,
                TaxAll = 0,
                TaxDayn = 0,
                TaxMaden = 0,
                TotalAll = 0,
                TotalDayn = 0,
                TotalMaden = 0,
                QuantityM = 3.50,
                IsExitComash = false,
                DeliveryDate = DateTime.Now.AddDays(10),
                ThePay = "Deferred آجل",
                CusNumber = CurrCustomer?.CusNumber,
            };
            new XtraFormDefultSize2(tblSell,true).ShowDialog();
            tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(c => c.CusNumber == CurrCustomer.CusNumber & c.BranchID == CurrCustomer.BranchID).ToList();
        }
        tblCustomer CurrCustomer => tblCustomerBindingSource.Current as tblCustomer;
        private void btnSize_Click(object sender, EventArgs e)
        {
            if (CurrCustomer == null)
                return;
            if (btnSave.Enabled)
            {
                XtraMessageBox.Show("قم بحفظ بيانات العميل اولا");
                return;
            }
            new XtraFormDefaultSize(CurrCustomer.CusNumber).ShowDialog();
        }
        tblSellInvoice CurrSale => tblSellInvoiceBindingSource.Current as tblSellInvoice;
        private void btnDelivry_Click(object sender, EventArgs e)
        {
            if (CurrCustomer == null || CurrSale == null)
                return;
            new XtraFormDelivry(CurrSale, CurrCustomer.CustomerName).ShowDialog();
            RefrechGrid(CurrCustomer, CurrSale);
        }
        private void btnUpdateTafseel_Click(object sender, EventArgs e)
        {
            if (CurrCustomer == null || CurrSale == null)
                return;
            new XtraFormDefultSize2(CurrSale,false).ShowDialog();
            RefrechGrid(CurrCustomer, CurrSale);
        }
        private void btnGomashType_Click(object sender, EventArgs e)
        {
            if (CurrCustomer == null || CurrSale == null)
                return;
            new XtraFormGomashType(CurrSale, CurrCustomer.CustomerName).ShowDialog();
            RefrechGrid(CurrCustomer, CurrSale);
        }
    
        public void Print(DevExpress.XtraReports.UI.XtraReport Report)
        {
            Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
            frmReport.documentViewer1.DocumentSource = Report;
            frmReport.ShowDialog();
        }
        tblPayment CurrPay => tblPaymentBindingSource.Current as tblPayment;
        private void btnUpdateDilvery_Click(object sender, EventArgs e)
        {
            if (CurrCustomer == null || CurrSale == null || CurrPay == null)
                return;
            new XtraFormUpdateDelivry(CurrSale, CurrCustomer.CustomerName, CurrPay).ShowDialog();
            RefrechGrid(CurrCustomer, CurrSale);
        }
        private void btnNoDelivry_Click(object sender, EventArgs e)
        {
            if (CurrCustomer == null)
                return;
            var invo = Session.tblSellInvoice.Where(c => c.CusNumber == CurrCustomer.CusNumber & c.BranchID == CurrCustomer.BranchID & c.QuanRemin > 0).ToList();
            tblSellInvoiceBindingSource.DataSource = invo;
            tblPaymentBindingSource.DataSource = Session.tblPayment.Where(c => invo.Any(x => x.InvoNumber == c.InvoNumber) & c.BranchID == CurrCustomer.BranchID).ToList();
        }
        private void btnOkDelivry_Click(object sender, EventArgs e)
        {
            if (CurrCustomer == null)
                return;
            var invo = Session.tblSellInvoice.Where(c => c.CusNumber == CurrCustomer.CusNumber & c.BranchID == CurrCustomer.BranchID & c.QuanRemin <= 0).ToList();
            tblSellInvoiceBindingSource.DataSource = invo;
            tblPaymentBindingSource.DataSource = Session.tblPayment.Where(c => invo.Any(x=>x.InvoNumber== c.InvoNumber) & c.BranchID == CurrCustomer.BranchID).ToList();
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            isNew = false;
            RefrechData();
            EnableOrDisyble(true);
        }
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
            btnSave.Enabled =btnNoSave.Enabled = !state;
        }
    
        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
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
                        catch (Exception ex) { SewingSystem.Classes.Logger.Log(ex); }
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
        public void ButtonPrint()
        {
            if (CurrSale == null)
                return;
            if (Session.tblSellInvoice.Where(i => i.ID == CurrSale.ID & i.BranchID == CurrSale.BranchID).Any())
            {
                var invo = MyFunaction.GetCopyFromInvoice(CurrSale);
                Task.Run(() => MyFunaction.FunactionPrintInvoice(invo));
            }
            else
            {
                if (Properties.Settings.Default.Language == "ar-SA")
                    XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "عفوا قم بحفظ الفاتورة اولا !!!!!", "حفظ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                else if (Properties.Settings.Default.Language == "en-US")
                    XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "Sorry, Save the bill first!!!!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
        private void PrintInvoice_Click(object sender, EventArgs e)
        {
            Program.PrintMode = "Direct";
            ButtonPrint();
        }

        private void btnDeleteInvoice_Click(object sender, EventArgs e)
        {
            if (CurrSale == null)
                return;
            if ((CurrSale.HowPrint ?? 0) > 0)
            {
                MyFunaction.MessageBoxPrintingInvoice();
                return;
            }
            if (MyFunaction.MessageBoxDeleteTafseel() != DialogResult.Yes)
                return;
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                db.tblPayments.DeleteAllOnSubmit(db.tblPayments.Where(m => m.InvoNumber == CurrSale.InvoNumber));
                db.tblSellInvoices.DeleteAllOnSubmit(db.tblSellInvoices.Where(m => m.ID == CurrSale.ID & m.BranchID == CurrSale.BranchID));
                db.SubmitChanges();
                Session.tblSellInvoice.Remove(Session.tblSellInvoice.Single(x => x.ID == CurrSale.ID));
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            //if (tblCustomerBindingSource.Current != null)
            //{
               
            //    if (MyFunaction.MessageBoxDeleteCustomer()== DialogResult.Yes)
            //    {
            //        using (var db = new DataClasses1DataContext(Program.ConnectionString))
            //        {
            //            tblCustomer Customer = tblCustomerBindingSource.Current as tblCustomer;
            //            var iv = Session.tblSellInvoice.Where(c => c.CusNumber == Customer.CusNumber & c.BranchID == Customer.BranchID).ToList();
            //            if (iv.Sum(s => ((s.HowPrint as short?) ?? 0)) > 0)
            //            {
            //                MyFunaction.MessageBoxDeletCustomerInvoice();
            //                return;
            //            }
            //            tblCustomerBindingSource.EndEdit();
            //            db.tblSellInvoices.DeleteAllOnSubmit(db.tblSellInvoices.Where(m => m.CusNumber == Customer.CusNumber));
            //            db.tblPayments.DeleteAllOnSubmit(db.tblPayments.Where(m => m.CusNumber == Customer.CusNumber));
            //            db.tblCustomers.DeleteAllOnSubmit(db.tblCustomers.Where(m => m.ID == Customer.ID));
            //            db.SubmitChanges();
            //        }
            //    }
            //}
        }
        Report.XtraReportCustomer rptCustomer;
        private void Print_Click(object sender, EventArgs e)
        {
            rptCustomer = new Report.XtraReportCustomer();
            rptCustomer.DataSource = (from cus in Session.tblCustomer
                                      select new
                                      {
                                          cus.ID,
                                          user =MyFunaction.getUserName(cus.UserID?? 0),
                                          BranchName =MyFunaction.getBranchName(cus.BranchID?? 0),
                                          cus.CustomerName,
                                          cus.Telephone,
                                          cus.CusNumber,
                                          cus.Mobil,
                                          cus.EnterTime,
                                          cus.Mobil2,
                                      }).ToList();
            Print(rptCustomer);
        }
        private void XtraFormCustomers_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            RefrechData();
        }

        public void RefrechGrid(tblCustomer Carr,tblSellInvoice sale)
        {
            tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(c => c.CusNumber == Carr.CusNumber & c.BranchID == Carr.BranchID).ToList();
            tblPaymentBindingSource.DataSource       = Session.tblPayment.Where(c => c.InvoNumber == sale.InvoNumber & c.BranchID == Carr.BranchID).ToList();
        }
        public void RefrechData()
        {
            tblCustomerBindingSource.DataSource = Session.tblCustomer.ToList();
            tblBranchBindingSource.DataSource = Session.tblBranche.ToList();
            tblUserBindingSource.DataSource = Session.tblUser.ToList();
            searchLookUpEditInvo.Properties.DataSource = Session.tblSellInvoice.ToList();
            if (CurrCustomer == null)
                return;
            tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(c => c.CusNumber == CurrCustomer.CusNumber & c.BranchID == CurrCustomer.BranchID).ToList();
            tblPaymentBindingSource.DataSource = Session.tblPayment.Where(c => c.CusNumber == CurrCustomer.CusNumber & c.BranchID == CurrCustomer.BranchID).ToList();
        }
        private void TblCustomerBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (CurrCustomer == null) return;
            tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(c => c.CusNumber == CurrCustomer.CusNumber & c.BranchID == CurrCustomer.BranchID).ToList();
            tblPaymentBindingSource.DataSource = Session.tblPayment.Where(c => c.CusNumber == CurrCustomer.CusNumber & c.BranchID == CurrCustomer.BranchID).ToList();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            isNew = false;
            EnableOrDisyble(false);
        }
       
        private void gridView1_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            btnUpdateTafseel.Enabled = true;
            btnUpdateDilvery.Enabled = false;
            var inv= gridView1.GetRow(e.RowHandle) as tblSellInvoice;
            tblPaymentBindingSource.DataSource = Session.tblPayment.Where(p=>p.InvoNumber== inv?.InvoNumber & p.BranchID==inv?.BranchID).ToList();
        }
        private void gridView2_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            btnUpdateTafseel.Enabled = false;
            btnUpdateDilvery.Enabled = true;
            var pay= gridView2.GetRow(e.RowHandle) as tblPayment;
            tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(p => p.InvoNumber ==pay?.InvoNumber & p.BranchID == pay?.BranchID);
        }
        private void searchLookUpEditCustomer_EditValueChanged(object sender, EventArgs e)
        {
            if (searchLookUpEditCustomer.EditValue == null || tblCustomerBindingSource.Count <= 0)
                return;
            tblCustomer Carr = searchLookUpEditCustomer.GetSelectedDataRow() as tblCustomer;
            if (Carr == null)
                return;
            tblCustomerBindingSource.Position = tblCustomerBindingSource.IndexOf(searchLookUpEditCustomer.GetSelectedDataRow());
            searchLookUpEditCustomer.EditValue = null;
        }
      

        private void searchLookUpEditInvo_EditValueChanged(object sender, EventArgs e)
        {
            if (searchLookUpEditInvo.EditValue == null)
                return;
            tblSellInvoice sale = searchLookUpEditInvo.GetSelectedDataRow() as tblSellInvoice;
            if (sale == null)
                return;
            var cus = Session.tblCustomer.Where(c => c.CusNumber == sale.CusNumber & c.BranchID == sale.BranchID).ToList();
            if (cus.Count() > 0)
            {
                tblCustomerBindingSource.Position = tblCustomerBindingSource.IndexOf(cus[0]);
                tblSellInvoiceBindingSource.DataSource = sale;
                tblPaymentBindingSource.DataSource = Session.tblPayment.Where(p => p.InvoNumber == sale.InvoNumber & p.BranchID == sale.BranchID).ToList();
            }
            searchLookUpEditInvo.EditValue = null;
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
        WhatsappMessage whatsappMessage = new WhatsappMessage();
        private void btnSearchByMobil_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SMS_Message = txtMessageSMS.Text;
            Properties.Settings.Default.Save();
            if (CustEmail.Text == string.Empty)
            {
                XtraMessageBox.Show("قم باضافة رقم الجوال لهذا العميل", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            whatsappMessage.SendSMS(CustEmail.Text,txtMessageSMS.Text);
        }
     
        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Program.PrintMode = "ShowDialog";
            ButtonPrint();
        }

        private void btnExcelExport_Click(object sender, EventArgs e)
        {
            xtraSaveFileDialog1.Filter = "Excel Files|*.Xlsx";
            if (xtraSaveFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }


            var workbook = new XLWorkbook();

            ///////////////////العملاء/////////////////////////////

            var worksheet_Customers = workbook.Worksheets.Add("العملاء");

            // إضافة رؤوس الأعمدة
            worksheet_Customers.Cell(1, 1).Value = "ID العميل";
            worksheet_Customers.Cell(1, 2).Value = "رقم العميل";
            worksheet_Customers.Cell(1, 3).Value = "اسم العميل";
            worksheet_Customers.Cell(1, 4).Value = "الهاتف";
            worksheet_Customers.Cell(1, 5).Value = "موبايل";
            worksheet_Customers.Cell(1, 6).Value = "موبايل2";
            worksheet_Customers.Cell(1, 7).Value = "ملاحظات";
            worksheet_Customers.Cell(1, 8).Value = "وقت الادخال";
            worksheet_Customers.Cell(1, 9).Value = "ID الفرع";

            // إضافة البيانات
            for (int i = 0; i < Session.tblCustomer.Count; i++)
            {
                worksheet_Customers.Cell(i + 2, 1).Value = Session.tblCustomer[i].ID;
                worksheet_Customers.Cell(i + 2, 2).Value = Session.tblCustomer[i].CusNumber;
                worksheet_Customers.Cell(i + 2, 3).Value = Session.tblCustomer[i].CustomerName;
                worksheet_Customers.Cell(i + 2, 4).Value = Session.tblCustomer[i].Telephone;
                worksheet_Customers.Cell(i + 2, 5).Value = Session.tblCustomer[i].Mobil;
                worksheet_Customers.Cell(i + 2, 6).Value = Session.tblCustomer[i].Mobil2;
                worksheet_Customers.Cell(i + 2, 7).Value = Session.tblCustomer[i].Notes;
                worksheet_Customers.Cell(i + 2, 8).Value = Session.tblCustomer[i].EnterTime;
                worksheet_Customers.Cell(i + 2, 9).Value = Session.tblCustomer[i].BranchID;
            }

            ////////////////////////////////////////////////////////////




            ///////////////////تفصيلات العملاء/////////////////////////////

            var worksheet_tafseel = workbook.Worksheets.Add("تفصيلات العملاء");

            // إضافة رؤوس الأعمدة
            worksheet_tafseel.Cell(1, 1).Value = "ID رقم التفصيل";
            worksheet_tafseel.Cell(1, 2).Value = "رقم العميل";
            worksheet_tafseel.Cell(1, 3).Value = "BranchID";
            worksheet_tafseel.Cell(1, 4).Value = "Cash";
            worksheet_tafseel.Cell(1, 5).Value = "Chik";
            worksheet_tafseel.Cell(1, 6).Value = "ClassNumber";
            worksheet_tafseel.Cell(1, 7).Value = "DeliveryDate";
            worksheet_tafseel.Cell(1, 8).Value = "Discount";
            worksheet_tafseel.Cell(1, 9).Value = "EnterTime";
            worksheet_tafseel.Cell(1, 10).Value = "ExitComash";
            worksheet_tafseel.Cell(1, 11).Value = "FactoreID";
            worksheet_tafseel.Cell(1, 12).Value = "HowPrint";
            worksheet_tafseel.Cell(1, 13).Value = "InvoNumber";
            worksheet_tafseel.Cell(1, 14).Value = "IsDelivery";
            worksheet_tafseel.Cell(1, 15).Value = "IsExitComash";
            worksheet_tafseel.Cell(1, 16).Value = "Meater";
            worksheet_tafseel.Cell(1, 17).Value = "MonyOrpon";
            worksheet_tafseel.Cell(1, 18).Value = "MonyPay";
            worksheet_tafseel.Cell(1, 19).Value = "MonyRemin";
            worksheet_tafseel.Cell(1, 20).Value = "Network";
            worksheet_tafseel.Cell(1, 21).Value = "Notes";
            worksheet_tafseel.Cell(1, 22).Value = "QuanRemin";
            worksheet_tafseel.Cell(1, 23).Value = "QuantityM";
            worksheet_tafseel.Cell(1, 24).Value = "SellDate";
            worksheet_tafseel.Cell(1, 25).Value = "SizeID";
            worksheet_tafseel.Cell(1, 26).Value = "TailorID";
            worksheet_tafseel.Cell(1, 27).Value = "TaxAll";
            worksheet_tafseel.Cell(1, 28).Value = "TaxDayn";
            worksheet_tafseel.Cell(1, 29).Value = "TaxMaden";
            worksheet_tafseel.Cell(1, 30).Value = "TheName";
            worksheet_tafseel.Cell(1, 31).Value = "ThePay";
            worksheet_tafseel.Cell(1, 32).Value = "TheQuantity";
            worksheet_tafseel.Cell(1, 33).Value = "TotalAfterDiscount";
            worksheet_tafseel.Cell(1, 34).Value = "TotalAll";
            worksheet_tafseel.Cell(1, 35).Value = "TotalDayn";
            worksheet_tafseel.Cell(1, 36).Value = "TotalFattInvoice";
            worksheet_tafseel.Cell(1, 37).Value = "TotalFinal";
            worksheet_tafseel.Cell(1, 38).Value = "TotalMaden";
            worksheet_tafseel.Cell(1, 39).Value = "TotalMony";
            worksheet_tafseel.Cell(1, 40).Value = "UserID";
            worksheet_tafseel.Cell(1, 41).Value = "Visa";

            // إضافة البيانات
            for (int i = 0; i < Session.tblSellInvoice.Count; i++)
            {
                worksheet_tafseel.Cell(i + 2, 1).Value = Session.tblSellInvoice[i].ID;
                worksheet_tafseel.Cell(i + 2, 2).Value = Session.tblSellInvoice[i].CusNumber;
                worksheet_tafseel.Cell(i + 2, 3).Value = Session.tblSellInvoice[i].BranchID;
                worksheet_tafseel.Cell(i + 2, 4).Value = Session.tblSellInvoice[i].Cash;
                worksheet_tafseel.Cell(i + 2, 5).Value = Session.tblSellInvoice[i].Chik;
                worksheet_tafseel.Cell(i + 2, 6).Value = Session.tblSellInvoice[i].ClassNumber;
                worksheet_tafseel.Cell(i + 2, 7).Value = Session.tblSellInvoice[i].DeliveryDate;
                worksheet_tafseel.Cell(i + 2, 8).Value = Session.tblSellInvoice[i].Discount;
                worksheet_tafseel.Cell(i + 2, 9).Value = Session.tblSellInvoice[i].EnterTime;
                worksheet_tafseel.Cell(i + 2, 10).Value = Session.tblSellInvoice[i].ExitComash;
                worksheet_tafseel.Cell(i + 2, 11).Value = Session.tblSellInvoice[i].FactoreID;
                worksheet_tafseel.Cell(i + 2, 12).Value = Session.tblSellInvoice[i].HowPrint;
                worksheet_tafseel.Cell(i + 2, 13).Value = Session.tblSellInvoice[i].InvoNumber;
                worksheet_tafseel.Cell(i + 2, 14).Value = Session.tblSellInvoice[i].IsDelivery.ToString();
                worksheet_tafseel.Cell(i + 2, 15).Value = Session.tblSellInvoice[i].IsExitComash.ToString();
                worksheet_tafseel.Cell(i + 2, 16).Value = Session.tblSellInvoice[i].Meater;
                worksheet_tafseel.Cell(i + 2, 17).Value = Session.tblSellInvoice[i].MonyOrpon;
                worksheet_tafseel.Cell(i + 2, 18).Value = Session.tblSellInvoice[i].MonyPay;
                worksheet_tafseel.Cell(i + 2, 19).Value = Session.tblSellInvoice[i].MonyRemin;
                worksheet_tafseel.Cell(i + 2, 20).Value = Session.tblSellInvoice[i].Network;
                worksheet_tafseel.Cell(i + 2, 21).Value = Session.tblSellInvoice[i].Notes;
                worksheet_tafseel.Cell(i + 2, 22).Value = Session.tblSellInvoice[i].QuanRemin;
                worksheet_tafseel.Cell(i + 2, 23).Value = Session.tblSellInvoice[i].QuantityM;
                worksheet_tafseel.Cell(i + 2, 24).Value = Session.tblSellInvoice[i].SellDate;
                worksheet_tafseel.Cell(i + 2, 25).Value = Session.tblSellInvoice[i].SizeID;
                worksheet_tafseel.Cell(i + 2, 26).Value = Session.tblSellInvoice[i].TailorID;
                worksheet_tafseel.Cell(i + 2, 27).Value = Session.tblSellInvoice[i].TaxAll;
                worksheet_tafseel.Cell(i + 2, 28).Value = Session.tblSellInvoice[i].TaxDayn;
                worksheet_tafseel.Cell(i + 2, 29).Value = Session.tblSellInvoice[i].TaxMaden;
                worksheet_tafseel.Cell(i + 2, 30).Value = Session.tblSellInvoice[i].TheName;
                worksheet_tafseel.Cell(i + 2, 31).Value = Session.tblSellInvoice[i].ThePay;
                worksheet_tafseel.Cell(i + 2, 32).Value = Session.tblSellInvoice[i].TheQuantity;
                worksheet_tafseel.Cell(i + 2, 33).Value = Session.tblSellInvoice[i].TotalAfterDiscount;
                worksheet_tafseel.Cell(i + 2, 34).Value = Session.tblSellInvoice[i].TotalAll;
                worksheet_tafseel.Cell(i + 2, 35).Value = Session.tblSellInvoice[i].TotalDayn;
                worksheet_tafseel.Cell(i + 2, 36).Value = Session.tblSellInvoice[i].TotalFattInvoice;
                worksheet_tafseel.Cell(i + 2, 37).Value = Session.tblSellInvoice[i].TotalFinal;
                worksheet_tafseel.Cell(i + 2, 38).Value = Session.tblSellInvoice[i].TotalMaden;
                worksheet_tafseel.Cell(i + 2, 39).Value = Session.tblSellInvoice[i].TotalMony;
                worksheet_tafseel.Cell(i + 2, 40).Value = Session.tblSellInvoice[i].UserID;
                worksheet_tafseel.Cell(i + 2, 41).Value = Session.tblSellInvoice[i].Visa;
            }

            ////////////////////////////////////////////////////////////


            ///////////////////الدفعات/////////////////////////////

            var worksheet_pay = workbook.Worksheets.Add("الدفعات");

            // إضافة رؤوس الأعمدة
            worksheet_pay.Cell(1, 1).Value = "ID رقم الدفعة";
            worksheet_pay.Cell(1, 2).Value = "رقم العميل";
            worksheet_pay.Cell(1, 3).Value = "BranchID";
            worksheet_pay.Cell(1, 4).Value = "Cash";
            worksheet_pay.Cell(1, 5).Value = "Chik";
            worksheet_pay.Cell(1, 6).Value = "Discount";
            worksheet_pay.Cell(1, 7).Value = "EnterTime";
            worksheet_pay.Cell(1, 8).Value = "InvoNumber";
            worksheet_pay.Cell(1, 9).Value = "Meater";
            worksheet_pay.Cell(1, 10).Value = "MonyPay";
            worksheet_pay.Cell(1, 11).Value = "MonyRemin";
            worksheet_pay.Cell(1, 12).Value = "Network";
            worksheet_pay.Cell(1, 13).Value = "Notes";
            worksheet_pay.Cell(1, 14).Value = "PayDate";
            worksheet_pay.Cell(1, 15).Value = "QuanDelivery";
            worksheet_pay.Cell(1, 16).Value = "QuanRemin";
            worksheet_pay.Cell(1, 17).Value = "TheAmount";
            worksheet_pay.Cell(1, 18).Value = "UserID";
            worksheet_pay.Cell(1, 19).Value = "Visa";

            // إضافة البيانات
            for (int i = 0; i < Session.tblPayment.Count; i++)
            {
                worksheet_pay.Cell(i + 2, 1).Value = Session.tblPayment[i].ID;
                worksheet_pay.Cell(i + 2, 2).Value = Session.tblPayment[i].CusNumber;
                worksheet_pay.Cell(i + 2, 3).Value = Session.tblPayment[i].BranchID;
                worksheet_pay.Cell(i + 2, 4).Value = Session.tblPayment[i].Cash;
                worksheet_pay.Cell(i + 2, 5).Value = Session.tblPayment[i].Chik;
                worksheet_pay.Cell(i + 2, 6).Value = Session.tblPayment[i].Discount;
                worksheet_pay.Cell(i + 2, 7).Value = Session.tblPayment[i].EnterTime;
                worksheet_pay.Cell(i + 2, 8).Value = Session.tblPayment[i].InvoNumber;
                worksheet_pay.Cell(i + 2, 9).Value = Session.tblPayment[i].Meater;
                worksheet_pay.Cell(i + 2, 10).Value = Session.tblPayment[i].MonyPay;
                worksheet_pay.Cell(i + 2, 11).Value = Session.tblPayment[i].MonyRemin;
                worksheet_pay.Cell(i + 2, 12).Value = Session.tblPayment[i].Network;
                worksheet_pay.Cell(i + 2, 13).Value = Session.tblPayment[i].Notes;
                worksheet_pay.Cell(i + 2, 14).Value = Session.tblPayment[i].PayDate;
                worksheet_pay.Cell(i + 2, 15).Value = Session.tblPayment[i].QuanDelivery;
                worksheet_pay.Cell(i + 2, 16).Value = Session.tblPayment[i].QuanRemin;
                worksheet_pay.Cell(i + 2, 17).Value = Session.tblPayment[i].TheAmount;
                worksheet_pay.Cell(i + 2, 18).Value = Session.tblPayment[i].UserID;
                worksheet_pay.Cell(i + 2, 19).Value = Session.tblPayment[i].Visa;
            }

            ////////////////////////////////////////////////////////////

            workbook.SaveAs(xtraSaveFileDialog1.FileName);
            MessageBox.Show("تمت العملية بنجاح");
        }
    }
}