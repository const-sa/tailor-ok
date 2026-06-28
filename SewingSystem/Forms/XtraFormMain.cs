using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Forms;
using System.Data.Entity;
using System.Text;
using DevExpress.XtraGrid;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraLayout;
using System.IO;
using SewingSystem.Classes;
using System.Threading.Tasks;
using System.Drawing;
using DevExpress.Utils;
using DevExpress.XtraBars;
using System.Timers;

namespace SewingSystem.Forms
{
    public partial class XtraFormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        MyFunaction MyFunaction = new MyFunaction();
        DAL.Class1 cls = new DAL.Class1();


        /////////////////// 4  auto backup  //////////////////////////
        static System.Windows.Forms.Timer backupTimer;
        static int backupHourInterval = Properties.Settings.Default.BackupFrequencyHours;
        static int backupDayInterval = Properties.Settings.Default.BackupFrequencyDays;
        static DateTime lastBackupTime = Properties.Settings.Default.LastBackupTime;
        static string backupPath = Properties.Settings.Default.BackupPath;


        private int CalculateNextBackupInterval()
        {
            int backupIntervalMs = 0;

            //نأخذ الذي ليس صفر
            if (backupHourInterval > 0)
            {
                backupIntervalMs = backupHourInterval * 3600000;
            }
            else
            {
                backupIntervalMs = backupDayInterval * 86400000;
            }

            TimeSpan nextBackupTimeSpan = lastBackupTime.AddMilliseconds(backupIntervalMs).Subtract(DateTime.Now);

            return Math.Min((int)nextBackupTimeSpan.TotalMilliseconds, backupIntervalMs);
        }

        private void OnTimedBackup(object sender, EventArgs e)
        {
            //تنفيذ النسخ الاحتياطي
            string ConString = Program.ConnectionString;
            string backupDirectory = Properties.Settings.Default.BackupPath.ToString();
            bool b = MyFunaction.BackupDatabase(ConString, backupDirectory);

            if (b)
            {
                // تحديث آخر وقت نسخ
                lastBackupTime = DateTime.Now;
                Properties.Settings.Default.LastBackupTime = lastBackupTime;
                Properties.Settings.Default.Save();

                // حساب الوقت القادم للنسخ
                backupTimer.Interval = CalculateNextBackupInterval();
            }
        }


        ///////////////////////////////////////////////////////

        static System.Windows.Forms.Timer tmr_trialVersion = new System.Windows.Forms.Timer();


        public XtraFormMain()
        {
            InitializeComponent();
            // WhatsApp ribbon button uses a generated WhatsApp-style icon (green bubble).
            btnWhatsapp.ImageOptions.Image = Classes.Whatsapp.WhatsappIcon.Get(16);
            btnWhatsapp.ImageOptions.LargeImage = Classes.Whatsapp.WhatsappIcon.Get(32);
            // ZATCA buttons get a tax / e-invoice (QR) icon instead of the generic database one.
            foreach (var b in new[] { btnZatcaSett, btnReturnInvoice, btnZatcaHelp, btnZatcaReport })
            {
                b.ImageOptions.Image = Classes.Zatca.ZatcaIcon.Get(16);
                b.ImageOptions.LargeImage = Classes.Zatca.ZatcaIcon.Get(32);
            }
            TrailRemainingDays();
        }



        ComponentFlyoutDialog flyDailog = new ComponentFlyoutDialog();
        protected override async void OnShown(EventArgs e)
        {
            SplashScreenManager.CloseForm();
            base.OnShown(e);
            flyDailog.WaitForm(this, 1);
            await Program.InitObjects();
            flyDailog.WaitForm(this, 0);
            barStaticItemBranch.Caption = Session.tblBranche.Where(b => b.ID == Program.User.BranchID).FirstOrDefault().BranchName + "      ";
            barStaticItemUser.Caption = Program.User.UserName + "      ";
            ShowConnectionStatus();
        }

