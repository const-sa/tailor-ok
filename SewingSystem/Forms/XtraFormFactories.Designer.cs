namespace SewingSystem.Forms
{
    partial class XtraFormFactories
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraFormFactories));
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.tblFactorieBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTheNameAr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTheNameEn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEnterTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBranchID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.tblBranchesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.colUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.tblUsersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bindingNavigator7 = new System.Windows.Forms.BindingNavigator(this.components);
            this.AddNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton11 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton12 = new System.Windows.Forms.ToolStripButton();
            this.toolStripTextBox2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton13 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton15 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator31 = new System.Windows.Forms.ToolStripSeparator();
            this.Delete = new System.Windows.Forms.ToolStripButton();
            this.Save = new System.Windows.Forms.ToolStripButton();
            this.Print = new System.Windows.Forms.ToolStripButton();
            this.Refresh = new System.Windows.Forms.ToolStripButton();
            this.TheNameArTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.TheNameEnTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.NotesTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.Branch = new DevExpress.XtraEditors.LookUpEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.UpdateRecord = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForNotes = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForTheNameAr = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForTheNameEn = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem21 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblFactorieBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblBranchesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblUsersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator7)).BeginInit();
            this.bindingNavigator7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TheNameArTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TheNameEnTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Branch.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateRecord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForTheNameAr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForTheNameEn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem21)).BeginInit();
            this.SuspendLayout();
            // 
            // dataLayoutControl1
            // 
            resources.ApplyResources(this.dataLayoutControl1, "dataLayoutControl1");
            this.dataLayoutControl1.Controls.Add(this.gridControl1);
            this.dataLayoutControl1.Controls.Add(this.bindingNavigator7);
            this.dataLayoutControl1.Controls.Add(this.TheNameArTextEdit);
            this.dataLayoutControl1.Controls.Add(this.TheNameEnTextEdit);
            this.dataLayoutControl1.Controls.Add(this.NotesTextEdit);
            this.dataLayoutControl1.Controls.Add(this.Branch);
            this.dataLayoutControl1.DataSource = this.tblFactorieBindingSource;
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.OptionsView.RightToLeftMirroringApplied = true;
            this.dataLayoutControl1.Root = this.Root;
            // 
            // gridControl1
            // 
            resources.ApplyResources(this.gridControl1, "gridControl1");
            this.gridControl1.DataSource = this.tblFactorieBindingSource;
            this.gridControl1.EmbeddedNavigator.AccessibleDescription = resources.GetString("gridControl1.EmbeddedNavigator.AccessibleDescription");
            this.gridControl1.EmbeddedNavigator.AccessibleName = resources.GetString("gridControl1.EmbeddedNavigator.AccessibleName");
            this.gridControl1.EmbeddedNavigator.AllowHtmlTextInToolTip = ((DevExpress.Utils.DefaultBoolean)(resources.GetObject("gridControl1.EmbeddedNavigator.AllowHtmlTextInToolTip")));
            this.gridControl1.EmbeddedNavigator.Anchor = ((System.Windows.Forms.AnchorStyles)(resources.GetObject("gridControl1.EmbeddedNavigator.Anchor")));
            this.gridControl1.EmbeddedNavigator.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("gridControl1.EmbeddedNavigator.BackgroundImage")));
            this.gridControl1.EmbeddedNavigator.BackgroundImageLayout = ((System.Windows.Forms.ImageLayout)(resources.GetObject("gridControl1.EmbeddedNavigator.BackgroundImageLayout")));
            this.gridControl1.EmbeddedNavigator.ImeMode = ((System.Windows.Forms.ImeMode)(resources.GetObject("gridControl1.EmbeddedNavigator.ImeMode")));
            this.gridControl1.EmbeddedNavigator.Margin = ((System.Windows.Forms.Padding)(resources.GetObject("gridControl1.EmbeddedNavigator.Margin")));
            this.gridControl1.EmbeddedNavigator.MaximumSize = ((System.Drawing.Size)(resources.GetObject("gridControl1.EmbeddedNavigator.MaximumSize")));
            this.gridControl1.EmbeddedNavigator.TextLocation = ((DevExpress.XtraEditors.NavigatorButtonsTextLocation)(resources.GetObject("gridControl1.EmbeddedNavigator.TextLocation")));
            this.gridControl1.EmbeddedNavigator.ToolTip = resources.GetString("gridControl1.EmbeddedNavigator.ToolTip");
            this.gridControl1.EmbeddedNavigator.ToolTipIconType = ((DevExpress.Utils.ToolTipIconType)(resources.GetObject("gridControl1.EmbeddedNavigator.ToolTipIconType")));
            this.gridControl1.EmbeddedNavigator.ToolTipTitle = resources.GetString("gridControl1.EmbeddedNavigator.ToolTipTitle");
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemLookUpEdit2});
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // tblFactorieBindingSource
            // 
            this.tblFactorieBindingSource.DataSource = typeof(SewingSystem.LinqModel.tblFactorie);
            this.tblFactorieBindingSource.CurrentChanged += new System.EventHandler(this.tblFactoriesBindingSource_CurrentChanged);
            // 
            // gridView1
            // 
            resources.ApplyResources(this.gridView1, "gridView1");
            this.gridView1.Appearance.EvenRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.gridView1.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colTheNameAr,
            this.colTheNameEn,
            this.colNotes,
            this.colEnterTime,
            this.colBranchID,
            this.colUserID});
            this.gridView1.DetailHeight = 363;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
            // 
            // colID
            // 
            resources.ApplyResources(this.colID, "colID");
            this.colID.AppearanceCell.Options.UseTextOptions = true;
            this.colID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colID.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.colID.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("colID.AppearanceHeader.Font")));
            this.colID.AppearanceHeader.Options.UseBackColor = true;
            this.colID.AppearanceHeader.Options.UseFont = true;
            this.colID.AppearanceHeader.Options.UseTextOptions = true;
            this.colID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colID.FieldName = "ID";
            this.colID.MinWidth = 18;
            this.colID.Name = "colID";
            this.colID.OptionsColumn.AllowEdit = false;
            this.colID.OptionsColumn.AllowFocus = false;
            this.colID.OptionsFilter.AllowFilter = false;
            // 
            // colTheNameAr
            // 
            resources.ApplyResources(this.colTheNameAr, "colTheNameAr");
            this.colTheNameAr.AppearanceCell.Options.UseTextOptions = true;
            this.colTheNameAr.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTheNameAr.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.colTheNameAr.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("colTheNameAr.AppearanceHeader.Font")));
            this.colTheNameAr.AppearanceHeader.Options.UseBackColor = true;
            this.colTheNameAr.AppearanceHeader.Options.UseFont = true;
            this.colTheNameAr.AppearanceHeader.Options.UseTextOptions = true;
            this.colTheNameAr.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTheNameAr.FieldName = "TheNameAr";
            this.colTheNameAr.MinWidth = 18;
            this.colTheNameAr.Name = "colTheNameAr";
            this.colTheNameAr.OptionsColumn.AllowEdit = false;
            this.colTheNameAr.OptionsColumn.AllowFocus = false;
            this.colTheNameAr.OptionsFilter.AllowFilter = false;
            // 
            // colTheNameEn
            // 
            resources.ApplyResources(this.colTheNameEn, "colTheNameEn");
            this.colTheNameEn.AppearanceCell.Options.UseTextOptions = true;
            this.colTheNameEn.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTheNameEn.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.colTheNameEn.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("colTheNameEn.AppearanceHeader.Font")));
            this.colTheNameEn.AppearanceHeader.Options.UseBackColor = true;
            this.colTheNameEn.AppearanceHeader.Options.UseFont = true;
            this.colTheNameEn.AppearanceHeader.Options.UseTextOptions = true;
            this.colTheNameEn.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTheNameEn.FieldName = "TheNameEn";
            this.colTheNameEn.MinWidth = 18;
            this.colTheNameEn.Name = "colTheNameEn";
            this.colTheNameEn.OptionsColumn.AllowEdit = false;
            this.colTheNameEn.OptionsColumn.AllowFocus = false;
            this.colTheNameEn.OptionsFilter.AllowFilter = false;
            // 
            // colNotes
            // 
            resources.ApplyResources(this.colNotes, "colNotes");
            this.colNotes.AppearanceCell.Options.UseTextOptions = true;
            this.colNotes.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colNotes.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.colNotes.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("colNotes.AppearanceHeader.Font")));
            this.colNotes.AppearanceHeader.Options.UseBackColor = true;
            this.colNotes.AppearanceHeader.Options.UseFont = true;
            this.colNotes.AppearanceHeader.Options.UseTextOptions = true;
            this.colNotes.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colNotes.FieldName = "Notes";
            this.colNotes.MinWidth = 18;
            this.colNotes.Name = "colNotes";
            this.colNotes.OptionsColumn.AllowEdit = false;
            this.colNotes.OptionsColumn.AllowFocus = false;
            this.colNotes.OptionsFilter.AllowFilter = false;
            // 
            // colEnterTime
            // 
            resources.ApplyResources(this.colEnterTime, "colEnterTime");
            this.colEnterTime.AppearanceCell.Options.UseTextOptions = true;
            this.colEnterTime.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colEnterTime.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.colEnterTime.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("colEnterTime.AppearanceHeader.Font")));
            this.colEnterTime.AppearanceHeader.Options.UseBackColor = true;
            this.colEnterTime.AppearanceHeader.Options.UseFont = true;
            this.colEnterTime.AppearanceHeader.Options.UseTextOptions = true;
            this.colEnterTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colEnterTime.FieldName = "EnterTime";
            this.colEnterTime.MinWidth = 18;
            this.colEnterTime.Name = "colEnterTime";
            this.colEnterTime.OptionsColumn.AllowEdit = false;
            this.colEnterTime.OptionsColumn.AllowFocus = false;
            this.colEnterTime.OptionsFilter.AllowFilter = false;
            // 
            // colBranchID
            // 
            resources.ApplyResources(this.colBranchID, "colBranchID");
            this.colBranchID.AppearanceCell.Options.UseTextOptions = true;
            this.colBranchID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colBranchID.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.colBranchID.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("colBranchID.AppearanceHeader.Font")));
            this.colBranchID.AppearanceHeader.Options.UseBackColor = true;
            this.colBranchID.AppearanceHeader.Options.UseFont = true;
            this.colBranchID.AppearanceHeader.Options.UseTextOptions = true;
            this.colBranchID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colBranchID.ColumnEdit = this.repositoryItemLookUpEdit2;
            this.colBranchID.FieldName = "BranchID";
            this.colBranchID.MinWidth = 18;
            this.colBranchID.Name = "colBranchID";
            this.colBranchID.OptionsColumn.AllowEdit = false;
            this.colBranchID.OptionsColumn.AllowFocus = false;
            this.colBranchID.OptionsFilter.AllowFilter = false;
            // 
            // repositoryItemLookUpEdit2
            // 
            resources.ApplyResources(this.repositoryItemLookUpEdit2, "repositoryItemLookUpEdit2");
            this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("repositoryItemLookUpEdit2.Buttons"))))});
            this.repositoryItemLookUpEdit2.DataSource = this.tblBranchesBindingSource;
            this.repositoryItemLookUpEdit2.DisplayMember = "BranchName";
            this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            this.repositoryItemLookUpEdit2.ValueMember = "ID";
            // 
            // tblBranchesBindingSource
            // 
            this.tblBranchesBindingSource.DataSource = typeof(SewingSystem.LinqModel.tblBranche);
            // 
            // colUserID
            // 
            resources.ApplyResources(this.colUserID, "colUserID");
            this.colUserID.AppearanceCell.Options.UseTextOptions = true;
            this.colUserID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUserID.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.colUserID.AppearanceHeader.Font = ((System.Drawing.Font)(resources.GetObject("colUserID.AppearanceHeader.Font")));
            this.colUserID.AppearanceHeader.Options.UseBackColor = true;
            this.colUserID.AppearanceHeader.Options.UseFont = true;
            this.colUserID.AppearanceHeader.Options.UseTextOptions = true;
            this.colUserID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUserID.ColumnEdit = this.repositoryItemLookUpEdit1;
            this.colUserID.FieldName = "UserID";
            this.colUserID.MinWidth = 18;
            this.colUserID.Name = "colUserID";
            this.colUserID.OptionsColumn.AllowEdit = false;
            this.colUserID.OptionsColumn.AllowFocus = false;
            this.colUserID.OptionsFilter.AllowFilter = false;
            // 
            // repositoryItemLookUpEdit1
            // 
            resources.ApplyResources(this.repositoryItemLookUpEdit1, "repositoryItemLookUpEdit1");
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("repositoryItemLookUpEdit1.Buttons"))))});
            this.repositoryItemLookUpEdit1.DataSource = this.tblUsersBindingSource;
            this.repositoryItemLookUpEdit1.DisplayMember = "UserName";
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.ValueMember = "ID";
            // 
            // tblUsersBindingSource
            // 
            this.tblUsersBindingSource.DataSource = typeof(SewingSystem.LinqModel.tblUser);
            // 
            // bindingNavigator7
            // 
            resources.ApplyResources(this.bindingNavigator7, "bindingNavigator7");
            this.bindingNavigator7.AddNewItem = this.AddNew;
            this.bindingNavigator7.BindingSource = this.tblFactorieBindingSource;
            this.bindingNavigator7.CountItem = this.toolStripLabel2;
            this.bindingNavigator7.DeleteItem = null;
            this.bindingNavigator7.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.bindingNavigator7.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator5,
            this.toolStripButton11,
            this.toolStripButton12,
            this.toolStripTextBox2,
            this.toolStripLabel2,
            this.toolStripSeparator6,
            this.toolStripButton13,
            this.toolStripButton15,
            this.toolStripSeparator31,
            this.AddNew,
            this.Delete,
            this.Save,
            this.Print,
            this.Refresh});
            this.bindingNavigator7.MoveFirstItem = this.toolStripButton11;
            this.bindingNavigator7.MoveLastItem = this.toolStripButton15;
            this.bindingNavigator7.MoveNextItem = this.toolStripButton13;
            this.bindingNavigator7.MovePreviousItem = this.toolStripButton12;
            this.bindingNavigator7.Name = "bindingNavigator7";
            this.bindingNavigator7.PositionItem = this.toolStripTextBox2;
            // 
            // AddNew
            // 
            resources.ApplyResources(this.AddNew, "AddNew");
            this.AddNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.AddNew.Image = global::SewingSystem.Properties.Resources.add_32x32;
            this.AddNew.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.AddNew.Name = "AddNew";
            this.AddNew.Click += new System.EventHandler(this.AddNew_Click);
            // 
            // toolStripLabel2
            // 
            resources.ApplyResources(this.toolStripLabel2, "toolStripLabel2");
            this.toolStripLabel2.Name = "toolStripLabel2";
            // 
            // toolStripSeparator5
            // 
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // toolStripButton11
            // 
            resources.ApplyResources(this.toolStripButton11, "toolStripButton11");
            this.toolStripButton11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton11.Name = "toolStripButton11";
            // 
            // toolStripButton12
            // 
            resources.ApplyResources(this.toolStripButton12, "toolStripButton12");
            this.toolStripButton12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton12.Name = "toolStripButton12";
            // 
            // toolStripTextBox2
            // 
            resources.ApplyResources(this.toolStripTextBox2, "toolStripTextBox2");
            this.toolStripTextBox2.Name = "toolStripTextBox2";
            // 
            // toolStripSeparator6
            // 
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            // 
            // toolStripButton13
            // 
            resources.ApplyResources(this.toolStripButton13, "toolStripButton13");
            this.toolStripButton13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton13.Name = "toolStripButton13";
            // 
            // toolStripButton15
            // 
            resources.ApplyResources(this.toolStripButton15, "toolStripButton15");
            this.toolStripButton15.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton15.Name = "toolStripButton15";
            // 
            // toolStripSeparator31
            // 
            resources.ApplyResources(this.toolStripSeparator31, "toolStripSeparator31");
            this.toolStripSeparator31.Name = "toolStripSeparator31";
            // 
            // Delete
            // 
            resources.ApplyResources(this.Delete, "Delete");
            this.Delete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Delete.Image = global::SewingSystem.Properties.Resources.cleartablestyle_32x321;
            this.Delete.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.Delete.Name = "Delete";
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // Save
            // 
            resources.ApplyResources(this.Save, "Save");
            this.Save.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Save.Image = global::SewingSystem.Properties.Resources.save_32x321;
            this.Save.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.Save.Name = "Save";
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Print
            // 
            resources.ApplyResources(this.Print, "Print");
            this.Print.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Print.Image = global::SewingSystem.Properties.Resources.printer_32x32;
            this.Print.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.Print.Name = "Print";
            this.Print.Click += new System.EventHandler(this.Print_Click);
            // 
            // Refresh
            // 
            resources.ApplyResources(this.Refresh, "Refresh");
            this.Refresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Refresh.Image = global::SewingSystem.Properties.Resources.refresh_32x32;
            this.Refresh.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.Refresh.Name = "Refresh";
            this.Refresh.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // TheNameArTextEdit
            // 
            resources.ApplyResources(this.TheNameArTextEdit, "TheNameArTextEdit");
            this.TheNameArTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.tblFactorieBindingSource, "TheNameAr", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TheNameArTextEdit.Name = "TheNameArTextEdit";
            this.TheNameArTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.TheNameArTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.TheNameArTextEdit.StyleController = this.dataLayoutControl1;
            // 
            // TheNameEnTextEdit
            // 
            resources.ApplyResources(this.TheNameEnTextEdit, "TheNameEnTextEdit");
            this.TheNameEnTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.tblFactorieBindingSource, "TheNameEn", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TheNameEnTextEdit.Name = "TheNameEnTextEdit";
            this.TheNameEnTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.TheNameEnTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.TheNameEnTextEdit.StyleController = this.dataLayoutControl1;
            // 
            // NotesTextEdit
            // 
            resources.ApplyResources(this.NotesTextEdit, "NotesTextEdit");
            this.NotesTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.tblFactorieBindingSource, "Notes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.NotesTextEdit.Name = "NotesTextEdit";
            this.NotesTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.NotesTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.NotesTextEdit.StyleController = this.dataLayoutControl1;
            // 
            // Branch
            // 
            resources.ApplyResources(this.Branch, "Branch");
            this.Branch.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.tblFactorieBindingSource, "BranchID", true));
            this.Branch.Name = "Branch";
            this.Branch.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("Branch.Properties.Buttons"))))});
            this.Branch.Properties.DataSource = this.tblBranchesBindingSource;
            this.Branch.Properties.DisplayMember = "BranchName";
            this.Branch.Properties.NullText = resources.GetString("Branch.Properties.NullText");
            this.Branch.Properties.PopupSizeable = false;
            this.Branch.Properties.ValueMember = "ID";
            this.Branch.StyleController = this.dataLayoutControl1;
            // 
            // Root
            // 
            resources.ApplyResources(this.Root, "Root");
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem1,
            this.layoutControlGroup1,
            this.UpdateRecord});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(877, 501);
            this.Root.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            resources.ApplyResources(this.layoutControlItem1, "layoutControlItem1");
            this.layoutControlItem1.Control = this.bindingNavigator7;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem1.MaxSize = new System.Drawing.Size(0, 43);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(81, 43);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(857, 43);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            resources.ApplyResources(this.layoutControlGroup1, "layoutControlGroup1");
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem2});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 140);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(857, 341);
            // 
            // layoutControlItem2
            // 
            resources.ApplyResources(this.layoutControlItem2, "layoutControlItem2");
            this.layoutControlItem2.Control = this.gridControl1;
            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(857, 341);
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // UpdateRecord
            // 
            resources.ApplyResources(this.UpdateRecord, "UpdateRecord");
            this.UpdateRecord.DataBindings.Add(new System.Windows.Forms.Binding("Enabled", global::SewingSystem.Properties.Settings.Default, "المعلمين_تعديل", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.UpdateRecord.Enabled = global::SewingSystem.Properties.Settings.Default.المعلمين_تعديل;
            this.UpdateRecord.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForNotes,
            this.ItemForTheNameAr,
            this.ItemForTheNameEn,
            this.layoutControlItem21});
            this.UpdateRecord.Location = new System.Drawing.Point(0, 43);
            this.UpdateRecord.Name = "UpdateRecord";
            this.UpdateRecord.Size = new System.Drawing.Size(857, 97);
            // 
            // ItemForNotes
            // 
            resources.ApplyResources(this.ItemForNotes, "ItemForNotes");
            this.ItemForNotes.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForNotes.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForNotes.Control = this.NotesTextEdit;
            this.ItemForNotes.Location = new System.Drawing.Point(0, 26);
            this.ItemForNotes.Name = "ItemForNotes";
            this.ItemForNotes.Size = new System.Drawing.Size(833, 26);
            this.ItemForNotes.TextSize = new System.Drawing.Size(69, 16);
            // 
            // ItemForTheNameAr
            // 
            resources.ApplyResources(this.ItemForTheNameAr, "ItemForTheNameAr");
            this.ItemForTheNameAr.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForTheNameAr.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForTheNameAr.Control = this.TheNameArTextEdit;
            this.ItemForTheNameAr.Location = new System.Drawing.Point(496, 0);
            this.ItemForTheNameAr.Name = "ItemForTheNameAr";
            this.ItemForTheNameAr.Size = new System.Drawing.Size(337, 26);
            this.ItemForTheNameAr.TextSize = new System.Drawing.Size(69, 16);
            // 
            // ItemForTheNameEn
            // 
            resources.ApplyResources(this.ItemForTheNameEn, "ItemForTheNameEn");
            this.ItemForTheNameEn.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForTheNameEn.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForTheNameEn.Control = this.TheNameEnTextEdit;
            this.ItemForTheNameEn.Location = new System.Drawing.Point(242, 0);
            this.ItemForTheNameEn.Name = "ItemForTheNameEn";
            this.ItemForTheNameEn.Size = new System.Drawing.Size(254, 26);
            this.ItemForTheNameEn.TextSize = new System.Drawing.Size(69, 16);
            // 
            // layoutControlItem21
            // 
            resources.ApplyResources(this.layoutControlItem21, "layoutControlItem21");
            this.layoutControlItem21.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem21.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem21.AppearanceItemCaptionDisabled.Font = ((System.Drawing.Font)(resources.GetObject("layoutControlItem21.AppearanceItemCaptionDisabled.Font")));
            this.layoutControlItem21.AppearanceItemCaptionDisabled.Options.UseFont = true;
            this.layoutControlItem21.Control = this.Branch;
            this.layoutControlItem21.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem21.Name = "layoutControlItem21";
            this.layoutControlItem21.OptionsPrint.AppearanceItem.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font")));
            this.layoutControlItem21.OptionsPrint.AppearanceItem.Options.UseFont = true;
            this.layoutControlItem21.OptionsPrint.AppearanceItemControl.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font1")));
            this.layoutControlItem21.OptionsPrint.AppearanceItemControl.Options.UseFont = true;
            this.layoutControlItem21.OptionsPrint.AppearanceItemText.Font = ((System.Drawing.Font)(resources.GetObject("resource.Font2")));
            this.layoutControlItem21.OptionsPrint.AppearanceItemText.Options.UseFont = true;
            this.layoutControlItem21.Size = new System.Drawing.Size(242, 26);
            this.layoutControlItem21.TextSize = new System.Drawing.Size(69, 13);
            // 
            // XtraFormFactories
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataLayoutControl1);
            this.IconOptions.LargeImage = global::SewingSystem.Properties.Resources.clip_32x32;
            this.Name = "XtraFormFactories";
            this.Load += new System.EventHandler(this.XtraFormFactories_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblFactorieBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblBranchesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblUsersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator7)).EndInit();
            this.bindingNavigator7.ResumeLayout(false);
            this.bindingNavigator7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TheNameArTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TheNameEnTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Branch.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateRecord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForTheNameAr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForTheNameEn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem21)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private System.Windows.Forms.BindingSource tblFactorieBindingSource;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colTheNameAr;
        private DevExpress.XtraGrid.Columns.GridColumn colTheNameEn;
        private DevExpress.XtraGrid.Columns.GridColumn colNotes;
        private DevExpress.XtraGrid.Columns.GridColumn colEnterTime;
        private DevExpress.XtraGrid.Columns.GridColumn colBranchID;
        private DevExpress.XtraGrid.Columns.GridColumn colUserID;
        private System.Windows.Forms.BindingNavigator bindingNavigator7;
        private System.Windows.Forms.ToolStripButton AddNew;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton11;
        private System.Windows.Forms.ToolStripButton toolStripButton12;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton toolStripButton13;
        private System.Windows.Forms.ToolStripButton toolStripButton15;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator31;
        private System.Windows.Forms.ToolStripButton Delete;
        private System.Windows.Forms.ToolStripButton Save;
        private System.Windows.Forms.ToolStripButton Print;
        private System.Windows.Forms.ToolStripButton Refresh;
        private DevExpress.XtraEditors.TextEdit TheNameArTextEdit;
        private DevExpress.XtraEditors.TextEdit TheNameEnTextEdit;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlGroup UpdateRecord;
        private DevExpress.XtraLayout.LayoutControlItem ItemForNotes;
        private DevExpress.XtraLayout.LayoutControlItem ItemForTheNameEn;
        private DevExpress.XtraLayout.LayoutControlItem ItemForTheNameAr;
        private DevExpress.XtraEditors.TextEdit NotesTextEdit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private System.Windows.Forms.BindingSource tblBranchesBindingSource;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private System.Windows.Forms.BindingSource tblUsersBindingSource;
        private DevExpress.XtraEditors.LookUpEdit Branch;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem21;
    }
}