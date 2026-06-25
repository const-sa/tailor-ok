namespace SewingSystem.Forms
{
    partial class FormMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMaster));
            this.bindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
            this.btnAddNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.Movefirst = new System.Windows.Forms.ToolStripButton();
            this.Moveprevious = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator19 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripTextBox7 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator20 = new System.Windows.Forms.ToolStripSeparator();
            this.Movenext = new System.Windows.Forms.ToolStripButton();
            this.Movelast = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator21 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnUpdatePermission = new System.Windows.Forms.ToolStripButton();
            this.btnReset = new System.Windows.Forms.ToolStripButton();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator)).BeginInit();
            this.bindingNavigator.SuspendLayout();
            this.SuspendLayout();
            // 
            // bindingNavigator
            // 
            this.bindingNavigator.AddNewItem = this.btnAddNew;
            resources.ApplyResources(this.bindingNavigator, "bindingNavigator");
            this.bindingNavigator.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.bindingNavigator.CountItem = this.toolStripLabel7;
            this.bindingNavigator.DeleteItem = null;
            this.bindingNavigator.ImageScalingSize = new System.Drawing.Size(36, 36);
            this.bindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Movefirst,
            this.Moveprevious,
            this.toolStripSeparator19,
            this.toolStripTextBox7,
            this.toolStripLabel7,
            this.toolStripSeparator20,
            this.Movenext,
            this.Movelast,
            this.toolStripSeparator21,
            this.btnAddNew,
            this.btnSave,
            this.btnUpdatePermission,
            this.btnReset,
            this.btnUpdate,
            this.btnPrint,
            this.btnDelete,
            this.btnRefresh,
            this.btnClose});
            this.bindingNavigator.MoveFirstItem = this.Movefirst;
            this.bindingNavigator.MoveLastItem = this.Movelast;
            this.bindingNavigator.MoveNextItem = this.Movenext;
            this.bindingNavigator.MovePreviousItem = this.Moveprevious;
            this.bindingNavigator.Name = "bindingNavigator";
            this.bindingNavigator.PositionItem = this.toolStripTextBox7;
            // 
            // btnAddNew
            // 
            this.btnAddNew.Image = global::SewingSystem.Properties.Resources.add_32x32;
            this.btnAddNew.Name = "btnAddNew";
            resources.ApplyResources(this.btnAddNew, "btnAddNew");
            // 
            // toolStripLabel7
            // 
            resources.ApplyResources(this.toolStripLabel7, "toolStripLabel7");
            this.toolStripLabel7.Name = "toolStripLabel7";
            // 
            // Movefirst
            // 
            this.Movefirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.Movefirst, "Movefirst");
            this.Movefirst.Name = "Movefirst";
            // 
            // Moveprevious
            // 
            this.Moveprevious.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.Moveprevious, "Moveprevious");
            this.Moveprevious.Name = "Moveprevious";
            // 
            // toolStripSeparator19
            // 
            this.toolStripSeparator19.Name = "toolStripSeparator19";
            resources.ApplyResources(this.toolStripSeparator19, "toolStripSeparator19");
            // 
            // toolStripTextBox7
            // 
            resources.ApplyResources(this.toolStripTextBox7, "toolStripTextBox7");
            this.toolStripTextBox7.Name = "toolStripTextBox7";
            // 
            // toolStripSeparator20
            // 
            this.toolStripSeparator20.Name = "toolStripSeparator20";
            resources.ApplyResources(this.toolStripSeparator20, "toolStripSeparator20");
            // 
            // Movenext
            // 
            this.Movenext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.Movenext, "Movenext");
            this.Movenext.Name = "Movenext";
            // 
            // Movelast
            // 
            this.Movelast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.Movelast, "Movelast");
            this.Movelast.Name = "Movelast";
            // 
            // toolStripSeparator21
            // 
            this.toolStripSeparator21.Name = "toolStripSeparator21";
            resources.ApplyResources(this.toolStripSeparator21, "toolStripSeparator21");
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdatePermission
            // 
            resources.ApplyResources(this.btnUpdatePermission, "btnUpdatePermission");
            this.btnUpdatePermission.Image = global::SewingSystem.Properties.Resources.bopermission_32x32;
            this.btnUpdatePermission.Name = "btnUpdatePermission";
            // 
            // btnReset
            // 
            resources.ApplyResources(this.btnReset, "btnReset");
            this.btnReset.Name = "btnReset";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::SewingSystem.Properties.Resources.editcontact_32x32;
            this.btnUpdate.Name = "btnUpdate";
            resources.ApplyResources(this.btnUpdate, "btnUpdate");
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.Name = "btnPrint";
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            // 
            // btnRefresh
            // 
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Name = "btnRefresh";
            // 
            // btnClose
            // 
            this.btnClose.Image = global::SewingSystem.Properties.Resources.cancel_32x32;
            this.btnClose.Name = "btnClose";
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormMaster
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.bindingNavigator);
            this.Name = "FormMaster";
            this.Load += new System.EventHandler(this.UC_MasterAll_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator)).EndInit();
            this.bindingNavigator.ResumeLayout(false);
            this.bindingNavigator.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.BindingNavigator bindingNavigator;
        public System.Windows.Forms.ToolStripButton btnAddNew;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator19;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator20;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator21;
        public System.Windows.Forms.ToolStripButton btnSave;
        public System.Windows.Forms.ToolStripButton btnUpdatePermission;
        public System.Windows.Forms.ToolStripButton btnReset;
        public System.Windows.Forms.ToolStripButton btnUpdate;
        public System.Windows.Forms.ToolStripButton btnPrint;
        public System.Windows.Forms.ToolStripButton btnDelete;
        public System.Windows.Forms.ToolStripButton btnRefresh;
        public System.Windows.Forms.ToolStripButton btnClose;
        public System.Windows.Forms.ToolStripButton Movefirst;
        public System.Windows.Forms.ToolStripButton Moveprevious;
        public System.Windows.Forms.ToolStripButton Movenext;
        public System.Windows.Forms.ToolStripButton Movelast;
    }
}
