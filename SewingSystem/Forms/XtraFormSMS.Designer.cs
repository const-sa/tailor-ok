namespace SewingSystem.Forms
{
    partial class XtraFormSMS
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraFormSMS));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.txtMobil = new DevExpress.XtraEditors.TextEdit();
            this.BtnSend = new DevExpress.XtraEditors.SimpleButton();
            this.txtMessage = new DevExpress.XtraEditors.MemoEdit();
            this.searchLookUpEditCustomer = new DevExpress.XtraEditors.SearchLookUpEdit();
            this.tblCustomerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.searchLookUpEdit1View2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn111 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit11 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.BtnSend1 = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSendWhatsApp = new DevExpress.XtraEditors.SimpleButton();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem11 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.layoutControlItem7 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
            this.layoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtMobil.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEditCustomer.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblCustomerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControl1
            // 
            this.layoutControl1.Appearance.Control.Font = ((System.Drawing.Font)(resources.GetObject("layoutControl1.Appearance.Control.Font")));
            this.layoutControl1.Appearance.Control.Options.UseFont = true;
            this.layoutControl1.Controls.Add(this.labelControl1);
            this.layoutControl1.Controls.Add(this.simpleButton1);
            this.layoutControl1.Controls.Add(this.txtMobil);
            this.layoutControl1.Controls.Add(this.BtnSend);
            this.layoutControl1.Controls.Add(this.txtMessage);
            this.layoutControl1.Controls.Add(this.searchLookUpEditCustomer);
            this.layoutControl1.Controls.Add(this.BtnSend1);
            this.layoutControl1.Controls.Add(this.BtnSendWhatsApp);
            resources.ApplyResources(this.layoutControl1, "layoutControl1");
            this.layoutControl1.Name = "layoutControl1";
            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(634, 89, 650, 400);
            this.layoutControl1.OptionsView.RightToLeftMirroringApplied = true;
            this.layoutControl1.Root = this.Root;
            // 
            // simpleButton1
            // 
            this.simpleButton1.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.ImageOptions.Image")));
            resources.ApplyResources(this.simpleButton1, "simpleButton1");
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.StyleController = this.layoutControl1;
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // txtMobil
            // 
            resources.ApplyResources(this.txtMobil, "txtMobil");
            this.txtMobil.Name = "txtMobil";
            this.txtMobil.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txtMobil.Properties.MaskSettings.Set("mask", "d");
            this.txtMobil.StyleController = this.layoutControl1;
            // 
            // BtnSend
            // 
            this.BtnSend.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("BtnSend.Appearance.Font")));
            this.BtnSend.Appearance.Options.UseFont = true;
            this.BtnSend.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSend.ImageOptions.SvgImage")));
            resources.ApplyResources(this.BtnSend, "BtnSend");
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.StyleController = this.layoutControl1;
            this.BtnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // txtMessage
            // 
            resources.ApplyResources(this.txtMessage, "txtMessage");
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.StyleController = this.layoutControl1;
            // 
            // searchLookUpEditCustomer
            // 
            resources.ApplyResources(this.searchLookUpEditCustomer, "searchLookUpEditCustomer");
            this.searchLookUpEditCustomer.Name = "searchLookUpEditCustomer";
            this.searchLookUpEditCustomer.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.searchLookUpEditCustomer.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("searchLookUpEditCustomer.Properties.Buttons"))))});
            this.searchLookUpEditCustomer.Properties.DataSource = this.tblCustomerBindingSource;
            this.searchLookUpEditCustomer.Properties.DisplayMember = "CustomerName";
            this.searchLookUpEditCustomer.Properties.NullText = resources.GetString("searchLookUpEditCustomer.Properties.NullText");
            this.searchLookUpEditCustomer.Properties.PopupView = this.searchLookUpEdit1View2;
            this.searchLookUpEditCustomer.Properties.ValueMember = "CusNumber";
            this.searchLookUpEditCustomer.StyleController = this.layoutControl1;
            this.searchLookUpEditCustomer.EditValueChanged += new System.EventHandler(this.searchLookUpEditCustomer_EditValueChanged);
            // 
            // tblCustomerBindingSource
            // 
            this.tblCustomerBindingSource.DataSource = typeof(SewingSystem.LinqModel.tblCustomer);
            // 
            // searchLookUpEdit1View2
            // 
            this.searchLookUpEdit1View2.Appearance.EvenRow.BackColor = System.Drawing.Color.AliceBlue;
            this.searchLookUpEdit1View2.Appearance.EvenRow.Options.UseBackColor = true;
            this.searchLookUpEdit1View2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn11,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn6,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn111,
            this.gridColumn12,
            this.gridColumn13});
            this.searchLookUpEdit1View2.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            gridFormatRule1.Name = "Format0";
            gridFormatRule1.Rule = null;
            gridFormatRule1.Tag = ((long)(0));
            this.searchLookUpEdit1View2.FormatRules.Add(gridFormatRule1);
            this.searchLookUpEdit1View2.Name = "searchLookUpEdit1View2";
            this.searchLookUpEdit1View2.OptionsLayout.StoreFormatRules = true;
            this.searchLookUpEdit1View2.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.searchLookUpEdit1View2.OptionsView.EnableAppearanceEvenRow = true;
            this.searchLookUpEdit1View2.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn11.AppearanceCell.Font")));
            this.gridColumn11.AppearanceCell.Options.UseFont = true;
            this.gridColumn11.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn11.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.gridColumn11.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn11.AppearanceHeader.Font")));
            this.gridColumn11.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.gridColumn11.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn11.AppearanceHeader.Options.UseFont = true;
            this.gridColumn11.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn11.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.gridColumn11, "gridColumn11");
            this.gridColumn11.FieldName = "CusNumber";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn3.AppearanceCell.Font")));
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.gridColumn3.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn3.AppearanceHeader.Font")));
            this.gridColumn3.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.gridColumn3.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.gridColumn3, "gridColumn3");
            this.gridColumn3.FieldName = "CustomerName";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn4.AppearanceCell.Font")));
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.gridColumn4.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn4.AppearanceHeader.Font")));
            this.gridColumn4.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.gridColumn4.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.gridColumn4, "gridColumn4");
            this.gridColumn4.DisplayFormat.FormatString = "d";
            this.gridColumn4.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn4.FieldName = "Telephone";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn6.AppearanceCell.Font")));
            this.gridColumn6.AppearanceCell.Options.UseFont = true;
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.gridColumn6.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn6.AppearanceHeader.Font")));
            this.gridColumn6.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.gridColumn6.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.gridColumn6, "gridColumn6");
            this.gridColumn6.DisplayFormat.FormatString = "d";
            this.gridColumn6.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn6.FieldName = "Mobil2";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn9.AppearanceCell.Font")));
            this.gridColumn9.AppearanceCell.Options.UseFont = true;
            this.gridColumn9.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn9.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.gridColumn9.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn9.AppearanceHeader.Font")));
            this.gridColumn9.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.gridColumn9.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn9.AppearanceHeader.Options.UseFont = true;
            this.gridColumn9.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn9.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.gridColumn9, "gridColumn9");
            this.gridColumn9.DisplayFormat.FormatString = "d";
            this.gridColumn9.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn9.FieldName = "Mobil";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn10.AppearanceCell.Font")));
            this.gridColumn10.AppearanceCell.Options.UseFont = true;
            this.gridColumn10.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn10.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.gridColumn10.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn10.AppearanceHeader.Font")));
            this.gridColumn10.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.gridColumn10.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn10.AppearanceHeader.Options.UseFont = true;
            this.gridColumn10.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn10.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.gridColumn10, "gridColumn10");
            this.gridColumn10.FieldName = "Telephone";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn111
            // 
            this.gridColumn111.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn111.AppearanceCell.Font")));
            this.gridColumn111.AppearanceCell.Options.UseFont = true;
            this.gridColumn111.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn111.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn111.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.gridColumn111.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn111.AppearanceHeader.Font")));
            this.gridColumn111.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.gridColumn111.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn111.AppearanceHeader.Options.UseFont = true;
            this.gridColumn111.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn111.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn111.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.gridColumn111, "gridColumn111");
            this.gridColumn111.FieldName = "Telephone";
            this.gridColumn111.Name = "gridColumn111";
            this.gridColumn111.OptionsColumn.AllowEdit = false;
            this.gridColumn111.OptionsColumn.AllowFocus = false;
            // 
            // gridColumn12
            // 
            this.gridColumn12.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn12.AppearanceCell.Font")));
            this.gridColumn12.AppearanceCell.Options.UseFont = true;
            this.gridColumn12.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn12.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.gridColumn12.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn12.AppearanceHeader.Font")));
            this.gridColumn12.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.gridColumn12.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn12.AppearanceHeader.Options.UseFont = true;
            this.gridColumn12.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn12.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.gridColumn12, "gridColumn12");
            this.gridColumn12.ColumnEdit = this.repositoryItemLookUpEdit11;
            this.gridColumn12.FieldName = "UserID";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.OptionsColumn.AllowEdit = false;
            this.gridColumn12.OptionsColumn.AllowFocus = false;
            // 
            // repositoryItemLookUpEdit11
            // 
            resources.ApplyResources(this.repositoryItemLookUpEdit11, "repositoryItemLookUpEdit11");
            this.repositoryItemLookUpEdit11.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("repositoryItemLookUpEdit11.Buttons"))))});
            this.repositoryItemLookUpEdit11.DisplayMember = "CustomerName";
            this.repositoryItemLookUpEdit11.Name = "repositoryItemLookUpEdit11";
            this.repositoryItemLookUpEdit11.ValueMember = "CusNumber";
            // 
            // gridColumn13
            // 
            this.gridColumn13.AppearanceCell.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn13.AppearanceCell.Font")));
            this.gridColumn13.AppearanceCell.Options.UseFont = true;
            this.gridColumn13.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn13.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.gridColumn13.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("gridColumn13.AppearanceHeader.Font")));
            this.gridColumn13.AppearanceHeader.ForeColor = System.Drawing.Color.White;
            this.gridColumn13.AppearanceHeader.Options.UseBackColor = true;
            this.gridColumn13.AppearanceHeader.Options.UseFont = true;
            this.gridColumn13.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn13.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.gridColumn13, "gridColumn13");
            this.gridColumn13.FieldName = "EnterTime";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.AllowEdit = false;
            this.gridColumn13.OptionsColumn.AllowFocus = false;
            // 
            // BtnSend1
            // 
            this.BtnSend1.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("BtnSend1.Appearance.Font")));
            this.BtnSend1.Appearance.Options.UseFont = true;
            this.BtnSend1.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSend1.ImageOptions.SvgImage")));
            resources.ApplyResources(this.BtnSend1, "BtnSend1");
            this.BtnSend1.Name = "BtnSend1";
            this.BtnSend1.StyleController = this.layoutControl1;
            this.BtnSend1.Click += new System.EventHandler(this.BtnSend1_Click);
            // 
            // BtnSendWhatsApp
            // 
            this.BtnSendWhatsApp.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("BtnSendWhatsApp.Appearance.Font")));
            this.BtnSendWhatsApp.Appearance.Options.UseFont = true;
            this.BtnSendWhatsApp.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSendWhatsApp.ImageOptions.SvgImage")));
            resources.ApplyResources(this.BtnSendWhatsApp, "BtnSendWhatsApp");
            this.BtnSendWhatsApp.Name = "BtnSendWhatsApp";
            this.BtnSendWhatsApp.StyleController = this.layoutControl1;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(548, 417);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem4,
            this.layoutControlItem11,
            this.layoutControlItem2,
            this.layoutControlItem1,
            this.layoutControlItem3,
            this.layoutControlItem5,
            this.layoutControlItem6,
            this.layoutControlItem7});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Size = new System.Drawing.Size(532, 401);
            resources.ApplyResources(this.layoutControlGroup1, "layoutControlGroup1");
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.txtMobil;
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 74);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(512, 48);
            resources.ApplyResources(this.layoutControlItem4, "layoutControlItem4");
            this.layoutControlItem4.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(68, 16);
            // 
            // layoutControlItem11
            // 
            this.layoutControlItem11.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem11.Control = this.searchLookUpEditCustomer;
            resources.ApplyResources(this.layoutControlItem11, "layoutControlItem11");
            this.layoutControlItem11.Location = new System.Drawing.Point(0, 21);
            this.layoutControlItem11.Name = "layoutControlItem11";
            this.layoutControlItem11.OptionsPrint.AppearanceItem.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font")));
            this.layoutControlItem11.OptionsPrint.AppearanceItem.Options.UseFont = true;
            this.layoutControlItem11.OptionsPrint.AppearanceItemControl.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font1")));
            this.layoutControlItem11.OptionsPrint.AppearanceItemControl.Options.UseFont = true;
            this.layoutControlItem11.OptionsPrint.AppearanceItemText.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font2")));
            this.layoutControlItem11.OptionsPrint.AppearanceItemText.Options.UseFont = true;
            this.layoutControlItem11.Size = new System.Drawing.Size(512, 53);
            this.layoutControlItem11.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem11.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem11.TextSize = new System.Drawing.Size(69, 16);
            this.layoutControlItem11.TextToControlDistance = 5;
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.BtnSend;
            this.layoutControlItem2.Location = new System.Drawing.Point(252, 217);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(92, 40);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(260, 70);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.Spacing = new DevExpress.XtraLayout.Utils.Padding(4, 4, 0, 0);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.txtMessage;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 122);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(512, 95);
            resources.ApplyResources(this.layoutControlItem1, "layoutControlItem1");
            this.layoutControlItem1.TextLocation = DevExpress.Utils.Locations.Top;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(68, 16);
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.Control = this.simpleButton1;
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 217);
            this.layoutControlItem3.MinSize = new System.Drawing.Size(73, 34);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.Size = new System.Drawing.Size(252, 70);
            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem3.Spacing = new DevExpress.XtraLayout.Utils.Padding(4, 4, 0, 0);
            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.Control = this.BtnSend1;
            resources.ApplyResources(this.layoutControlItem5, "layoutControlItem5");
            this.layoutControlItem5.Location = new System.Drawing.Point(252, 287);
            this.layoutControlItem5.MinSize = new System.Drawing.Size(92, 40);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(260, 71);
            this.layoutControlItem5.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem5.Spacing = new DevExpress.XtraLayout.Utils.Padding(4, 4, 0, 0);
            this.layoutControlItem5.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem5.TextVisible = false;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.BtnSendWhatsApp;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 287);
            this.layoutControlItem6.MinSize = new System.Drawing.Size(92, 40);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(252, 71);
            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem6.Spacing = new DevExpress.XtraLayout.Utils.Padding(4, 4, 0, 0);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("labelControl1.Appearance.Font")));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            resources.ApplyResources(this.labelControl1, "labelControl1");
            this.labelControl1.LineLocation = DevExpress.XtraEditors.LineLocation.Center;
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.StyleController = this.layoutControl1;
            // 
            // layoutControlItem7
            // 
            this.layoutControlItem7.Control = this.labelControl1;
            this.layoutControlItem7.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem7.Name = "layoutControlItem7";
            this.layoutControlItem7.Size = new System.Drawing.Size(512, 21);
            this.layoutControlItem7.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem7.TextVisible = false;
            // 
            // XtraFormSMS
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.layoutControl1);
            this.Name = "XtraFormSMS";
            this.Load += new System.EventHandler(this.XtraFormSMS_Load);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
            this.layoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtMobil.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMessage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEditCustomer.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblCustomerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.searchLookUpEdit1View2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem7)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControl1;
        private DevExpress.XtraEditors.SimpleButton BtnSend;
        private DevExpress.XtraEditors.MemoEdit txtMessage;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraEditors.TextEdit txtMobil;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.SearchLookUpEdit searchLookUpEditCustomer;
        private System.Windows.Forms.BindingSource tblCustomerBindingSource;
        private DevExpress.XtraGrid.Views.Grid.GridView searchLookUpEdit1View2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn111;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem11;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private DevExpress.XtraEditors.SimpleButton BtnSend1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraEditors.SimpleButton BtnSendWhatsApp;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem7;
    }
}