using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using SewingSystem.LinqModel;
using SewingSystem.Classes;
using System.Linq;
using static DevExpress.XtraEditors.Mask.MaskSettings;

namespace SewingSystem.Report
{
    public partial class XtraReportInvoiceGenaral : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReportInvoiceGenaral()
        {
            InitializeComponent();
           
        }

        private void XtraReportInvoiceAllColumns_BeforePrint(object sender, CancelEventArgs e)
        {
            lbl_user.Text = Program.User.UserName;
            if (Program.Branch != null)
            {
                lbl_CompanyName.Text = Program.Branch.CompanyName;
                lbl_CompanyAddress.Text = Program.Branch.BranchAddress;
                lbl_CompanyEmail.Text = Program.Branch.Fax;
                lbl_CompanyPhone.Text = Program.Branch.BranchTelephone;
                lbl_CompanyMobil.Text = Program.Branch.Mobile;
                lbl_BranchName.Text = Session.tblBranche.SingleOrDefault(b => b.ID == Program.User.BranchID).BranchName;
                //var v = this.DataSource as View_Invoice;

                //lbl_Customer.DataBindings.Add("Text", this.DataSource, "Customer");
                //xrPictureLogo.DataBindings.Add("Image", Program.Branch.LogoImage.ToArray(), "LogoImage");

                if (Program.Branch.LogoImage != null)
                {
                    xrPictureLogo.Image = Master.FromByteArray<Image>(Program.Branch.LogoImage.ToArray());
                }
            }
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
            xrTableCell20.Row.Visible = (Program.Branch.UseTax as bool?) ?? false;
        }

        private void bindingSource1_CurrentChanged(object sender, EventArgs e)
        {
            if (!(Program.Branch.UseTax as bool?) ?? false)
            {
                foreach (tblSellInvoice item in bindingSource1)
                {
                    item.TotalFattInvoice = 0;
                }
                //if (bindingSource1.Current!=null)
                //{
                //  tblSellInvoice inv = bindingSource1.Current as tblSellInvoice;
                //inv.TotalFattInvoice = 0;
                //}
            }
        }

        private void xrTableCellMobil_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = sender as XRTableCell;
            cell.Text = $"{Session.tblCustomer.FirstOrDefault(x=>x.CusNumber==Convert.ToInt32(GetCurrentColumnValue("CusNumber")))?.Mobil}";
        }

        private void xrTableCell37_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = sender as XRTableCell;
            cell.Text = $"{Session.tblCustomer.FirstOrDefault(x => x.CusNumber == Convert.ToInt32(GetCurrentColumnValue("CusNumber")))?.CustomerName}";
        }

        private void xrTableCell24_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = sender as XRTableCell;
            cell.Text = $"{Session.tblBranche.FirstOrDefault(x => x.ID == Convert.ToInt32(GetCurrentColumnValue("BranchID")))?.BranchName}";
        }

        private void xrTableCell21_BeforePrint(object sender, CancelEventArgs e)
        {
            XRTableCell cell = sender as XRTableCell;
            cell.Text = $"{Session.tblUser.FirstOrDefault(x => x.ID == Convert.ToInt32(GetCurrentColumnValue("UserID")))?.UserName}";
        }
    }
}
