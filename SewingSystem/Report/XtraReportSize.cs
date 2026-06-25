using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using SewingSystem.Classes;
using System.Linq;
using SewingSystem.LinqModel;

namespace SewingSystem.Report
{
    public partial class XtraReportSize : DevExpress.XtraReports.UI.XtraReport
    {
        string Tax_Number = "";
        public XtraReportSize()
        {
            InitializeComponent();
            
        }
        void BindData()
        {
            //lbl_ID.DataBindings.Add("Text", this.DataSource, "InvoNumber");
            ////XRBarCode1.DataBindings.Add("Text", this.DataSource, "QRCode");
            //lbl_TotalFinal.DataBindings.Add("Text", this.DataSource, "TheSafy");
            //lbl_Total.DataBindings.Add("Text", this.DataSource, "InvoValue");
            //lbl_CustomerPhone.DataBindings.Add("Text", this.DataSource, "Phone");
            ////lbl_user.DataBindings.Add("Text", this.DataSource, "user");
            //lbl_TheName.DataBindings.Add("Text", this.DataSource, "TheName");
            ////xrPictureBox1.DataBindings.Add("Image", this.DataSource, "LogoImage");
            ////lbl_Discount.DataBindings.Add("Text", this.DataSource, "Discount");
            ////lbl_Total.DataBindings.Add("Text", this.DataSource, "TotalAftDis");
            //lbl_Tax.DataBindings.Add("Text", this.DataSource, "TheTax");
            //lbl_TotalFinal.DataBindings.Add("Text", this.DataSource, "TheSafy");
            //lbl_Paid.DataBindings.Add("Text", this.DataSource, "MonyPay");
            //lbl_Remining.DataBindings.Add("Text", this.DataSource, "MonyRemin");
            //lbl_TheQuantity.DataBindings.Add("Text", this.DataSource, "TheQuantity");
        }
      
        static string BillDate;
        static string TypeInvo;
        public static void Print(object ds, long id, string ThePay,string sellDate,string Type)
        {
            //XtraReportSize rpt1 = new XtraReportSize();
            //BillDate = sellDate;
            //Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
            //rpt1.ApplyLocalization(Properties.Settings.Default.Language);
           
            //rpt1.PrintingSystem.ContinuousPageNumbering = true;
            //rpt1.DataSource = ds;
            //rpt1.BindData();
            //rpt1.CreateDocument();
            //if ((Program.Branch.PrintTowBill as bool?)??false)
            //{
            //    XtraReportSize rpt2 = new XtraReportSize();
            //    rpt2.PrintingSystem.ContinuousPageNumbering = true;
            //    rpt2.DataSource = ds;
            //    rpt2.BindData();
            //    rpt2.CreateDocument();
            //    rpt1.Pages.AddRange(rpt2.Pages);
            //}
           
            //switch (Program.PrintMode)
            //{
            //    case "Direct":
            //        rpt1.Print();
            //        break;
            //    case "ShowDialog":
                    
            //        frmReport.documentViewer1.DocumentSource
            //         = rpt1;
            //        frmReport.ShowDialog();
            //        break;
            //    default:
                  
            //        break;
            //}
        }
        private void XtraReportPayAajel_BeforePrint(object sender, CancelEventArgs e)
        {
            //lbl_user.Text = Program.User.UserName;
            xrLabel1.Text = Program.User.UserName;
            //if (Program.Branch != null)
            //{
            //    lbl_CompanyName.Text = Program.Branch.CompanyName;
            //    lbl_CompanyAddress.Text = Program.Branch.BranchAddress;
            //    lbl_CompanyEmail.Text = Program.Branch.Email;
            //    lbl_CompanyPhone.Text = Program.Branch.BranchTelephone;
            //    lbl_CompanyMobil.Text = Program.Branch.Mobile;
            //    lbl_BranchName.Text = Program.Branch.BranchName;
            //}
            if (Properties.Settings.Default.Language == "ar-SA")
            {
                this.RightToLeft = RightToLeft.No;
                this.RightToLeftLayout = RightToLeftLayout.No;
            }
            else if (Properties.Settings.Default.Language == "en-US")
            {
                this.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = RightToLeftLayout.Yes;
            }
            //lbl_SellDate.Text = BillDate;
            //TaxNumber.Visible = (Program.Branch.UseTax as bool?) ?? false;
            if (Program.Branch != null)
                Tax_Number = Program.Branch.TaxNumber;
            if (Properties.Settings.Default.Language == "ar-SA")
            {
                this.RightToLeft = RightToLeft.No;
                this.RightToLeftLayout = RightToLeftLayout.No;
                //if (Program.Branch.UseTax?? false)
                //{
                //    TaxNumber.Text = "    الرقم الضريبي: " + Tax_Number;
                //    xrTableCell31.Font.Size.Equals(6);
                //}
                //else
                //{
                //    xrTableCell31.Text = "السعر";
                //    xrTableCell31.Font.Size.Equals(8);
                //}
            }
            else if (Properties.Settings.Default.Language == "en-US")
            {
                this.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = RightToLeftLayout.Yes;
                //if (Program.Branch.UseTax?? false)
                //{
                //    TaxNumber.Text = "Tax Number:       " + Tax_Number;
                //    //xrTableCell31.Text = "Price includes tax";
                //    xrTableCell31.Font.Size.Equals(6);
                //}
                //else
                //{
                //    xrTableCell31.Text = "Price";
                //    xrTableCell31.Font.Size.Equals(8);
                //}
            }
            if (Program.Branch.UseTax?? false)
            {
                lbl_Total.Visible = true;
                lbl_Total.Row.Visible = true;
                xrTableCell24.Visible = true;
                lbl_Tax.Visible = true;
                lbl_Tax.Row.Visible = true;
                xrTableCell26.Visible = true;
            }
            else
            {
                lbl_Total.Row.Visible = false;
                  lbl_Total.Visible = false;
                xrTableCell24.Visible = false;
                lbl_Tax.Row.Visible = false;
                lbl_Tax.Visible = false;
                xrTableCell26.Visible = false;
            }
        }

        private void xrTableCell49_BeforePrint(object sender, CancelEventArgs e)
        {

        }

        private void tall_BeforePrint(object sender, CancelEventArgs e)
        {

        }
    }
}
