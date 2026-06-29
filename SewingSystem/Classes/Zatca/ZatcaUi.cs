using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SewingSystem.Classes.Zatca
{
    /// <summary>
    /// عناصر واجهة موحّدة لشاشات الزكاة: أيقونة الفورم وشريط علوي (Header) احترافي
    /// بلون الهيئة التركوازي. تُستخدم من كل شاشات الزكاة للحفاظ على مظهر موحّد.
    /// </summary>
    public static class ZatcaUi
    {
        public static readonly Color Teal = Color.FromArgb(14, 124, 123);

        /// <summary>يضع أيقونة الزكاة على شريط عنوان الفورم وشريط المهام.</summary>
        public static void ApplyIcon(XtraForm form)
        {
            try
            {
                form.IconOptions.Image = ZatcaIcon.Get(32);
                using (var bmp = new Bitmap(ZatcaIcon.Get(32)))
                    form.Icon = Icon.FromHandle(bmp.GetHicon());
            }
            catch { /* الأيقونة تجميلية — لا توقف الفورم أبداً */ }
        }

        /// <summary>
        /// يجعل الحقل يُظهر الأرقام إنجليزية (لاتينية) بدل العربية الهندية: السبب أن
        /// تشكيل الأرقام في GDI يتبع اتجاه الحقل؛ فبجعل الحقل LTR تظهر الأرقام لاتينية،
        /// مع محاذاة لليمين ليبقى شكله مناسباً داخل الفورم العربي.
        /// </summary>
        public static void Ltr(params BaseEdit[] edits)
        {
            foreach (var e in edits)
            {
                if (e == null) continue;
                e.RightToLeft = RightToLeft.No;
                e.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Properties.Appearance.Options.UseTextOptions = true;
            }
        }

        /// <summary>شريط علوي ملوّن يحمل أيقونة الزكاة وعنواناً رئيسياً وفرعياً (RTL).</summary>
        public static PanelControl Header(string title, string subtitle)
        {
            var header = new PanelControl { Dock = DockStyle.Top, Height = 64 };
            header.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            header.Appearance.BackColor = Teal;
            header.Appearance.Options.UseBackColor = true;
            header.RightToLeft = RightToLeft.Yes;

            var pic = new PictureEdit { Dock = DockStyle.Right, Width = 64, Image = ZatcaIcon.Get(40) };
            pic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            pic.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            pic.Properties.ShowMenu = false;
            pic.Properties.Appearance.BackColor = Teal;
            pic.Properties.Appearance.Options.UseBackColor = true;

            var lbl = new LabelControl
            {
                Dock = DockStyle.Fill,
                Text = string.IsNullOrEmpty(subtitle) ? title : title + "\r\n" + subtitle,
                Padding = new System.Windows.Forms.Padding(14, 0, 8, 0)
            };
            lbl.Appearance.Font = new Font("Tahoma", 12.75f, FontStyle.Bold);
            lbl.Appearance.ForeColor = Color.White;
            lbl.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            lbl.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            lbl.Appearance.Options.UseFont = true;
            lbl.Appearance.Options.UseForeColor = true;
            lbl.Appearance.Options.UseTextOptions = true;

            header.Controls.Add(lbl);
            header.Controls.Add(pic);
            return header;
        }
    }
}
