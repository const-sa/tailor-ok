using System.Windows.Forms;

namespace SewingSystem.Forms
{
    partial class UC_Factorie
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
            this.FactorieBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.treeListName = new DevExpress.XtraTreeList.Columns.TreeListColumn();
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colTheNameAr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTheNameEn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTheAddressAr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colThePhone1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colThePhone2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTheEmail = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNotes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEnterTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUserID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.NotesTextEdit = new DevExpress.XtraEditors.MemoEdit();
            this.TheNameArTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.ThePhone1TextEdit = new DevExpress.XtraEditors.TextEdit();
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.Factories_layoutControlGroup29 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForNotes = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForTheNameAr = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForThePhone1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.FactorieBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TheNameArTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThePhone1TextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Factories_layoutControlGroup29)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForTheNameAr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForThePhone1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            this.SuspendLayout();
            // 
            // FactorieBindingSource
            // 
            this.FactorieBindingSource.DataSource = typeof(SewingSystem.LinqModel.tblFactorie);
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
            this.dataLayoutControl1.Controls.Add(this.NotesTextEdit);
            this.dataLayoutControl1.Controls.Add(this.TheNameArTextEdit);
            this.dataLayoutControl1.Controls.Add(this.ThePhone1TextEdit);
            this.dataLayoutControl1.DataSource = this.FactorieBindingSource;
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(0, 59);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.OptionsView.IsReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.dataLayoutControl1.OptionsView.RightToLeftMirroringApplied = true;
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(1445, 584);
            this.dataLayoutControl1.TabIndex = 6;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // gridControl1
            // 
            this.gridControl1.DataSource = this.FactorieBindingSource;
            this.gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.gridControl1.Location = new System.Drawing.Point(12, 133);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1421, 439);
            this.gridControl1.TabIndex = 22;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Appearance.EvenRow.BackColor = System.Drawing.Color.MintCream;
            this.gridView1.Appearance.EvenRow.BackColor2 = System.Drawing.Color.AliceBlue;
            this.gridView1.Appearance.EvenRow.Options.UseBackColor = true;
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colTheNameAr,
            this.colTheNameEn,
            this.colTheAddressAr,
            this.colThePhone1,
            this.colThePhone2,
            this.colTheEmail,
            this.colNotes,
            this.colEnterTime,
            this.colUserID});
            this.gridView1.DetailHeight = 629;
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsView.EnableAppearanceEvenRow = true;
            // 
            // colTheNameAr
            // 
            this.colTheNameAr.Caption = "اسم المعلم";
            this.colTheNameAr.FieldName = "Name";
            this.colTheNameAr.MinWidth = 35;
            this.colTheNameAr.Name = "colTheNameAr";
            this.colTheNameAr.OptionsColumn.AllowEdit = false;
            this.colTheNameAr.OptionsColumn.AllowFocus = false;
            this.colTheNameAr.OptionsFilter.AllowFilter = false;
            this.colTheNameAr.Visible = true;
            this.colTheNameAr.VisibleIndex = 0;
            this.colTheNameAr.Width = 130;
            // 
            // colTheNameEn
            // 
            this.colTheNameEn.Caption = "الفرع";
            this.colTheNameEn.FieldName = "BranchID";
            this.colTheNameEn.MinWidth = 35;
            this.colTheNameEn.Name = "colTheNameEn";
            this.colTheNameEn.OptionsColumn.AllowEdit = false;
            this.colTheNameEn.OptionsColumn.AllowFocus = false;
            this.colTheNameEn.OptionsFilter.AllowFilter = false;
            this.colTheNameEn.Visible = true;
            this.colTheNameEn.VisibleIndex = 5;
            this.colTheNameEn.Width = 130;
            // 
            // colTheAddressAr
            // 
            this.colTheAddressAr.Caption = "ملاحظات";
            this.colTheAddressAr.FieldName = "Notes";
            this.colTheAddressAr.MinWidth = 35;
            this.colTheAddressAr.Name = "colTheAddressAr";
            this.colTheAddressAr.OptionsColumn.AllowEdit = false;
            this.colTheAddressAr.OptionsColumn.AllowFocus = false;
            this.colTheAddressAr.OptionsFilter.AllowFilter = false;
            this.colTheAddressAr.Visible = true;
            this.colTheAddressAr.VisibleIndex = 2;
            this.colTheAddressAr.Width = 130;
            // 
            // colThePhone1
            // 
            this.colThePhone1.Caption = "الجوال";
            this.colThePhone1.FieldName = "Mobile";
            this.colThePhone1.MinWidth = 35;
            this.colThePhone1.Name = "colThePhone1";
            this.colThePhone1.OptionsColumn.AllowEdit = false;
            this.colThePhone1.OptionsColumn.AllowFocus = false;
            this.colThePhone1.OptionsFilter.AllowFilter = false;
            this.colThePhone1.Visible = true;
            this.colThePhone1.VisibleIndex = 1;
            this.colThePhone1.Width = 130;
            // 
            // colThePhone2
            // 
            this.colThePhone2.FieldName = "Mobil2";
            this.colThePhone2.MinWidth = 35;
            this.colThePhone2.Name = "colThePhone2";
            this.colThePhone2.OptionsColumn.AllowEdit = false;
            this.colThePhone2.OptionsColumn.AllowFocus = false;
            this.colThePhone2.OptionsFilter.AllowFilter = false;
            this.colThePhone2.Width = 130;
            // 
            // colTheEmail
            // 
            this.colTheEmail.FieldName = "Email";
            this.colTheEmail.MinWidth = 35;
            this.colTheEmail.Name = "colTheEmail";
            this.colTheEmail.OptionsColumn.AllowEdit = false;
            this.colTheEmail.OptionsColumn.AllowFocus = false;
            this.colTheEmail.OptionsFilter.AllowFilter = false;
            this.colTheEmail.Width = 130;
            // 
            // colNotes
            // 
            this.colNotes.FieldName = "Notes";
            this.colNotes.MinWidth = 35;
            this.colNotes.Name = "colNotes";
            this.colNotes.OptionsColumn.AllowEdit = false;
            this.colNotes.OptionsColumn.AllowFocus = false;
            this.colNotes.OptionsFilter.AllowFilter = false;
            this.colNotes.Width = 130;
            // 
            // colEnterTime
            // 
            this.colEnterTime.Caption = "تاريخ الادخال";
            this.colEnterTime.FieldName = "EnterTime";
            this.colEnterTime.MinWidth = 35;
            this.colEnterTime.Name = "colEnterTime";
            this.colEnterTime.OptionsColumn.AllowEdit = false;
            this.colEnterTime.OptionsColumn.AllowFocus = false;
            this.colEnterTime.OptionsFilter.AllowFilter = false;
            this.colEnterTime.Visible = true;
            this.colEnterTime.VisibleIndex = 3;
            this.colEnterTime.Width = 130;
            // 
            // colUserID
            // 
            this.colUserID.Caption = "المستخدم";
            this.colUserID.FieldName = "UserID";
            this.colUserID.MinWidth = 35;
            this.colUserID.Name = "colUserID";
            this.colUserID.OptionsColumn.AllowEdit = false;
            this.colUserID.OptionsColumn.AllowFocus = false;
            this.colUserID.OptionsFilter.AllowFilter = false;
            this.colUserID.Visible = true;
            this.colUserID.VisibleIndex = 4;
            this.colUserID.Width = 130;
            // 
            // NotesTextEdit
            // 
            this.NotesTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.FactorieBindingSource, "Notes", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.NotesTextEdit.Location = new System.Drawing.Point(29, 78);
            this.NotesTextEdit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NotesTextEdit.Name = "NotesTextEdit";
            this.NotesTextEdit.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.NotesTextEdit.Properties.Appearance.Options.UseFont = true;
            this.NotesTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.NotesTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.NotesTextEdit.Properties.ReadOnly = true;
            this.NotesTextEdit.Size = new System.Drawing.Size(1297, 37);
            this.NotesTextEdit.StyleController = this.dataLayoutControl1;
            this.NotesTextEdit.TabIndex = 20;
            // 
            // TheNameArTextEdit
            // 
            this.TheNameArTextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.FactorieBindingSource, "Name", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.TheNameArTextEdit.Location = new System.Drawing.Point(725, 52);
            this.TheNameArTextEdit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TheNameArTextEdit.Name = "TheNameArTextEdit";
            this.TheNameArTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.TheNameArTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.TheNameArTextEdit.Properties.ReadOnly = true;
            this.TheNameArTextEdit.Size = new System.Drawing.Size(601, 22);
            this.TheNameArTextEdit.StyleController = this.dataLayoutControl1;
            this.TheNameArTextEdit.TabIndex = 11;
            // 
            // ThePhone1TextEdit
            // 
            this.ThePhone1TextEdit.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", this.FactorieBindingSource, "Mobile", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ThePhone1TextEdit.Location = new System.Drawing.Point(29, 52);
            this.ThePhone1TextEdit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.ThePhone1TextEdit.Name = "ThePhone1TextEdit";
            this.ThePhone1TextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.ThePhone1TextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.ThePhone1TextEdit.Properties.ReadOnly = true;
            this.ThePhone1TextEdit.Size = new System.Drawing.Size(600, 22);
            this.ThePhone1TextEdit.StyleController = this.dataLayoutControl1;
            this.ThePhone1TextEdit.TabIndex = 15;
            // 
            // Root
            // 
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1,
            this.layoutControlItem1});
            this.Root.Name = "Root";
            this.Root.Size = new System.Drawing.Size(1445, 584);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.Factories_layoutControlGroup29});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(1425, 121);
            // 
            // Factories_layoutControlGroup29
            // 
            this.Factories_layoutControlGroup29.CustomizationFormText = "بيانات المعلم";
            this.Factories_layoutControlGroup29.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForNotes,
            this.ItemForTheNameAr,
            this.ItemForThePhone1});
            this.Factories_layoutControlGroup29.Location = new System.Drawing.Point(0, 0);
            this.Factories_layoutControlGroup29.Name = "Factories_layoutControlGroup29";
            this.Factories_layoutControlGroup29.OptionsItemText.TextToControlDistance = 3;
            this.Factories_layoutControlGroup29.Padding = new DevExpress.XtraLayout.Utils.Padding(12, 12, 11, 11);
            this.Factories_layoutControlGroup29.Size = new System.Drawing.Size(1425, 121);
            this.Factories_layoutControlGroup29.Spacing = new DevExpress.XtraLayout.Utils.Padding(3, 3, 2, 2);
            this.Factories_layoutControlGroup29.Text = "بيانات المعلمين";
            // 
            // ItemForNotes
            // 
            this.ItemForNotes.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForNotes.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForNotes.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.ItemForNotes.Control = this.NotesTextEdit;
            this.ItemForNotes.ControlAlignment = System.Drawing.ContentAlignment.TopRight;
            this.ItemForNotes.CustomizationFormText = " ملاحظات :";
            this.ItemForNotes.Location = new System.Drawing.Point(0, 26);
            this.ItemForNotes.Name = "ItemForNotes";
            this.ItemForNotes.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 2, 2);
            this.ItemForNotes.Size = new System.Drawing.Size(1393, 41);
            this.ItemForNotes.Text = " ملاحظات :";
            this.ItemForNotes.TextSize = new System.Drawing.Size(78, 17);
            // 
            // ItemForTheNameAr
            // 
            this.ItemForTheNameAr.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForTheNameAr.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForTheNameAr.Control = this.TheNameArTextEdit;
            this.ItemForTheNameAr.ControlAlignment = System.Drawing.ContentAlignment.TopRight;
            this.ItemForTheNameAr.CustomizationFormText = "اسم المعلم :";
            this.ItemForTheNameAr.Location = new System.Drawing.Point(696, 0);
            this.ItemForTheNameAr.Name = "ItemForTheNameAr";
            this.ItemForTheNameAr.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 2, 2);
            this.ItemForTheNameAr.Size = new System.Drawing.Size(697, 26);
            this.ItemForTheNameAr.Text = "اسم المعلم :";
            this.ItemForTheNameAr.TextSize = new System.Drawing.Size(78, 17);
            // 
            // ItemForThePhone1
            // 
            this.ItemForThePhone1.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForThePhone1.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForThePhone1.Control = this.ThePhone1TextEdit;
            this.ItemForThePhone1.ControlAlignment = System.Drawing.ContentAlignment.TopRight;
            this.ItemForThePhone1.CustomizationFormText = "رقم الهاتف 1:";
            this.ItemForThePhone1.Location = new System.Drawing.Point(0, 0);
            this.ItemForThePhone1.Name = "ItemForThePhone1";
            this.ItemForThePhone1.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 2, 2);
            this.ItemForThePhone1.Size = new System.Drawing.Size(696, 26);
            this.ItemForThePhone1.Text = "الجوال:";
            this.ItemForThePhone1.TextSize = new System.Drawing.Size(78, 17);
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.gridControl1;
            this.layoutControlItem1.Location = new System.Drawing.Point(0, 121);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(1425, 443);
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // UC_Factorie
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.bindingSource = this.FactorieBindingSource;
            this.Controls.Add(this.dataLayoutControl1);
            this.dataLayout = this.dataLayoutControl1;
            this.Font = new System.Drawing.Font("Tahoma", 8F);
            this.gridControl = this.gridControl1;
            this.GroupMain = this.Factories_layoutControlGroup29;
            this.Name = "UC_Factorie";
            this.NameAr = "المعلم";
            this.NameEn = "Factorie";
            this.Size = new System.Drawing.Size(1445, 643);
            this.SumNameAr = "المعلمين";
            this.SumNameEn = "Factories";
            this.Controls.SetChildIndex(this.dataLayoutControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.FactorieBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NotesTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TheNameArTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThePhone1TextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Factories_layoutControlGroup29)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForNotes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForTheNameAr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForThePhone1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListName;
        private System.Windows.Forms.BindingSource FactorieBindingSource;
        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.MemoEdit NotesTextEdit;
        private DevExpress.XtraEditors.TextEdit TheNameArTextEdit;
        private DevExpress.XtraEditors.TextEdit ThePhone1TextEdit;
        private DevExpress.XtraLayout.LayoutControlGroup Factories_layoutControlGroup29;
        private DevExpress.XtraLayout.LayoutControlItem ItemForNotes;
        private DevExpress.XtraLayout.LayoutControlItem ItemForTheNameAr;
        private DevExpress.XtraLayout.LayoutControlItem ItemForThePhone1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colTheNameAr;
        private DevExpress.XtraGrid.Columns.GridColumn colTheNameEn;
        private DevExpress.XtraGrid.Columns.GridColumn colTheAddressAr;
        private DevExpress.XtraGrid.Columns.GridColumn colThePhone1;
        private DevExpress.XtraGrid.Columns.GridColumn colThePhone2;
        private DevExpress.XtraGrid.Columns.GridColumn colTheEmail;
        private DevExpress.XtraGrid.Columns.GridColumn colNotes;
        private DevExpress.XtraGrid.Columns.GridColumn colEnterTime;
        private DevExpress.XtraGrid.Columns.GridColumn colUserID;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
    }
}
