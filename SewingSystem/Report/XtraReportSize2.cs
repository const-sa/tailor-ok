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
    public partial class XtraReportSize2 : DevExpress.XtraReports.UI.XtraReport
    {
        string Tax_Number = "";
        public XtraReportSize2()
        {
            InitializeComponent();
            
        }
        static string BillDate;
        private void XtraReportPayAajel_BeforePrint(object sender, CancelEventArgs e)
        {
            xrLabel1.Text = Program.User.UserName;
         
            if (Properties.Settings.Default.Language == "ar-SA")
            {
                this.ApplyLocalization("ar");
                this.RightToLeft = RightToLeft.No;
                this.RightToLeftLayout = RightToLeftLayout.No;
                xrTableRow20.Visible = xrTableRow21.Visible = false;
                xrTableRow2.Visible = xrTableRow3.Visible = xrTableRow17.Visible = true;
            }
            else if (Properties.Settings.Default.Language == "en-US")
            {
                this.ApplyLocalization("en");
                this.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = RightToLeftLayout.Yes;
                xrTableRow20.Visible = xrTableRow21.Visible = true;
                xrTableRow2.Visible = xrTableRow3.Visible = xrTableRow17.Visible = false;
            }
            lbl_SellDate.Text = BillDate;
            if (Program.Branch != null)
                Tax_Number = Program.Branch.TaxNumber;
         
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
