using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SewingSystem.Forms
{
    public partial class XtraFormReport : DevExpress.XtraEditors.XtraForm
    {
        public XtraFormReport()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;   // فتح المعاينة بملء الشاشة ليظهر كامل المحتوى
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.WindowState = FormWindowState.Maximized;
            // الزووم يُضبط بعد اكتمال بناء المستند (وإلا يُعاد تعيينه)، فتظهر الصفحة كاملة
            // داخل نافذة المعاينة ولا يختفي أسفلها (الملاحظات/اسم القماش...).
            var t = new System.Windows.Forms.Timer { Interval = 800 };
            t.Tick += (s, a) =>
            {
                t.Stop(); t.Dispose();
                try { documentViewer1.Zoom = 0.6f; } catch { }
            };
            t.Start();
        }
    }
}