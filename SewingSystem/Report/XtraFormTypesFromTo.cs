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
using System.Data.SqlClient;

namespace SewingSystem.Report
{
    public partial class XtraFormTypesFromTo : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        List<tblSellInvoiceDetaile> data = new List<tblSellInvoiceDetaile>();
        Report.XtraReportTypes XtraReportTypes;
        public XtraFormTypesFromTo()
        {
            InitializeComponent();
        }
        private void Print_Click(object sender, EventArgs e)
        {
            RReportTypesFun();
            Print(XtraReportTypes);
        }
        Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
        public void Print(DevExpress.XtraReports.UI.XtraReport Report)
        {
            frmReport.documentViewer1.DocumentSource = Report;
            frmReport.ShowDialog();
        }
        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }
        public void RReportTypesFun()
        {
            XtraReportTypes = new XtraReportTypes();
            XtraReportTypes.ApplyLocalization(Properties.Settings.Default.Language);
            XtraReportTypes.lbl_CompanyAddress.Text =  Program.Branch.BranchAddress;
            XtraReportTypes.lbl_BranchName.Text =      Program.Branch.BranchName;
            XtraReportTypes.lbl_CompanyName.Text =     Program.Branch.CompanyName;
            XtraReportTypes.lbl_CompanyEmail.Text =    Program.Branch.Fax;
            XtraReportTypes.lbl_CompanyMobil.Text =    Program.Branch.Mobile;
            XtraReportTypes.lbl_CompanyPhone.Text = Program.Branch.BranchTelephone;
            XtraReportTypes.lbl_user.Text = Program.User.UserName;
            if (Properties.Settings.Default.Language== "ar-SA")
                XtraReportTypes.xrLabelTaital.Text += dtime_Start.DateTime.ToShortDateString() + "  الى  " + dtime_End.DateTime.ToShortDateString();
           else if (Properties.Settings.Default.Language == "en-US")
                XtraReportTypes.xrLabelTaital.Text += dtime_Start.DateTime.ToShortDateString() + "  To  " + dtime_End.DateTime.ToShortDateString();
            XtraReportTypes.DataSource = reportTypesResultBindingSource.DataSource;
            
        }
        private void ExportPDF_Click(object sender, EventArgs e)
        {
            RReportTypesFun();
            xtraSaveFileDialog1.Filter = "PDF Files|*.pdf";
            if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
                XtraReportTypes.ExportToPdf(xtraSaveFileDialog1.FileName);
        }

        private void Refreash_Click(object sender, EventArgs e)
        {
            BranchIDLookupedite.ResetText();
            Refresh_Types();
        }

        private void Dtime_Start_EditValueChanged(object sender, EventArgs e)
        {
            Refresh_Types();
        }

        private void ExportExcel_Click(object sender, EventArgs e)
        {
            RReportTypesFun();
            xtraSaveFileDialog1.Filter = "Excel Files|*.Xls";
            if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
                XtraReportTypes.ExportToXls(xtraSaveFileDialog1.FileName);
        }
        public void Refresh_Types()
        {
            if (dtime_End.Text == "")
                dtime_End.DateTime = DateTime.Now;
            if (dtime_Start.Text == "")
                dtime_Start.DateTime = DateTime.Now;
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                //var report = db.Report_TypesLast(dtime_Start.DateTime.Date, checkEdit11.Checked, dtime_End.DateTime.Date).ToList();
                //if (Program.User.UserState == "Admin")
                //{
                //    short Bid = (BranchIDLookupedite.EditValue as short?) ?? 0;
                //    if (Bid > 0)
                //        reportTypesResultBindingSource.DataSource = report.Where(b => b.BranchID == Bid);
                //    else
                //        reportTypesResultBindingSource.DataSource = report;
                //}
                //else
                //    reportTypesResultBindingSource.DataSource = report.Where(b => b.BranchID == Program.User.BranchID);
            }
        }
        private void XtraFormTypesFromTo_Load(object sender, EventArgs e)
        {
            if (Program.User.UserState == "Admin")
                ItemForUserID1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            tblBranchesBindingSource.DataSource = Session.tblBranche.ToList();
            BranchIDLookupedite.EditValueChanged += BranchIDLookupedite_EditValueChanged;
            Refresh_Types();
        }

        private void BranchIDLookupedite_EditValueChanged(object sender, EventArgs e)
        {
            Refresh_Types();
        }

        private void CheckEdit11_CheckedChanged(object sender, EventArgs e)
        {
            Refresh_Types();
        }

       
    }
}