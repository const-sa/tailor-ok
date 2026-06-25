using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Editors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraReports.UI;
using System.IO;
using System.Data.Entity;
using SewingSystem.LinqModel;
using SewingSystem.Classes;

namespace SewingSystem.Report
{
    public partial class XtraFormReportsInvBuy : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        Report.XtraReportInvoiceBuy XtraReportInvoiceBuy;
        public XtraFormReportsInvBuy()
        {
            InitializeComponent();
        }
        List<tblBuyInvoice> report;
        public void RefreshData(short U_id, short B_id)
        {
            int id1 = ((SupID1.EditValue as int?) ?? 0);
            int id2 = ((SupID2.EditValue as int?) ?? 0);
            //report.Clear();
            report = new List<tblBuyInvoice>();
            if (id1 == 0 & id2 == 0)
                report = Session.tblBuyInvoice.ToList();
            else if (id1 == 0 & id2 > 0)
                report = Session.tblBuyInvoice.Where(p => p.SupID == id2).ToList();
            else if (id2 == 0 & id1 > 0)
                report = Session.tblBuyInvoice.Where(p => p.SupID == id1).ToList();
            else if (id1 > 0 & id2 > 0)
                report = Session.tblBuyInvoice.Where(p => p.SupID >= id1 & p.SupID <= id2).ToList();

            id1 = ((InvoID1.EditValue as int?) ?? 0);
            id2 = ((InvoID2.EditValue as int?) ?? 0);
            if (id1 == 0 & id2 > 0)
                report = report.Where(p => p.ID == id2).ToList();
            else if (id2 == 0 & id1 > 0)
                report = report.Where(p => p.ID == id1).ToList();
            else if (id1 > 0 & id2 > 0)
                report = report.Where(p => p.ID >= id1 & p.ID <= id2).ToList();

            if (U_id > 0 & B_id > 0)
                        tblBuyInvoiceBindingSource.DataSource = report.Where(i => i.BuyDate.Value.Date >= dtime_Start.DateTime.Date & i.BuyDate.Value.Date <= dtime_End.DateTime.Date & i.UserID == U_id & i.BranchID == B_id).ToList();
                    else if (U_id > 0 & B_id <= 0)
                        tblBuyInvoiceBindingSource.DataSource = report.Where(i => i.BuyDate.Value.Date >= dtime_Start.DateTime.Date & i.BuyDate.Value.Date <= dtime_End.DateTime.Date & i.UserID == U_id).ToList();
                    else if (U_id <= 0 & B_id > 0)
                        tblBuyInvoiceBindingSource.DataSource = report.Where(i => i.BuyDate.Value.Date >= dtime_Start.DateTime.Date & i.BuyDate.Value.Date <= dtime_End.DateTime.Date & i.BranchID == B_id).ToList();
                    else if (U_id <= 0 & B_id <= 0)
                        tblBuyInvoiceBindingSource.DataSource = report.Where(i => i.BuyDate.Value.Date >= dtime_Start.DateTime.Date & i.BuyDate.Value.Date <= dtime_End.DateTime.Date).ToList();
        }
        private void Print_Click(object sender, EventArgs e)
        {
                ReportInvoiceAllColumnFun();
                Print(XtraReportInvoiceBuy);
        }
        Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
        public void Print(DevExpress.XtraReports.UI.XtraReport Report)
        {
            frmReport.documentViewer1.DocumentSource = Report;
            frmReport.ShowDialog();
        }
        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        public void ReportInvoiceAllColumnFun()
        {
            XtraReportInvoiceBuy = new XtraReportInvoiceBuy();

            XtraReportInvoiceBuy.ApplyLocalization(Properties.Settings.Default.Language);
            XtraReportInvoiceBuy.xrFromDate.Text = dtime_Start.DateTime.ToShortDateString();
            XtraReportInvoiceBuy.xrDateTo.Text = dtime_End.DateTime.ToShortDateString();
            XtraReportInvoiceBuy.lbl_user.Text = Program.User.UserName;
            XtraReportInvoiceBuy.DataSource = (from cus in Session.tblBuyInvoice
                                             select new
                                             {
                                                 cus.ID,
                                                 SupName = getSupName((cus.SupID as int?) ?? 0),
                                                 user = getUserName((cus.UserID as short?) ?? 0),
                                                 BranchName = getBranchName((cus.BranchID as short?) ?? 0),
                                                 cus.Discount,
                                                 cus.Bayan,
                                                 cus.BuyDate,
                                                 cus.InvoValue,
                                                 cus.TheQuantity,
                                                 cus.EnterTime,
                                                 cus.TheSafy,
                                                 cus.TheTax,
                                                 cus.TotalAftDis,
                                             }).ToList();
        }
        public string getSupName(int SupID)
        {
            if (SupID == 0)
                return "";
            else
            {
                var g1 = Session.tblSupplier.Where(g => g.ID == SupID).ToList();
                if (g1.Count() > 0)
                    return ((g1[0].SupName as string) ?? "");
                else
                    return "";
            }
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
                ReportInvoiceAllColumnFun();
                xtraSaveFileDialog1.Filter = "PDF Files|*.pdf";
                if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
                    XtraReportInvoiceBuy.ExportToPdf(xtraSaveFileDialog1.FileName);
        }

        private void Refreash_Click(object sender, EventArgs e)
        {
            UserIDTextEdit.ResetText();
            BranchIDLookupedite.ResetText();
            short Uid = (UserIDTextEdit.EditValue as short?) ?? 0;
            short Bid = (BranchIDLookupedite.EditValue as short?) ?? 0;
            RefreshData(Uid, Bid);
        }

        private void Dtime_Start_EditValueChanged(object sender, EventArgs e)
        {
            if (IsActive)
            {
                short Uid = (UserIDTextEdit.EditValue as short?) ?? 0;
                short Bid = (BranchIDLookupedite.EditValue as short?) ?? 0;
                RefreshData(Uid, Bid);
            }
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

        private void ExportExcel_Click(object sender, EventArgs e)
        {
                ReportInvoiceAllColumnFun();
                xtraSaveFileDialog1.Filter = "Excel Files|*.Xls";
                if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
                    XtraReportInvoiceBuy.ExportToXls(xtraSaveFileDialog1.FileName);
        }
        private void XtraFormReportsByDate_Load(object sender, EventArgs e)
        {
            if (Program.User.UserState == "Admin")
            {
                ItemForUserID.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                ItemForUserID1.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
            tblUsersBindingSource.DataSource = Session.tblUser.ToList();
            tblBrancheBindingSource.DataSource = Session.tblBranche.ToList();
                tblBuyInvoiceBindingSource.DataSource = Session.tblBuyInvoice;
            if (tblBuyInvoiceBindingSource.DataSource != null)
            {
                var ff = (BindingList<tblBuyInvoice>)tblBuyInvoiceBindingSource.DataSource;
                dtime_End.DateTime = ff.Max(s => s.BuyDate.Value);
                dtime_Start.DateTime = ff.Min(s => s.BuyDate.Value);
            }
            labelControl1.Text = this.Text;
        }
    }
}