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
using SewingSystem.Classes;

namespace SewingSystem.Report
{
    public partial class XtraFormReportCustomerBillFromTo : DevExpress.XtraEditors.XtraForm
    {
        public XtraFormReportCustomerBillFromTo()
        {
            InitializeComponent();
        }
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        List<tblSellInvoiceDetaile> data = new List<tblSellInvoiceDetaile>();
        Report.XtraReportCustomerBills XtraReportCustomerBills;
        private void Print_Click(object sender, EventArgs e)
            {
                RReportTypesFun();
                Print(XtraReportCustomerBills);
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
                XtraReportCustomerBills = new XtraReportCustomerBills();
                XtraReportCustomerBills.ApplyLocalization(Properties.Settings.Default.Language);
                XtraReportCustomerBills.lbl_CompanyAddress.Text = Program.Branch.BranchAddress;
                XtraReportCustomerBills.lbl_BranchName.Text =     Program.Branch.BranchName;
                XtraReportCustomerBills.lbl_CompanyName.Text =    Program.Branch.CompanyName;
                XtraReportCustomerBills.lbl_CompanyEmail.Text =   Program.Branch.Fax;
                XtraReportCustomerBills.lbl_CompanyMobil.Text =   Program.Branch.Mobile;
                XtraReportCustomerBills.lbl_CompanyPhone.Text =   Program.Branch.BranchTelephone;
                XtraReportCustomerBills.lbl_user.Text = Program.User.UserName;
                if (Properties.Settings.Default.Language == "ar-SA")
                    XtraReportCustomerBills.xrLabelTaital.Text += dtime_Start.DateTime.ToShortDateString() + "  الى  " + dtime_End.DateTime.ToShortDateString();
                else if (Properties.Settings.Default.Language == "en-US")
                    XtraReportCustomerBills.xrLabelTaital.Text += dtime_Start.DateTime.ToShortDateString() + "  To  " + dtime_End.DateTime.ToShortDateString();
                XtraReportCustomerBills.DataSource = reporttblAccForCusAndSupBindingSource.DataSource;
            XtraReportCustomerBills.DataSource = (from cus in (List<tblAccForCusAndSup>)reporttblAccForCusAndSupBindingSource.DataSource
                                                  select new
                                    {
                                        cus.ID,
                                        user = getUserName((cus.UserID as short?) ?? 0),
                                        BranchName = getBranchName((cus.BranchID as short?) ?? 0),
                                        cus.AccName,
                                        cus.Credit,
                                        cus.Debit,
                                        cus.DocID,
                                        cus.EnterTime,
                                        cus.DocType,
                                        cus.DocDate,
                                        cus.IDCusOrSup,
                                        }).ToList();
        }
        public string getUserName(short UserID)
        {
            if (UserID == 0)
                return "";
            else
            {
                var g1 = Session.tblUser.Where(g => g.ID == UserID).ToList();
                if (g1.Count() > 0)
                    return ((g1[0].UserName as string) ?? "");
                else
                    return "";
            }
        }
        public string getBranchName(short BranchID)
        {
            if (BranchID == 0)
                return "";
            else
            {
                var g1 = Session.tblBranche.Where(g => g.ID == BranchID).ToList();
                if (g1.Count() > 0)
                    return ((g1[0].BranchName as string) ?? "");
                else
                    return "";
            }
        }
        private void ExportPDF_Click(object sender, EventArgs e)
            {
                RReportTypesFun();
                xtraSaveFileDialog1.Filter = "PDF Files|*.pdf";
                if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
                    XtraReportCustomerBills.ExportToPdf(xtraSaveFileDialog1.FileName);
            }

            private void Refreash_Click(object sender, EventArgs e)
            {
                BranchIDLookupedite.ResetText();
                Refresh_Customers();
            }

            private void Dtime_Start_EditValueChanged(object sender, EventArgs e)
            {
                Refresh_Customers();
            }

            private void ExportExcel_Click(object sender, EventArgs e)
            {
                RReportTypesFun();
                xtraSaveFileDialog1.Filter = "Excel Files|*.Xls";
                if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
                    XtraReportCustomerBills.ExportToXls(xtraSaveFileDialog1.FileName);
            }
            List<tblAccForCusAndSup> report;
            public void Refresh_Customers()
            {
                if (dtime_End.Text == "")
                    dtime_End.DateTime = DateTime.Now;
            if (dtime_Start.Text == "")
                dtime_Start.DateTime = DateTime.Now;
            int cusid1 = ((CustID1.EditValue as int?) ?? 0);
            int cusid2 = ((CustID2.EditValue as int?) ?? 0);
            report = new List<tblAccForCusAndSup>();
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                if (cusid1==0& cusid2 == 0)
                    report = db.tblAccForCusAndSups.Where(p => p.TypeCusOrSup == true).ToList();
               else if (cusid1 == 0 & cusid2 >0)
                    report = db.tblAccForCusAndSups.Where(p => p.TypeCusOrSup == true&p.IDCusOrSup== cusid2).ToList();
                else if (cusid2 == 0 & cusid1 > 0)
                    report = db.tblAccForCusAndSups.Where(p => p.TypeCusOrSup == true & p.IDCusOrSup == cusid1).ToList();
                else if (cusid1 > 0 & cusid2 > 0)
                    report = db.tblAccForCusAndSups.Where(p => p.TypeCusOrSup == true & p.IDCusOrSup >= cusid1 & p.IDCusOrSup <= cusid2).ToList();
                if (Program.User.UserState == "Admin")
                {
                    short Bid = (BranchIDLookupedite.EditValue as short?) ?? 0;
                    if (Bid > 0)
                        reporttblAccForCusAndSupBindingSource.DataSource = report.Where(b => b.BranchID == Bid);
                    else
                        reporttblAccForCusAndSupBindingSource.DataSource = report;
                }
                else
                    reporttblAccForCusAndSupBindingSource.DataSource = report.Where(b => b.BranchID == Program.User.BranchID);

            }
        }
            private void XtraFormCustomersFromTo_Load(object sender, EventArgs e)
            {
            tblBrancheBindingSource.DataSource = Session.tblBranche.ToList();
            tblCustomerBindingSource.DataSource = Session.tblCustomer.ToList();
            if (Program.User.UserState=="Admin")
                ItemForUserID1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            BranchIDLookupedite.EditValueChanged += BranchIDLookupedite_EditValueChanged;
            Refresh_Customers();
        }

        private void BranchIDLookupedite_EditValueChanged(object sender, EventArgs e)
        {
            Refresh_Customers();
        }

        private void CustID1_EditValueChanged(object sender, EventArgs e)
        {
            Refresh_Customers();
        }
    }
}