using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SewingSystem.Forms
{
    /// <summary>
    /// نسخة مبسّطة من FormMaster تُستخدم كإطار موحّد لشاشات الزكاة: شريط أدوات علوي
    /// يحوي فقط (حفظ / تحديث / طباعة / إغلاق) — بلا أزرار الإضافة/الحذف/التعديل
    /// أو التنقّل بين السجلات التي لا علاقة لها بشاشات الزكاة.
    /// المحتوى يوضع داخل <see cref="ContentPanel"/>؛ والأزرار تستدعي دوال virtual
    /// تتجاوزها كل شاشة حسب حاجتها (OnSaveClick / OnRefreshClick / OnPrintClick).
    /// </summary>
    public class FormZatcaMaster : DevExpress.XtraEditors.XtraForm
    {
        protected ToolStrip Toolbar { get; private set; }
        public PanelControl ContentPanel { get; private set; }
        protected ToolStripButton BtnSave, BtnRefresh, BtnPrint, BtnClose;

        public FormZatcaMaster()
        {
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;     // اتجاه يمين‑يسار كامل لكل شاشات الزكاة
            ContentPanel = new PanelControl { Dock = DockStyle.Fill };
            ContentPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

            BuildToolbar();
            Controls.Add(ContentPanel);   // fill — added first
            Controls.Add(Toolbar);        // top — added last → sits above the content
        }

        private void BuildToolbar()
        {
            Toolbar = new ToolStrip
            {
                Dock = DockStyle.Top,
                GripStyle = ToolStripGripStyle.Hidden,
                ImageScalingSize = new Size(28, 28),
                RightToLeft = RightToLeft.Yes,
                Padding = new Padding(6, 4, 6, 4),
                AutoSize = false,
                Height = 46,
                Font = new Font("Tahoma", 9.75f, FontStyle.Bold)
            };

            BtnSave = MkBtn("حفظ", SewingSystem.Properties.Resources.saveall_32x32, (s, e) => OnSaveClick());
            BtnRefresh = MkBtn("تحديث", SewingSystem.Properties.Resources.refresh_32x32, (s, e) => OnRefreshClick());
            BtnPrint = MkBtn("طباعة", SewingSystem.Properties.Resources.printer_32x32, (s, e) => OnPrintClick());
            BtnPrint.Visible = false;     // shown only by forms that support printing
            BtnClose = MkBtn("إغلاق", SewingSystem.Properties.Resources.cancel_32x32, (s, e) => Close());

            Toolbar.Items.AddRange(new ToolStripItem[]
            {
                BtnSave, BtnRefresh, BtnPrint, new ToolStripSeparator(), BtnClose
            });
        }

        private ToolStripButton MkBtn(string text, Image img, EventHandler click)
        {
            return new ToolStripButton(text, img, click)
            {
                DisplayStyle = ToolStripItemDisplayStyle.ImageAndText,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                AutoSize = true,
                Padding = new Padding(8, 0, 8, 0)
            };
        }

        /// <summary>زر «حفظ» — تتجاوزه الشاشة لتنفيذ الحفظ.</summary>
        protected virtual void OnSaveClick() { }

        /// <summary>زر «تحديث» — تتجاوزه الشاشة لإعادة التحميل.</summary>
        protected virtual void OnRefreshClick() { }

        /// <summary>زر «طباعة» — تتجاوزه الشاشات التي تدعم الطباعة.</summary>
        protected virtual void OnPrintClick() { }
    }
}
