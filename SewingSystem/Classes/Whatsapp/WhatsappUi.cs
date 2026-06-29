using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SewingSystem.Classes.Whatsapp
{
    /// <summary>
    /// عناصر واجهة موحّدة لشاشة الواتساب: أيقونة الفورم وشريط علوي (Header) احترافي
    /// بلون الواتساب الداكن مع أيقونة الفقاعة الخضراء.
    /// </summary>
    public static class WhatsappUi
    {
        public static readonly Color BrandDark = Color.FromArgb(7, 94, 84);   // WhatsApp dark teal-green (#075E54)

        /// <summary>يضع أيقونة الواتساب على شريط عنوان الفورم وشريط المهام.</summary>
        public static void ApplyIcon(XtraForm form)
        {
            try
            {
                form.IconOptions.Image = WhatsappIcon.Get(32);
                using (var bmp = new Bitmap(WhatsappIcon.Get(32)))
                    form.Icon = Icon.FromHandle(bmp.GetHicon());
            }
            catch { /* الأيقونة تجميلية — لا توقف الفورم أبداً */ }
        }

        /// <summary>
        /// يجعل الحقل يُظهر الأرقام إنجليزية (لاتينية): تشكيل أرقام GDI يتبع اتجاه الحقل،
        /// فبجعله LTR تظهر الأرقام لاتينية مع محاذاة لليمين.
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

        /// <summary>شريط علوي أخضر يحمل أيقونة الواتساب وعنواناً رئيسياً وفرعياً (RTL).</summary>
        public static PanelControl Header(string title, string subtitle)
        {
            var header = new PanelControl { Dock = DockStyle.Top, Height = 64 };
            header.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            header.Appearance.BackColor = BrandDark;
            header.Appearance.Options.UseBackColor = true;
            header.RightToLeft = RightToLeft.Yes;

            var pic = new PictureEdit { Dock = DockStyle.Right, Width = 64, Image = WhatsappIcon.Get(40) };
            pic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            pic.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            pic.Properties.ShowMenu = false;
            pic.Properties.Appearance.BackColor = BrandDark;
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
