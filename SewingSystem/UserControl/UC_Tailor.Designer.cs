using System.Windows.Forms;

namespace SewingSystem.Forms
{
    partial class UC_Tailor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.TailorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.treeListName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTtailorName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAddress = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMobil = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMobil2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmail = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEnterTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colBranchID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.IDTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.TtailorNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.AddressTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.MobilTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.Mobil2TextEdit = new DevExpress.XtraEditors.TextEdit();
            this.EmailTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.NotesMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForNotes = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForMobil = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForID = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForMobil2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForTtailorName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForAddress = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForEmail = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.TailorBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.IDTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TtailorNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddressTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MobilTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mobil2TextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmailTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForMobil)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForMobil2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForTtailorName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAddress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // TailorBindingSource
            // 
            this.TailorBindingSource.DataSource = typeof(SewingSystem.LinqModel.tblTailor);
            // 
            // treeListName
            // 
            this.treeListName.FieldName = "Name";
            this.treeListName.Name = "treeListName";
            this.treeListName.Visible = true;
            this.treeListName.VisibleIndex = 0;
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Controls.Add(this.gridControl1);
            this.dataLayoutControl1.Controls.Add(this.IDTextEdit);
            this.dataLayoutControl1.Controls.Add(this.TtailorNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.AddressTextEdit);
            this.dataLayoutControl1.Controls.Add(this.MobilTextEdit);
            this.dataLayoutControl1.Controls.Add(this.Mobil2TextEdit);
            this.dataLayoutControl1.Controls.Add(this.EmailTextEdit);
            this.dataLayoutControl1.Controls.Add(this.NotesMemoEdit);
            this.dataLayoutControl1.DataSource = this.TailorBindingSource;
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 59);
            this.dataLayoutControl1.Margin = new System.Windows.Forms.Padding(2);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.OptionsView.IsReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.dataLayoutControl1.OptionsView.RightToLeftMirroringApplied = true;
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(1089, 527);
            this.dataLayoutControl1.TabIndex = 6;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.TailorBindingSource;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.gridControl1.Location = new System.Drawing.Point(12, 144);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(2);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1065, 371);
            this.gridControl1.TabIndex = 30;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colTtailorName,
            this.colAddress,
            this.colMobil,
            this.colMobil2,
            this.colEmail,
            this.colNotes,
            this.colUserID,
            this.colEnterTime,
            this.colBranchID});
            this.gridView1.DetailHeight = 233;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // colID
            // 
            this.colID.Caption = "الرقم";
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            this.colID.Visible = true;
            this.colID.VisibleIndex = 0;
            this.colID.Width = 94;
            // 
            // colTtailorName
            // 
            this.colTtailorName.Caption = "اسم الخياط";
            this.colTtailorName.FieldName = "Name";
            this.colTtailorName.Name = "colTtailorName";
            this.colTtailorName.Visible = true;
            this.colTtailorName.VisibleIndex = 1;
            this.colTtailorName.Width = 94;
            // 
            // colAddress
            // 
            this.colAddress.Caption = "العنوان";
            this.colAddress.FieldName = "Address";
            this.colAddress.Name = "colAddress";
            this.colAddress.Visible = true;
            this.colAddress.VisibleIndex = 2;
            this.colAddress.Width = 94;
            // 
            // colMobil
            // 
            this.colMobil.Caption = "الجوال1";
            this.colMobil.FieldName = "Mobil";
            this.colMobil.Name = "colMobil";
            this.colMobil.Visible = true;
            this.colMobil.VisibleIndex = 3;
            this.colMobil.Width = 94;
            // 
            // colMobil2
            // 
            this.colMobil2.Caption = "الجوال2";
            this.colMobil2.FieldName = "Mobil2";
            this.colMobil2.Name = "colMobil2";
            this.colMobil2.Visible = true;
            this.colMobil2.VisibleIndex = 4;
            this.colMobil2.Width = 94;
            // 
            // colEmail
            // 
            this.colEmail.Caption = "ايميل";
            this.colEmail.FieldName = "Email";
            this.colEmail.Name = "colEmail";
            this.colEmail.Visible = true;
            this.colEmail.VisibleIndex = 5;
            this.colEmail.Width = 94;
            // 
            // colNotes
            // 
            this.colNotes.Caption = "ملاحظات";
            this.colNotes.FieldName = "Notes";
            this.colNotes.Name = "colNotes";
            this.colNotes.Visible = true;
            this.colNotes.VisibleIndex = 6;
            this.colNotes.Width = 94;
            // 
            // colUserID
            // 
            this.colUserID.FieldName = "UserID";
            this.colUserID.Name = "colUserID";
            this.colUserID.Width = 94;
            // 
            // colEnterTime
            // 
            this.colEnterTime.FieldName = "EnterTime";
            this.colEnterTime.Name = "colEnterTime";
            this.colEnterTime.Width = 94;
            // 
            // colBranchID
            // 
            this.colBranchID.FieldName = "BranchID";
            this.colBranchID.Name = "colBranchID";
            this.colBranchID.Width = 94;
            // 
            // IDTextEdit
            // 
            this.IDTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.TailorBindingSource, "ID", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.IDTextEdit.Location = new System.Drawing.Point(720, 50);
            this.IDTextEdit.Margin = new System.Windows.Forms.Padding(2);
            this.IDTextEdit.Name = "IDTextEdit";
            this.IDTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.IDTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.IDTextEdit.Properties.Mask.EditMask = "N0";
            this.IDTextEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.IDTextEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.IDTextEdit.Properties.ReadOnly = true;
            this.IDTextEdit.Size = new System.Drawing.Size(267, 22);
            this.IDTextEdit.StyleController = this.dataLayoutControl1;
            this.IDTextEdit.TabIndex = 23;
            // 
            // TtailorNameTextEdit
            // 
            this.TtailorNameTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.TailorBindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TtailorNameTextEdit.Location = new System.Drawing.Point(373, 50);
            this.TtailorNameTextEdit.Margin = new System.Windows.Forms.Padding(2);
            this.TtailorNameTextEdit.Name = "TtailorNameTextEdit";
            this.TtailorNameTextEdit.Properties.ReadOnly = true;
            this.TtailorNameTextEdit.Size = new System.Drawing.Size(265, 22);
            this.TtailorNameTextEdit.StyleController = this.dataLayoutControl1;
            this.TtailorNameTextEdit.TabIndex = 24;
            // 
            // AddressTextEdit
            // 
            this.AddressTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.TailorBindingSource, "Address", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.AddressTextEdit.Location = new System.Drawing.Point(24, 50);
            this.AddressTextEdit.Margin = new System.Windows.Forms.Padding(2);
            this.AddressTextEdit.Name = "AddressTextEdit";
            this.AddressTextEdit.Properties.ReadOnly = true;
            this.AddressTextEdit.Size = new System.Drawing.Size(267, 22);
            this.AddressTextEdit.StyleController = this.dataLayoutControl1;
            this.AddressTextEdit.TabIndex = 25;
            // 
            // MobilTextEdit
            // 
            this.MobilTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.TailorBindingSource, "Mobil", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.MobilTextEdit.Location = new System.Drawing.Point(720, 76);
            this.MobilTextEdit.Margin = new System.Windows.Forms.Padding(2);
            this.MobilTextEdit.Name = "MobilTextEdit";
            this.MobilTextEdit.Properties.ReadOnly = true;
            this.MobilTextEdit.Size = new System.Drawing.Size(267, 22);
            this.MobilTextEdit.StyleController = this.dataLayoutControl1;
            this.MobilTextEdit.TabIndex = 26;
            // 
            // Mobil2TextEdit
            // 
            this.Mobil2TextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.TailorBindingSource, "Mobil2", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Mobil2TextEdit.Location = new System.Drawing.Point(373, 76);
            this.Mobil2TextEdit.Margin = new System.Windows.Forms.Padding(2);
            this.Mobil2TextEdit.Name = "Mobil2TextEdit";
            this.Mobil2TextEdit.Properties.ReadOnly = true;
            this.Mobil2TextEdit.Size = new System.Drawing.Size(265, 22);
            this.Mobil2TextEdit.StyleController = this.dataLayoutControl1;
            this.Mobil2TextEdit.TabIndex = 27;
            // 
            // EmailTextEdit
            // 
            this.EmailTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.TailorBindingSource, "Email", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.EmailTextEdit.Location = new System.Drawing.Point(24, 76);
            this.EmailTextEdit.Margin = new System.Windows.Forms.Padding(2);
            this.EmailTextEdit.Name = "EmailTextEdit";
            this.EmailTextEdit.Properties.ReadOnly = true;
            this.EmailTextEdit.Size = new System.Drawing.Size(267, 22);
            this.EmailTextEdit.StyleController = this.dataLayoutControl1;
            this.EmailTextEdit.TabIndex = 28;
            // 
            // NotesMemoEdit
            // 
            this.NotesMemoEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.TailorBindingSource, "Notes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.NotesMemoEdit.Location = new System.Drawing.Point(24, 102);
            this.NotesMemoEdit.Margin = new System.Windows.Forms.Padding(2);
            this.NotesMemoEdit.Name = "NotesMemoEdit";
            this.NotesMemoEdit.Properties.ReadOnly = true;
            this.NotesMemoEdit.Size = new System.Drawing.Size(963, 26);
            this.NotesMemoEdit.StyleController = this.dataLayoutControl1;
            this.NotesMemoEdit.TabIndex = 29;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1089, 527);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(1069, 507);
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForNotes,
            this.ItemForMobil,
            this.ItemForID,
            this.ItemForMobil2,
            this.ItemForTtailorName,
            this.ItemForAddress,
            this.ItemForEmail});
            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(1069, 132);
            this.layoutControlGroup2.Text = "بيانات الخياطين";
            // 
            // ItemForNotes
            // 
            this.ItemForNotes.Control = this.NotesMemoEdit;
            this.ItemForNotes.Location = new System.Drawing.Point(0, 52);
            this.ItemForNotes.Name = "ItemForNotes";
            this.ItemForNotes.Size = new System.Drawing.Size(1045, 30);
            this.ItemForNotes.StartNewLine = true;
            this.ItemForNotes.Text = "ملاحظات";
            this.ItemForNotes.TextSize = new System.Drawing.Size(66, 17);
            // 
            // ItemForMobil
            // 
            this.ItemForMobil.Control = this.MobilTextEdit;
            this.ItemForMobil.Location = new System.Drawing.Point(696, 26);
            this.ItemForMobil.Name = "ItemForMobil";
            this.ItemForMobil.Size = new System.Drawing.Size(349, 26);
            this.ItemForMobil.Text = "الجوال1";
            this.ItemForMobil.TextSize = new System.Drawing.Size(66, 17);
            // 
            // ItemForID
            // 
            this.ItemForID.Control = this.IDTextEdit;
            this.ItemForID.Location = new System.Drawing.Point(696, 0);
            this.ItemForID.Name = "ItemForID";
            this.ItemForID.Size = new System.Drawing.Size(349, 26);
            this.ItemForID.Text = "الرقم";
            this.ItemForID.TextSize = new System.Drawing.Size(66, 17);
            // 
            // ItemForMobil2
            // 
            this.ItemForMobil2.Control = this.Mobil2TextEdit;
            this.ItemForMobil2.Location = new System.Drawing.Point(349, 26);
            this.ItemForMobil2.Name = "ItemForMobil2";
            this.ItemForMobil2.Size = new System.Drawing.Size(347, 26);
            this.ItemForMobil2.Text = "الجوال2";
            this.ItemForMobil2.TextSize = new System.Drawing.Size(66, 17);
            // 
            // ItemForTtailorName
            // 
            this.ItemForTtailorName.Control = this.TtailorNameTextEdit;
            this.ItemForTtailorName.Location = new System.Drawing.Point(349, 0);
            this.ItemForTtailorName.Name = "ItemForTtailorName";
            this.ItemForTtailorName.Size = new System.Drawing.Size(347, 26);
            this.ItemForTtailorName.Text = "اسم الخياط";
            this.ItemForTtailorName.TextSize = new System.Drawing.Size(66, 17);
            // 
            // ItemForAddress
            // 
            this.ItemForAddress.Control = this.AddressTextEdit;
            this.ItemForAddress.Location = new System.Drawing.Point(0, 0);
            this.ItemForAddress.Name = "ItemForAddress";
            this.ItemForAddress.Size = new System.Drawing.Size(349, 26);
            this.ItemForAddress.Text = "العنوان";
            this.ItemForAddress.TextSize = new System.Drawing.Size(66, 17);
            // 
            // ItemForEmail
            // 
            this.ItemForEmail.Control = this.EmailTextEdit;
            this.ItemForEmail.Location = new System.Drawing.Point(0, 26);
            this.ItemForEmail.Name = "ItemForEmail";
            this.ItemForEmail.Size = new System.Drawing.Size(349, 26);
            this.ItemForEmail.Text = "ايميل";
            this.ItemForEmail.TextSize = new System.Drawing.Size(66, 17);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 132);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1069, 375);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // UC_Tailor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.bindingSource = this.TailorBindingSource;
            this.Controls.Add(this.dataLayoutControl1);
            this.dataLayout = this.dataLayoutControl1;
            this.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.gridControl = this.gridControl1;
            this.GroupMain = this.layoutControlGroup2;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UC_Tailor";
            this.NameAr = "الخياط";
            this.NameEn = "Tailor";
            this.Size = new System.Drawing.Size(1089, 586);
            this.SumNameAr = "الخياطين";
            this.SumNameEn = "Tailors";
            this.Controls.SetChildIndex(this.dataLayoutControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.TailorBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.IDTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TtailorNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AddressTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MobilTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Mobil2TextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EmailTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForMobil)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForMobil2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForTtailorName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForAddress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListName;
        private System.Windows.Forms.BindingSource TailorBindingSource;
        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.TextEdit IDTextEdit;
        private DevExpress.XtraEditors.TextEdit TtailorNameTextEdit;
        private DevExpress.XtraEditors.TextEdit AddressTextEdit;
        private DevExpress.XtraEditors.TextEdit MobilTextEdit;
        private DevExpress.XtraEditors.TextEdit Mobil2TextEdit;
        private DevExpress.XtraEditors.TextEdit EmailTextEdit;
        private DevExpress.XtraEditors.MemoEdit NotesMemoEdit;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem ItemForNotes;
        private DevExpress.XtraLayout.LayoutControlItem ItemForEmail;
        private DevExpress.XtraLayout.LayoutControlItem ItemForMobil2;
        private DevExpress.XtraLayout.LayoutControlItem ItemForMobil;
        private DevExpress.XtraLayout.LayoutControlItem ItemForAddress;
        private DevExpress.XtraLayout.LayoutControlItem ItemForTtailorName;
        private DevExpress.XtraLayout.LayoutControlItem ItemForID;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colTtailorName;
        private DevExpress.XtraGrid.Columns.GridColumn colAddress;
        private DevExpress.XtraGrid.Columns.GridColumn colMobil;
        private DevExpress.XtraGrid.Columns.GridColumn colMobil2;
        private DevExpress.XtraGrid.Columns.GridColumn colEmail;
        private DevExpress.XtraGrid.Columns.GridColumn colNotes;
        private DevExpress.XtraGrid.Columns.GridColumn colUserID;
        private DevExpress.XtraGrid.Columns.GridColumn colEnterTime;
        private DevExpress.XtraGrid.Columns.GridColumn colBranchID;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}
