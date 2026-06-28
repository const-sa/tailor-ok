using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SewingSystem.Classes.Zatca;

namespace SewingSystem.Forms
{
    /// <summary>
    /// إعدادات ربط الفوترة الإلكترونية - المرحلة الثانية (هيئة الزكاة).
    /// UI is built in code (no designer/resx) to stay simple and self-contained.
    /// </summary>
    public class XtraFormZatcaSettings : DevExpress.XtraEditors.XtraForm
    {
        private ComboBox cboEnv;
        private CheckBox chkEnabled;
        private TextBox txtOrg, txtVat, txtCr, txtEgs, txtShort, txtStreet, txtBuilding, txtSecondary, txtDistrict, txtCity, txtPostal, txtOtp;
        private TextBox txtStatus;
        private Button btnSave, btnGenCsr, btnActivateTrial, btnActivateProd, btnReportInvoice;

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
            ClientSize = new Size(640, 620);
            Font = new Font("Tahoma", 9.75f);

            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 2,
                RightToLeft = RightToLeft.Yes,
                Padding = new Padding(12),
                AutoSize = true,
                Height = 470
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            cboEnv = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            cboEnv.Items.AddRange(new object[] { "sandbox", "simulation", "production" });
            chkEnabled = new CheckBox { Text = "تفعيل الإرسال للهيئة", AutoSize = true };
            txtOrg = NewBox(); txtVat = NewBox(); txtCr = NewBox(); txtEgs = NewBox();
            txtShort = NewBox(); txtStreet = NewBox(); txtBuilding = NewBox(); txtSecondary = NewBox();
            txtDistrict = NewBox(); txtCity = NewBox(); txtPostal = NewBox();
            txtOtp = NewBox();

            AddRow(table, "البيئة:", cboEnv);
            AddRow(table, "الحالة:", chkEnabled);
            AddRow(table, "الاسم النظامي:", txtOrg);
            AddRow(table, "الرقم الضريبي (VAT):", txtVat);
            AddRow(table, "السجل التجاري (CR):", txtCr);
            AddRow(table, "الرقم التسلسلي (EGS):", txtEgs);
            AddRow(table, "العنوان المختصر:", txtShort);
            AddRow(table, "رقم المبنى:", txtBuilding);
            AddRow(table, "الشارع:", txtStreet);
            AddRow(table, "الرقم الفرعي:", txtSecondary);
            AddRow(table, "الحي:", txtDistrict);
            AddRow(table, "المدينة:", txtCity);
            AddRow(table, "الرمز البريدي:", txtPostal);
            AddRow(table, "رمز OTP (وقت التهيئة):", txtOtp);

            // Buttons
            var pnlBtns = new FlowLayoutPanel
            {
                Dock = DockStyle.Top, RightToLeft = RightToLeft.Yes, FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(12, 4, 12, 4), Height = 80, WrapContents = true, AutoSize = false
            };
            btnSave = NewBtn("حفظ الإعدادات", BtnSave_Click);
            btnGenCsr = NewBtn("توليد المفاتيح + CSR", BtnGenCsr_Click);
            btnActivateTrial = NewBtn("تفعيل تجريبي (محاكاة)", (s, e) => Activate("simulation"));
            btnActivateProd = NewBtn("تفعيل مباشر (إنتاج)", (s, e) => Activate("production"));
            btnReportInvoice = NewBtn("إبلاغ فاتورة برقمها", BtnReportInvoice_Click);
            pnlBtns.Controls.AddRange(new Control[] { btnSave, btnGenCsr, btnActivateTrial, btnActivateProd, btnReportInvoice });

            // Status box
            txtStatus = new TextBox
            {
                Dock = DockStyle.Fill, Multiline = true, ReadOnly = true,
                ScrollBars = ScrollBars.Vertical, BackColor = Color.White,
                Font = new Font("Consolas", 9f)
            };
            var grpStatus = new GroupBox { Text = "الحالة الحالية", Dock = DockStyle.Fill, Padding = new Padding(8) };
            grpStatus.Controls.Add(txtStatus);

            Controls.Add(grpStatus);  // fills remaining
            Controls.Add(pnlBtns);
            Controls.Add(table);
        }

        private TextBox NewBox() => new TextBox { Dock = DockStyle.Fill };

        private Button NewBtn(string text, EventHandler onClick)
        {
            var b = new Button { Text = text, AutoSize = true, Height = 32, Margin = new Padding(4), MinimumSize = new Size(140, 32) };
            b.Click += onClick;
            return b;
        }

