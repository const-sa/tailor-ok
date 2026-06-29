using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTab;
using SewingSystem.Classes.Whatsapp;

namespace SewingSystem.Forms
{
    /// <summary>
    /// إعدادات الواتساب (c-wts) + قوالب الرسائل: ترحيب، بيانات التفصيل،
    /// جاهز للاستلام، تم التسليم. مبنية بعناصر DevExpress ومواضع ثابتة بنفس
    /// نمط الشاشات السابقة (GroupControl + LabelControl + TextEdit) لتفادي التداخل.
    /// </summary>
    public class XtraFormWhatsapp : DevExpress.XtraEditors.XtraForm
    {
        private TextEdit txtInstance, txtToken, txtTestNo;
        private MemoEdit txtWelcome, txtOrder, txtReady, txtDelivered, txtStatus;
        private CheckEdit chkEnabled, chkOnSave, chkOnReady, chkOnDelivery;
        private ComboBoxEdit cboTestTpl;
        private SimpleButton btnVerify, btnSave, btnTest;
        private WhatsappConfig _cfg;

        private static readonly Font LblFont = new Font("Tahoma", 9.75F, FontStyle.Bold);

        public XtraFormWhatsapp() { BuildUi(); LoadCfg(); }

        private void BuildUi()
        {
            Text = "الواتساب والرسائل (c-wts)";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = false;            // keep literal coordinates; text still renders RTL
            AutoScaleMode = AutoScaleMode.None;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(700, 726);
            WhatsappUi.ApplyIcon(this);

            // ===== group 1: connection =====
            var grpConn = Grp("بيانات الاتصال (c-wts)", new Point(12, 10), new Size(676, 196));
            txtInstance = new TextEdit { Location = new Point(120, 30), Size = new Size(410, 24) };
            txtToken = new TextEdit { Location = new Point(120, 62), Size = new Size(410, 24) };
            chkEnabled = new CheckEdit { Text = "تفعيل إرسال الواتساب", Location = new Point(360, 95), Size = new Size(300, 22) };
            chkOnSave = new CheckEdit { Text = "إرسال «بيانات التفصيل» عند الحفظ", Location = new Point(360, 119), Size = new Size(300, 22) };
            chkOnReady = new CheckEdit { Text = "إرسال «جاهز للاستلام» عند التجهيز", Location = new Point(360, 143), Size = new Size(300, 22) };
            chkOnDelivery = new CheckEdit { Text = "إرسال «تم التسليم» عند التسليم", Location = new Point(360, 167), Size = new Size(300, 22) };
            grpConn.Controls.Add(Lbl("instance_id:", 540, 33, 120));
            grpConn.Controls.Add(txtInstance);
            grpConn.Controls.Add(Lbl("access_token:", 540, 65, 120));
            grpConn.Controls.Add(txtToken);
            grpConn.Controls.Add(chkEnabled);
            grpConn.Controls.Add(chkOnSave);
            grpConn.Controls.Add(chkOnReady);
            grpConn.Controls.Add(chkOnDelivery);

            // ===== group 2: templates =====
            var grpTpl = Grp("قوالب الرسائل", new Point(12, 212), new Size(676, 250));
            var hint = new LabelControl
            {
                Location = new Point(14, 26), Size = new Size(648, 32),
                AutoSizeMode = LabelAutoSizeMode.None
            };
            hint.Appearance.ForeColor = Color.DimGray;
            hint.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            hint.Text = "الرموز المتاحة: {اسم_العميل} {الشركة} {الهاتف} {الرقم_الضريبي} {رقم_الفاتورة} {الاجمالي} {الضريبة} {الواصل} {الباقي} {تاريخ_التسليم}";
            var tabs = new XtraTabControl { Location = new Point(12, 60), Size = new Size(652, 182), RightToLeft = RightToLeft.Yes };
            txtWelcome = TplBox(); txtOrder = TplBox(); txtReady = TplBox(); txtDelivered = TplBox();
            tabs.TabPages.AddRange(new XtraTabPage[] { Tab("ترحيب بالعميل", txtWelcome), Tab("بيانات التفصيل", txtOrder), Tab("جاهز للاستلام", txtReady), Tab("تم التسليم", txtDelivered) });
            grpTpl.Controls.Add(hint);
            grpTpl.Controls.Add(tabs);

            // ===== group 3: test + save =====
            var grpTest = Grp("تجربة وحفظ", new Point(12, 468), new Size(676, 182));
            txtTestNo = new TextEdit { Location = new Point(430, 30), Size = new Size(135, 24) };
            WhatsappUi.Ltr(txtInstance, txtToken, txtTestNo);     // أرقام إنجليزية (لاتينية)
            cboTestTpl = new ComboBoxEdit { Location = new Point(285, 30), Size = new Size(140, 24) };
            cboTestTpl.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cboTestTpl.Properties.Items.AddRange(new object[] { "ترحيب", "بيانات التفصيل", "جاهز للاستلام", "تم التسليم" });
            cboTestTpl.SelectedIndex = 0;
            btnTest = Btn("إرسال تجريبي", 135, 24, 150, 38, WhatsappIcon.Get(32), BtnTest_Click);
            btnVerify = Btn("تحقق من الاتصال", 345, 66, 160, 40, WhatsappIcon.Get(32), BtnVerify_Click);
            btnSave = Btn("حفظ", 175, 66, 160, 40, Properties.Resources.saveall_32x32, BtnSave_Click);
            btnSave.Appearance.BackColor = WhatsappUi.BrandDark;
            btnSave.Appearance.ForeColor = Color.White;
            btnSave.Appearance.Options.UseBackColor = true;
            btnSave.Appearance.Options.UseForeColor = true;
            txtStatus = new MemoEdit { Location = new Point(14, 110), Size = new Size(648, 60) };
            txtStatus.Properties.ReadOnly = true;
            grpTest.Controls.Add(Lbl("جوال التجربة:", 570, 33, 90));
            grpTest.Controls.Add(txtTestNo);
            grpTest.Controls.Add(cboTestTpl);
            grpTest.Controls.Add(btnTest);
            grpTest.Controls.Add(btnVerify);
            grpTest.Controls.Add(btnSave);
            grpTest.Controls.Add(txtStatus);

            var content = new PanelControl { Dock = DockStyle.Fill };
            content.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            content.Controls.Add(grpConn);
            content.Controls.Add(grpTpl);
            content.Controls.Add(grpTest);

            Controls.Add(content);
            Controls.Add(WhatsappUi.Header("الواتساب والرسائل", "ربط c-wts وقوالب الرسائل التلقائية"));
        }

