using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using SewingSystem.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SewingSystem
{
    public partial class ReportHeader : XtraReport
    {
        public ReportHeader(string LabelReport)
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
