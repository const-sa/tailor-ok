namespace SewingSystem
{
    partial class ReportHeaderWidth
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportHeaderWidth));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabelReport = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelBranchTrade2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable9 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow20 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell51 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell52 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrPictureBoxBranch = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabelBranchNameEn = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelBranchAddressEn = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabelBranchMailBox2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.baseControlStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.evenDetailStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.oddDetailStyle = new DevExpress.XtraReports.UI.XRControlStyle();
            this.BranchbindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BranchbindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabelReport,
            this.xrLabel11,
            this.xrLabel12,
            this.xrLabel13,
            this.xrLabel14,
            this.xrLabel15,
            this.xrLabelBranchTrade2,
            this.xrTable9,
            this.xrPictureBoxBranch,
            this.xrLabelBranchNameEn,
            this.xrLabelBranchAddressEn,
            this.xrLabelBranchMailBox2,
            this.xrLabel1,
            this.xrTable1});
            resources.ApplyResources(this.Detail, "Detail");
            this.Detail.HierarchyPrintOptions.Indent = 50.8F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            // 
            // xrLabelReport
            // 
            this.xrLabelReport.AutoWidth = true;
            resources.ApplyResources(this.xrLabelReport, "xrLabelReport");
            this.xrLabelReport.Name = "xrLabelReport";
            this.xrLabelReport.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelReport.StylePriority.UseFont = false;
            this.xrLabelReport.StylePriority.UseForeColor = false;
            this.xrLabelReport.StylePriority.UseTextAlignment = false;
            // 
            // xrLabel11
            // 
            this.xrLabel11.AutoWidth = true;
            resources.ApplyResources(this.xrLabel11, "xrLabel11");
            this.xrLabel11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[brnMailBox]")});
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UsePadding = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.WordWrap = false;
            // 
            // xrLabel12
            // 
            this.xrLabel12.AutoWidth = true;
            resources.ApplyResources(this.xrLabel12, "xrLabel12");
            this.xrLabel12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[brnPhnNo]")});
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UsePadding = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.WordWrap = false;
            // 
            // xrLabel13
            // 
            this.xrLabel13.CanShrink = true;
            resources.ApplyResources(this.xrLabel13, "xrLabel13");
            this.xrLabel13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[brnAddress]")});
            this.xrLabel13.Multiline = true;
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UsePadding = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            // 
            // xrLabel14
            // 
            this.xrLabel14.AutoWidth = true;
            resources.ApplyResources(this.xrLabel14, "xrLabel14");
            this.xrLabel14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[brnNameEn]")});
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UsePadding = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.WordWrap = false;
            // 
            // xrLabel15
            // 
            this.xrLabel15.AutoWidth = true;
            resources.ApplyResources(this.xrLabel15, "xrLabel15");
            this.xrLabel15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[brnTradeNo]")});
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UsePadding = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.WordWrap = false;
            // 
            // xrLabelBranchTrade2
            // 
            this.xrLabelBranchTrade2.AutoWidth = true;
            resources.ApplyResources(this.xrLabelBranchTrade2, "xrLabelBranchTrade2");
            this.xrLabelBranchTrade2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[brnTradeNo]")});
            this.xrLabelBranchTrade2.Name = "xrLabelBranchTrade2";
            this.xrLabelBranchTrade2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelBranchTrade2.StylePriority.UseFont = false;
            this.xrLabelBranchTrade2.StylePriority.UseTextAlignment = false;
            this.xrLabelBranchTrade2.WordWrap = false;
            // 
            // xrTable9
            // 
            resources.ApplyResources(this.xrTable9, "xrTable9");
            this.xrTable9.Name = "xrTable9";
            this.xrTable9.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow20});
            // 
            // xrTableRow20
            // 
            this.xrTableRow20.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell51,
            this.xrTableCell52});
            resources.ApplyResources(this.xrTableRow20, "xrTableRow20");
            this.xrTableRow20.Name = "xrTableRow20";
            // 
            // xrTableCell51
            // 
            resources.ApplyResources(this.xrTableCell51, "xrTableCell51");
            this.xrTableCell51.Multiline = true;
            this.xrTableCell51.Name = "xrTableCell51";
            this.xrTableCell51.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell51.StylePriority.UseFont = false;
            this.xrTableCell51.StylePriority.UseTextAlignment = false;
            this.xrTableCell51.WordWrap = false;
            // 
            // xrTableCell52
            // 
            resources.ApplyResources(this.xrTableCell52, "xrTableCell52");
            this.xrTableCell52.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[brnPhnNo]")});
            this.xrTableCell52.Multiline = true;
            this.xrTableCell52.Name = "xrTableCell52";
            this.xrTableCell52.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell52.StylePriority.UseFont = false;
            this.xrTableCell52.StylePriority.UseTextAlignment = false;
            this.xrTableCell52.WordWrap = false;
            // 
            // xrPictureBoxBranch
            // 
            resources.ApplyResources(this.xrPictureBoxBranch, "xrPictureBoxBranch");
            this.xrPictureBoxBranch.Name = "xrPictureBoxBranch";
            this.xrPictureBoxBranch.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrLabelBranchNameEn
            // 
            this.xrLabelBranchNameEn.AutoWidth = true;
            resources.ApplyResources(this.xrLabelBranchNameEn, "xrLabelBranchNameEn");
            this.xrLabelBranchNameEn.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[brnName]")});
            this.xrLabelBranchNameEn.Name = "xrLabelBranchNameEn";
            this.xrLabelBranchNameEn.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelBranchNameEn.StylePriority.UseFont = false;
            this.xrLabelBranchNameEn.StylePriority.UseTextAlignment = false;
            this.xrLabelBranchNameEn.WordWrap = false;
            // 
            // xrLabelBranchAddressEn
            // 
            this.xrLabelBranchAddressEn.CanShrink = true;
            resources.ApplyResources(this.xrLabelBranchAddressEn, "xrLabelBranchAddressEn");
            this.xrLabelBranchAddressEn.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[brnAddressEn]")});
            this.xrLabelBranchAddressEn.Multiline = true;
            this.xrLabelBranchAddressEn.Name = "xrLabelBranchAddressEn";
            this.xrLabelBranchAddressEn.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelBranchAddressEn.StylePriority.UseFont = false;
            this.xrLabelBranchAddressEn.StylePriority.UseTextAlignment = false;
            // 
            // xrLabelBranchMailBox2
            // 
            this.xrLabelBranchMailBox2.AutoWidth = true;
            resources.ApplyResources(this.xrLabelBranchMailBox2, "xrLabelBranchMailBox2");
            this.xrLabelBranchMailBox2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[brnMailBox]")});
            this.xrLabelBranchMailBox2.Name = "xrLabelBranchMailBox2";
            this.xrLabelBranchMailBox2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabelBranchMailBox2.StylePriority.UseFont = false;
            this.xrLabelBranchMailBox2.StylePriority.UseTextAlignment = false;
            this.xrLabelBranchMailBox2.WordWrap = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.BorderWidth = 2F;
            resources.ApplyResources(this.xrLabel1, "xrLabel1");
            this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[brnTaxNo]")});
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.StylePriority.UseBorderWidth = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UsePadding = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.WordWrap = false;
            // 
            // xrTable1
            // 
            resources.ApplyResources(this.xrTable1, "xrTable1");
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell2});
            resources.ApplyResources(this.xrTableRow1, "xrTableRow1");
            this.xrTableRow1.Name = "xrTableRow1";
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BorderWidth = 2F;
            resources.ApplyResources(this.xrTableCell1, "xrTableCell1");
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrTableCell1.StylePriority.UseBorderWidth = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.WordWrap = false;
            // 
            // xrTableCell2
            // 
            resources.ApplyResources(this.xrTableCell2, "xrTableCell2");
            this.xrTableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[brnTaxNo]")});
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.WordWrap = false;
            // 
            // TopMargin
            // 
            resources.ApplyResources(this.TopMargin, "TopMargin");
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            // 
            // BottomMargin
            // 
            resources.ApplyResources(this.BottomMargin, "BottomMargin");
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 254F);
            // 
            // baseControlStyle
            // 
            this.baseControlStyle.Font = new DevExpress.Drawing.DXFont("Segoe UI", 9.75F);
            this.baseControlStyle.Name = "baseControlStyle";
            this.baseControlStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            // 
            // evenDetailStyle
            // 
            this.evenDetailStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(233)))), ((int)(((byte)(234)))));
            this.evenDetailStyle.Name = "evenDetailStyle";
            this.evenDetailStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            // 
            // oddDetailStyle
            // 
            this.oddDetailStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.oddDetailStyle.Name = "oddDetailStyle";
            this.oddDetailStyle.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            // 
            // BranchbindingSource
            // 
            this.BranchbindingSource.DataSource = typeof(SewingSystem.LinqModel.tblBranche);
            // 
            // ReportHeaderWidth
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.BorderWidth = 2F;
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.BranchbindingSource});
            this.DataSource = this.BranchbindingSource;
            resources.ApplyResources(this, "$this");
            this.RightToLeft = DevExpress.XtraReports.UI.RightToLeft.Yes;
            this.RightToLeftLayout = DevExpress.XtraReports.UI.RightToLeftLayout.Yes;
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.baseControlStyle,
            this.evenDetailStyle,
            this.oddDetailStyle});
            this.Version = "22.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTable9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BranchbindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion
        
        private DevExpress.XtraReports.UI.DetailBand Detail;
        public DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRControlStyle baseControlStyle;
        private DevExpress.XtraReports.UI.XRControlStyle evenDetailStyle;
        private DevExpress.XtraReports.UI.XRControlStyle oddDetailStyle;
        public DevExpress.XtraReports.UI.XRPictureBox xrPictureBoxBranch;
        public DevExpress.XtraReports.UI.XRLabel xrLabelBranchNameEn;
        public DevExpress.XtraReports.UI.XRLabel xrLabelBranchAddressEn;
        public DevExpress.XtraReports.UI.XRLabel xrLabelBranchMailBox2;
        public DevExpress.XtraReports.UI.XRLabel xrLabelBranchTrade2;
        public DevExpress.XtraReports.UI.XRLabel xrLabel11;
        public DevExpress.XtraReports.UI.XRLabel xrLabel12;
        public DevExpress.XtraReports.UI.XRLabel xrLabel13;
        public DevExpress.XtraReports.UI.XRLabel xrLabel14;
        public DevExpress.XtraReports.UI.XRLabel xrLabel15;
        private DevExpress.XtraReports.UI.XRTable xrTable9;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow20;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell51;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell52;
        private System.Windows.Forms.BindingSource BranchbindingSource;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell2;
        private DevExpress.XtraReports.UI.XRLabel xrLabelReport;
    }
}
