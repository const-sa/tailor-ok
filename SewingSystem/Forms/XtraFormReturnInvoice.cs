using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using SewingSystem.Classes.Zatca;

namespace SewingSystem.Forms
{
    /// <summary>
    /// مرتجع مبيعات (إشعار دائن - Credit Note, نوع 381). مرتبط بفاتورة بيع أصلية،
    /// يدعم الإرجاع الكامل أو الجزئي على مستوى الأصناف/الكميات، يعكس الضريبة، يُرجع
    /// المخزون (tblClasse.QuantityRemin)، ويهيّئ السجل للإبلاغ للهيئة. الواجهة مبنية
    /// بالكود (RTL) بدون designer/resx. ملاحظة: شاشة الإشعارات الدائنة/المدينة بالمبلغ
    /// (XtraFormCreditNote / tblNote) تبقى كما هي لأغراض التسويات المالية.
    /// </summary>
    public class XtraFormReturnInvoice : DevExpress.XtraEditors.XtraForm
    {
        private TextBox txtInvoNo, txtRetAmount, txtReason, txtInfo;
        private DataGridView grid;
        private CheckBox chkRestoreStock, chkReportNow;
        private Button btnLoad, btnFull, btnSave;
        private DataTable _lines;

        private int _origId = -1, _origInvoNo = -1, _branchId = 1, _cusNumber;
        private string _customer;
        private double _origTotal, _origTax, _taxRate = 15;
        private bool _useTax = true;

        public XtraFormReturnInvoice() { BuildUi(); }

        private void BuildUi()
        {
            Text = "مرتجع مبيعات (إشعار دائن)";
            RightToLeft = RightToLeft.Yes; RightToLeftLayout = true;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(820, 600);
            Font = new Font("Tahoma", 9.75f);

            var top = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.RightToLeft, RightToLeft = RightToLeft.Yes, Height = 44, Padding = new Padding(10, 8, 10, 4) };
            txtInvoNo = new TextBox { Width = 140 };
            btnLoad = MkBtn("تحميل الفاتورة", (s, e) => LoadInvoice());
            top.Controls.Add(new Label { Text = "رقم الفاتورة الأصلية:", AutoSize = true, Padding = new Padding(4, 6, 0, 0) });
            top.Controls.Add(txtInvoNo);
            top.Controls.Add(btnLoad);

            txtInfo = new TextBox { Dock = DockStyle.Top, ReadOnly = true, Multiline = true, Height = 70, BackColor = Color.WhiteSmoke };

            grid = new DataGridView
            {
                Dock = DockStyle.Fill, RightToLeft = RightToLeft.Yes, AllowUserToAddRows = false,
                AllowUserToDeleteRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.CellSelect
            };

