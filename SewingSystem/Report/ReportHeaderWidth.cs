using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using SewingSystem.Classes;

namespace SewingSystem
{
    public partial class ReportHeaderWidth : XtraReport
    {
        public ReportHeaderWidth(string LabelReport)
        {
            InitializeComponent();
            BranchbindingSource.DataSource = Program.Branch;
            SetRTL();
            xrLabelReport.Text = LabelReport;
        }
        private void SetRTL()
        {
            if (!Session.LangEng) return;

            this.RightToLeft = RightToLeft.No;
            this.RightToLeftLayout = RightToLeftLayout.No;
        }
    }
}
