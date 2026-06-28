using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using Newtonsoft.Json.Linq;

namespace SewingSystem.Forms
{
    /// <summary>
    /// تقارير الزكاة: قائمة الفواتير المُبلّغة وحالتها، مع استخراج الأخطاء
    /// والتنبيهات القادمة من رد الهيئة وأسبابها. مبنية بعناصر DevExpress.
    /// </summary>
    public class XtraFormZatcaReport : DevExpress.XtraEditors.XtraForm
    {
        private static readonly Font Bold = new Font("Tahoma", 9.75F, FontStyle.Bold);
        private GridControl grid;
        private GridView view;
        private ComboBoxEdit cboFilter;
        private MemoEdit txtDetail;
        private DataTable _table;

        public XtraFormZatcaReport() { BuildUi(); LoadData(); }

        private void BuildUi()
        {
            Text = "تقارير الزكاة (الأخطاء والتنبيهات)";
            RightToLeft = RightToLeft.Yes; RightToLeftLayout = false;
            AutoScaleMode = AutoScaleMode.None;
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(900, 600);

            // top filter bar
            var grpTop = Grp("تصفية", new Point(10, 8), new Size(880, 56));
            cboFilter = new ComboBoxEdit { Location = new Point(690, 22), Size = new Size(180, 24) };
            cboFilter.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            cboFilter.Properties.Items.AddRange(new object[] { "الكل", "أخطاء فقط", "تنبيهات", "مبلّغة بنجاح", "غير مبلّغة" });
            cboFilter.SelectedIndex = 0;
            cboFilter.SelectedIndexChanged += (s, e) => ApplyFilter();
            var btnRefresh = new SimpleButton { Text = "تحديث", Location = new Point(520, 20), Size = new Size(150, 28) };
            btnRefresh.ImageOptions.Image = Classes.Zatca.ZatcaIcon.Get(16);
            btnRefresh.Click += (s, e) => LoadData();
            grpTop.Controls.Add(new LabelControl { Text = "العرض:", Location = new Point(815, 25), AutoSizeMode = LabelAutoSizeMode.None, Size = new Size(50, 18), Appearance = { Font = Bold, TextOptions = { HAlignment = DevExpress.Utils.HorzAlignment.Far } } });
            grpTop.Controls.Add(cboFilter);
            grpTop.Controls.Add(btnRefresh);

            // grid
            var grpGrid = Grp("الفواتير وحالة الإبلاغ", new Point(10, 70), new Size(880, 350));
            grid = new GridControl { Location = new Point(8, 26), Size = new Size(864, 316), RightToLeft = RightToLeft.Yes };
            view = new GridView(grid) { GridControl = grid };
            grid.MainView = view;
            view.OptionsBehavior.Editable = false;
            view.OptionsView.ColumnAutoWidth = true;
            view.OptionsView.ShowGroupPanel = false;
            view.Appearance.HeaderPanel.Font = Bold;
            view.Appearance.HeaderPanel.Options.UseFont = true;
            view.FocusedRowChanged += (s, e) => ShowDetail();
            view.RowStyle += View_RowStyle;
            grpGrid.Controls.Add(grid);

            // detail
            var grpDetail = Grp("تفاصيل الرد / السبب", new Point(10, 426), new Size(880, 165));
            txtDetail = new MemoEdit { Location = new Point(8, 26), Size = new Size(864, 130) };
            txtDetail.Properties.ReadOnly = true;
            grpDetail.Controls.Add(txtDetail);

            Controls.Add(grpGrid);
            Controls.Add(grpDetail);
            Controls.Add(grpTop);
        }

        private GroupControl Grp(string text, Point loc, Size size)
        {
            var g = new GroupControl { Text = text, Location = loc, Size = size };
            g.AppearanceCaption.Font = Bold; g.AppearanceCaption.Options.UseFont = true;
            return g;
        }

        private void LoadData()
        {
            try
            {
                _table = new DataTable();
                _table.Columns.Add("رقم الفاتورة", typeof(int));
                _table.Columns.Add("التاريخ", typeof(string));
                _table.Columns.Add("النوع", typeof(string));
                _table.Columns.Add("الحالة", typeof(string));
                _table.Columns.Add("تنبيهات", typeof(int));
                _table.Columns.Add("السبب", typeof(string));
                _table.Columns.Add("_raw", typeof(string));
                _table.Columns.Add("_level", typeof(string));

                using (var con = new SqlConnection(Program.ConnectionString))
                using (var cmd = new SqlCommand(
                    "SELECT InvoNumber, ZatcaTypeCode, ZatcaStatus, ZatcaResponse, ISNULL(ZatcaReportedAt, SellDate) AS Dt " +
                    "FROM dbo.tblSellInvoice WHERE ZatcaStatus IS NOT NULL ORDER BY ISNULL(ZatcaReportedAt, SellDate) DESC", con))
                {
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                        while (r.Read())
                        {
                            string status = r["ZatcaStatus"] == DBNull.Value ? "" : r["ZatcaStatus"].ToString();
                            string raw = r["ZatcaResponse"] == DBNull.Value ? "" : r["ZatcaResponse"].ToString();
                            string type = r["ZatcaTypeCode"]?.ToString() == "381" ? "مرتجع (381)" : "فاتورة (388)";
                            var parsed = Parse(status, raw);
                            _table.Rows.Add(
                                r["InvoNumber"] == DBNull.Value ? 0 : Convert.ToInt32(r["InvoNumber"]),
                                r["Dt"] == DBNull.Value ? "" : Convert.ToDateTime(r["Dt"]).ToString("yyyy-MM-dd HH:mm"),
                                type, StatusText(parsed.Level, status), parsed.WarnCount, parsed.Reason, raw, parsed.Level);
                        }
                }
                grid.DataSource = _table.DefaultView;
                if (view.Columns["_raw"] != null) view.Columns["_raw"].Visible = false;
                if (view.Columns["_level"] != null) view.Columns["_level"].Visible = false;
                if (view.Columns["السبب"] != null) view.Columns["السبب"].Width = 380;
                ApplyFilter();
            }
            catch (Exception ex) { XtraMessageBox.Show("تعذّر تحميل التقرير: " + ex.Message); }
        }

