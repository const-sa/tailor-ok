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
using SewingSystem.Reports;
using static SewingSystem.Classes.Session;

namespace SewingSystem.Report
{
    public partial class XtraFormReportGapth : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        Report.XtraReportGapth XtraReportGapth;
        public XtraFormReportGapth()
        {
            InitializeComponent();
            if (Session.tblPayment.Count() > 0)
            {
                dtime_Start.DateTime = Convert.ToDateTime(Session.tblPayment.Min(d => d.PayDate.Value));
                dtime_End.DateTime = Convert.ToDateTime(Session.tblPayment.Max(d => d.PayDate.Value));
                tblPaymentBindingSource.DataSource = (from cus in Session.tblPayment
                                                                                     select new
                                                                                     {
                                                                                         cus.ID,
                                                                                         cus.CusNumber,
                                                                                         cus.InvoNumber,
                                                                                         UserName = MyFunaction.getUserName((cus.UserID as short?) ?? 0),
                                                                                         BranchName = MyFunaction.getBranchName((cus.BranchID as short?) ?? 0),
                                                                                         TheName = MyFunaction.getCusName((cus.CusNumber as int?) ?? 0),
                                                                                         cus.TheAmount,
                                                                                         cus.PayDate,
                                                                                         cus.Notes,
                                                                                         cus.EnterTime,
                                                                                         cus.UserID,
                                                                                         cus.MonyRemin,
                                                                                         cus.QuanDelivery,
                                                                                         cus.QuanRemin,
                                                                                         cus.BranchID,
                                                                                         cus.Discount,
                                                                                     }).ToList();
                ID1.Properties.DataSource = Session.tblPayment.ToList();
                ID2.Properties.DataSource = Session.tblPayment.ToList();
            }
            tblUsersBindingSource.DataSource = Session.tblUser;
            tblBranchesBindingSource.DataSource = Session.tblBranche;
        }
      
     
       
        private void btnPrint_Click(object sender, EventArgs e)
        {
            GridReportP.Print(gcInvoice, this.Text, "", true, true, PrintFileType.Printer);
            //RReportExpensesFun();
            //    Print(XtraReportGapth);
        }
        public void RReportExpensesFun()
        {
                XtraReportGapth = new XtraReportGapth();
                XtraReportGapth.ApplyLocalization(Properties.Settings.Default.Language);
                XtraReportGapth.lbl_user.Text = Program.User.UserName;
                if (Properties.Settings.Default.Language == "ar-SA")
                    XtraReportGapth.xrLabel1.Text += dtime_Start.DateTime.ToShortDateString() + "  الى  " + dtime_End.DateTime.ToShortDateString();
                else if (Properties.Settings.Default.Language == "en-US")
                    XtraReportGapth.xrLabel1.Text += dtime_Start.DateTime.ToShortDateString() + "  To  " + dtime_End.DateTime.ToShortDateString();
                XtraReportGapth.DataSource = tblPaymentBindingSource.DataSource;
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
            //        XtraReportGapth.ExportToPdf(xtraSaveFileDialog1.FileName);
        }
        private void btnRefreash_Click(object sender, EventArgs e)
        {
            UserIDTextEdit.ResetText();
            BranchIDLookupedite.ResetText();
            ID2.ResetText();
            ID1.ResetText();
            dtime_Start.DateTime = Convert.ToDateTime(Session.tblPayment.Min(d => d.PayDate.Value));
            dtime_End.DateTime = Convert.ToDateTime(Session.tblPayment.Max(d => d.PayDate.Value));
            tblPaymentBindingSource.DataSource = Session.tblPayment.ToList();
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
            //        XtraReportGapth.ExportToXls(xtraSaveFileDialog1.FileName);
        }
        public void RefreshData(short U_id, short B_id)
        {
            int id1 = ((ID1.EditValue as int?) ?? 0);
            int id2 = ((ID2.EditValue as int?) ?? 0);
         var   report = (from cus in Session.tblPayment
                      select new
                      {
                          cus.ID,
                          cus.CusNumber,
                          cus.InvoNumber,
                          UserName = MyFunaction.getUserName((cus.UserID as short?) ?? 0),
                          BranchName = MyFunaction.getBranchName((cus.BranchID as short?) ?? 0),
                          TheName = MyFunaction.getCusName((cus.CusNumber as int?) ??0),
                          cus.TheAmount,
                          cus.PayDate,
                          cus.Notes,
                          cus.EnterTime,
                          cus.UserID,
                          cus.MonyRemin,
                          cus.QuanDelivery,
                          cus.QuanRemin,
                          cus.Discount,
                          cus.BranchID,
                      });
            if (id1 == 0 & id2 == 0)
                report = report.ToList();
            else if (id1 == 0 & id2 > 0)
                report = report.Where(p => p.ID == id2).ToList();
            else if (id2 == 0 & id1 > 0)
                report = report.Where(p => p.ID == id1).ToList();
            else if (id1 > 0 & id2 > 0)
                report = report.Where(p => p.ID >= id1 & p.ID <= id2).ToList();
            if (U_id > 0 & B_id > 0)
                tblPaymentBindingSource.DataSource = report.Where(i => i.PayDate.Value.Date >= dtime_Start.DateTime.Date & i.PayDate.Value.Date <= dtime_End.DateTime.Date & i.UserID == U_id & i.BranchID == B_id).ToList();
            else if (U_id > 0 & B_id <= 0)
                tblPaymentBindingSource.DataSource = report.Where(i => i.PayDate.Value.Date >= dtime_Start.DateTime.Date & i.PayDate.Value.Date <= dtime_End.DateTime.Date & i.UserID == U_id).ToList();
            else if (U_id <= 0 & B_id > 0)
                tblPaymentBindingSource.DataSource = report.Where(i => i.PayDate.Value.Date >= dtime_Start.DateTime.Date & i.PayDate.Value.Date <= dtime_End.DateTime.Date & i.BranchID == B_id).ToList();
            else if (U_id <= 0 & B_id <= 0)
                tblPaymentBindingSource.DataSource = report.Where(i => i.PayDate.Value.Date >= dtime_Start.DateTime.Date & i.PayDate.Value.Date <= dtime_End.DateTime.Date).ToList();
            gcInvoice.DataSource = tblPaymentBindingSource.List;
        }
    }
}