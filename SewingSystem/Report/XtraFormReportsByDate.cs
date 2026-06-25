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
using DevExpress.XtraExport.Helpers;
using SewingSystem.Reports;
using static SewingSystem.Classes.Session;

namespace SewingSystem.Report
{
    public partial class XtraFormReportsByDate : DevExpress.XtraEditors.XtraForm
    {
        List<tblSellInvoice> Filter_data = new List<tblSellInvoice>();
        Report.XtraReportInvoiceGenaral XtraReportInvoiceGenaral;
        Forms.XtraReportInvoice XtraReportInvoice;
        string TypeForm;
        public XtraFormReportsByDate(string type, string LableForm)
        {
            InitializeComponent();
            this.Text = LableForm;
            gridView1.CustomColumnDisplayText += GridView1_CustomColumnDisplayText;
            TypeForm = type;
            clsAppearance.AppearanceGridView(gridView1);
        }
        MyFunaction clsAppearance = new MyFunaction();
        private void GridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value == null) return;
                switch (e.Column.Name)
                {
                    case "colCustomerName" when e.Value is int b:
                        e.DisplayText = Session.tblCustomer?.FirstOrDefault(x => x.CusNumber == b)?.CustomerName;
                        break;
                    case "colUserName" when e.Value is short b:
                        e.DisplayText = Session.tblUser?.FirstOrDefault(x => x.ID == b)?.UserName;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                return;
            }
        }

        public void RefreshData(short U_id, short B_id)
        {
            switch (TypeForm)
            {
                case "All":
                    if (U_id > 0 & B_id > 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.UserID == U_id & i.BranchID == B_id).ToList();
                    else if (U_id > 0 & B_id <= 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.UserID == U_id).ToList();
                    else if (U_id <= 0 & B_id > 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.BranchID == B_id).ToList();
                    else if (U_id <= 0 & B_id <= 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date).ToList();
                    break;
                case "NotPaid":
                    if (U_id > 0 & B_id > 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.MonyRemin > 0 & i.UserID == U_id & i.BranchID == B_id).ToList();
                    else if (U_id > 0 & B_id <= 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.MonyRemin > 0 & i.UserID == U_id).ToList();
                    else if (U_id <= 0 & B_id > 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.MonyRemin > 0 & i.BranchID == B_id).ToList();
                    else if (U_id <= 0 & B_id <= 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.MonyRemin > 0).ToList();
                    break;
                case "Paid":
                    if (U_id > 0 & B_id > 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.MonyRemin <= 0 & i.UserID == U_id & i.BranchID == B_id).ToList();
                    else if (U_id > 0 & B_id <= 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.MonyRemin <= 0 & i.UserID == U_id).ToList();
                    else if (U_id <= 0 & B_id > 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.MonyRemin <= 0 & i.BranchID == B_id).ToList();
                    else if (U_id <= 0 & B_id <= 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.MonyRemin <= 0).ToList();
                    break;
                case "NotDlivery":
                    if (U_id > 0 & B_id > 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.QuanRemin > 0 & i.UserID == U_id & i.BranchID == B_id).ToList();
                    else if (U_id > 0 & B_id <= 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.QuanRemin > 0 & i.UserID == U_id).ToList();
                    else if (U_id <= 0 & B_id > 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.QuanRemin > 0 & i.BranchID == B_id).ToList();
                    else if (U_id <= 0 & B_id <= 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.QuanRemin > 0).ToList();
                    break;
                case "Dlivery":
                    if (U_id > 0 & B_id > 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.QuanRemin <= 0 & i.UserID == U_id & i.BranchID == B_id).ToList();
                    else if (U_id > 0 & B_id <= 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.QuanRemin <= 0 & i.UserID == U_id).ToList();
                    else if (U_id <= 0 & B_id > 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.QuanRemin <= 0 & i.BranchID == B_id).ToList();
                    else if (U_id <= 0 & B_id <= 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.QuanRemin <= 0).ToList();
                    break;
                case "NotOrpon":
                    if (U_id > 0 & B_id > 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.MonyOrpon <= 0 & i.UserID == U_id & i.BranchID == B_id).ToList();
                    else if (U_id > 0 & B_id <= 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.MonyOrpon <= 0 & i.UserID == U_id).ToList();
                    else if (U_id <= 0 & B_id > 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.MonyOrpon <= 0 & i.BranchID == B_id).ToList();
                    else if (U_id <= 0 & B_id <= 0)
                        viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.MonyOrpon <= 0).ToList();
                    break;
            }
        }
        private void Print_Click(object sender, EventArgs e)
        {
            //GridReportP.Print(gcInvoice, Session.LangEng ? $"List of Sales" : $"قائمة الفواتير", "", true, true, PrintFileType.Printer);
            if (TheReportTypeTextEdit.Text == "عام")
            {
                ReportInvoiceAllColumnFun();
                Print(XtraReportInvoiceGenaral);
            }
            else if (TheReportTypeTextEdit.Text == "مالي فقط")
            {
                ReportInvoiceFun();
                Print(XtraReportInvoice);
            }
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
            XtraReportInvoiceGenaral = new XtraReportInvoiceGenaral();

            XtraReportInvoiceGenaral.ApplyLocalization(Properties.Settings.Default.Language);
            XtraReportInvoiceGenaral.xrFromDate.Text = dtime_Start.DateTime.ToShortDateString();
            XtraReportInvoiceGenaral.xrDateTo.Text = dtime_End.DateTime.ToShortDateString();
            XtraReportInvoiceGenaral.lbl_user.Text = Program.User.UserName;
            if (!string.Empty.Contains(gridView1.FilterPanelText))
            {
                Filter_data.Clear();
                for (int i = 0; i < gridView1.RowCount; i++)
                    Filter_data.Add(gridView1.GetRow(i) as tblSellInvoice);
                XtraReportInvoiceGenaral.DataSource = Filter_data;
            }
            else
                XtraReportInvoiceGenaral.DataSource = gridView1.DataSource;
        }
        public void ReportInvoiceFun()
        {
            XtraReportInvoice = new Forms.XtraReportInvoice();
            XtraReportInvoice.ApplyLocalization(Properties.Settings.Default.Language);
            XtraReportInvoice.lbl_user.Text = Program.User.UserName;
            if (Properties.Settings.Default.Language == "ar-SA")
                XtraReportInvoice.xrLabel1.Text += dtime_Start.DateTime.ToShortDateString() + " الى " + dtime_End.DateTime.ToShortDateString();
            else if (Properties.Settings.Default.Language == "en-US")
                XtraReportInvoice.xrLabel1.Text += dtime_Start.DateTime.ToShortDateString() + " To " + dtime_End.DateTime.ToShortDateString();
            XtraReportInvoice.DataSource = gcInvoice.DataSource;
        }
        private void ExportPDF_Click(object sender, EventArgs e)
        {
            if (TheReportTypeTextEdit.Text == "عام")
            {
                ReportInvoiceAllColumnFun();
                xtraSaveFileDialog1.Filter = "PDF Files|*.pdf";
                if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
                    XtraReportInvoiceGenaral.ExportToPdf(xtraSaveFileDialog1.FileName);
            }
            else if (TheReportTypeTextEdit.Text == "مالي فقط")
            {
                ReportInvoiceFun();
                xtraSaveFileDialog1.Filter = "PDF Files|*.pdf";
                if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
                    XtraReportInvoice.ExportToPdf(xtraSaveFileDialog1.FileName);
            }
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
            if (TheReportTypeTextEdit.Text == "عام")
            {
                ReportInvoiceAllColumnFun();
                xtraSaveFileDialog1.Filter = "Excel Files|*.Xls";
                if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
                    XtraReportInvoiceGenaral.ExportToXls(xtraSaveFileDialog1.FileName);
            }
            else if (TheReportTypeTextEdit.Text == "مالي فقط")
            {
                ReportInvoiceFun();
                xtraSaveFileDialog1.Filter = "Excel Files|*.Xls";
                if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
                    XtraReportInvoice.ExportToXls(xtraSaveFileDialog1.FileName);
            }
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
            if (TypeForm == "All")
                viewInvoiceBindingSource.DataSource = Session.tblSellInvoice;
            else if (TypeForm == "NotPaid")
                viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.MonyRemin > 0).ToList();
            else if (TypeForm == "NotDlivery")
                viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.QuanRemin >0).ToList();
            else if (TypeForm == "Paid")
                viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.MonyRemin <= 0).ToList();
            else if (TypeForm == "Dlivery")
                viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.QuanRemin <= 0).ToList();
            else if (TypeForm == "NotOrpon")
                viewInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.MonyOrpon <= 0).ToList();
            if (viewInvoiceBindingSource.DataSource != null)
            {
                if (viewInvoiceBindingSource.List.Count>0)
                {
                    dtime_End.DateTime = ((List<tblSellInvoice>)viewInvoiceBindingSource.DataSource).Max(s => s.SellDate.Value);
                    dtime_Start.DateTime = ((List<tblSellInvoice>)viewInvoiceBindingSource.DataSource).Min(s => s.SellDate.Value);
                }
            }
            labelControl1.Text = this.Text;
        }
    }
}