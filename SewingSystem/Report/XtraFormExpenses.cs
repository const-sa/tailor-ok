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
using static SewingSystem.Classes.Session;
using SewingSystem.Reports;

namespace SewingSystem.Report
{
    public partial class XtraFormExpenses : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        Report.XtraReportExpenses XtraReportExpenses;
        //Report.XtraReportExpensesGroupBand XtraReportExpensesGroupBand;
        public XtraFormExpenses()
        {
            InitializeComponent();
            if (Session.tblSanadSarf.Count() > 0)
            {
                dtime_Start.DateTime = Convert.ToDateTime(Session.tblSanadSarf.Min(d => d.TheDate.Value));
                dtime_End.DateTime = Convert.ToDateTime(Session.tblSanadSarf.Max(d => d.TheDate.Value));
                tblSanadSarfBindingSource.DataSource = (from cus in Session.tblSanadSarf
                                                                                     select new
                                                                                     {
                                                                                         cus.ID,
                                                                                         //BandName = MyFunaction.getBandName((cus.BandID as int?) ?? 0),
                                                                                         user = MyFunaction.getUserName((cus.UserID as short?) ?? 0),
                                                                                         BranchName = MyFunaction.getBranchName((cus.BranchID as short?) ?? 0),
                                                                                         //TheName = MyFunaction.getSupName((cus.CusNumber as int?) ?? 0),
                                                                                         cus.TheAmount,
                                                                                         cus.TheDate,
                                                                                         cus.TheDetails,
                                                                                         cus.EnterTime,
                                                                                         cus.SanadID,
                                                                                         cus.UserID,
                                                                                         cus.BranchID,
                                                                                         cus.BandID,
                                                                                     }).ToList();
                SanadID1.Properties.DataSource= Session.tblSanadSarf.ToList();
                SanadID2.Properties.DataSource = Session.tblSanadSarf.ToList();
                ID1.Properties.DataSource = Session.tblSanadSarf.ToList();
                ID2.Properties.DataSource = Session.tblSanadSarf.ToList();
            }
            tblUsersBindingSource.DataSource = Session.tblUser;
            tblBranchesBindingSource.DataSource = Session.tblBranche;
            //tblExpensBandBindingSource.DataSource = Session.tblExpensBand;
        }
      
     
       
        private void btnPrint_Click(object sender, EventArgs e)
        {
            GridReportP.Print(gcInvoice, this.Text, "", true, true, PrintFileType.Printer);
            //RReportExpensesFun();
            ////if (chedit_GroupBand.Checked)
            ////    Print(XtraReportExpensesGroupBand);
            ////else
            //    Print(XtraReportExpenses);
        }
        public void RReportExpensesFun()
        {
            //if (chedit_GroupBand.Checked)
            //{
            //    XtraReportExpensesGroupBand = new XtraReportExpensesGroupBand();
            //    XtraReportExpensesGroupBand.ApplyLocalization(Properties.Settings.Default.Language);
            //    XtraReportExpensesGroupBand.lbl_user.Text = Program.User.UserName;
            //    if (Properties.Settings.Default.Language == "ar-SA")
            //        XtraReportExpensesGroupBand.xrLabel1.Text += dtime_Start.DateTime.ToShortDateString() + "  الى  " + dtime_End.DateTime.ToShortDateString();
            //    else if (Properties.Settings.Default.Language == "en-US")
            //        XtraReportExpensesGroupBand.xrLabel1.Text += dtime_Start.DateTime.ToShortDateString() + "  To  " + dtime_End.DateTime.ToShortDateString();
            //    XtraReportExpensesGroupBand.DataSource = tblSanadSarfBindingSource.DataSource;
            //}
            //else
            //{
                XtraReportExpenses = new XtraReportExpenses();
                XtraReportExpenses.ApplyLocalization(Properties.Settings.Default.Language);
                XtraReportExpenses.lbl_user.Text = Program.User.UserName;
                if (Properties.Settings.Default.Language == "ar-SA")
                    XtraReportExpenses.xrLabel1.Text += dtime_Start.DateTime.ToShortDateString() + "  الى  " + dtime_End.DateTime.ToShortDateString();
                else if (Properties.Settings.Default.Language == "en-US")
                    XtraReportExpenses.xrLabel1.Text += dtime_Start.DateTime.ToShortDateString() + "  To  " + dtime_End.DateTime.ToShortDateString();
                XtraReportExpenses.DataSource = tblSanadSarfBindingSource.DataSource;
            //}
        }
        Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
        public void Print(DevExpress.XtraReports.UI.XtraReport Report)
        {
            frmReport.documentViewer1.DocumentSource = Report;
            frmReport.ShowDialog();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void UserIDLookUpEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            short id = (e.NewValue as short?) ?? 0;
            if (id > 0)
            {
                short Bid = (BranchIDLookupedite.EditValue as short?) ?? 0;
                RefreshData(id, Bid);
            }
        }

