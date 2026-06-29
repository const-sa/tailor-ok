using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using SewingSystem.Classes.Zatca;

namespace SewingSystem.Forms
{
    /// <summary>
    /// إعدادات ربط الفوترة الإلكترونية - المرحلة الثانية (هيئة الزكاة).
    /// واجهة احترافية مبنية بعناصر DevExpress (LayoutControl) بدون مصمم/resx.
    /// </summary>
    public class XtraFormZatcaSettings : FormZatcaMaster
    {
        private ComboBoxEdit cboEnv;
        private ToggleSwitch chkEnabled;
        private TextEdit txtOrg, txtVat, txtCr, txtEgs, txtShort, txtStreet, txtBuilding, txtSecondary, txtDistrict, txtCity, txtPostal, txtOtp;
        private MemoEdit txtStatus;
        private SimpleButton btnGenCsr, btnActivateTrial, btnActivateProd, btnReportInvoice;

        private ZatcaConfig _cfg;

        public XtraFormZatcaSettings()
        {
            BuildUi();
            LoadConfig();
        }

        private void BuildUi()
        {
            Text = "إعدادات ربط الزكاة (المرحلة الثانية)";
            RightToLeft = RightToLeft.Yes;
            RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(840, 744);
            MinimumSize = new Size(720, 640);
            Font = new Font("Tahoma", 9.75f);
            ZatcaUi.ApplyIcon(this);

            // ---------- editors ----------
            cboEnv = new ComboBoxEdit();
            cboEnv.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cboEnv.Properties.Items.AddRange(new object[] { "sandbox", "simulation", "production" });

            chkEnabled = new ToggleSwitch();
            chkEnabled.Properties.OnText = "مُفعّل";
            chkEnabled.Properties.OffText = "موقوف";

            txtOrg = NewBox(); txtVat = NewBox(); txtCr = NewBox(); txtEgs = NewBox();
            txtShort = NewBox(); txtStreet = NewBox(); txtBuilding = NewBox(); txtSecondary = NewBox();
            txtDistrict = NewBox(); txtCity = NewBox(); txtPostal = NewBox();
            txtOtp = NewBox();
            txtOtp.Properties.NullValuePrompt = "الصق رمز OTP من بوابة فاتورة";

            // الحقول الرقمية/المعرّفات تظهر بأرقام إنجليزية (لاتينية)
            ZatcaUi.Ltr(txtVat, txtCr, txtEgs, txtShort, txtBuilding, txtSecondary, txtPostal, txtOtp);

            txtStatus = new MemoEdit();
            txtStatus.Properties.ReadOnly = true;
            txtStatus.Properties.ScrollBars = ScrollBars.Vertical;
            txtStatus.Properties.Appearance.Font = new Font("Consolas", 9f);
            txtStatus.Properties.Appearance.Options.UseFont = true;

            // ---------- buttons (الحفظ صار في شريط الأدوات العلوي) ----------
            btnGenCsr = NewBtn("توليد المفاتيح + CSR", BtnGenCsr_Click);
            btnActivateTrial = NewBtn("تفعيل تجريبي (محاكاة)", (s, e) => Activate("simulation"));
            btnActivateProd = NewBtn("تفعيل مباشر (إنتاج)", (s, e) => Activate("production"));
            btnActivateProd.ImageOptions.Image = ZatcaIcon.Get(16);
            btnActivateProd.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            btnReportInvoice = NewBtn("إبلاغ فاتورة برقمها", BtnReportInvoice_Click);

            // ---------- layout ----------
            var lc = new LayoutControl { Dock = DockStyle.Fill, RightToLeft = RightToLeft.Yes };
            var root = lc.Root;
            root.GroupBordersVisible = false;
            root.Padding = new DevExpress.XtraLayout.Utils.Padding(10, 10, 10, 10);

            // environment + status toggle (side by side)
            var gEnv = root.AddGroup("البيئة والحالة");
            var itEnv = gEnv.AddItem("البيئة", cboEnv);
            gEnv.AddItem("تفعيل الإرسال للهيئة", chkEnabled, itEnv, InsertType.Right);

            // org data | national address (two columns)
            var gOrg = root.AddGroup("بيانات المنشأة");
            gOrg.AddItem("الاسم النظامي", txtOrg);
            gOrg.AddItem("الرقم الضريبي (VAT)", txtVat);
            gOrg.AddItem("السجل التجاري (CR)", txtCr);
            gOrg.AddItem("الرقم التسلسلي (EGS)", txtEgs);

            var gAddr = root.AddGroup("العنوان الوطني", gOrg, InsertType.Right);
            gAddr.AddItem("العنوان المختصر", txtShort);
            gAddr.AddItem("رقم المبنى", txtBuilding);
            gAddr.AddItem("الشارع", txtStreet);
            gAddr.AddItem("الرقم الفرعي", txtSecondary);
            gAddr.AddItem("الحي", txtDistrict);
            gAddr.AddItem("المدينة", txtCity);
            gAddr.AddItem("الرمز البريدي", txtPostal);

            // activation + actions
            var gAct = root.AddGroup("التفعيل والإجراءات");
            gAct.AddItem("رمز OTP (وقت التهيئة)", txtOtp);

            var itCsr = AddButtonItem(gAct, btnGenCsr, null);
            var itTrial = AddButtonItem(gAct, btnActivateTrial, itCsr);
            var itProd = AddButtonItem(gAct, btnActivateProd, itTrial);
            AddButtonItem(gAct, btnReportInvoice, itProd);

            // status
            var gStatus = root.AddGroup("الحالة الحالية");
            var itStatus = gStatus.AddItem(string.Empty, txtStatus);
            itStatus.TextVisible = false;
            itStatus.SizeConstraintsType = SizeConstraintsType.Custom;
            itStatus.MinSize = new Size(100, 140);

            var header = ZatcaUi.Header("الفوترة الإلكترونية — المرحلة الثانية",
                                        "هيئة الزكاة والضريبة والجمارك (فاتورة)");

            ContentPanel.Controls.Add(lc);      // fill (added first → fills remaining space)
            ContentPanel.Controls.Add(header);  // top banner (under the toolbar)
        }