        private BarStaticItem barConnStatus;
        /// <summary>
        /// يضيف مؤشر حالة الاتصال إلى شريط الحالة: دائرة خضراء = متصل، حمراء =
        /// غير متصل، مع نوع الاتصال (داخلي/خارجي).
        /// </summary>
        private void ShowConnectionStatus()
        {
            try
            {
                bool connected = Classes.ConnectionStatus.TestCurrent();
                string type = Classes.ConnectionStatus.TypeCaption(Program.ConnType);

                if (barConnStatus == null)
                {
                    barConnStatus = new BarStaticItem();
                    ribbonControl1.Items.Add(barConnStatus);
                    ribbonStatusBar1.ItemLinks.Add(barConnStatus);
                }
                barConnStatus.ImageOptions.Image = Classes.ConnectionStatus.Dot(connected, 14);
                barConnStatus.Caption = (connected ? "متصل" : "غير متصل") + " (" + type + ")      ";
            }
            catch (Exception ex) { Classes.Logger.Log(ex); }
        }
        private void TrailRemainingDays()
        {
            //return;
            try
            {
                ClsEncryption clsEncrp = new ClsEncryption();
                string flagStr = clsEncrp.DecryptString(Properties.Settings.Default.accConFlag);
                string flag = flagStr.Substring(flagStr.Length - 1, 1);
                barListItem2.Visibility = BarItemVisibility.Never;
                if (flag == "T") return;

                string conValStr = clsEncrp.DecryptString(Properties.Settings.Default.accConVal);
                int conVal = Convert.ToInt32(conValStr.Substring(0, conValStr.IndexOf("-")));
                string days = "";
                switch (conVal)
                {
                    case 1:
                        days = "يوم واحد";
                        break;
                    case 2:
                        days = "يومين";
                        break;
                    default:
                        if (conVal <= 10 && conVal > 2)
                            days = $"{conVal} ايام";
                        else
                            days = $"{conVal} يوم";
                        break;
                }
                barListItem2.Caption = $"نسخة تجريبية: متبقي {days} حتى إنتها النسخة";
                barListItem2.Visibility = BarItemVisibility.Always;
            }
            catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }
        }

        #region Classes 
        private void ClassesbarButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MyFunaction.OpenForm(new UC_Class(), "الاصناف");
        }
        #endregion


        Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
        public void Print(DevExpress.XtraReports.UI.XtraReport Report)
        {
            frmReport.documentViewer1.DocumentSource = Report;
            frmReport.ShowDialog();
        }
        XtraFormCustomers frmCustomers;
        private void CustomerbarButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmCustomers = new XtraFormCustomers();
            frmCustomers.Show();
        }



        #region InvoiceNew
        //XtraFormInvoice FormInvoice;
        private void InvoiceNewbarButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //try
            //{
            //    if (Application.OpenForms["XtraFormInvoice"] != null)
            //    {
            //        Application.OpenForms["XtraFormInvoice"].Activate();
            //    }
            //    else
            //    {
            //        FormInvoice = new XtraFormInvoice();
            //        FormInvoice.Show();
            //    }
            //}
            //catch (Exception ex)
            //{

            //    XtraMessageBox.Show(ex.Message);
            //}

        }
        #endregion


        XtraFormCompanyData CompanyData;
        private void DataMainForWasherbarButtonItem16_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            CompanyData = new XtraFormCompanyData();
            CompanyData.Show();
        }
        XtraFormUsers frmUsers;
        private void UserAdminbarButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmUsers = new XtraFormUsers();
            frmUsers.Show();
        }

        Report.XtraFormReportsByDate frm;
        private void ReportAllInvoicebarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraFormReportDay = new Report.XtraFormReportDay("الكل");
            XtraFormReportDay.Show();
        }

        Report.XtraFormReportDay XtraFormReportDay;
        private void ReportDaybarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraFormReportDay = new Report.XtraFormReportDay("اليوم");
            XtraFormReportDay.Show();
        }

        private void ButtonItem17_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraFormReportDay = new Report.XtraFormReportDay("الشهر");
            XtraFormReportDay.Show();
        }
        private void ButtonItem27_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm = new Report.XtraFormReportsByDate("All", Session.LangEng ? "Reports of all invoices" : "تقارير كل الفواتير ");
            frm.Show();
        }
        private void ButtonBillNotPaid_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm = new Report.XtraFormReportsByDate("NotPaid", Session.LangEng ? "Unpaid invoice reports" : "تقارير الفواتير غير المدفوعه");
            frm.Show();
        }
        private void btnInvoPayed_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm = new Report.XtraFormReportsByDate("Paid", Session.LangEng ? "Paid invoice reports" : "تقارير الفواتير المدفوعه");
            frm.Show();
        }
        private void btnReportOrpon_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm = new Report.XtraFormReportsByDate("NotOrpon", Session.LangEng ? "Billing reports - no deposit paid" : "تقارير الفواتير - لم يدفع عربون");
            frm.Show();
        }
        private void ButtonNotDlivery_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm = new Report.XtraFormReportsByDate("NotDlivery", Session.LangEng ? "Unreceived invoice reports" : "تقارير الفواتير غير المستلمه");
            frm.Show();
        }
        private void ButtonReportCustomerBills_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frm = new Report.XtraFormReportsByDate("Dlivery", Session.LangEng ? "Received invoice reports" : "تقارير الفواتير المستلمه");
            frm.Show();
        }
        #region Permission
        XtraFormPermissionUser frmPermissionUser;
        private void PermissionbarButton_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmPermissionUser = new XtraFormPermissionUser();
            frmPermissionUser.Show();
        }
        XtraFormBranch frmBranch;
        Report.XtraReportClasses XtraReportClasses;
        private void ButtonReportClasses_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            XtraReportClasses = new Report.XtraReportClasses();
            XtraReportClasses.DataSource = (from cus in Session.tblClasses
                                            select new
                                            {
                                                cus.ID,
                                                user = MyFunaction.getUserName((cus.UserID as short?) ?? 0),
                                                BranchName = MyFunaction.getBranchName((cus.BranchID as short?) ?? 0),
                                                cus.ClassName,
                                                cus.EnterTime,
                                                cus.Notes,
                                            }).ToList();
            Print(XtraReportClasses);
        }


        Report.XtraReportCustomer ReportCustomer;
        private void ButtonReportCustomer_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ReportCustomer = new Report.XtraReportCustomer();
            ReportCustomer.DataSource = (from cus in Session.tblCustomer
                                         select new
                                         {
                                             cus.ID,
                                             user = MyFunaction.getUserName((cus.UserID as short?) ?? 0),
                                             BranchName = MyFunaction.getBranchName((cus.BranchID as short?) ?? 0),
                                             LogoImage = Program.Branch.LogoImage,
                                             cus.CustomerName,
                                             cus.Mobil2,
                                             cus.CusNumber,
                                             cus.Mobil,
                                             cus.EnterTime,
                                             cus.Telephone,
                                         }).ToList();
            Print(ReportCustomer);
        }

        private void Branch_barBut_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            frmBranch = new XtraFormBranch();
            frmBranch.Show();
        }

        public string getCusName(int CusID)
        {
            if (CusID == 0)
                return "";
            else
            {
                var g1 = Session.tblCustomer.Where(g => g.ID == CusID).ToList();
                if (g1.Count() > 0)
                    return ((g1[0].CustomerName as string) ?? "");
                else
                    return "";
            }
        }

        public string getCusMobile(int CusID)
        {
            if (CusID == 0)
                return "";
            else
            {
                var g1 = Session.tblCustomer.Where(g => g.ID == CusID).ToList();
                if (g1.Count() > 0)
                    return ((g1[0].Mobil as string) ?? "");
                else
                    return "";
            }
        }

        public string getClassName(int ClassID)
        {
            if (ClassID == 0)
                return "";
            else
            {
                var g1 = Session.tblClasses.Where(g => g.ID == ClassID).ToList();
                if (g1.Count() > 0)
                    return ((g1[0].ClassName as string) ?? "");
                else
                    return "";
            }
        }

        #endregion

        XtraFormConnectionDB ConnectionDB;
        private void ButtonItem42_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ConnectionDB = new XtraFormConnectionDB();
            ConnectionDB.Show();
        }

        private void OnTimedTrailVersion(object sender, EventArgs e)
        {
            if (Program.is5DaysTrail)
            {
                int val = cls.SettingGetValue();
                cls.SettingUpdateValue(++val);

                ProcessRemainTime(val);
            }
        }

        void ProcessRemainTime(int val)
        {
            if (val >= Program.TargetValue)
            {
                //this.FormClosing -= FormMain_FormClosing;
                Application.Exit();
                return;
            }

            int remainingMinutes = Program.TargetValue - val;
            int hh = remainingMinutes / 60;
            int mm = remainingMinutes % 60;

            string formattedTime = $"{hh:D2} ساعة و {mm:D2} دقيقة";

            barListItem2.Caption = "الوقت المتبقي لانتهاء النسخة التجريبية: " + formattedTime;
            barListItem2.Visibility = BarItemVisibility.Always;
        }

        private void XtraFormMain_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SetFont();


            int val = cls.SettingGetValue();
            ProcessRemainTime(val);

            tmr_trialVersion.Tick += OnTimedTrailVersion;
            tmr_trialVersion.Interval = 1000 * 60;
            tmr_trialVersion.Enabled = true;


            // حساب الوقت القادم للنسخ الاحتياطي
            double backupInterval = CalculateNextBackupInterval();

            if (backupInterval <= 0) //وقت النسخ القادم مر بالفعل
            {
                //تنفيذ النسخ الاحتياطي
                string ConString = Program.ConnectionString;
                string backupDirectory = Properties.Settings.Default.BackupPath.ToString();
                bool b = MyFunaction.BackupDatabase(ConString, backupDirectory);

                if (b)
                {
                    // تحديث آخر وقت نسخ
                    lastBackupTime = DateTime.Now;
                    Properties.Settings.Default.LastBackupTime = lastBackupTime;
                    Properties.Settings.Default.Save();

                    // حساب الوقت القادم للنسخ
                    backupInterval = CalculateNextBackupInterval();
                }
            }
            if (backupInterval > 0) //قد تكون حدثت مشكلة عند اجراء النسخ الاحتياطي ولم يتغير الوقت القادم وبقي سالباً
            {

                backupTimer = new System.Windows.Forms.Timer();
                backupTimer.Interval = (int)backupInterval;

                backupTimer.Tick += OnTimedBackup;

                backupTimer.Enabled = true;
            }
        }

        XtraFormPayment FormPayment;
        private void BtnGabth_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormPayment = new XtraFormPayment();
            FormPayment.Show();
        }
        XtraFormSanadSarf FormSanadSarf;
        private void BtnSarf_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormSanadSarf = new XtraFormSanadSarf();
            FormSanadSarf.Show();
        }
        //XtraFormInvoTafseels2 FormInvoTafseels;
        private void BtnTafseeal_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //FormInvoTafseels = new XtraFormInvoTafseels2();
            //FormInvoTafseels.Show();
        }
        Report.XtraFormReportGapth FormReportGapth;
        private void BtnReportGapth_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormReportGapth = new Report.XtraFormReportGapth();
            FormReportGapth.Show();
        }
        Report.XtraFormExpenses FormExpenses;
        private void BtnReportBuy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormExpenses = new Report.XtraFormExpenses();
            FormExpenses.Show();
        }
        XtraFormCreditNote FormCreditNote;
        private void BtnCreditNote_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormCreditNote = new XtraFormCreditNote();
            FormCreditNote.Show();
        }
        //XtraFormInvoTafseels FormInvoTafseels1;
        private void BtnTafseel1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //FormInvoTafseels1 = new XtraFormInvoTafseels();
            //FormInvoTafseels1.Show();
        }
        XtraFormBuyInvoice FormBuyInvoice;
        private void BtnSupliy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormBuyInvoice = new XtraFormBuyInvoice();
            FormBuyInvoice.Show();
        }
        XtraFormFactories FormFactories;
        private void BtnFactore_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormFactories = new XtraFormFactories();
            FormFactories.Show();
        }
        XtraFormTailor FormTailor;
        private void BtnTailor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormTailor = new XtraFormTailor();
            FormTailor.Show();
        }
        XtraFormSMS FormSMS;
        private void BtnSms_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            FormSMS = new XtraFormSMS();
            FormSMS.Show();
        }

        private void BtnFont_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            using (FontDialog dialog = new FontDialog())
            {
                dialog.Font = WindowsFormsSettings.DefaultFont;
                if (dialog.ShowDialog() == DialogResult.OK)
                {

                    Properties.Settings.Default.SystemFont = dialog.Font;
                    Properties.Settings.Default.Save();
                    SetFont();
                }
            }
        }
        public void SetFont()
        {
            WindowsFormsSettings.DefaultFont =
                AppearanceObject.DefaultFont =
                AppearanceObject.DefaultMenuFont =
                WindowsFormsSettings.DefaultFont =
                AppearanceObject.DefaultFont = Properties.Settings.Default.SystemFont;
            ribbonControl1.Items.Where(x => x is BarItem bbi).ToList().ForEach(x => x.ItemAppearance.SetFont(Properties.Settings.Default.SystemFont));
        }

        XtraForm_Customers frm_Customers;
        private void btn_Customers_ItemClick(object sender, ItemClickEventArgs e)
        {
            frm_Customers = new XtraForm_Customers();
            frm_Customers.Show();
        }

        XtraFormBackupSett frmBackupSett;
        private void btnBackupSett_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmBackupSett = new XtraFormBackupSett();
            frmBackupSett.Show();
        }

        XtraFormZatcaSettings frmZatcaSett;
        private void btnZatcaSett_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmZatcaSett = new XtraFormZatcaSettings();
            frmZatcaSett.Show();
        }

        XtraFormReturnInvoice frmReturnInvoice;
        private void btnReturnInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmReturnInvoice = new XtraFormReturnInvoice();
            frmReturnInvoice.Show();
        }

        XtraFormZatcaHelp frmZatcaHelp;
        private void btnZatcaHelp_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmZatcaHelp = new XtraFormZatcaHelp();
            frmZatcaHelp.Show();
        }

        XtraFormZatcaReport frmZatcaReport;
        private void btnZatcaReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmZatcaReport = new XtraFormZatcaReport();
            frmZatcaReport.Show();
        }

        XtraFormWhatsapp frmWhatsapp;
        private void btnWhatsapp_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmWhatsapp = new XtraFormWhatsapp();
            frmWhatsapp.Show();
        }

        XtraFormExplanations frmExplanations;
        private void btn_explanations_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmExplanations = new XtraFormExplanations();
            frmExplanations.Show();
        }

        XtraFormSuppliers frmSuppliers;
        private void btnSuppliers_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmSuppliers = new XtraFormSuppliers();
            frmSuppliers.Show();
        }
    }
}