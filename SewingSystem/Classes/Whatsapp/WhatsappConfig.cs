using System;
using System.Data.SqlClient;

namespace SewingSystem.Classes.Whatsapp
{
    /// <summary>
    /// Single-row (Id=1) configuration for the c-wts WhatsApp integration,
    /// backed by dbo.tblWhatsappConfig. The access token is AES-encrypted at rest
    /// (reusing <see cref="MyFunaction.Encryption"/>) and returned decrypted here.
    /// </summary>
    public class WhatsappConfig
    {
        public const int RowId = 1;

        /// <summary>القالب الافتراضي لرسالة «جاهز للاستلام» (يُستخدم ما لم يُحفظ نص مخصّص).</summary>
        public const string DefaultReady =
            "عميلنا {اسم_العميل} 🌹\n" +
            "طلبك جاهز للاستلام ✅\n" +
            "نشكركم على الثقة بنا\n" +
            "{الشركة}";

        public string Provider { get; set; } = "c-wts";
        public string Instance { get; set; }
        public string Token { get; set; }
        public bool Enabled { get; set; }
        public string TplWelcome { get; set; }
        public string TplOrder { get; set; }
        public string TplReady { get; set; } = DefaultReady;
        public string TplDelivered { get; set; }
        public bool SendOnSave { get; set; }
        public bool SendOnReady { get; set; }
        public bool SendOnDelivery { get; set; }

        private static string Enc(string v) => string.IsNullOrEmpty(v) ? v : MyFunaction.Encryption(v);
        private static string Dec(string v)
        {
            if (string.IsNullOrWhiteSpace(v)) return v;
            try { return MyFunaction.Decryption(v); } catch { return v; }
        }
        private static string S(object o) => o == null || o == DBNull.Value ? null : o.ToString();
        private static bool B(object o) => o != null && o != DBNull.Value && Convert.ToBoolean(o);

        public static WhatsappConfig Load()
        {
            var c = new WhatsappConfig();
            using (var con = new SqlConnection(Program.ConnectionString))
            using (var cmd = new SqlCommand("SELECT * FROM dbo.tblWhatsappConfig WHERE Id=@id", con))
            {
                cmd.Parameters.AddWithValue("@id", RowId);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return c;
                    c.Provider = S(r["Provider"]) ?? "c-wts";
                    c.Instance = S(r["Instance"]);
                    c.Token = Dec(S(r["Token"]));
                    c.Enabled = B(r["Enabled"]);
                    c.TplWelcome = S(r["TplWelcome"]);
                    c.TplOrder = S(r["TplOrder"]);
                    var ready = S(r["TplReady"]); if (!string.IsNullOrWhiteSpace(ready)) c.TplReady = ready;
                    c.TplDelivered = S(r["TplDelivered"]);
                    c.SendOnSave = B(r["SendOnSave"]);
                    c.SendOnReady = B(r["SendOnReady"]);
                    c.SendOnDelivery = B(r["SendOnDelivery"]);
                }
            }
            return c;
        }

        public void Save()
        {
            const string sql = @"
UPDATE dbo.tblWhatsappConfig SET
    Provider=@Provider, Instance=@Instance, Token=@Token, Enabled=@Enabled,
    TplWelcome=@TplWelcome, TplOrder=@TplOrder, TplReady=@TplReady, TplDelivered=@TplDelivered,
    SendOnSave=@SendOnSave, SendOnReady=@SendOnReady, SendOnDelivery=@SendOnDelivery, UpdatedAt=GETDATE()
WHERE Id=@Id;";
            using (var con = new SqlConnection(Program.ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                void P(string n, object v) => cmd.Parameters.AddWithValue(n, (object)v ?? DBNull.Value);
                P("@Id", RowId);
                P("@Provider", Provider);
                P("@Instance", Instance);
                P("@Token", Enc(Token));
                P("@Enabled", Enabled);
                P("@TplWelcome", TplWelcome);
                P("@TplOrder", TplOrder);
                P("@TplReady", TplReady);
                P("@TplDelivered", TplDelivered);
                P("@SendOnSave", SendOnSave);
                P("@SendOnReady", SendOnReady);
                P("@SendOnDelivery", SendOnDelivery);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