        private void ApplyFilter()
        {
            if (_table == null) return;
            string f;
            switch (cboFilter.SelectedIndex)
            {
                case 1: f = "_level='ERROR'"; break;
                case 2: f = "_level='WARNING'"; break;
                case 3: f = "_level='OK' OR _level='WARNING'"; break;
                case 4: f = "_level='NONE'"; break;
                default: f = ""; break;
            }
            _table.DefaultView.RowFilter = f;
            ShowDetail();
        }

        private void ShowDetail()
        {
            try
            {
                int h = view.FocusedRowHandle;
                if (h < 0) { txtDetail.Text = ""; return; }
                string reason = Convert.ToString(view.GetRowCellValue(h, "السبب"));
                string raw = Convert.ToString(view.GetRowCellValue(h, "_raw"));
                txtDetail.Text = (string.IsNullOrWhiteSpace(reason) ? "" : "السبب:\r\n" + reason + "\r\n\r\n") +
                                 "الرد الكامل:\r\n" + raw;
            }
            catch { }
        }

        private void View_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            if (e.RowHandle < 0) return;
            string lvl = Convert.ToString(view.GetRowCellValue(e.RowHandle, "_level"));
            if (lvl == "ERROR") { e.Appearance.BackColor = Color.FromArgb(255, 224, 224); }
            else if (lvl == "WARNING") { e.Appearance.BackColor = Color.FromArgb(255, 247, 214); }
            else if (lvl == "OK") { e.Appearance.BackColor = Color.FromArgb(224, 245, 224); }
        }

        private static string StatusText(string level, string status)
        {
            switch (level)
            {
                case "ERROR": return "خطأ ✘";
                case "WARNING": return "تنبيه ⚠";
                case "OK": return "مبلّغة ✔";
                case "NONE": return "غير مبلّغة";
                default: return status;
            }
        }

        private struct ParsedResp { public string Level; public string Reason; public int WarnCount; }

        /// <summary>Extracts a human-readable reason + level (ERROR/WARNING/OK/NONE) from a ZATCA response.</summary>
        private static ParsedResp Parse(string status, string json)
        {
            var p = new ParsedResp { Level = "OK", Reason = "", WarnCount = 0 };
            if (string.Equals(status, "ERROR", StringComparison.OrdinalIgnoreCase)) p.Level = "ERROR";
            if (string.Equals(status, "NONE", StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(status)) p.Level = "NONE";

            if (string.IsNullOrWhiteSpace(json)) return p;
            try
            {
                var j = JObject.Parse(json);
                var sb = new StringBuilder();
                var vr = j["validationResults"];
                AppendMsgs(vr?["errorMessages"], "خطأ", sb, out int errs);
                AppendMsgs(j["errors"], "خطأ", sb, out int errs2);
                int warnCount;
                AppendMsgs(vr?["warningMessages"], "تنبيه", sb, out warnCount);
                p.WarnCount = warnCount;

                if (errs + errs2 > 0) p.Level = "ERROR";
                else if (warnCount > 0 && p.Level != "ERROR" && p.Level != "NONE") p.Level = "WARNING";

                string disp = (string)(j["dispositionMessage"] ?? j["reportingStatus"] ?? j["clearanceStatus"]);
                if (sb.Length == 0 && !string.IsNullOrEmpty(disp)) sb.Append(disp);
                p.Reason = sb.ToString().Trim();
            }
            catch { p.Reason = json; }
            return p;
        }

        private static void AppendMsgs(JToken arr, string tag, StringBuilder sb, out int count)
        {
            count = 0;
            if (!(arr is JArray a)) return;
            foreach (var m in a)
            {
                string code = (string)(m["code"] ?? m["type"]);
                string msg = (string)(m["message"] ?? m["category"] ?? m);
                sb.Append("• [").Append(tag).Append(code == null ? "" : " " + code).Append("] ").Append(msg).Append("\r\n");
                count++;
            }
        }
    }
}