        private void BranchIDLookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            short id = (e.NewValue as short?) ?? 0;
            if (id > 0)
            {
                short Uid = (UserIDTextEdit.EditValue as short?) ?? 0;
                RefreshData(Uid, id);
            }
        }
        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            GridReportP.Print(gcInvoice, this.Text, "", true, true, PrintFileType.PDF);
            //RReportExpensesFun();
            //xtraSaveFileDialog1.Filter = "PDF Files|*.pdf";
            //if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    //if (chedit_GroupBand.Checked)
            //    //    XtraReportExpensesGroupBand.ExportToPdf(xtraSaveFileDialog1.FileName);
            //    //else
            //        XtraReportExpenses.ExportToPdf(xtraSaveFileDialog1.FileName);
            //}
        }
        private void btnRefreash_Click(object sender, EventArgs e)
        {
            UserIDTextEdit.ResetText();
            BranchIDLookupedite.ResetText();
            BandIDLookupedite1.ResetText();
            SanadID2.ResetText();
            SanadID1.ResetText();
            ID2.ResetText();
            ID1.ResetText();
            dtime_Start.DateTime = Convert.ToDateTime(Session.tblSanadSarf.Min(d => d.TheDate.Value));
            dtime_End.DateTime = Convert.ToDateTime(Session.tblSanadSarf.Max(d => d.TheDate.Value));
            tblSanadSarfBindingSource.DataSource = Session.tblSanadSarf.ToList();
        }
        private void dtime_Start_EditValueChanged(object sender, EventArgs e)
        {
            if (IsActive)
            {
                short Uid = (UserIDTextEdit.EditValue as short?) ?? 0;
                short Bid = (BranchIDLookupedite.EditValue as short?) ?? 0;
                RefreshData(Uid, Bid);
            }
        }
        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            GridReportP.Print(gcInvoice, this.Text, "", true, true, PrintFileType.Xlsx);
            //RReportExpensesFun();
            //xtraSaveFileDialog1.Filter = "Excel Files|*.Xls";
            //if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    //if (chedit_GroupBand.Checked)
            //    //    XtraReportExpensesGroupBand.ExportToXls(xtraSaveFileDialog1.FileName);
            //    //else
            //        XtraReportExpenses.ExportToXls(xtraSaveFileDialog1.FileName);
            //}
        }
        public void RefreshData(short U_id, short B_id)
        {
            int id1 = ((ID1.EditValue as int?) ?? 0);
            int id2 = ((ID2.EditValue as int?) ?? 0);
         var   report = (from cus in Session.tblSanadSarf
                      select new
                      {
                          cus.ID,
                          //BandName = MyFunaction.getBandName((cus.BandID as int?) ?? 0),
                          user = MyFunaction.getUserName((cus.UserID as short?) ?? 0),
                          BranchName = MyFunaction.getBranchName((cus.BranchID as short?) ?? 0),
                          //TheName = MyFunaction.getSupName((cus.CusNumber as int?) ?? 0),
                          cus.TheAmount,
                          cus.TheDate,
                          cus.TheDetails,
                          cus.EnterTime,
                          cus.SanadID,
                          cus.UserID,
                          cus.BranchID,
                          cus.BandID,
                      });
            if (id1 == 0 & id2 == 0)
                report = report.ToList();
            else if (id1 == 0 & id2 > 0)
                report = report.Where(p => p.ID == id2).ToList();
            else if (id2 == 0 & id1 > 0)
                report = report.Where(p => p.ID == id1).ToList();
            else if (id1 > 0 & id2 > 0)
                report = report.Where(p => p.ID >= id1 & p.ID <= id2).ToList();

            id1 = ((SanadID1.EditValue as int?) ?? 0);
            id2 = ((SanadID2.EditValue as int?) ?? 0);
            if (id1 == 0 & id2 > 0)
                report = report.Where(p => int.Parse(p.SanadID) == id2).ToList();
            else if (id2 == 0 & id1 > 0)
                report = report.Where(p => int.Parse(p.SanadID) == id1).ToList();
            else if (id1 > 0 & id2 > 0)
                report = report.Where(p => int.Parse(p.SanadID) >= id1 & int.Parse(p.SanadID) <= id2).ToList();

            if (U_id > 0 & B_id > 0)
                tblSanadSarfBindingSource.DataSource = report.Where(i => i.TheDate.Value.Date >= dtime_Start.DateTime.Date & i.TheDate.Value.Date <= dtime_End.DateTime.Date & i.UserID == U_id & i.BranchID == B_id).ToList();
            else if (U_id > 0 & B_id <= 0)
                tblSanadSarfBindingSource.DataSource = report.Where(i => i.TheDate.Value.Date >= dtime_Start.DateTime.Date & i.TheDate.Value.Date <= dtime_End.DateTime.Date & i.UserID == U_id).ToList();
            else if (U_id <= 0 & B_id > 0)
                tblSanadSarfBindingSource.DataSource = report.Where(i => i.TheDate.Value.Date >= dtime_Start.DateTime.Date & i.TheDate.Value.Date <= dtime_End.DateTime.Date & i.BranchID == B_id).ToList();
            else if (U_id <= 0 & B_id <= 0)
                tblSanadSarfBindingSource.DataSource = report.Where(i => i.TheDate.Value.Date >= dtime_Start.DateTime.Date & i.TheDate.Value.Date <= dtime_End.DateTime.Date).ToList();
            if(((BandIDLookupedite1.EditValue as int?) ?? 0)>0)
                tblSanadSarfBindingSource.DataSource = report.Where(i => i.BandID==((BandIDLookupedite1.EditValue as int?) ?? 0)).ToList();
            gcInvoice.DataSource = tblSanadSarfBindingSource.List;
        }
    }
}