        private void AddRow(TableLayoutPanel t, string label, Control input)
        {
            int row = t.RowCount++;
            t.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));
            t.Controls.Add(new Label { Text = label, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill, AutoEllipsis = true }, 0, row);
            t.Controls.Add(input, 1, row);
        }

        private void LoadConfig()
        {
            try
            {
                _cfg = ZatcaConfig.Load();
                cboEnv.SelectedItem = _cfg.Environment ?? "simulation";
                if (cboEnv.SelectedIndex < 0) cboEnv.SelectedItem = "simulation";
                chkEnabled.Checked = _cfg.Enabled;
                txtOrg.Text = _cfg.OrgName; txtVat.Text = _cfg.VatNumber; txtCr.Text = _cfg.CrNumber;
                txtEgs.Text = _cfg.EgsSerialNumber; txtShort.Text = _cfg.AddrShort;
                txtStreet.Text = _cfg.AddrStreet; txtBuilding.Text = _cfg.AddrBuilding;
                txtSecondary.Text = _cfg.AddrSecondary; txtDistrict.Text = _cfg.AddrDistrict;
                txtCity.Text = _cfg.AddrCity; txtPostal.Text = _cfg.AddrPostal;
                RefreshStatus();
            }
            catch (Exception ex) { MessageBox.Show("تعذّر تحميل الإعدادات: " + ex.Message); }
        }

        private void ReadInto(ZatcaConfig c)
        {
            c.Environment = cboEnv.SelectedItem?.ToString() ?? "simulation";
            c.Enabled = chkEnabled.Checked;
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
            try { ReadInto(_cfg); _cfg.Save(); RefreshStatus(); MessageBox.Show("تم حفظ الإعدادات بنجاح."); }
            catch (Exception ex) { MessageBox.Show("فشل الحفظ: " + ex.Message); }
        }

        private void BtnGenCsr_Click(object sender, EventArgs e)
        {
            try
            {
                ReadInto(_cfg);
                if (string.IsNullOrWhiteSpace(_cfg.VatNumber) || string.IsNullOrWhiteSpace(_cfg.EgsSerialNumber))
                {
                    MessageBox.Show("يجب إدخال الرقم الضريبي والرقم التسلسلي (EGS) أولاً.");
                    return;
                }
                if (!string.IsNullOrEmpty(_cfg.PrivateKeyPem) &&
                    MessageBox.Show("يوجد مفتاح مولّد مسبقاً. إعادة التوليد ستُبطل الشهادات الحالية. متابعة؟",
                        "تأكيد", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;

                Cursor = Cursors.WaitCursor;
                ZatcaCrypto.GenerateKeyPairAndCsr(_cfg);
                // regenerating invalidates any prior certificates
                _cfg.ComplianceCert = _cfg.ComplianceSecret = _cfg.ComplianceRequestId = null;
                _cfg.ProductionCert = _cfg.ProductionSecret = _cfg.ProductionRequestId = null;
                _cfg.Save();
                RefreshStatus();
                MessageBox.Show("تم توليد المفتاح وطلب الشهادة (CSR) بنجاح. الخطوة التالية: طلب شهادة الامتثال بإدخال OTP.");
            }
            catch (Exception ex) { MessageBox.Show("فشل توليد المفاتيح/CSR: " + ex.Message); }
            finally { Cursor = Cursors.Default; }
        }

        private void BtnReportInvoice_Click(object sender, EventArgs e)
        {
            string s = DevExpress.XtraEditors.XtraInputBox.Show("أدخل رقم الفاتورة (InvoNumber) لإبلاغها للزكاة:", "إبلاغ فاتورة", "");
            if (string.IsNullOrWhiteSpace(s)) return;
            if (!int.TryParse(s.Trim(), out int invoNo)) { MessageBox.Show("رقم غير صحيح."); return; }

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
                        if (o == null || o == DBNull.Value) { MessageBox.Show("لم يتم العثور على الفاتورة."); return; }
                        id = Convert.ToInt32(o);
                    }
                }
                var log = new StringBuilder();
                Classes.Zatca.ZatcaService.ReportExistingInvoice(id, log);
                txtStatus.Text = log.ToString();
                LoadConfig();
                txtStatus.AppendText(Environment.NewLine + "----------------" + Environment.NewLine + log.ToString());
            }
            catch (Exception ex) { MessageBox.Show("تعذّر الإبلاغ: " + ex.Message); }
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
                    MessageBox.Show("الصق رمز OTP من بوابة فاتورة أولاً (صالح لدقائق).");
                    return;
                }

                string label = environment == "production" ? "المباشر (إنتاج)" : "التجريبي (محاكاة)";
                if (MessageBox.Show("سيبدأ التفعيل " + label + " على رابط:\n" + _cfg.ApiBaseUrl +
                        "\n\nمتابعة؟", "تأكيد التفعيل", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                Cursor = Cursors.WaitCursor;
                var log = new StringBuilder();
                bool ok;
                try { ok = Classes.Zatca.ZatcaService.Activate(_cfg, otp, log); }
                catch (Exception ex) { log.AppendLine("✘ استثناء: " + ex.Message); ok = false; }

                txtStatus.Text = log.ToString() + Environment.NewLine + "----------------" + Environment.NewLine;
                LoadConfig(); // refresh status fields from saved config
                txtStatus.AppendText(log.ToString());
                MessageBox.Show(ok ? "تم التفعيل بنجاح." : "لم يكتمل التفعيل — راجع تفاصيل الحالة بالأسفل.",
                    "نتيجة التفعيل", MessageBoxButtons.OK, ok ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            catch (Exception ex) { MessageBox.Show("خطأ: " + ex.Message); }
            finally { Cursor = Cursors.Default; }
        }
    }
}
