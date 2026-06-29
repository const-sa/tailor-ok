using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SewingSystem.Classes;

namespace SewingSystem.Forms
{
    /// <summary>
    /// شاشة مستقلة لإدارة «صور المقاسات». تقرأ الصور فعليّاً من شاشة التفصيل (DefultSize2) وقت
    /// التشغيل وتجمع المكرّر، فتظهر كل صورة مرة واحدة. الاستبدال ينطبق على كل نسخ نفس الصورة.
    /// الاسم/الترتيب/الصورة المستبدلة تُخزّن في dbo.tblSizeImages.
    /// </summary>
    public class XtraFormSizeImages : DevExpress.XtraEditors.XtraForm
    {
        private class ImgGroup
        {
            public string Key;                          // المفتاح الممثّل (أول اسم)
            public List<string> Names = new List<string>(); // كل العناصر التي تشترك بنفس الصورة
            public Image Sample;                        // الصورة المضمّنة الأصلية
            public string Caption;
            public int Order;
            public Point Pos;                           // موضع المربّع في شاشة التفصيل (لمطابقة الترتيب)
        }

        private readonly FlowLayoutPanel _flow;
        private readonly List<(string Key, SpinEdit Spin)> _orderInputs = new List<(string, SpinEdit)>();

        public XtraFormSizeImages()
        {
            Text = "إدارة صور المقاسات";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(1000, 660);

            var top = new PanelControl { Dock = DockStyle.Top, Height = 44, RightToLeft = RightToLeft.Yes };
            var btnSaveOrder = new SimpleButton { Text = "حفظ الترتيب", Location = new Point(10, 8), Size = new Size(150, 28) };
            btnSaveOrder.Appearance.BackColor = Color.FromArgb(76, 175, 80);
            btnSaveOrder.Appearance.ForeColor = Color.White;
            btnSaveOrder.Appearance.Options.UseBackColor = true;
            btnSaveOrder.Appearance.Options.UseForeColor = true;
            btnSaveOrder.Click += (s, e) => SaveOrder();
            var hint = new LabelControl
            {
                Location = new Point(170, 13),
                Size = new Size(810, 20),
                Text = "اكتب رقم الترتيب تحت كل صورة ثم اضغط «حفظ الترتيب» (يبدأ 1 من اليمين) • عدّل الاسم بالكتابة • «استبدال» لرفع صورة جديدة.",
                AutoSizeMode = LabelAutoSizeMode.None
            };
            hint.Appearance.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            hint.Appearance.Options.UseFont = true;
            top.Controls.Add(btnSaveOrder);
            top.Controls.Add(hint);

            _flow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                // الإعداد العربي القياسي: أول كرت (الترتيب 1) يبدأ من اليمين ثم يتدفّق لليسار
                // وينزل لصفّ جديد. (RightToLeft=Yes + LeftToRight يعمل بثبات داخل فورم RTL.)
                RightToLeft = RightToLeft.Yes,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(8)
            };

            Controls.Add(_flow);
            Controls.Add(top);

            // التحميل يتم بعد ظهور الشاشة مع مؤشّر «يتم جلب الصور من قاعدة البيانات»
            // لأن القراءة قد تكون من خادم خارجي بعيد. يُنفّذ مرّة واحدة.
            this.Shown += XtraFormSizeImages_Shown;
        }

