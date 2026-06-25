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
using static SewingSystem.Classes.Session;

namespace SewingSystem.Report
{
    public partial class XtraFormReportsInvSale : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        Report.XtraReportInvoiceTax XtraReportInvoiceBuy;
            string InvType;
        public XtraFormReportsInvSale(string invType)
        {
            InitializeComponent();
            InvType = invType;
        }
        public void RefreshData(short U_id, short B_id)
        {
            int id1 = ((searchLookUpEditFromCusOrSup.EditValue as int?) ?? 0);
            int id2 = ((searchLookUpEditToCusOrSup.EditValue as int?) ?? 0);
           var report=Session.tblInvoices.ToList();
            if (report.Count()>0)
            {
                if (id1 == 0 & id2 > 0)
                    report = Session.tblInvoices.Where(p => p.CusOrSupID == id2).ToList();
                else if (id2 == 0 & id1 > 0)
                    report = Session.tblInvoices.Where(p => p.CusOrSupID == id1).ToList();
                else if (id1 > 0 & id2 > 0)
                    report = Session.tblInvoices.Where(p => p.CusOrSupID >= id1 & p.CusOrSupID <= id2).ToList();

                id1 = ((searchLookUpEditFromInvo.EditValue as int?) ?? 0);
                id2 = ((searchLookUpEditToInvo.EditValue as int?) ?? 0);
                if (id1 == 0 & id2 > 0)
                    report = report.Where(p => p.ID == id2).ToList();
                else if (id2 == 0 & id1 > 0)
                    report = report.Where(p => p.ID == id1).ToList();
                else if (id1 > 0 & id2 > 0)
                    report = report.Where(p => p.ID >= id1 & p.ID <= id2).ToList();

                if (U_id > 0 & B_id > 0)
                    gcInvoice.DataSource = report.Where(i => i.TheDate.Value.Date >= dtime_Start.DateTime.Date & i.TheDate.Value.Date <= dtime_End.DateTime.Date & i.UserID == U_id & i.BranchID == B_id).ToList();
                else if (U_id > 0 & B_id <= 0)
                    gcInvoice.DataSource = report.Where(i => i.TheDate.Value.Date >= dtime_Start.DateTime.Date & i.TheDate.Value.Date <= dtime_End.DateTime.Date & i.UserID == U_id).ToList();
                else if (U_id <= 0 & B_id > 0)
                    gcInvoice.DataSource = report.Where(i => i.TheDate.Value.Date >= dtime_Start.DateTime.Date & i.TheDate.Value.Date <= dtime_End.DateTime.Date & i.BranchID == B_id).ToList();
                else if (U_id <= 0 & B_id <= 0)
                    gcInvoice.DataSource = report.Where(i => i.TheDate.Value.Date >= dtime_Start.DateTime.Date & i.TheDate.Value.Date <= dtime_End.DateTime.Date).ToList();
                txtInvoValue.Text = (((gridView1.Columns["TotalAftDis"].SummaryItem.SummaryValue as decimal?) ?? 0)+
                    (((gridView1.Columns["TheSafyAndMaden"].SummaryItem.SummaryValue as decimal?) ?? 0) - ((gridView1.Columns["TheTaxAndMaden"].SummaryItem.SummaryValue as decimal?) ?? 0)) -
                    (((gridView1.Columns["TheSafyAndNotic"].SummaryItem.SummaryValue as decimal?) ?? 0)- (gridView1.Columns["TheTaxAndDayn"].SummaryItem.SummaryValue as decimal?) ?? 0)).ToString();

                txtTheTax.Text = (((gridView1.Columns["TheTax"].SummaryItem.SummaryValue as decimal?) ?? 0) +
                    ((gridView1.Columns["TheTaxAndMaden"].SummaryItem.SummaryValue as decimal?) ?? 0) -
                    ((gridView1.Columns["TheTaxAndDayn"].SummaryItem.SummaryValue as decimal?) ?? 0)).ToString();

                txtTheSafy.Text = (((gridView1.Columns["TheSafy"].SummaryItem.SummaryValue as decimal?) ?? 0) +
                  ((gridView1.Columns["TheSafyAndMaden"].SummaryItem.SummaryValue as decimal?) ?? 0) -
                  ((gridView1.Columns["TheSafyAndNotic"].SummaryItem.SummaryValue as decimal?) ?? 0)).ToString();
            }
        }
        private void gridView1_RowCountChanged(object sender, EventArgs e)
        {
            txtInvoValue.Text = (((gridView1.Columns["TotalAftDis"].SummaryItem.SummaryValue as decimal?) ?? 0) +
                  (((gridView1.Columns["TheSafyAndMaden"].SummaryItem.SummaryValue as decimal?) ?? 0) - ((gridView1.Columns["TheTaxAndMaden"].SummaryItem.SummaryValue as decimal?) ?? 0)) -
                  (((gridView1.Columns["TheSafyAndNotic"].SummaryItem.SummaryValue as decimal?) ?? 0) - (gridView1.Columns["TheTaxAndDayn"].SummaryItem.SummaryValue as decimal?) ?? 0)).ToString();

            txtTheTax.Text = (((gridView1.Columns["TheTax"].SummaryItem.SummaryValue as decimal?) ?? 0) +
                ((gridView1.Columns["TheTaxAndMaden"].SummaryItem.SummaryValue as decimal?) ?? 0) -
                ((gridView1.Columns["TheTaxAndDayn"].SummaryItem.SummaryValue as decimal?) ?? 0)).ToString();

            txtTheSafy.Text = (((gridView1.Columns["TheSafy"].SummaryItem.SummaryValue as decimal?) ?? 0) +
              ((gridView1.Columns["TheSafyAndMaden"].SummaryItem.SummaryValue as decimal?) ?? 0) -
              ((gridView1.Columns["TheSafyAndNotic"].SummaryItem.SummaryValue as decimal?) ?? 0)).ToString();
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
        List<Invoices> Filter_data = new List<Invoices>();
        public void ReportInvoiceAllColumnFun()
        {
            XtraReportInvoiceBuy = new XtraReportInvoiceTax(InvType);

            XtraReportInvoiceBuy.ApplyLocalization(Properties.Settings.Default.Language);
            XtraReportInvoiceBuy.xrFromDate.Text = dtime_Start.DateTime.ToShortDateString();
            XtraReportInvoiceBuy.xrDateTo.Text = dtime_End.DateTime.ToShortDateString();
            XtraReportInvoiceBuy.lbl_user.Text = Program.User.UserName;
            XtraReportInvoiceBuy.total.Text = txtInvoValue.Text;
            XtraReportInvoiceBuy.tax.Text = txtTheTax.Text;
            XtraReportInvoiceBuy.fainal.Text = txtTheSafy.Text;
            if (!string.Empty.Contains(gridView1.FilterPanelText))
            {
                Filter_data.Clear();
                for (int i = 0; i < gridView1.RowCount; i++)
                    Filter_data.Add(gridView1.GetRow(i) as Invoices);
                XtraReportInvoiceBuy.DataSource = Filter_data;
            }
            else
                XtraReportInvoiceBuy.DataSource = gridView1.DataSource;
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
            Program.currTypInvo = InvType;
            if (Program.currTypInvo == Program.Buy| Program.currTypInvo == Program.ReturnBuy)
            {
                tblSupplier f = new LinqModel.tblSupplier();
                searchLookUpEditFromCusOrSup.Properties.DataSource= Session.tblSupplier.ToList();
                searchLookUpEditToCusOrSup.Properties.DataSource = Session.tblSupplier.ToList();
                gridView1.Columns[nameof(f.TheName)].Caption = "اسم المورد";
                layoutControlItem25.Text = "من رقم المورد:";
                layoutControlItem10.Text = "الى رقم المورد:";
                if (Program.currTypInvo == Program.Buy)
                    labelControl1.Text = "تقارير المشتريات";
                else if (Program.currTypInvo == Program.ReturnBuy)
                    labelControl1.Text = "تقارير مردودات المشتريات";
            }
            else if (Program.currTypInvo == Program.Sale| Program.currTypInvo == Program.ReturnSale)
            {
                tblCustomer f=new LinqModel.tblCustomer();
                searchLookUpEditFromCusOrSup.Properties.DataSource = Session.tblCustomer.ToList();
                searchLookUpEditToCusOrSup.Properties.DataSource = Session.tblCustomer.ToList();
                gridView1.Columns[nameof(f.TheName)].Caption = "اسم العميل";
                layoutControlItem25.Text = "من رقم العميل:";
                layoutControlItem10.Text = "الى رقم العميل:";
                //if (Program.currTypInvo == Program.Sale)
                //    labelControl1.Text = "تقارير المبيعات";
                //else if (Program.currTypInvo == Program.ReturnSale)
                //    labelControl1.Text = "تقارير مردودات المبيعات";
            }
            this.Text = labelControl1.Text;
                var ff = Session.tblInvoices.ToList();
            gcInvoice.DataSource = ff.ToList();
            searchLookUpEditFromInvo.Properties.DataSource = gcInvoice.DataSource;
            searchLookUpEditToInvo.Properties.DataSource = gcInvoice.DataSource;
            if (ff != null)
            {
                if (ff.Count()>0)
                {
                    dtime_End.DateTime = ff.Max(s => s.TheDate.Value);
                    dtime_Start.DateTime = ff.Min(s => s.TheDate.Value);
                }
            }
            labelControl1.Text = this.Text;
        }

       
    }
}