            var bottom = new TableLayoutPanel { Dock = DockStyle.Bottom, ColumnCount = 2, RightToLeft = RightToLeft.Yes, Height = 150, Padding = new Padding(10) };
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170));
            bottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            txtRetAmount = new TextBox { Dock = DockStyle.Fill };
            txtReason = new TextBox { Dock = DockStyle.Fill, Text = "مرتجع - Return" };
            chkRestoreStock = new CheckBox { Text = "إرجاع الكميات للمخزون", Checked = true, AutoSize = true };
            chkReportNow = new CheckBox { Text = "إبلاغ الزكاة الآن (يتطلب تفعيلاً ناجحاً)", Checked = false, AutoSize = true };
            AddRow(bottom, "مبلغ المرتجع (شامل الضريبة):", txtRetAmount);
            AddRow(bottom, "سبب المرتجع:", txtReason);
            AddRow(bottom, "", chkRestoreStock);
            AddRow(bottom, "", chkReportNow);

            var btns = new FlowLayoutPanel { Dock = DockStyle.Bottom, FlowDirection = FlowDirection.RightToLeft, RightToLeft = RightToLeft.Yes, Height = 48, Padding = new Padding(10, 6, 10, 6) };
            btnFull = MkBtn("مرتجع كامل", (s, e) => FillFull());
            btnSave = MkBtn("حفظ المرتجع", (s, e) => Save());
            btns.Controls.Add(btnSave); btns.Controls.Add(btnFull);

            Controls.Add(grid);
            Controls.Add(bottom);
            Controls.Add(btns);
            Controls.Add(txtInfo);
            Controls.Add(top);

            SetEnabled(false);
        }

        private Button MkBtn(string t, EventHandler h) { var b = new Button { Text = t, AutoSize = true, Height = 32, MinimumSize = new Size(130, 32), Margin = new Padding(4) }; b.Click += h; return b; }
        private void AddRow(TableLayoutPanel t, string label, Control c)
        {
            int r = t.RowCount++; t.RowStyles.Add(new RowStyle(SizeType.Absolute, 32));
            t.Controls.Add(new Label { Text = label, TextAlign = ContentAlignment.MiddleRight, Dock = DockStyle.Fill }, 0, r);
            t.Controls.Add(c, 1, r);
        }
        private void SetEnabled(bool on) { grid.Enabled = txtRetAmount.Enabled = txtReason.Enabled = btnFull.Enabled = btnSave.Enabled = chkRestoreStock.Enabled = chkReportNow.Enabled = on; }

        private void LoadInvoice()
        {
            if (!int.TryParse(txtInvoNo.Text.Trim(), out int no)) { MessageBox.Show("أدخل رقم فاتورة صحيح."); return; }
            try
            {
                using (var con = new SqlConnection(Program.ConnectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand(
                        "SELECT TOP 1 ID, InvoNumber, TheName, SellDate, TheQuantity, TotalFinal, TaxAll, CusNumber, BranchID, ISNULL(IsReturn,0) FROM dbo.tblSellInvoice WHERE InvoNumber=@n ORDER BY ID", con))
                    {
                        cmd.Parameters.AddWithValue("@n", no);
                        using (var r = cmd.ExecuteReader())
                        {
                            if (!r.Read()) { MessageBox.Show("لم يتم العثور على الفاتورة."); return; }
                            if (Convert.ToBoolean(r[9])) { MessageBox.Show("هذه الفاتورة نفسها مرتجع، لا يمكن إرجاعها."); return; }
                            _origId = Convert.ToInt32(r[0]); _origInvoNo = Convert.ToInt32(r[1]);
                            _customer = r[2] == DBNull.Value ? "" : r[2].ToString();
                            _origTotal = r[5] == DBNull.Value ? 0 : Convert.ToDouble(r[5]);
                            _origTax = r[6] == DBNull.Value ? 0 : Convert.ToDouble(r[6]);
                            _cusNumber = r[7] == DBNull.Value ? 0 : Convert.ToInt32(r[7]);
                            _branchId = r[8] == DBNull.Value ? 1 : Convert.ToInt32(r[8]);
                            txtInfo.Text = $"العميل: {_customer}    التاريخ: {(r[3] == DBNull.Value ? "" : Convert.ToDateTime(r[3]).ToString("yyyy-MM-dd"))}\r\n" +
                                           $"الإجمالي: {_origTotal:0.00}    الضريبة: {_origTax:0.00}    الكمية: {(r[4] == DBNull.Value ? "0" : r[4].ToString())}";
                        }
                    }
                    using (var cmd = new SqlCommand("SELECT ISNULL(TaxRate,15), ISNULL(UseTax,1) FROM dbo.tblBranche WHERE ID=@b", con))
                    {
                        cmd.Parameters.AddWithValue("@b", _branchId);
                        using (var r = cmd.ExecuteReader()) if (r.Read()) { _taxRate = Convert.ToDouble(r[0]); _useTax = Convert.ToBoolean(r[1]); }
                    }
                    _lines = new DataTable();
                    _lines.Columns.Add("ClassNumber", typeof(string));
                    _lines.Columns.Add("ClassName", typeof(string));
                    _lines.Columns.Add("OrigLength", typeof(double));
                    _lines.Columns.Add("OrigCount", typeof(int));
                    _lines.Columns.Add("RetLength", typeof(double));
                    _lines.Columns.Add("RetCount", typeof(int));
                    using (var cmd = new SqlCommand(
                        "SELECT d.ClassNumber, ISNULL(c.ClassName,''), ISNULL(d.GomashLength,0), ISNULL(d.NumClothes,0) " +
                        "FROM dbo.tblSellInvoiceDetaile d LEFT JOIN dbo.tblClasse c ON c.ClassNumber=d.ClassNumber WHERE d.InvoNumber=@n", con))
                    {
                        cmd.Parameters.AddWithValue("@n", no);
                        using (var r = cmd.ExecuteReader())
                            while (r.Read())
                                _lines.Rows.Add(r.IsDBNull(0) ? "" : r.GetString(0), r.GetString(1), r.GetDouble(2), r.GetInt32(3), 0d, 0);
                    }
                }
                BindGrid();
                txtRetAmount.Text = _origTotal.ToString("0.00", CultureInfo.InvariantCulture);
                SetEnabled(true);
            }
            catch (Exception ex) { MessageBox.Show("تعذّر التحميل: " + ex.Message); }
        }

        private void BindGrid()
        {
            grid.DataSource = _lines;
            string[] ro = { "ClassNumber", "ClassName", "OrigLength", "OrigCount" };
            foreach (DataGridViewColumn col in grid.Columns) col.ReadOnly = Array.IndexOf(ro, col.Name) >= 0;
            void H(string n, string h) { if (grid.Columns[n] != null) grid.Columns[n].HeaderText = h; }
            H("ClassNumber", "رقم الصنف"); H("ClassName", "اسم الصنف"); H("OrigLength", "الطول الأصلي");
            H("OrigCount", "العدد الأصلي"); H("RetLength", "الطول المرتجع"); H("RetCount", "العدد المرتجع");
        }

        private void FillFull()
        {
            foreach (DataRow row in _lines.Rows) { row["RetLength"] = row["OrigLength"]; row["RetCount"] = row["OrigCount"]; }
            grid.Refresh();
            txtRetAmount.Text = _origTotal.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private void Save()
        {
            grid.EndEdit();
            if (_origId < 0) { MessageBox.Show("حمّل فاتورة أولاً."); return; }
            if (!decimal.TryParse(txtRetAmount.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal retTotal) || retTotal <= 0)
            { MessageBox.Show("أدخل مبلغ مرتجع صحيحاً."); return; }
            if (retTotal > (decimal)_origTotal + 0.01m) { MessageBox.Show("مبلغ المرتجع أكبر من إجمالي الفاتورة."); return; }

            foreach (DataRow row in _lines.Rows)
            {
                double rl = ToD(row["RetLength"]), ol = ToD(row["OrigLength"]);
                int rc = ToI(row["RetCount"]), oc = ToI(row["OrigCount"]);
                if (rl < 0 || rc < 0 || rl > ol + 0.0001 || rc > oc) { MessageBox.Show($"كمية مرتجعة غير صحيحة للصنف {row["ClassNumber"]}."); return; }
            }

            decimal retTax = _useTax ? Math.Round(retTotal - retTotal / (1 + (decimal)_taxRate / 100m), 2) : 0m;
            int retQty = 0; foreach (DataRow row in _lines.Rows) retQty += ToI(row["RetCount"]);

            try
            {
                int newId, newInvoNo;
                using (var con = new SqlConnection(Program.ConnectionString))
                {
                    con.Open();
                    using (var tx = con.BeginTransaction())
                    {
                        newInvoNo = NextInvoNo(con, tx);
                        using (var cmd = new SqlCommand(@"
INSERT INTO dbo.tblSellInvoice
 (TheName, SellDate, TheQuantity, TotalMony, TotalAfterDiscount, TotalFattInvoice, TotalFinal, TaxAll, TotalAll,
  Notes, UserID, EnterTime, BranchID, CusNumber, InvoNumber, IsReturn, OriginalInvoiceID, ZatcaTypeCode, ZatcaStatus, ZatcaUUID)
 VALUES
 (@name, GETDATE(), @qty, @net, @net, @tax, @total, @tax, @total,
  @notes, @uid, GETDATE(), @branch, @cus, @invo, 1, @orig, '381', 'NONE', NEWID());
SELECT CAST(SCOPE_IDENTITY() AS int);", con, tx))
                        {
                            decimal net = retTotal - retTax;
                            cmd.Parameters.AddWithValue("@name", (object)_customer ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@qty", retQty);
                            cmd.Parameters.AddWithValue("@net", (double)net);
                            cmd.Parameters.AddWithValue("@tax", (double)retTax);
                            cmd.Parameters.AddWithValue("@total", (double)retTotal);
                            cmd.Parameters.AddWithValue("@notes", "مرتجع للفاتورة " + _origInvoNo + " - " + txtReason.Text.Trim());
                            cmd.Parameters.AddWithValue("@uid", (object)Program.User?.ID ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@branch", _branchId);
                            cmd.Parameters.AddWithValue("@cus", _cusNumber);
                            cmd.Parameters.AddWithValue("@invo", newInvoNo);
                            cmd.Parameters.AddWithValue("@orig", _origId);
                            newId = (int)cmd.ExecuteScalar();
                        }
                        foreach (DataRow row in _lines.Rows)
                        {
                            double rl = ToD(row["RetLength"]); int rc = ToI(row["RetCount"]);
                            if (rl <= 0 && rc <= 0) continue;
                            using (var cmd = new SqlCommand("INSERT INTO dbo.tblSellInvoiceDetaile (ClassNumber, Notes, EnterTime, UserID, BranchID, GomashLength, NumClothes, InvoNumber) VALUES (@cn, N'مرتجع', GETDATE(), @uid, @branch, @len, @cnt, @invo)", con, tx))
                            {
                                cmd.Parameters.AddWithValue("@cn", (object)row["ClassNumber"] ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@uid", (object)Program.User?.ID ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@branch", _branchId);
                                cmd.Parameters.AddWithValue("@len", rl);
                                cmd.Parameters.AddWithValue("@cnt", rc);
                                cmd.Parameters.AddWithValue("@invo", newInvoNo);
                                cmd.ExecuteNonQuery();
                            }
                            if (chkRestoreStock.Checked && rl > 0)
                                using (var cmd = new SqlCommand("UPDATE dbo.tblClasse SET QuantityRemin = ISNULL(QuantityRemin,0) + @len WHERE ClassNumber=@cn", con, tx))
                                {
                                    cmd.Parameters.AddWithValue("@len", rl);
                                    cmd.Parameters.AddWithValue("@cn", (object)row["ClassNumber"] ?? DBNull.Value);
                                    cmd.ExecuteNonQuery();
                                }
                        }
                        tx.Commit();
                    }
                }

                string extra = "";
                if (chkReportNow.Checked) extra = ReportToZatca(newId, newInvoNo, retTotal, retTax);

                MessageBox.Show($"تم حفظ المرتجع برقم {newInvoNo} (مبلغ {retTotal:0.00}، ضريبة {retTax:0.00}).{extra}", "نجاح");
                SetEnabled(false); _origId = -1; grid.DataSource = null; txtInfo.Clear(); txtInvoNo.Clear();
            }
            catch (Exception ex) { MessageBox.Show("فشل الحفظ: " + ex.Message); }
        }

        private string ReportToZatca(int newId, int invoNo, decimal total, decimal tax)
        {
            try
            {
                var cfg = ZatcaConfig.Load();
                if (!cfg.Enabled || string.IsNullOrEmpty(cfg.ProductionCert)) return "\n(لم يُبلَّغ للزكاة: النظام غير مفعّل.)";
                var d = new ZatcaInvoiceData
                {
                    InvoiceNumber = "CN-" + invoNo,
                    Uuid = Guid.NewGuid().ToString(),
                    IssueDateTime = DateTime.Now,
                    IsCreditNote = true,
                    OriginalInvoiceNumber = _origInvoNo.ToString(),
                    ReturnReason = txtReason.Text.Trim(),
                    SellerName = cfg.OrgName, SellerVat = cfg.VatNumber,
                    Street = cfg.AddrStreet, Building = cfg.AddrBuilding, Plot = cfg.AddrSecondary, District = cfg.AddrDistrict,
                    City = cfg.AddrCity, Postal = cfg.AddrPostal, Country = cfg.AddrCountry
                };
                decimal net = total - tax;
                d.Lines.Add(new ZatcaLine { Name = "مرتجع", Quantity = 1, UnitPrice = net, VatRate = (decimal)_taxRate });
                var r = ZatcaService.ReportInvoice(cfg, d, out var signed);
                using (var con = new SqlConnection(Program.ConnectionString))
                {
                    con.Open();
                    using (var cmd = new SqlCommand("UPDATE dbo.tblSellInvoice SET ZatcaStatus=@st, ZatcaResponse=@resp, ZatcaQR=@qr, ZatcaInvoiceHash=@h, ZatcaICV=@icv, ZatcaReportedAt=GETDATE() WHERE ID=@id", con))
                    {
                        cmd.Parameters.AddWithValue("@st", r.Ok ? (object)(r.ReportingStatus ?? "REPORTED") : "ERROR");
                        cmd.Parameters.AddWithValue("@resp", (object)(r.RawBody ?? "") ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@qr", (object)signed.QrBase64 ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@h", (object)signed.InvoiceHash ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@icv", d.Icv);
                        cmd.Parameters.AddWithValue("@id", newId);
                        cmd.ExecuteNonQuery();
                    }
                }
                return r.Ok ? "\n(تم الإبلاغ للزكاة بنجاح.)" : "\n(فشل الإبلاغ للزكاة: " + (r.Errors ?? r.RawBody) + ")";
            }
            catch (Exception ex) { return "\n(خطأ بالإبلاغ: " + ex.Message + ")"; }
        }

        private static int NextInvoNo(SqlConnection con, SqlTransaction tx)
        {
            using (var cmd = new SqlCommand("SELECT ISNULL(MAX(InvoNumber),0)+1 FROM dbo.tblSellInvoice", con, tx))
                return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static double ToD(object o) => o == null || o == DBNull.Value ? 0 : Convert.ToDouble(o);
        private static int ToI(object o) => o == null || o == DBNull.Value ? 0 : Convert.ToInt32(o);
    }
}
