using System;
using System.Data.SqlClient;

namespace SewingSystem.Classes
{
    /// <summary>
    /// قراءة/حفظ حقول «نموذج التفصيل» الإضافية (Brufa واسم القماش) المخزّنة مباشرة
    /// على صف dbo.tblSellInvoice. يستخدم ADO.NET مع تجاهل الأخطاء بأمان، حتى لا
    /// يتعطّل حفظ الفاتورة لو لم تُرحّل الأعمدة بعد على قاعدة معيّنة (محلية/أونلاين).
    /// </summary>
    public static class OrderSlip
    {
        public class SlipData
        {
            public string Brufa;
            public string FabricName;
        }

        /// <summary>يقرأ قيم النموذج لفاتورة بالمعرّف. يعيد قيماً فارغة لو تعذّرت القراءة.</summary>
        public static SlipData Load(int invoiceId)
        {
            var d = new SlipData();
            if (invoiceId <= 0) return d;
            try
            {
                using (var con = new SqlConnection(Program.ConnectionString))
                using (var cmd = new SqlCommand(
                    "SELECT Brufa, FabricName FROM dbo.tblSellInvoice WHERE ID=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", invoiceId);
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                    {
                        if (r.Read())
                        {
                            d.Brufa = r["Brufa"] == DBNull.Value ? null : r["Brufa"].ToString();
                            d.FabricName = r["FabricName"] == DBNull.Value ? null : r["FabricName"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex) { Logger.Log(ex); }
            return d;
        }

        /// <summary>يحفظ قيم النموذج على صف الفاتورة. لا يرمي استثناءً (يُسجّل فقط).</summary>
        public static void Save(int invoiceId, string brufa, string fabricName)
        {
            if (invoiceId <= 0) return;
            try
            {
                using (var con = new SqlConnection(Program.ConnectionString))
                using (var cmd = new SqlCommand(
                    "UPDATE dbo.tblSellInvoice SET Brufa=@b, FabricName=@f WHERE ID=@id", con))
                {
                    cmd.Parameters.AddWithValue("@b", string.IsNullOrWhiteSpace(brufa) ? (object)DBNull.Value : brufa.Trim());
                    cmd.Parameters.AddWithValue("@f", string.IsNullOrWhiteSpace(fabricName) ? (object)DBNull.Value : fabricName.Trim());
                    cmd.Parameters.AddWithValue("@id", invoiceId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { Logger.Log(ex); }
        }
    }
}
