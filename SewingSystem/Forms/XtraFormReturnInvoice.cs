using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
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
    public class XtraFormReturnInvoice : FormZatcaMaster
    {
        private static readonly Font Bold = new Font("Tahoma", 9.75F, FontStyle.Bold);
        private TextEdit txtInvoNo, txtRetAmount, txtReason;
        private MemoEdit txtInfo;
        private GridControl grid;
        private GridView gridView;
        private CheckEdit chkRestoreStock, chkReportNow;
        private SimpleButton btnLoad, btnFull;
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
            ClientSize = new Size(860, 744);
            MinimumSize = new Size(720, 604);
            Font = new Font("Tahoma", 9.75f);
            ZatcaUi.ApplyIcon(this);

            // ---- editors ----
            txtInvoNo = new TextEdit();
            btnLoad = MkBtn("تحميل الفاتورة", (s, e) => LoadInvoice());
            txtInfo = new MemoEdit();
            txtInfo.Properties.ReadOnly = true;

            grid = new GridControl { RightToLeft = RightToLeft.Yes };
            gridView = new GridView(grid) { GridControl = grid };
            grid.MainView = gridView;
            gridView.OptionsView.ShowGroupPanel = false;
            gridView.OptionsView.ColumnAutoWidth = true;
            gridView.OptionsBehavior.Editable = true;
            gridView.OptionsSelection.MultiSelect = false;
            gridView.Appearance.HeaderPanel.Font = Bold;
            gridView.Appearance.HeaderPanel.Options.UseFont = true;

            txtRetAmount = new TextEdit();
            txtReason = new TextEdit(); txtReason.EditValue = "مرتجع - Return";
            ZatcaUi.Ltr(txtInvoNo, txtRetAmount);     // أرقام إنجليزية (لاتينية)
            chkRestoreStock = new CheckEdit { Text = "إرجاع الكميات للمخزون", Checked = true };
            chkReportNow = new CheckEdit { Text = "إبلاغ الزكاة الآن (يتطلب تفعيلاً ناجحاً)", Checked = false };

            btnFull = MkBtn("مرتجع كامل", (s, e) => FillFull());     // الحفظ صار في شريط الأدوات العلوي

            // ---- layout ----
            var lc = new LayoutControl { Dock = DockStyle.Fill, RightToLeft = RightToLeft.Yes };
            var root = lc.Root;
            root.GroupBordersVisible = false;
            root.Padding = new DevExpress.XtraLayout.Utils.Padding(10);

            // original invoice
            var gInv = root.AddGroup("الفاتورة الأصلية");
            var itNo = gInv.AddItem("رقم الفاتورة الأصلية", txtInvoNo);
            itNo.SizeConstraintsType = SizeConstraintsType.Custom; itNo.MaxSize = new Size(320, 26); itNo.MinSize = new Size(260, 26);
            AddButtonItem(gInv, btnLoad, itNo);
            var itInfo = gInv.AddItem(string.Empty, txtInfo);
            itInfo.TextVisible = false;
            itInfo.SizeConstraintsType = SizeConstraintsType.Custom; itInfo.MinSize = new Size(100, 48); itInfo.MaxSize = new Size(0, 48);

            // items grid (fills remaining height)
            var gItems = root.AddGroup("أصناف الفاتورة — حدّد الكميات المرتجعة");
            var itGrid = gItems.AddItem(string.Empty, grid);
            itGrid.TextVisible = false;

            // return details
            var gRet = root.AddGroup("بيانات المرتجع");
            var itAmount = gRet.AddItem("مبلغ المرتجع (شامل الضريبة)", txtRetAmount);
            itAmount.SizeConstraintsType = SizeConstraintsType.Custom; itAmount.MaxSize = new Size(360, 26); itAmount.MinSize = new Size(300, 26);
            gRet.AddItem("سبب المرتجع", txtReason);
            var itChk1 = gRet.AddItem(string.Empty, chkRestoreStock); itChk1.TextVisible = false;
            var itChk2 = gRet.AddItem(string.Empty, chkReportNow, itChk1, InsertType.Right); itChk2.TextVisible = false;
            AddButtonItem(gRet, btnFull, itChk1, InsertType.Bottom);

            ContentPanel.Controls.Add(lc);   // fill (added first → fills remaining space)
            ContentPanel.Controls.Add(ZatcaUi.Header("مرتجع مبيعات (إشعار دائن)", "نوع 381 — مرتبط بفاتورة بيع أصلية"));

            SetEnabled(false);
        }

        protected override void OnSaveClick() => Save();
        protected override void OnRefreshClick() => LoadInvoice();

        private SimpleButton MkBtn(string t, EventHandler h) { var b = new SimpleButton { Text = t }; b.Click += h; return b; }

        private LayoutControlItem AddButtonItem(LayoutControlGroup g, SimpleButton btn, BaseLayoutItem rel, InsertType insert = InsertType.Right)
        {
            var it = rel == null ? g.AddItem(string.Empty, btn) : g.AddItem(string.Empty, btn, rel, insert);
            it.TextVisible = false;
            it.SizeConstraintsType = SizeConstraintsType.Custom;
            it.MinSize = new Size(150, 38);
            it.MaxSize = new Size(170, 38);
            return it;
        }

        private void SetEnabled(bool on) { grid.Enabled = txtRetAmount.Enabled = txtReason.Enabled = btnFull.Enabled = chkRestoreStock.Enabled = chkReportNow.Enabled = on; BtnSave.Enabled = on; }

        private void LoadInvoice()
        {
            if (!int.TryParse(txtInvoNo.Text.Trim(), out int no)) { XtraMessageBox.Show("أدخل رقم فاتورة صحيح."); return; }
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
                            if (!r.Read()) { XtraMessageBox.Show("لم يتم العثور على الفاتورة."); return; }
                            if (Convert.ToBoolean(r[9])) { XtraMessageBox.Show("هذه الفاتورة نفسها مرتجع، لا يمكن إرجاعها."); return; }
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
            catch (Exception ex) { XtraMessageBox.Show("تعذّر التحميل: " + ex.Message); }
        }

        private void BindGrid()
        {
            grid.DataSource = _lines;
            gridView.PopulateColumns();
            string[] ro = { "ClassNumber", "ClassName", "OrigLength", "OrigCount" };
            foreach (DevExpress.XtraGrid.Columns.GridColumn col in gridView.Columns)
                col.OptionsColumn.AllowEdit = Array.IndexOf(ro, col.FieldName) < 0;
            void H(string n, string h) { var c = gridView.Columns[n]; if (c != null) c.Caption = h; }
            H("ClassNumber", "رقم الصنف"); H("ClassName", "اسم الصنف"); H("OrigLength", "الطول الأصلي");
            H("OrigCount", "العدد الأصلي"); H("RetLength", "الطول المرتجع"); H("RetCount", "العدد المرتجع");
        }

        private void FillFull()
        {
            foreach (DataRow row in _lines.Rows) { row["RetLength"] = row["OrigLength"]; row["RetCount"] = row["OrigCount"]; }
            gridView.RefreshData();
            txtRetAmount.Text = _origTotal.ToString("0.00", CultureInfo.InvariantCulture);
        }

        private void Save()
        {
            gridView.CloseEditor(); gridView.UpdateCurrentRow();
            if (_origId < 0) { XtraMessageBox.Show("حمّل فاتورة أولاً."); return; }
            if (!decimal.TryParse(txtRetAmount.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal retTotal) || retTotal <= 0)
            { XtraMessageBox.Show("أدخل مبلغ مرتجع صحيحاً."); return; }
            if (retTotal > (decimal)_origTotal + 0.01m) { XtraMessageBox.Show("مبلغ المرتجع أكبر من إجمالي الفاتورة."); return; }

            foreach (DataRow row in _lines.Rows)
            {
                double rl = ToD(row["RetLength"]), ol = ToD(row["OrigLength"]);
                int rc = ToI(row["RetCount"]), oc = ToI(row["OrigCount"]);
                if (rl < 0 || rc < 0 || rl > ol + 0.0001 || rc > oc) { XtraMessageBox.Show($"كمية مرتجعة غير صحيحة للصنف {row["ClassNumber"]}."); return; }
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

                XtraMessageBox.Show($"تم حفظ المرتجع برقم {newInvoNo} (مبلغ {retTotal:0.00}، ضريبة {retTax:0.00}).{extra}", "نجاح");
                SetEnabled(false); _origId = -1; grid.DataSource = null; txtInfo.Clear(); txtInvoNo.Clear();
            }
            catch (Exception ex) { XtraMessageBox.Show("فشل الحفظ: " + ex.Message); }
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
