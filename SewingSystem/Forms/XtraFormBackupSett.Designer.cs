namespace SewingSystem.Forms
{
    partial class XtraFormBackupSett
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
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.btn_save = new DevExpress.XtraEditors.SimpleButton();
            this.btnBrows = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.txt_path = new DevExpress.XtraEditors.TextEdit();
            this.txt_days = new DevExpress.XtraEditors.TextEdit();
            this.txt_Hours = new DevExpress.XtraEditors.TextEdit();
            this.btn_backup = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_path.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_days.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Hours.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.btn_save);
            this.groupControl1.Controls.Add(this.btnBrows);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.labelControl2);
            this.groupControl1.Controls.Add(this.labelControl5);
            this.groupControl1.Controls.Add(this.labelControl3);
            this.groupControl1.Controls.Add(this.labelControl4);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Controls.Add(this.txt_path);
            this.groupControl1.Controls.Add(this.txt_days);
            this.groupControl1.Controls.Add(this.txt_Hours);
            this.groupControl1.Location = new System.Drawing.Point(39, 21);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(433, 294);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "إعدادت النسخ الاحتياطي";
            // 
            // btn_save
            // 
            this.btn_save.ImageOptions.Image = global::SewingSystem.Properties.Resources.saveall_32x32;
            this.btn_save.Location = new System.Drawing.Point(154, 221);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(112, 41);
            this.btn_save.TabIndex = 2;
            this.btn_save.Text = "حفظ";
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btnBrows
            // 
            this.btnBrows.Location = new System.Drawing.Point(9, 174);
            this.btnBrows.Name = "btnBrows";
            this.btnBrows.Size = new System.Drawing.Size(30, 23);
            this.btnBrows.TabIndex = 2;
            this.btnBrows.Text = "...";
            this.btnBrows.Click += new System.EventHandler(this.btnBrows_Click);
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelControl6.Appearance.Options.UseFont = true;
            this.labelControl6.Location = new System.Drawing.Point(269, 153);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(147, 16);
            this.labelControl6.TabIndex = 1;
            this.labelControl6.Text = "مسار النسخ الاحتياطي:";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(184, 46);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(232, 16);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "معدل تكرار النسخ الاحتياطي التلقائي:";
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(264, 106);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(18, 16);
            this.labelControl5.TabIndex = 1;
            this.labelControl5.Text = "يوم";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(392, 106);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(15, 16);
            this.labelControl3.TabIndex = 1;
            this.labelControl3.Text = "كل";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(247, 78);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(32, 16);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "ساعة";
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(392, 78);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(15, 16);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "كل";
            // 
            // txt_path
            // 
            this.txt_path.Location = new System.Drawing.Point(41, 175);
            this.txt_path.Name = "txt_path";
            this.txt_path.Size = new System.Drawing.Size(344, 22);
            this.txt_path.TabIndex = 0;
            // 
            // txt_days
            // 
            this.txt_days.Location = new System.Drawing.Point(285, 103);
            this.txt_days.Name = "txt_days";
            this.txt_days.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txt_days.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
            this.txt_days.Properties.MaskSettings.Set("mask", "d");
            this.txt_days.Size = new System.Drawing.Size(100, 22);
            this.txt_days.TabIndex = 0;
            // 
            // txt_Hours
            // 
            this.txt_Hours.Location = new System.Drawing.Point(285, 75);
            this.txt_Hours.Name = "txt_Hours";
            this.txt_Hours.Properties.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.txt_Hours.Properties.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
            this.txt_Hours.Properties.MaskSettings.Set("mask", "d");
            this.txt_Hours.Size = new System.Drawing.Size(100, 22);
            this.txt_Hours.TabIndex = 0;
            // 
            // btn_backup
            // 
            this.btn_backup.ImageOptions.Image = global::SewingSystem.Properties.Resources.database_32x322;
            this.btn_backup.Location = new System.Drawing.Point(161, 348);
            this.btn_backup.Name = "btn_backup";
            this.btn_backup.Size = new System.Drawing.Size(225, 41);
            this.btn_backup.TabIndex = 2;
            this.btn_backup.Text = "أخذ نسخة احتياطية الأن";
            this.btn_backup.Click += new System.EventHandler(this.btn_backup_Click);
            // 
            // XtraFormBackupSett
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 422);
            this.Controls.Add(this.btn_backup);
            this.Controls.Add(this.groupControl1);
            this.Name = "XtraFormBackupSett";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.RightToLeftLayout = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "إعداد النسخ الاحتياطي";
            this.Load += new System.EventHandler(this.XtraFormBackupSett_Load);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txt_path.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_days.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_Hours.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit txt_days;
        private DevExpress.XtraEditors.TextEdit txt_Hours;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.SimpleButton btnBrows;
        private DevExpress.XtraEditors.TextEdit txt_path;
        private DevExpress.XtraEditors.SimpleButton btn_save;
        private DevExpress.XtraEditors.SimpleButton btn_backup;
    }
}