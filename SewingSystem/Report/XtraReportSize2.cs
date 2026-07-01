using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using SewingSystem.Classes;
using System.Linq;
using SewingSystem.LinqModel;

namespace SewingSystem.Report
{
    public partial class XtraReportSize2 : DevExpress.XtraReports.UI.XtraReport
    {
        string Tax_Number = "";
        public XtraReportSize2()
        {
            InitializeComponent();
            
        }
        static string BillDate;

        /// <summary>
        /// يضع صور الأنماط (قلابات/كبك/جيوب) في صناديق الصور حسب الرمز (J/K/S/Q)، من
        /// مصدر يُمرَّر من الشاشة (صور الشاشة الحالية). بهذا تطبع الورقة دائماً نفس صور
        /// الشاشة، وأي تغيير لاحق في صور الشاشة يظهر في الطباعة تلقائياً.
        /// المفتاح = رمز النمط، القيمة = الصورة. يُتجاهل أي رمز بلا صورة.
        /// </summary>
        public void SetStyleImages(System.Collections.Generic.IDictionary<string, Image> imgs)
        {
            if (imgs == null) return;
            void Set(XRPictureBox pb, string flag)
            {
                if (pb != null && imgs.TryGetValue(flag, out var im) && im != null)
                    pb.Image = im;
            }
            Set(xrPictureBox4, "J1");  Set(xrPictureBox5, "J2");  Set(xrPictureBox2, "J3");
            Set(xrPictureBox18, "J4"); Set(xrPictureBox3, "J5");  Set(xrPictureBox17, "J6");
            Set(xrPictureBox19, "J7");
            Set(xrPictureBox14, "K1"); Set(xrPictureBox13, "K2"); Set(xrPictureBox15, "K3");
            Set(xrPictureBox16, "K4"); Set(xrPictureBox24, "K5");
            Set(xrPictureBox12, "S1"); Set(xrPictureBox9, "S2");  Set(xrPictureBox11, "S3");
            Set(xrPictureBox31, "Q1"); Set(xrPictureBox7, "Q2");  Set(xrPictureBox6, "Q3");
            Set(xrPictureBox8, "Q4");  Set(xrPictureBox1, "Q5");  Set(xrPictureBox32, "Q6");
            Set(xrPictureBox21, "Q7"); Set(xrPictureBox22, "Q8"); Set(xrPictureBox20, "Q9");
            Set(xrPictureBox23, "Q10");
        }

        private void XtraReportPayAajel_BeforePrint(object sender, CancelEventArgs e)
        {
            // تنسيق صور المقاسات: Zoom بدل Stretch ليحافظ على نسبة الرمز ووضوحه عند الطباعة
            foreach (var pb in this.AllControls<XRPictureBox>())
                pb.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;

            xrLabel1.Text = Program.User.UserName;
         
            if (Properties.Settings.Default.Language == "ar-SA")
            {
                this.ApplyLocalization("ar");
                this.RightToLeft = RightToLeft.No;
                this.RightToLeftLayout = RightToLeftLayout.No;
                xrTableRow20.Visible = xrTableRow21.Visible = false;
                xrTableRow2.Visible = xrTableRow3.Visible = xrTableRow17.Visible = true;
            }
            else if (Properties.Settings.Default.Language == "en-US")
            {
                this.ApplyLocalization("en");
                this.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = RightToLeftLayout.Yes;
                xrTableRow20.Visible = xrTableRow21.Visible = true;
                xrTableRow2.Visible = xrTableRow3.Visible = xrTableRow17.Visible = false;
            }
            lbl_SellDate.Text = BillDate;
            if (Program.Branch != null)
                Tax_Number = Program.Branch.TaxNumber;
         
            if (Program.Branch.UseTax?? false)
            {
                lbl_Total.Visible = true;
                lbl_Total.Row.Visible = true;
                xrTableCell24.Visible = true;
                lbl_Tax.Visible = true;
                lbl_Tax.Row.Visible = true;
                xrTableCell26.Visible = true;
            }
            else
            {
                lbl_Total.Row.Visible = false;
                  lbl_Total.Visible = false;
                xrTableCell24.Visible = false;
                lbl_Tax.Row.Visible = false;
                lbl_Tax.Visible = false;
                xrTableCell26.Visible = false;
            }
        }

        private void xrTableCell49_BeforePrint(object sender, CancelEventArgs e)
        {

        }

        private void tall_BeforePrint(object sender, CancelEventArgs e)
        {

        }
    }
}