        private GroupControl Grp(string text, Point loc, Size size)
        {
            var g = new GroupControl { Text = text, Location = loc, Size = size };
            g.AppearanceCaption.Font = LblFont;                 // bold header
            g.AppearanceCaption.Options.UseFont = true;
            return g;
        }

        private LabelControl Lbl(string text, int x, int y, int w)
        {
            var l = new LabelControl { Text = text, Location = new Point(x, y), Size = new Size(w, 18), AutoSizeMode = LabelAutoSizeMode.None };
            l.Appearance.Font = LblFont; l.Appearance.Options.UseFont = true;
            l.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            return l;
        }
        private SimpleButton Btn(string text, int x, int y, int w, int h, Image img, EventHandler click)
        {
            var b = new SimpleButton { Text = text, Location = new Point(x, y), Size = new Size(w, h) };
            if (img != null) b.ImageOptions.Image = img;
            b.Click += click;
            return b;
        }
        private MemoEdit TplBox() { var m = new MemoEdit { Dock = DockStyle.Fill, RightToLeft = RightToLeft.Yes }; m.Properties.ScrollBars = ScrollBars.Vertical; return m; }
        private XtraTabPage Tab(string title, Control c) { var p = new XtraTabPage { Text = title, RightToLeft = RightToLeft.Yes }; c.Dock = DockStyle.Fill; p.Controls.Add(c); return p; }

        private void LoadCfg()
        {
            try
            {
                _cfg = WhatsappConfig.Load();
                txtInstance.Text = _cfg.Instance; txtToken.Text = _cfg.Token;
                chkEnabled.Checked = _cfg.Enabled;
                chkOnSave.Checked = _cfg.SendOnSave; chkOnReady.Checked = _cfg.SendOnReady; chkOnDelivery.Checked = _cfg.SendOnDelivery;
                txtWelcome.Text = _cfg.TplWelcome; txtOrder.Text = _cfg.TplOrder;
                txtReady.Text = _cfg.TplReady; txtDelivered.Text = _cfg.TplDelivered;
            }
            catch (Exception ex) { XtraMessageBox.Show("تعذّر تحميل الإعدادات: " + ex.Message); }
        }

        private void ReadInto(WhatsappConfig c)
        {
            c.Instance = txtInstance.Text.Trim(); c.Token = txtToken.Text.Trim();
            c.Enabled = chkEnabled.Checked;
            c.SendOnSave = chkOnSave.Checked; c.SendOnReady = chkOnReady.Checked; c.SendOnDelivery = chkOnDelivery.Checked;
            c.TplWelcome = txtWelcome.Text; c.TplOrder = txtOrder.Text;
            c.TplReady = txtReady.Text; c.TplDelivered = txtDelivered.Text;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try { ReadInto(_cfg); _cfg.Save(); XtraMessageBox.Show("تم حفظ إعدادات الواتساب."); }
            catch (Exception ex) { XtraMessageBox.Show("فشل الحفظ: " + ex.Message); }
        }

        private void BtnVerify_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                var r = new CwtsClient(txtInstance.Text.Trim(), txtToken.Text.Trim()).GetStatus();
                txtStatus.Text = r.Ok
                    ? $"متصل: {r.Status}   الرقم: {r.Phone}   متبقٍ بالاشتراك: {(r.DaysRemaining?.ToString() ?? "-")} يوم"
                    : $"غير متصل (HTTP {r.HttpStatus}): {r.Error ?? r.RawBody}";
            }
            catch (Exception ex) { txtStatus.Text = "خطأ: " + ex.Message; }
            finally { Cursor = Cursors.Default; }
        }

        private void BtnTest_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTestNo.Text)) { XtraMessageBox.Show("أدخل رقم جوال للتجربة."); return; }
            ReadInto(_cfg);
            string tpl;
            switch (cboTestTpl.SelectedIndex)
            {
                case 1: tpl = _cfg.TplOrder; break;
                case 2: tpl = _cfg.TplReady; break;
                case 3: tpl = _cfg.TplDelivered; break;
                default: tpl = _cfg.TplWelcome; break;
            }
            var tokens = WhatsappService.Tokens("عميل تجريبي", "0", "0.00", "0.00", "0.00", "0.00", DateTime.Now.ToString("yyyy/MM/dd"));
            string msg = WhatsappService.Format(tpl, tokens);
            try
            {
                Cursor = Cursors.WaitCursor;
                var r = WhatsappService.Send(_cfg, txtTestNo.Text.Trim(), msg);
                txtStatus.Text = r.Ok ? $"تم الإرسال (id={r.MessageId})." : $"فشل الإرسال (HTTP {r.HttpStatus}): {r.Error ?? r.RawBody}";
            }
            catch (Exception ex) { txtStatus.Text = "خطأ: " + ex.Message; }
            finally { Cursor = Cursors.Default; }
        }
    }
}