        protected override void OnSaveClick() => BtnSave_Click(this, EventArgs.Empty);
        protected override void OnRefreshClick() => LoadConfig();

        private TextEdit NewBox() => new TextEdit();

        private SimpleButton NewBtn(string text, EventHandler onClick)
        {
            var b = new SimpleButton { Text = text, Height = 34 };
            b.Click += onClick;
            return b;
        }

        private LayoutControlItem AddButtonItem(LayoutControlGroup g, SimpleButton btn, BaseLayoutItem leftOf)
        {
            var it = leftOf == null ? g.AddItem(string.Empty, btn) : g.AddItem(string.Empty, btn, leftOf, InsertType.Right);
            it.TextVisible = false;
            it.SizeConstraintsType = SizeConstraintsType.Custom;
            it.MinSize = new Size(150, 38);
            it.MaxSize = new Size(0, 38);
            return it;
        }

        private void LoadConfig()
        {
            try
            {
                _cfg = ZatcaConfig.Load();
                cboEnv.SelectedItem = _cfg.Environment ?? "simulation";
                if (cboEnv.SelectedIndex < 0) cboEnv.SelectedItem = "simulation";
                chkEnabled.IsOn = _cfg.Enabled;
                txtOrg.Text = _cfg.OrgName; txtVat.Text = _cfg.VatNumber; txtCr.Text = _cfg.CrNumber;
                txtEgs.Text = _cfg.EgsSerialNumber; txtShort.Text = _cfg.AddrShort;
                txtStreet.Text = _cfg.AddrStreet; txtBuilding.Text = _cfg.AddrBuilding;
                txtSecondary.Text = _cfg.AddrSecondary; txtDistrict.Text = _cfg.AddrDistrict;
                txtCity.Text = _cfg.AddrCity; txtPostal.Text = _cfg.AddrPostal;
                RefreshStatus();
            }
            catch (Exception ex) { XtraMessageBox.Show("تعذّر تحميل الإعدادات: " + ex.Message); }
        }

        private void ReadInto(ZatcaConfig c)
        {
            c.Environment = cboEnv.SelectedItem?.ToString() ?? "simulation";
            c.Enabled = chkEnabled.IsOn;
            c.OrgName = txtOrg.Text.Trim(); c.VatNumber = txtVat.Text.Trim(); c.CrNumber = txtCr.Text.Trim();
            c.EgsSerialNumber = txtEgs.Text.Trim(); c.AddrShort = txtShort.Text.Trim();
            c.AddrStreet = txtStreet.Text.Trim(); c.AddrBuilding = txtBuilding.Text.Trim();
            c.AddrSecondary = txtSecondary.Text.Trim(); c.AddrDistrict = txtDistrict.Text.Trim();
            c.AddrCity = txtCity.Text.Trim(); c.AddrPostal = txtPostal.Text.Trim();
        }

        private void RefreshStatus()
        {
            string yn(bool b) => b ? "نعم ✔" : "لا ✘";
            txtStatus.Text =
                "البيئة:               " + _cfg.Environment + Environment.NewLine +
                "رابط الهيئة:          " + _cfg.ApiBaseUrl + Environment.NewLine +
                "قالب الشهادة:         " + _cfg.CsrTemplateName + Environment.NewLine +
                "المفتاح الخاص مولّد:   " + yn(!string.IsNullOrEmpty(_cfg.PrivateKeyPem)) + Environment.NewLine +
                "طلب الشهادة CSR جاهز:  " + yn(!string.IsNullOrEmpty(_cfg.Csr)) + Environment.NewLine +
                "شهادة الامتثال CCSID:  " + yn(!string.IsNullOrEmpty(_cfg.ComplianceCert)) + Environment.NewLine +
                "شهادة الإنتاج PCSID:   " + yn(!string.IsNullOrEmpty(_cfg.ProductionCert)) + Environment.NewLine +
                "آخر ICV:              " + _cfg.LastICV + Environment.NewLine +
                "آخر PIH:              " + (string.IsNullOrEmpty(_cfg.LastPIH) ? "(تكوين)" : "محفوظ");
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try { ReadInto(_cfg); _cfg.Save(); RefreshStatus(); XtraMessageBox.Show("تم حفظ الإعدادات بنجاح."); }
            catch (Exception ex) { XtraMessageBox.Show("فشل الحفظ: " + ex.Message); }
        }

