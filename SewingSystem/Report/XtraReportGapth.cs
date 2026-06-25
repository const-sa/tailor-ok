using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using SewingSystem.Classes;
using System.Linq;

namespace SewingSystem.Report
{
    public partial class XtraReportGapth : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraReportGapth()
        {
            InitializeComponent();
          
        }

        private void XtraReportGapth_BeforePrint(object sender, CancelEventArgs e)
        {
            lbl_user.Text = Program.User.UserName;
            if (Program.Branch != null)
            {
                lbl_CompanyName.Text = Program.Branch.CompanyName;
                lbl_CompanyAddress.Text = Program.Branch.BranchAddress;
                lbl_CompanyEmail.Text = Program.Branch.Email;
                lbl_CompanyPhone.Text = Program.Branch.BranchTelephone;
                lbl_CompanyMobil.Text = Program.Branch.Mobile;
                lbl_BranchName.Text = Session.tblBranche.SingleOrDefault(b => b.ID == Program.User.BranchID).BranchName;
                if (Program.Branch.LogoImage != null)
                {
                    xrPictureBox1.Image = Master.FromByteArray<Image>(Program.Branch.LogoImage.ToArray());
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
        }
    }
}
