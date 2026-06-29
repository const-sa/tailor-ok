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
                RightToLeft = RightToLeft.No, // مع RightToLeft للتدفّق = الرقم 1 يبدأ من اليمين
                FlowDirection = FlowDirection.RightToLeft,
                WrapContents = true,
                AutoScroll = true,
                Padding = new Padding(8)
            };

            Controls.Add(_flow);
            Controls.Add(top);

            Reload();
        }

        // قراءة كل عناصر الصور من شاشة التفصيل وتجميع المكرّر حسب محتوى الصورة.
        private List<ImgGroup> Enumerate()
        {
            var collected = new List<(string Name, Image Img)>();
            XtraFormDefultSize2 probe = null;
            try
            {
                probe = new XtraFormDefultSize2(new LinqModel.tblSellInvoice(), false);
                CollectChecks(probe, collected);
            }
            catch { }

            var byHash = new Dictionary<string, ImgGroup>();
            foreach (var item in collected)
            {
                if (item.Img == null) continue;
                var h = Hash(item.Img) ?? item.Name;
                if (!byHash.TryGetValue(h, out var g))
                {
                    g = new ImgGroup { Sample = CloneImage(item.Img) };
                    byHash[h] = g;
                }
                g.Names.Add(item.Name);
            }
            try { probe?.Dispose(); } catch { }

            var groups = byHash.Values.ToList();
            if (groups.Count == 0) // احتياطي: الكتالوج الثابت إن فشلت القراءة
                foreach (var c in SizeImageStore.Catalog)
                    groups.Add(new ImgGroup { Names = new List<string> { c.Key }, Sample = SizeImageStore.GetEmbedded(c.Key) });

            int idx = 0;
            foreach (var g in groups)
            {
                g.Names = g.Names.Distinct().OrderBy(x => x).ToList();
                g.Key = g.Names.First();
                var info = SizeImageStore.LoadOne(g.Key, SizeImageStore.DefaultCaption(g.Key), idx);
                g.Caption = info.Caption;
                g.Order = info.SortOrder;
                idx++;
            }
            return groups.OrderBy(g => g.Order).ThenBy(g => g.Key).ToList();
        }

        private static void CollectChecks(Control root, List<(string, Image)> outp)
        {
            foreach (Control c in root.Controls)
            {
                if (c is DevExpress.XtraEditors.CheckEdit chk && chk.BackgroundImage != null)
                    outp.Add((chk.Name, chk.BackgroundImage));
                if (c.Controls.Count > 0) CollectChecks(c, outp);
            }
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

        private void Reload()
        {
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
                var db = SizeImageStore.DbImage(n);
                if (db != null) return db;
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
