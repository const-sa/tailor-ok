namespace SewingSystem.Forms
{
    partial class XtraFormLogin
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
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule1 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule2 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule conditionValidationRule3 = new DevExpress.XtraEditors.DXErrorProvider.ConditionValidationRule();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XtraFormLogin));
            this.dataLayoutControl1 = new DevExpress.XtraDataLayout.DataLayoutControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.comboBoxEditLang = new DevExpress.XtraEditors.ComboBoxEdit();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnLogIn = new DevExpress.XtraEditors.SimpleButton();
            this.UserNameTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.UserPasswordTextEdit = new DevExpress.XtraEditors.TextEdit();
            this.BranchIDTextEdit = new DevExpress.XtraEditors.LookUpEdit();
            this.tblBranchesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel11 = new System.Windows.Forms.Panel();
            this.tblUsersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.Root = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.ItemForBranchID = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForUserName = new DevExpress.XtraLayout.LayoutControlItem();
            this.ItemForUserPassword = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem5 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem4 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
            this.dxValidationProvider1 = new DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider(this.components);
            this.checkEdit11 = new DevExpress.XtraEditors.CheckEdit();
            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).BeginInit();
            this.dataLayoutControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditLang.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserNameTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserPasswordTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BranchIDTextEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblBranchesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblUsersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBranchID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUserName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUserPassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit11.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
            this.SuspendLayout();
            // 
            // dataLayoutControl1
            // 
            this.dataLayoutControl1.Appearance.Control.BackColor = System.Drawing.Color.SeaGreen;
            this.dataLayoutControl1.Appearance.Control.BackColor2 = System.Drawing.Color.SeaGreen;
            this.dataLayoutControl1.Appearance.Control.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal;
            this.dataLayoutControl1.Appearance.Control.Options.UseBackColor = true;
            this.dataLayoutControl1.Controls.Add(this.labelControl1);
            this.dataLayoutControl1.Controls.Add(this.comboBoxEditLang);
            this.dataLayoutControl1.Controls.Add(this.btnCancel);
            this.dataLayoutControl1.Controls.Add(this.btnLogIn);
            this.dataLayoutControl1.Controls.Add(this.UserNameTextEdit);
            this.dataLayoutControl1.Controls.Add(this.UserPasswordTextEdit);
            this.dataLayoutControl1.Controls.Add(this.BranchIDTextEdit);
            this.dataLayoutControl1.Controls.Add(this.checkEdit11);
            this.dataLayoutControl1.Controls.Add(this.panel11);
            this.dataLayoutControl1.DataSource = this.tblUsersBindingSource;
            this.dataLayoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataLayoutControl1.Location = new System.Drawing.Point(5, 10);
            this.dataLayoutControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataLayoutControl1.Name = "dataLayoutControl1";
            this.dataLayoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(495, 328, 650, 400);
            this.dataLayoutControl1.OptionsView.RightToLeftMirroringApplied = true;
            this.dataLayoutControl1.Root = this.Root;
            this.dataLayoutControl1.Size = new System.Drawing.Size(635, 267);
            this.dataLayoutControl1.TabIndex = 3;
            this.dataLayoutControl1.Text = "dataLayoutControl1";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Appearance.Options.UseTextOptions = true;
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Bottom;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(8, 2);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Padding = new System.Windows.Forms.Padding(5);
            this.labelControl1.Size = new System.Drawing.Size(619, 64);
            this.labelControl1.StyleController = this.dataLayoutControl1;
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Tailoring Shop System\r\nنظام محلات الخياطة";
            // 
            // comboBoxEditLang
            // 
            this.comboBoxEditLang.Location = new System.Drawing.Point(305, 163);
            this.comboBoxEditLang.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxEditLang.Name = "comboBoxEditLang";
            this.comboBoxEditLang.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.comboBoxEditLang.Properties.Appearance.Options.UseBackColor = true;
            this.comboBoxEditLang.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.comboBoxEditLang.Properties.AppearanceDropDown.Options.UseFont = true;
            this.comboBoxEditLang.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.comboBoxEditLang.Properties.Items.AddRange(new object[] {
            "اللغة العربية",
            "English"});
            this.comboBoxEditLang.Size = new System.Drawing.Size(206, 22);
            this.comboBoxEditLang.StyleController = this.dataLayoutControl1;
            this.comboBoxEditLang.TabIndex = 10;
            this.comboBoxEditLang.EditValueChanging += new DevExpress.XtraEditors.Controls.ChangingEventHandler(this.comboBoxEditLang_EditValueChanging);
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance.Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Appearance.Options.UseFont = true;
            this.btnCancel.ImageOptions.Image = global::SewingSystem.Properties.Resources.cancel_32x321;
            this.btnCancel.Location = new System.Drawing.Point(293, 227);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(118, 32);
            this.btnCancel.StyleController = this.dataLayoutControl1;
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "خروج";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnLogIn
            // 
            this.btnLogIn.Appearance.Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Bold);
            this.btnLogIn.Appearance.Options.UseFont = true;
            this.btnLogIn.ImageOptions.Image = global::SewingSystem.Properties.Resources.apply_32x32;
            this.btnLogIn.Location = new System.Drawing.Point(415, 227);
            this.btnLogIn.Margin = new System.Windows.Forms.Padding(5);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(212, 32);
            this.btnLogIn.StyleController = this.dataLayoutControl1;
            this.btnLogIn.TabIndex = 7;
            this.btnLogIn.Text = "تسجيل الدخول";
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // UserNameTextEdit
            // 
            this.UserNameTextEdit.Location = new System.Drawing.Point(305, 109);
            this.UserNameTextEdit.Margin = new System.Windows.Forms.Padding(5);
            this.UserNameTextEdit.Name = "UserNameTextEdit";
            this.UserNameTextEdit.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.UserNameTextEdit.Properties.Appearance.Options.UseBackColor = true;
            this.UserNameTextEdit.Properties.Name = "UserNameTextEdit";
            this.UserNameTextEdit.Size = new System.Drawing.Size(206, 22);
            this.UserNameTextEdit.StyleController = this.dataLayoutControl1;
            this.UserNameTextEdit.TabIndex = 4;
            conditionValidationRule1.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule1.ErrorText = "This value is not valid";
            this.dxValidationProvider1.SetValidationRule(this.UserNameTextEdit, conditionValidationRule1);
            // 
            // UserPasswordTextEdit
            // 
            this.UserPasswordTextEdit.Location = new System.Drawing.Point(305, 136);
            this.UserPasswordTextEdit.Margin = new System.Windows.Forms.Padding(5);
            this.UserPasswordTextEdit.Name = "UserPasswordTextEdit";
            this.UserPasswordTextEdit.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.UserPasswordTextEdit.Properties.Appearance.Options.UseBackColor = true;
            this.UserPasswordTextEdit.Properties.Name = "UserPasswordTextEdit";
            this.UserPasswordTextEdit.Properties.PasswordChar = '*';
            this.UserPasswordTextEdit.Properties.UseSystemPasswordChar = true;
            this.UserPasswordTextEdit.Size = new System.Drawing.Size(206, 22);
            this.UserPasswordTextEdit.StyleController = this.dataLayoutControl1;
            this.UserPasswordTextEdit.TabIndex = 5;
            conditionValidationRule2.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule2.ErrorText = "This value is not valid";
            this.dxValidationProvider1.SetValidationRule(this.UserPasswordTextEdit, conditionValidationRule2);
            // 
            // BranchIDTextEdit
            // 
            this.BranchIDTextEdit.Location = new System.Drawing.Point(305, 82);
            this.BranchIDTextEdit.Margin = new System.Windows.Forms.Padding(5);
            this.BranchIDTextEdit.Name = "BranchIDTextEdit";
            this.BranchIDTextEdit.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.BranchIDTextEdit.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.BranchIDTextEdit.Properties.Appearance.Options.UseBackColor = true;
            this.BranchIDTextEdit.Properties.Appearance.Options.UseTextOptions = true;
            this.BranchIDTextEdit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.BranchIDTextEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.BranchIDTextEdit.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("BranchName", "", 126, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near, DevExpress.Data.ColumnSortOrder.None, DevExpress.Utils.DefaultBoolean.Default)});
            this.BranchIDTextEdit.Properties.DataSource = this.tblBranchesBindingSource;
            this.BranchIDTextEdit.Properties.DisplayMember = "BranchName";
            this.BranchIDTextEdit.Properties.NullText = "";
            this.BranchIDTextEdit.Properties.ShowFooter = false;
            this.BranchIDTextEdit.Properties.ShowHeader = false;
            this.BranchIDTextEdit.Properties.ShowLines = false;
            this.BranchIDTextEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.BranchIDTextEdit.Properties.ValueMember = "ID";
            this.BranchIDTextEdit.Size = new System.Drawing.Size(206, 22);
            this.BranchIDTextEdit.StyleController = this.dataLayoutControl1;
            this.BranchIDTextEdit.TabIndex = 6;
            conditionValidationRule3.ConditionOperator = DevExpress.XtraEditors.DXErrorProvider.ConditionOperator.IsNotBlank;
            conditionValidationRule3.ErrorText = "This value is not valid";
            this.dxValidationProvider1.SetValidationRule(this.BranchIDTextEdit, conditionValidationRule3);
            // 
            // tblBranchesBindingSource
            // 
            this.tblBranchesBindingSource.DataSource = typeof(SewingSystem.LinqModel.tblBranche);
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.Silver;
            this.panel11.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel11.BackgroundImage")));
            this.panel11.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel11.Location = new System.Drawing.Point(8, 70);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(281, 170);
            this.panel11.TabIndex = 0;
            // 
            // tblUsersBindingSource
            // 
            this.tblUsersBindingSource.DataSource = typeof(SewingSystem.LinqModel.tblUser);
            // 
            // Root
            // 
            this.Root.AppearanceGroup.Options.UseFont = true;
            this.Root.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.Root.GroupBordersVisible = false;
            this.Root.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup1});
            this.Root.Name = "Root";
            this.Root.Padding = new DevExpress.XtraLayout.Utils.Padding(6, 6, 0, 6);
            this.Root.Size = new System.Drawing.Size(635, 267);
            this.Root.TextVisible = false;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.AllowDrawBackground = false;
            this.layoutControlGroup1.GroupBordersVisible = false;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.layoutControlGroup2,
            this.layoutControlItem6,
            this.layoutControlItem4,
            this.layoutControlItem2,
            this.layoutControlItem1});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "autoGeneratedGroup0";
            this.layoutControlGroup1.Size = new System.Drawing.Size(623, 261);
            // 
            // layoutControlGroup2
            // 
            this.layoutControlGroup2.AppearanceGroup.BorderColor = System.Drawing.Color.Gray;
            this.layoutControlGroup2.AppearanceGroup.Options.UseBorderColor = true;
            this.layoutControlGroup2.AppearanceItemCaption.BorderColor = System.Drawing.Color.Silver;
            this.layoutControlGroup2.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.layoutControlGroup2.AppearanceItemCaption.Options.UseBorderColor = true;
            this.layoutControlGroup2.AppearanceItemCaption.Options.UseFont = true;
            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.ItemForBranchID,
            this.ItemForUserName,
            this.ItemForUserPassword,
            this.layoutControlItem5,
            this.layoutControlItem3});
            this.layoutControlGroup2.Location = new System.Drawing.Point(285, 68);
            this.layoutControlGroup2.Name = "layoutControlGroup2";
            this.layoutControlGroup2.Size = new System.Drawing.Size(338, 157);
            this.layoutControlGroup2.TextVisible = false;
            // 
            // ItemForBranchID
            // 
            this.ItemForBranchID.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForBranchID.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForBranchID.Control = this.BranchIDTextEdit;
            this.ItemForBranchID.Location = new System.Drawing.Point(0, 0);
            this.ItemForBranchID.Name = "ItemForBranchID";
            this.ItemForBranchID.Size = new System.Drawing.Size(314, 27);
            this.ItemForBranchID.Text = "الفرع :";
            this.ItemForBranchID.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.ItemForBranchID.TextSize = new System.Drawing.Size(99, 23);
            this.ItemForBranchID.TextToControlDistance = 5;
            // 
            // ItemForUserName
            // 
            this.ItemForUserName.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForUserName.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForUserName.Control = this.UserNameTextEdit;
            this.ItemForUserName.Location = new System.Drawing.Point(0, 27);
            this.ItemForUserName.Name = "ItemForUserName";
            this.ItemForUserName.Size = new System.Drawing.Size(314, 27);
            this.ItemForUserName.Text = "اسم المستخدم :";
            this.ItemForUserName.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.ItemForUserName.TextSize = new System.Drawing.Size(99, 23);
            this.ItemForUserName.TextToControlDistance = 5;
            // 
            // ItemForUserPassword
            // 
            this.ItemForUserPassword.AppearanceItemCaption.Options.UseTextOptions = true;
            this.ItemForUserPassword.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.ItemForUserPassword.AppearanceItemCaption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.ItemForUserPassword.Control = this.UserPasswordTextEdit;
            this.ItemForUserPassword.Location = new System.Drawing.Point(0, 54);
            this.ItemForUserPassword.Name = "ItemForUserPassword";
            this.ItemForUserPassword.Size = new System.Drawing.Size(314, 27);
            this.ItemForUserPassword.Text = "كلمة المرور :";
            this.ItemForUserPassword.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.ItemForUserPassword.TextSize = new System.Drawing.Size(99, 23);
            this.ItemForUserPassword.TextToControlDistance = 5;
            // 
            // layoutControlItem5
            // 
            this.layoutControlItem5.AppearanceItemCaption.Options.UseTextOptions = true;
            this.layoutControlItem5.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.layoutControlItem5.Control = this.comboBoxEditLang;
            this.layoutControlItem5.Location = new System.Drawing.Point(0, 81);
            this.layoutControlItem5.Name = "layoutControlItem5";
            this.layoutControlItem5.Size = new System.Drawing.Size(314, 27);
            this.layoutControlItem5.Text = "اللغة  :";
            this.layoutControlItem5.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.layoutControlItem5.TextSize = new System.Drawing.Size(99, 23);
            this.layoutControlItem5.TextToControlDistance = 5;
            // 
            // layoutControlItem6
            // 
            this.layoutControlItem6.Control = this.labelControl1;
            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
            this.layoutControlItem6.Name = "layoutControlItem6";
            this.layoutControlItem6.Size = new System.Drawing.Size(623, 68);
            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem4
            // 
            this.layoutControlItem4.Control = this.panel11;
            this.layoutControlItem4.ControlAlignment = System.Drawing.ContentAlignment.TopRight;
            this.layoutControlItem4.CustomizationFormText = "layoutControlItem3";
            this.layoutControlItem4.Location = new System.Drawing.Point(0, 68);
            this.layoutControlItem4.Name = "layoutControlItem4";
            this.layoutControlItem4.Size = new System.Drawing.Size(285, 193);
            this.layoutControlItem4.Text = "كواكب التقنية لخدمات الويب والبرامج - 0506499275";
            this.layoutControlItem4.TextLocation = DevExpress.Utils.Locations.Bottom;
            this.layoutControlItem4.TextSize = new System.Drawing.Size(280, 16);
            // 
            // layoutControlItem2
            // 
            this.layoutControlItem2.Control = this.btnCancel;
            this.layoutControlItem2.Location = new System.Drawing.Point(285, 225);
            this.layoutControlItem2.MinSize = new System.Drawing.Size(79, 34);
            this.layoutControlItem2.Name = "layoutControlItem2";
            this.layoutControlItem2.Size = new System.Drawing.Size(122, 36);
            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlItem1
            // 
            this.layoutControlItem1.Control = this.btnLogIn;
            this.layoutControlItem1.Location = new System.Drawing.Point(407, 225);
            this.layoutControlItem1.MinSize = new System.Drawing.Size(132, 34);
            this.layoutControlItem1.Name = "layoutControlItem1";
            this.layoutControlItem1.Size = new System.Drawing.Size(216, 36);
            this.layoutControlItem1.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
            this.layoutControlItem1.TextVisible = false;
            // 
            // checkEdit11
            // 
            this.checkEdit11.DataBindings.Add(new System.Windows.Forms.Binding("EditValue", global::SewingSystem.Properties.Settings.Default, "Remember_me", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkEdit11.EditValue = global::SewingSystem.Properties.Settings.Default.Remember_me;
            this.checkEdit11.Location = new System.Drawing.Point(305, 190);
            this.checkEdit11.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.checkEdit11.Name = "checkEdit11";
            this.checkEdit11.Properties.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.checkEdit11.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 10F, System.Drawing.FontStyle.Bold);
            this.checkEdit11.Properties.Appearance.Options.UseBackColor = true;
            this.checkEdit11.Properties.Appearance.Options.UseFont = true;
            this.checkEdit11.Properties.Caption = "تذكرني";
            this.checkEdit11.Size = new System.Drawing.Size(206, 21);
            this.checkEdit11.StyleController = this.dataLayoutControl1;
            this.checkEdit11.TabIndex = 11;
            // 
            // layoutControlItem3
            // 
            this.layoutControlItem3.ContentHorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.layoutControlItem3.Control = this.checkEdit11;
            this.layoutControlItem3.ControlAlignment = System.Drawing.ContentAlignment.TopRight;
            this.layoutControlItem3.CustomizationFormText = "                   ";
            this.layoutControlItem3.Location = new System.Drawing.Point(0, 108);
            this.layoutControlItem3.Name = "layoutControlItem3";
            this.layoutControlItem3.OptionsPrint.AppearanceItem.Options.UseFont = true;
            this.layoutControlItem3.OptionsPrint.AppearanceItemControl.Options.UseFont = true;
            this.layoutControlItem3.OptionsPrint.AppearanceItemText.Options.UseFont = true;
            this.layoutControlItem3.Size = new System.Drawing.Size(314, 25);
            this.layoutControlItem3.Text = "                                 ";
            this.layoutControlItem3.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.AutoSize;
            this.layoutControlItem3.TextSize = new System.Drawing.Size(99, 13);
            this.layoutControlItem3.TextToControlDistance = 5;
            // 
            // XtraFormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 282);
            this.Controls.Add(this.dataLayoutControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IconOptions.Image = global::SewingSystem.Properties.Resources.LOGO3;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "XtraFormLogin";
            this.Padding = new System.Windows.Forms.Padding(5, 10, 5, 5);
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "XtraFormLogin";
            this.Load += new System.EventHandler(this.LogIn_Load);
            this.Shown += new System.EventHandler(this.LogIn_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataLayoutControl1)).EndInit();
            this.dataLayoutControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEditLang.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserNameTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UserPasswordTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BranchIDTextEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblBranchesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tblUsersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Root)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForBranchID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUserName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ItemForUserPassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dxValidationProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.checkEdit11.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource tblBranchesBindingSource;
        private System.Windows.Forms.BindingSource tblUsersBindingSource;
        private DevExpress.XtraDataLayout.DataLayoutControl dataLayoutControl1;
        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEditLang;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.SimpleButton btnLogIn;
        private DevExpress.XtraEditors.TextEdit UserNameTextEdit;
        private DevExpress.XtraEditors.TextEdit UserPasswordTextEdit;
        private DevExpress.XtraEditors.LookUpEdit BranchIDTextEdit;
        private DevExpress.XtraLayout.LayoutControlGroup Root;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
        private DevExpress.XtraLayout.LayoutControlItem ItemForBranchID;
        private DevExpress.XtraLayout.LayoutControlItem ItemForUserName;
        private DevExpress.XtraLayout.LayoutControlItem ItemForUserPassword;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem5;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
        private DevExpress.XtraEditors.CheckEdit checkEdit11;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
        private System.Windows.Forms.Panel panel11;
        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem4;
        private DevExpress.XtraEditors.DXErrorProvider.DXValidationProvider dxValidationProvider1;
    }
}