        private bool _loaded;
        private void XtraFormSizeImages_Shown(object sender, EventArgs e)
        {
            if (_loaded) return;
            _loaded = true;
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(this, typeof(WaitForm1));
            try
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormCaption("جاري التحميل");
                DevExpress.XtraSplashScreen.SplashScreenManager.Default.SetWaitFormDescription("يتم جلب الصور من قاعدة البيانات...");
                Reload();
            }
            finally
            {
                DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            }
        }

        // قراءة كل عناصر الصور من شاشة التفصيل وتجميع المكرّر حسب محتوى الصورة.
        private List<ImgGroup> Enumerate()
        {
            var collected = new List<(string Name, Image Img, Point Pos)>();
            var catalogKeys = new HashSet<string>(SizeImageStore.Catalog.Select(c => c.Key));
            XtraFormDefultSize2 probe = null;
            try
            {
                probe = new XtraFormDefultSize2(new LinqModel.tblSellInvoice(), false);
                // فرض تخطيط الشاشة (بدون إظهارها) لقراءة المواضع الفعلية للمربعات.
                try { probe.Width = 1700; probe.Height = 950; probe.CreateControl(); probe.PerformLayout(); } catch { }
                CollectChecks(probe, probe, collected, catalogKeys);
            }
            catch { }

            var byHash = new Dictionary<string, ImgGroup>();
            foreach (var item in collected)
            {
                if (item.Img == null) continue;
                var h = Hash(item.Img) ?? item.Name;
                if (!byHash.TryGetValue(h, out var g))
                {
                    g = new ImgGroup { Sample = CloneImage(item.Img), Pos = item.Pos };
                    byHash[h] = g;
                }
                // الموضع الممثّل = الأعلى ثم الأيمن بين كل نسخ نفس الصورة
                if (item.Pos.Y < g.Pos.Y || (item.Pos.Y == g.Pos.Y && item.Pos.X > g.Pos.X))
                    g.Pos = item.Pos;
                g.Names.Add(item.Name);
            }
            try { probe?.Dispose(); } catch { }

            var groups = byHash.Values.ToList();
            if (groups.Count == 0) // احتياطي: الكتالوج الثابت إن فشلت القراءة
                foreach (var c in SizeImageStore.Catalog)
                    groups.Add(new ImgGroup { Names = new List<string> { c.Key }, Sample = SizeImageStore.GetEmbedded(c.Key) });

            foreach (var g in groups)
            {
                g.Names = g.Names.Distinct().OrderBy(x => x).ToList();
                g.Key = g.Names.First();
                g.Caption = (_infoByKey != null && _infoByKey.TryGetValue(g.Key, out var info))
                    ? info.Caption : SizeImageStore.DefaultCaption(g.Key);
            }

            // الترتيب مطابق لشاشة التفصيل: صفوف من الأعلى للأسفل، وداخل كل صف من اليمين لليسار.
            bool havePos = groups.Any(g => g.Pos != Point.Empty);
            return havePos
                ? groups.OrderBy(g => g.Pos.Y / 24).ThenByDescending(g => g.Pos.X).ToList()
                : groups.OrderBy(g => g.Key).ToList();
        }

        // يجمع المربعات (المطابقة لمفاتيح الكتالوج) مع موضعها النسبي داخل الفورم.
        private static void CollectChecks(Control node, Control formRoot,
            List<(string, Image, Point)> outp, HashSet<string> keys)
        {
            foreach (Control c in node.Controls)
            {
                if (c is DevExpress.XtraEditors.CheckEdit chk && chk.BackgroundImage != null && keys.Contains(chk.Name))
                    outp.Add((chk.Name, chk.BackgroundImage, AbsPos(chk, formRoot)));
                if (c.Controls.Count > 0) CollectChecks(c, formRoot, outp, keys);
            }
        }

        // موضع العنصر بالنسبة للفورم (مجموع الإزاحات صعودًا في شجرة الآباء).
        private static Point AbsPos(Control c, Control root)
        {
            int x = 0, y = 0;
            for (var cur = c; cur != null && cur != root; cur = cur.Parent) { x += cur.Left; y += cur.Top; }
            return new Point(x, y);
        }

        private static string Hash(Image img)
        {
            try
            {
                var bytes = (byte[])new ImageConverter().ConvertTo(img, typeof(byte[]));
                using (var md5 = System.Security.Cryptography.MD5.Create())
                    return BitConverter.ToString(md5.ComputeHash(bytes));
            }
            catch { return null; }
        }

        private static Image CloneImage(Image img)
        {
            try { return new Bitmap(img); } catch { return img; }
        }

        // كاش يُقرأ من القاعدة مرّة واحدة لكل تحميل (استعلامان فقط بدل استعلام لكل صورة).
        private Dictionary<string, byte[]> _overrideBytes;
        private Dictionary<string, SizeImageInfo> _infoByKey;

        private void Reload()
        {
            _overrideBytes = SizeImageStore.GetAllOverrides();                     // كل الصور المستبدلة دفعة واحدة
            _infoByKey = SizeImageStore.LoadAll().ToDictionary(x => x.Key, x => x); // الأسماء/الترتيب دفعة واحدة

            var groups = Enumerate();
            _flow.SuspendLayout();
            _flow.Controls.Clear();
            _orderInputs.Clear();
            for (int i = 0; i < groups.Count; i++)
                _flow.Controls.Add(BuildCard(groups[i], i + 1));
            _flow.ResumeLayout();
        }

        private Control BuildCard(ImgGroup g, int displayNumber)
        {
            var card = new PanelControl { Width = 220, Height = 232, Margin = new Padding(6), RightToLeft = RightToLeft.Yes };

            var pic = new PictureEdit { Location = new Point(8, 8), Size = new Size(204, 110) };
            pic.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            pic.Properties.ShowMenu = false;
            pic.Properties.ReadOnly = true;
            pic.Image = CurrentImage(g);

            var txtCaption = new TextEdit { Location = new Point(8, 122), Size = new Size(204, 24), RightToLeft = RightToLeft.Yes };
            txtCaption.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            txtCaption.Properties.Appearance.Options.UseTextOptions = true;
            txtCaption.EditValue = g.Caption;
            txtCaption.Leave += (s, e) => { try { SizeImageStore.SaveCaption(g.Key, txtCaption.Text); } catch { } };

            var lblNo = new LabelControl { Location = new Point(150, 152), Size = new Size(62, 24), Text = "الترتيب:", AutoSizeMode = LabelAutoSizeMode.None };
            lblNo.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            lblNo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            lblNo.Appearance.Options.UseTextOptions = true;
            var spin = new SpinEdit { Location = new Point(8, 150), Size = new Size(138, 24) };
            spin.Properties.IsFloatValue = false;
            spin.Properties.MinValue = 1;
            spin.Properties.MaxValue = 999;
            spin.Value = displayNumber;
            _orderInputs.Add((g.Key, spin));

            var btnReplace = new SimpleButton { Text = "استبدال", Location = new Point(8, 178), Size = new Size(204, 24) };
            btnReplace.Appearance.BackColor = Color.FromArgb(33, 150, 243);
            btnReplace.Appearance.ForeColor = Color.White;
            btnReplace.Appearance.Options.UseBackColor = true;
            btnReplace.Appearance.Options.UseForeColor = true;
            btnReplace.Click += (s, e) => Replace(g, pic);

            var btnReset = new SimpleButton { Text = "استرجاع الأصلية", Location = new Point(8, 204), Size = new Size(204, 22) };
            btnReset.Click += (s, e) => ResetOne(g, pic);

            card.Controls.Add(pic);
            card.Controls.Add(txtCaption);
            card.Controls.Add(lblNo);
            card.Controls.Add(spin);
            card.Controls.Add(btnReplace);
            card.Controls.Add(btnReset);
            return card;
        }

        private Image CurrentImage(ImgGroup g)
        {
            foreach (var n in g.Names)
            {
                if (_overrideBytes != null && _overrideBytes.TryGetValue(n, out var bytes))
                {
                    var db = SizeImageStore.ImageFromBytes(bytes);
                    if (db != null) return db;
                }
            }
            return g.Sample;
        }

        private void SaveOrder()
        {
            try
            {
                var keys = _orderInputs
                    .Select((x, idx) => new { x.Key, Num = Convert.ToInt32(x.Spin.Value), Idx = idx })
                    .OrderBy(a => a.Num).ThenBy(a => a.Idx)
                    .Select(a => a.Key)
                    .ToList();
                SizeImageStore.SaveOrder(keys);
                Reload();
                XtraMessageBox.Show("تم حفظ الترتيب ✅", "صور المقاسات", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("تعذّر حفظ الترتيب: " + ex.Message, "صور المقاسات", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Replace(ImgGroup g, PictureEdit pic)
        {
            using (var ofd = new OpenFileDialog
            {
                Title = "اختر صورة جديدة",
                Filter = "ملفات الصور|*.jpg;*.jpeg;*.png;*.bmp;*.gif|كل الملفات|*.*"
            })
            {
                if (ofd.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    var bytes = System.IO.File.ReadAllBytes(ofd.FileName);
                    SizeImageStore.SaveImageMany(g.Names, bytes);
                    pic.Image = CurrentImage(g);
                    XtraMessageBox.Show("تم استبدال الصورة بنجاح ✅", "صور المقاسات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("تعذّر حفظ الصورة: " + ex.Message, "صور المقاسات", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ResetOne(ImgGroup g, PictureEdit pic)
        {
            if (XtraMessageBox.Show("إرجاع هذه الصورة للأصلية؟", "تأكيد", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;
            try
            {
                SizeImageStore.ResetMany(g.Names);
                pic.Image = g.Sample;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("تعذّر الاسترجاع: " + ex.Message, "صور المقاسات", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