        private void BtnGenCsr_Click(object sender, EventArgs e)
        {
            try
            {
                ReadInto(_cfg);
                if (string.IsNullOrWhiteSpace(_cfg.VatNumber) || string.IsNullOrWhiteSpace(_cfg.EgsSerialNumber))
                {
                    XtraMessageBox.Show("يجب إدخال الرقم الضريبي والرقم التسلسلي (EGS) أولاً.");
                    return;
                }
                if (!string.IsNullOrEmpty(_cfg.PrivateKeyPem) &&
                    XtraMessageBox.Show("يوجد مفتاح مولّد مسبقاً. إعادة التوليد ستُبطل الشهادات الحالية. متابعة؟",
                        "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;

                Cursor = Cursors.WaitCursor;
                ZatcaCrypto.GenerateKeyPairAndCsr(_cfg);
                // regenerating invalidates any prior certificates
                _cfg.ComplianceCert = _cfg.ComplianceSecret = _cfg.ComplianceRequestId = null;
                _cfg.ProductionCert = _cfg.ProductionSecret = _cfg.ProductionRequestId = null;
                _cfg.Save();
                RefreshStatus();
                XtraMessageBox.Show("تم توليد المفتاح وطلب الشهادة (CSR) بنجاح. الخطوة التالية: طلب شهادة الامتثال بإدخال OTP.");
            }
            catch (Exception ex) { XtraMessageBox.Show("فشل توليد المفاتيح/CSR: " + ex.Message); }
            finally { Cursor = Cursors.Default; }
        }

        private void BtnReportInvoice_Click(object sender, EventArgs e)
        {
            string s = DevExpress.XtraEditors.XtraInputBox.Show("أدخل رقم الفاتورة (InvoNumber) لإبلاغها للزكاة:", "إبلاغ فاتورة", "");
            if (string.IsNullOrWhiteSpace(s)) return;
            if (!int.TryParse(s.Trim(), out int invoNo)) { XtraMessageBox.Show("رقم غير صحيح."); return; }

            try
            {
                Cursor = Cursors.WaitCursor;
                int id = -1;
                using (var con = new System.Data.SqlClient.SqlConnection(Program.ConnectionString))
                {
                    con.Open();
                    using (var cmd = new System.Data.SqlClient.SqlCommand(
                        "SELECT TOP 1 ID FROM dbo.tblSellInvoice WHERE InvoNumber=@n ORDER BY ID DESC", con))
                    {
                        cmd.Parameters.AddWithValue("@n", invoNo);
                        var o = cmd.ExecuteScalar();
                        if (o == null || o == DBNull.Value) { XtraMessageBox.Show("لم يتم العثور على الفاتورة."); return; }
                        id = Convert.ToInt32(o);
                    }
                }
                var log = new StringBuilder();
                Classes.Zatca.ZatcaService.ReportExistingInvoice(id, log);
                txtStatus.Text = log.ToString();
                LoadConfig();
                txtStatus.Text += Environment.NewLine + "----------------" + Environment.NewLine + log.ToString();
            }
            catch (Exception ex) { XtraMessageBox.Show("تعذّر الإبلاغ: " + ex.Message); }
            finally { Cursor = Cursors.Default; }
        }

        private void Activate(string environment)
        {
            try
            {
                ReadInto(_cfg);
                _cfg.Environment = environment;
                cboEnv.SelectedItem = environment;
                _cfg.Save();

                string otp = txtOtp.Text.Trim();
                if (string.IsNullOrEmpty(otp))
                {
                    XtraMessageBox.Show("الصق رمز OTP من بوابة فاتورة أولاً (صالح لدقائق).");
                    return;
                }

                string label = environment == "production" ? "المباشر (إنتاج)" : "التجريبي (محاكاة)";
                if (XtraMessageBox.Show("سيبدأ التفعيل " + label + " على رابط:\n" + _cfg.ApiBaseUrl +
                        "\n\nمتابعة؟", "تأكيد التفعيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                Cursor = Cursors.WaitCursor;
                var log = new StringBuilder();
                bool ok;
                try { ok = Classes.Zatca.ZatcaService.Activate(_cfg, otp, log); }
                catch (Exception ex) { log.AppendLine("✘ استثناء: " + ex.Message); ok = false; }

                txtStatus.Text = log.ToString() + Environment.NewLine + "----------------" + Environment.NewLine;
                LoadConfig(); // refresh status fields from saved config
                txtStatus.Text += log.ToString();
                XtraMessageBox.Show(ok ? "تم التفعيل بنجاح." : "لم يكتمل التفعيل — راجع تفاصيل الحالة بالأسفل.",
                    "نتيجة التفعيل", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex) { XtraMessageBox.Show("خطأ: " + ex.Message); }
            finally { Cursor = Cursors.Default; }
        }
    }
}
