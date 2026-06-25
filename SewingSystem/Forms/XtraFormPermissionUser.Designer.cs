namespace SewingSystem.Forms
{
    partial class XtraFormPermissionUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraFormPermissionUser));
            this.tblPermissionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.UserGroupBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Permission_dataLayoutCon = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.checkButton1 = new DevExpress.XtraEditors.CheckButton();
            this.Permission_treeList = new DevExpress.XtraTreeList.TreeList();
            this.colObjectName1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colTheValues1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colPermissionName1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.colUserGroupID1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.repositoryItemLookUpEdit7 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton8 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator22 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox8 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator23 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton14 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton21 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator24 = new System.Windows.Forms.ToolStripSeparator();
            this.PermissionSave = new System.Windows.Forms.ToolStripButton();
            this.Print_Permission = new System.Windows.Forms.ToolStripButton();
            this.RefreachPermission = new System.Windows.Forms.ToolStripButton();
            this.UserGroup_lookUpEdit = new DevExpress.XtraEditors.MemoEdit();
            this.layoutControlGroup33 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup34 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem37 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup38 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem46 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlGroup75 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlItem51 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem48 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.tblPermissionsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserGroupBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Permission_dataLayoutCon)).BeginInit();
            this.Permission_dataLayoutCon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Permission_treeList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UserGroup_lookUpEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup33)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup34)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem37)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup38)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem46)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup75)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem51)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem48)).BeginInit();
            this.SuspendLayout();
            // 
            // tblPermissionsBindingSource
            // 
            this.tblPermissionsBindingSource.DataSource = typeof(SewingSystem.LinqModel.tblPermission);
            // 
            // UserGroupBindingSource
            // 
            this.UserGroupBindingSource.DataSource = typeof(SewingSystem.LinqModel.tblUserGroup);
            this.UserGroupBindingSource.CurrentChanged += new System.EventHandler(this.UserGroupBindingSource_CurrentChanged);
            // 
            // Permission_dataLayoutCon
            // 
            this.Permission_dataLayoutCon.AllowGeneratingNestedGroups = DevExpress.Utils.DefaultBoolean.True;
            this.Permission_dataLayoutCon.Controls.Add(this.checkButton1);
            this.Permission_dataLayoutCon.Controls.Add(this.Permission_treeList);
            this.Permission_dataLayoutCon.Controls.Add(this.bindingNavigator1);
            this.Permission_dataLayoutCon.Controls.Add(this.UserGroup_lookUpEdit);
            this.Permission_dataLayoutCon.CustomizationMode = DevExpress.XtraLayout.CustomizationModes.Quick;
            this.Permission_dataLayoutCon.DataSource = this.UserGroupBindingSource;
            resources.ApplyResources(this.Permission_dataLayoutCon, "Permission_dataLayoutCon");
            this.Permission_dataLayoutCon.Name = "Permission_dataLayoutCon";
            this.Permission_dataLayoutCon.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(635, 211, 650, 400);
            this.Permission_dataLayoutCon.OptionsFocus.MoveFocusDirection = DevExpress.XtraLayout.MoveFocusDirection.DownThenAcross;
            this.Permission_dataLayoutCon.OptionsView.RightToLeftMirroringApplied = true;
            this.Permission_dataLayoutCon.Root = this.layoutControlGroup33;
            // 
            // checkButton1
            // 
            this.checkButton1.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("checkButton1.Appearance.Font")));
            this.checkButton1.Appearance.Options.UseFont = true;
            this.checkButton1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.checkButton1.ImageOptions.Image = global::SewingSystem.Properties.Resources.bopermission_32x32;
            resources.ApplyResources(this.checkButton1, "checkButton1");
            this.checkButton1.Name = "checkButton1";
            this.checkButton1.StyleController = this.Permission_dataLayoutCon;
            this.checkButton1.CheckedChanged += new System.EventHandler(this.checkButton1_CheckedChanged);
            // 
            // Permission_treeList
            // 
            this.Permission_treeList.Appearance.EvenRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Permission_treeList.Appearance.EvenRow.BackColor2 = ((System.Drawing.Color)(resources.GetObject("Permission_treeList.Appearance.EvenRow.BackColor2")));
            this.Permission_treeList.Appearance.EvenRow.Options.UseBackColor = true;
            this.Permission_treeList.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.Permission_treeList.Appearance.FocusedRow.Options.UseBackColor = true;
            this.Permission_treeList.Appearance.OddRow.BackColor = System.Drawing.Color.MistyRose;
            this.Permission_treeList.Appearance.OddRow.BackColor2 = ((System.Drawing.Color)(resources.GetObject("Permission_treeList.Appearance.OddRow.BackColor2")));
            this.Permission_treeList.Appearance.OddRow.Options.UseBackColor = true;
            this.Permission_treeList.AppearancePrint.BandPanel.Options.UseTextOptions = true;
            this.Permission_treeList.AppearancePrint.BandPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Permission_treeList.AppearancePrint.Preview.Options.UseTextOptions = true;
            this.Permission_treeList.AppearancePrint.Preview.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Permission_treeList.AppearancePrint.Row.Options.UseTextOptions = true;
            this.Permission_treeList.AppearancePrint.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Permission_treeList.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
            this.colObjectName1,
            this.colTheValues1,
            this.colPermissionName1,
            this.colUserGroupID1});
            this.Permission_treeList.CustomizationFormBounds = new System.Drawing.Rectangle(734, 384, 252, 266);
            this.Permission_treeList.DataSource = this.tblPermissionsBindingSource;
            resources.ApplyResources(this.Permission_treeList, "Permission_treeList");
            this.Permission_treeList.MinWidth = 23;
            this.Permission_treeList.Name = "Permission_treeList";
            this.Permission_treeList.OptionsBehavior.PopulateServiceColumns = true;
            this.Permission_treeList.OptionsView.EnableAppearanceEvenRow = true;
            this.Permission_treeList.OptionsView.EnableAppearanceOddRow = true;
            this.Permission_treeList.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit7});
            this.Permission_treeList.TreeLevelWidth = 21;
            // 
            // colObjectName1
            // 
            this.colObjectName1.AppearanceCell.Options.UseTextOptions = true;
            this.colObjectName1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colObjectName1.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.colObjectName1.AppearanceHeader.Options.UseBackColor = true;
            this.colObjectName1.AppearanceHeader.Options.UseTextOptions = true;
            this.colObjectName1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.colObjectName1, "colObjectName1");
            this.colObjectName1.FieldName = "ObjectName";
            this.colObjectName1.Name = "colObjectName1";
            this.colObjectName1.OptionsColumn.AllowEdit = false;
            this.colObjectName1.OptionsColumn.AllowFocus = false;
            this.colObjectName1.OptionsFilter.AllowFilter = false;
            // 
            // colTheValues1
            // 
            this.colTheValues1.AppearanceCell.Options.UseTextOptions = true;
            this.colTheValues1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colTheValues1.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.colTheValues1.AppearanceHeader.Options.UseBackColor = true;
            this.colTheValues1.AppearanceHeader.Options.UseTextOptions = true;
            this.colTheValues1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.colTheValues1, "colTheValues1");
            this.colTheValues1.FieldName = "TheValues";
            this.colTheValues1.Name = "colTheValues1";
            // 
            // colPermissionName1
            // 
            this.colPermissionName1.AppearanceCell.Options.UseTextOptions = true;
            this.colPermissionName1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colPermissionName1.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.colPermissionName1.AppearanceHeader.Options.UseBackColor = true;
            this.colPermissionName1.AppearanceHeader.Options.UseTextOptions = true;
            this.colPermissionName1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.colPermissionName1, "colPermissionName1");
            this.colPermissionName1.FieldName = "PermissionName";
            this.colPermissionName1.Name = "colPermissionName1";
            this.colPermissionName1.OptionsColumn.AllowEdit = false;
            this.colPermissionName1.OptionsColumn.AllowFocus = false;
            this.colPermissionName1.OptionsFilter.AllowFilter = false;
            // 
            // colUserGroupID1
            // 
            this.colUserGroupID1.AppearanceCell.Options.UseTextOptions = true;
            this.colUserGroupID1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colUserGroupID1.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen;
            this.colUserGroupID1.AppearanceHeader.Options.UseBackColor = true;
            this.colUserGroupID1.AppearanceHeader.Options.UseTextOptions = true;
            this.colUserGroupID1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            resources.ApplyResources(this.colUserGroupID1, "colUserGroupID1");
            this.colUserGroupID1.ColumnEdit = this.repositoryItemLookUpEdit7;
            this.colUserGroupID1.FieldName = "UserGroupID";
            this.colUserGroupID1.Name = "colUserGroupID1";
            this.colUserGroupID1.OptionsColumn.AllowEdit = false;
            this.colUserGroupID1.OptionsColumn.AllowFocus = false;
            this.colUserGroupID1.OptionsColumn.ReadOnly = true;
            // 
            // repositoryItemLookUpEdit7
            // 
            resources.ApplyResources(this.repositoryItemLookUpEdit7, "repositoryItemLookUpEdit7");
            this.repositoryItemLookUpEdit7.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("repositoryItemLookUpEdit7.Buttons"))))});
            this.repositoryItemLookUpEdit7.DataSource = this.UserGroupBindingSource;
            this.repositoryItemLookUpEdit7.DisplayMember = "UserGroupName";
            this.repositoryItemLookUpEdit7.Name = "repositoryItemLookUpEdit7";
            this.repositoryItemLookUpEdit7.ValueMember = "ID";
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            resources.ApplyResources(this.bindingNavigator1, "bindingNavigator1");
            this.bindingNavigator1.BindingSource = this.UserGroupBindingSource;
            this.bindingNavigator1.CountItem = this.toolStripLabel8;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton8,
            this.toolStripButton9,
            this.toolStripSeparator22,
            this.toolStripTextBox8,
            this.toolStripLabel8,
            this.toolStripSeparator23,
            this.toolStripButton14,
            this.toolStripButton21,
            this.toolStripSeparator24,
            this.PermissionSave,
            this.Print_Permission,
            this.RefreachPermission});
            this.bindingNavigator1.MoveFirstItem = this.toolStripButton8;
            this.bindingNavigator1.MoveLastItem = this.toolStripButton21;
            this.bindingNavigator1.MoveNextItem = this.toolStripButton14;
            this.bindingNavigator1.MovePreviousItem = this.toolStripButton9;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.toolStripTextBox8;
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            resources.ApplyResources(this.toolStripLabel8, "toolStripLabel8");
            // 
            // toolStripButton8
            // 
            this.toolStripButton8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton8, "toolStripButton8");
            this.toolStripButton8.Name = "toolStripButton8";
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton9, "toolStripButton9");
            this.toolStripButton9.Name = "toolStripButton9";
            // 
            // toolStripSeparator22
            // 
            this.toolStripSeparator22.Name = "toolStripSeparator22";
            resources.ApplyResources(this.toolStripSeparator22, "toolStripSeparator22");
            // 
            // toolStripTextBox8
            // 
            resources.ApplyResources(this.toolStripTextBox8, "toolStripTextBox8");
            this.toolStripTextBox8.Name = "toolStripTextBox8";
            // 
            // toolStripSeparator23
            // 
            this.toolStripSeparator23.Name = "toolStripSeparator23";
            resources.ApplyResources(this.toolStripSeparator23, "toolStripSeparator23");
            // 
            // toolStripButton14
            // 
            this.toolStripButton14.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton14, "toolStripButton14");
            this.toolStripButton14.Name = "toolStripButton14";
            // 
            // toolStripButton21
            // 
            this.toolStripButton21.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.toolStripButton21, "toolStripButton21");
            this.toolStripButton21.Name = "toolStripButton21";
            // 
            // toolStripSeparator24
            // 
            this.toolStripSeparator24.Name = "toolStripSeparator24";
            resources.ApplyResources(this.toolStripSeparator24, "toolStripSeparator24");
            // 
            // PermissionSave
            // 
            this.PermissionSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.PermissionSave.Image = global::SewingSystem.Properties.Resources.save_32x32;
            resources.ApplyResources(this.PermissionSave, "PermissionSave");
            this.PermissionSave.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.PermissionSave.Name = "PermissionSave";
            this.PermissionSave.Click += new System.EventHandler(this.PermissionSave_Click);
            // 
            // Print_Permission
            // 
            this.Print_Permission.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Print_Permission.Image = global::SewingSystem.Properties.Resources.printer_32x32;
            resources.ApplyResources(this.Print_Permission, "Print_Permission");
            this.Print_Permission.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.Print_Permission.Name = "Print_Permission";
            this.Print_Permission.Click += new System.EventHandler(this.Print_Permission_Click);
            // 
            // RefreachPermission
            // 
            this.RefreachPermission.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.RefreachPermission.Image = global::SewingSystem.Properties.Resources.refresh_32x32;
            resources.ApplyResources(this.RefreachPermission, "RefreachPermission");
            this.RefreachPermission.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.RefreachPermission.Name = "RefreachPermission";
            this.RefreachPermission.Click += new System.EventHandler(this.Refresh_Click);
            // 
            // UserGroup_lookUpEdit
            // 
            this.UserGroup_lookUpEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.UserGroupBindingSource, "UserGroupName", true,System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            resources.ApplyResources(this.UserGroup_lookUpEdit, "UserGroup_lookUpEdit");
            this.UserGroup_lookUpEdit.Name = "UserGroup_lookUpEdit";
            this.UserGroup_lookUpEdit.Properties.Appearance.Font = ((System.Drawing.Font)(resources.GetObject("UserGroup_lookUpEdit.Properties.Appearance.Font")));
            this.UserGroup_lookUpEdit.Properties.Appearance.Options.UseFont = true;
            this.UserGroup_lookUpEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.UserGroup_lookUpEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.UserGroup_lookUpEdit.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.UserGroup_lookUpEdit.Properties.ReadOnly = true;
            this.UserGroup_lookUpEdit.Properties.UseReadOnlyAppearance = false;
            this.UserGroup_lookUpEdit.StyleController = this.Permission_dataLayoutCon;
            // 
            // layoutControlGroup33
            // 
            this.layoutControlGroup33.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup33.GroupBordersVisible = false;
            this.layoutControlGroup33.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup34});
            this.layoutControlGroup33.Name = "Root";
            this.layoutControlGroup33.Size = new System.Drawing.Size(1094, 613);
            this.layoutControlGroup33.TextVisible = false;
            // 
            // layoutControlGroup34
            // 
            this.layoutControlGroup34.AllowDrawBackground = false;
            this.layoutControlGroup34.GroupBordersVisible = false;
            this.layoutControlGroup34.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem37,
            this.layoutControlGroup38,
            this.layoutControlGroup75});
            this.layoutControlGroup34.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup34.Name = "autoGeneratedGroup0";
            this.layoutControlGroup34.Size = new System.Drawing.Size(1074, 593);
            // 
            // layoutControlItem37
            // 
            this.layoutControlItem37.Control = this.bindingNavigator1;
            this.layoutControlItem37.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem37.MinSize = new System.Drawing.Size(104, 34);
            this.layoutControlItem37.Name = "layoutControlItem115";
            this.layoutControlItem37.OptionsPrint.AllowPrint = false;
            this.layoutControlItem37.Size = new System.Drawing.Size(1074, 59);
            this.layoutControlItem37.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem37.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem37.TextVisible = false;
            // 
            // layoutControlGroup38
            // 
            this.layoutControlGroup38.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem46});
            this.layoutControlGroup38.Location = new System.Drawing.Point(0, 123);
            this.layoutControlGroup38.Name = "layoutControlGroup38";
            this.layoutControlGroup38.Size = new System.Drawing.Size(1074, 470);
            resources.ApplyResources(this.layoutControlGroup38, "layoutControlGroup38");
            // 
            // layoutControlItem46
            // 
            this.layoutControlItem46.Control = this.Permission_treeList;
            this.layoutControlItem46.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem46.Name = "layoutControlItem46";
            this.layoutControlItem46.Size = new System.Drawing.Size(1050, 425);
            this.layoutControlItem46.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem46.TextVisible = false;
            // 
            // layoutControlGroup75
            // 
            this.layoutControlGroup75.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlItem51,
            this.layoutControlItem48});
            this.layoutControlGroup75.Location = new System.Drawing.Point(0, 59);
            this.layoutControlGroup75.Name = "layoutControlGroup75";
            this.layoutControlGroup75.Size = new System.Drawing.Size(1074, 64);
            this.layoutControlGroup75.TextVisible = false;
            // 
            // layoutControlItem51
            // 
            this.layoutControlItem51.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem51.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem51.Control = this.checkButton1;
            this.layoutControlItem51.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem51.Name = "layoutControlItem51";
            this.layoutControlItem51.OptionsPrint.AllowPrint = false;
            this.layoutControlItem51.Size = new System.Drawing.Size(525, 40);
            this.layoutControlItem51.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem51.TextVisible = false;
            // 
            // layoutControlItem48
            // 
            this.layoutControlItem48.AppearanceItemCaption.Font = ((System.Drawing.Font)(resources.GetObject("layoutControlItem48.AppearanceItemCaption.Font")));
            this.layoutControlItem48.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlItem48.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem48.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem48.ContentVertAlignment = DevExpress.Utils.VertAlignment.Center;
            this.layoutControlItem48.Control = this.UserGroup_lookUpEdit;
            this.layoutControlItem48.Location = new System.Drawing.Point(525, 0);
            this.layoutControlItem48.Name = "layoutControlItem48";
            this.layoutControlItem48.Size = new System.Drawing.Size(525, 40);
            resources.ApplyResources(this.layoutControlItem48, "layoutControlItem48");
            this.layoutControlItem48.TextSize = new System.Drawing.Size(142, 17);
            // 
            // XtraFormPermissionUser
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Permission_dataLayoutCon);
            this.IconOptions.LargeImage = global::SewingSystem.Properties.Resources.bopermission_32x321;
            this.Name = "XtraFormPermissionUser";
            this.Load += new System.EventHandler(this.XtraFormPermissionUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tblPermissionsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserGroupBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Permission_dataLayoutCon)).EndInit();
            this.Permission_dataLayoutCon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Permission_treeList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UserGroup_lookUpEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup33)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup34)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem37)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup38)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem46)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup75)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem51)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem48)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource tblPermissionsBindingSource;
        private System.Windows.Forms.BindingSource UserGroupBindingSource;
        private DevExpress.XtraDataLayout.DataLayoutControl Permission_dataLayoutCon;
        private DevExpress.XtraEditors.CheckButton checkButton1;
        private DevExpress.XtraTreeList.TreeList Permission_treeList;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colObjectName1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colTheValues1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colPermissionName1;
        private DevExpress.XtraTreeList.Columns.TreeListColumn colUserGroupID1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit7;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel8;
        private System.Windows.Forms.ToolStripButton toolStripButton8;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator22;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator23;
        private System.Windows.Forms.ToolStripButton toolStripButton14;
        private System.Windows.Forms.ToolStripButton toolStripButton21;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator24;
        private System.Windows.Forms.ToolStripButton PermissionSave;
        private System.Windows.Forms.ToolStripButton Print_Permission;
        private System.Windows.Forms.ToolStripButton RefreachPermission;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup33;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup34;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem37;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup38;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem46;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup75;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem51;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem48;
        private DevExpress.XtraEditors.MemoEdit UserGroup_lookUpEdit;
    }
}