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
using SewingSystem.Forms;
using DevExpress.XtraGrid.Views.Grid;

namespace SewingSystem.Report
{
    public partial class XtraFormReportDay : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();

        string TypeReport;
        public XtraFormReportDay(string typeReport)
        {
            InitializeComponent();
            TypeReport = typeReport;

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            dataLayoutControl1.ShowPrintPreview();
        }
        public void RefreshReport()
        {
            txtTotalFainlAll.Text = (MyFunaction.GetdecimalValue(gridView111.Columns["TheAmount"].SummaryItem.SummaryValue.ToString()) 
                + MyFunaction.GetdecimalValue(gridView11.Columns["MonyOrpon"].SummaryItem.SummaryValue.ToString())).ToString();
        }
        private void XtraFormReportDay_Load(object sender, EventArgs e)
        {
            BranchIDLookUpEdit.EditValueChanging += BranchIDLookUpEdit_EditValueChanging;
            UserIDLookUpEdit1.EditValueChanging += UserIDLookUpEdit1_EditValueChanging;
            btnRefreash.Click += BtnRefreash_Click;
            tblUserBindingSource.DataSource = Session.tblUser;
            tblBrancheBindingSource.DataSource = Session.tblBranche;
            tblCustomerBindingSource.DataSource = Session.tblCustomer.ToList();
            labelMony.Text =Session.LangEng? "cash report " : " تقرير النقدية ";
            if (Program.User.UserState != "Admin")
                layoutControlGroup2.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            if (TypeReport == "اليوم")
            {
                labelMony.Text += DateTime.Now.ToShortDateString();
                layoutControlGroup8.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                dt_Start = DateTime.Now.Date;
                dt_End = DateTime.Now.Date;
            }
            else if (TypeReport == "الشهر")
            {
                DateTime dt = DateTime.Now.Date;
                dt_Start = Convert.ToDateTime(dt.Year.ToString() + "/" + dt.Month.ToString() + "/1");
                dt_End = dt_Start.AddMonths(1);
                labelMony.Text +=( Session.LangEng ? " For a month" : " لشهر ") + DateTime.Now.Month.ToString();
            }
            else if (TypeReport == "الكل")
            {
                List<DateTime> dt = new List<DateTime>();
                if (Session.tblPayment.Count() > 0)
                {
                    dt.Add(Session.tblPayment.Min(m => m.PayDate.Value.Date));
                    dt.Add(Session.tblPayment.Max(m => m.PayDate.Value.Date));
                }
                if (Session.tblSellInvoice.Count() > 0)
                {
                    dt.Add(Session.tblSellInvoice.Min(m => m.SellDate.Value.Date));
                    dt.Add(Session.tblSellInvoice.Max(m => m.SellDate.Value.Date));
                }
                dt_Start = dt.Min();
                dt_End = dt.Max();
            }
            dtime_Start.DateTime = dt_Start;
            dtime_End.DateTime = dt_End;
            FillData(dt_Start, dt_End);
            RefreshReport();
        }

        private void BtnRefreash_Click(object sender, EventArgs e)
        {
            XtraFormReportDay_Load(sender, null);
        }

        private void UserIDLookUpEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            short id = (e.NewValue as short?) ?? 0;
            if (id > 0)
                UserFillData(id);
        }

        private void BranchIDLookUpEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            short id = (e.NewValue as short?) ?? 0;
            if (id > 0)
                BranchFillData(id);
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            xtraSaveFileDialog1.Filter = "Excel Files|*.Xls";
            if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
                dataLayoutControl1.ExportToXls(xtraSaveFileDialog1.FileName);

        }
        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            xtraSaveFileDialog1.Filter = "PDF Files|*.pdf";
            if (xtraSaveFileDialog1.ShowDialog() == DialogResult.OK)
                dataLayoutControl1.ExportToPdf(xtraSaveFileDialog1.FileName);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
        DateTime dt_Start, dt_End;
        public void FillData(DateTime dtSt, DateTime dtEnd)
        {
            tblPaymentBindingSource.DataSource = Session.tblPayment.Where(i => i.PayDate.Value.Date >= dtSt.Date & i.PayDate.Value.Date <= dtEnd.Date).ToList();
            tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtSt.Date & i.SellDate.Value.Date <= dtEnd.Date).ToList();
        }
        public void BranchFillData(short id)
        {
            tblPaymentBindingSource.DataSource = Session.tblPayment.Where(i => i.PayDate.Value.Date >= dtime_Start.DateTime.Date & i.PayDate.Value.Date <= dtime_End.DateTime.Date & i.BranchID == id).ToList();
            tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.BranchID == id).ToList();
        }
        public void UserFillData(short id)
        {
            tblPaymentBindingSource.DataSource = Session.tblPayment.Where(i => i.PayDate.Value.Date >= dtime_Start.DateTime.Date & i.PayDate.Value.Date <= dtime_End.DateTime.Date & i.UserID == id).ToList();
            tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.Where(i => i.SellDate.Value.Date >= dtime_Start.DateTime.Date & i.SellDate.Value.Date <= dtime_End.DateTime.Date & i.UserID == id).ToList();
        }
        private void dtime_Start_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (((DateEdit)sender).Name == "dtime_Start")
                FillData(Convert.ToDateTime(e.NewValue.ToString()).Date, dtime_End.DateTime.Date);
            if (((DateEdit)sender).Name == "dtime_End")
                FillData(dtime_Start.DateTime.Date, Convert.ToDateTime(e.NewValue.ToString()).Date);
            RefreshReport();
        }
        //XtraFormShowInvoiceData ShowInvoiceData;
        private void gridViewInvoiceManager_RowClick(object sender, RowClickEventArgs e)
        {
            //if (e.Clicks == 2)
            //{
            //    tblPayment d = gridView11.GetRow(e.RowHandle) as tblPayment;
            //    tblSellInvoice invo = Session.tblSellInvoice.SingleOrDefault(s => s.ID == d.InvoID & s.BranchID == d.BranchID);
            //    ShowInvoiceData = new Forms.XtraFormShowInvoiceData(invo);
            //    ShowInvoiceData.ShowDialog();
            //}
        }

    }
}