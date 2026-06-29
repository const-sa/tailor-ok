using System;
using System.Data.SqlClient;

namespace SewingSystem.Classes
{
    /// <summary>
    /// حالة تفعيل رخصة البرنامج، مخزّنة في قاعدة البيانات (صف واحد Id=1) في جدول
    /// dbo.tblActivation بدل ملف App.config المحلي — فتنتقل مع نسخة قاعدة البيانات.
    /// مرتبطة برقم المعالج (MachineSerial) للتحقق من الجهاز. الجدول يُنشأ تلقائياً إن لم يوجد.
    /// </summary>
    public class ActivationConfig
    {
        public const int RowId = 1;

        public bool IsActivated { get; set; }
        public string MachineSerial { get; set; }
        public DateTime? ActivatedAt { get; set; }

        private static void EnsureTable(SqlConnection con)
        {
            const string sql = @"
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'tblActivation')
BEGIN
    CREATE TABLE dbo.tblActivation (
        Id            int          NOT NULL CONSTRAINT PK_tblActivation PRIMARY KEY,
        IsActivated   bit          NOT NULL CONSTRAINT DF_tblActivation_IsActivated DEFAULT(0),
        MachineSerial nvarchar(200) NULL,
        ActivatedAt   datetime     NULL,
        UpdatedAt     datetime     NULL
    );
    INSERT INTO dbo.tblActivation (Id, IsActivated) VALUES (1, 0);
END
ELSE IF NOT EXISTS (SELECT 1 FROM dbo.tblActivation WHERE Id = 1)
    INSERT INTO dbo.tblActivation (Id, IsActivated) VALUES (1, 0);";
            using (var cmd = new SqlCommand(sql, con))
                cmd.ExecuteNonQuery();
        }

        private static string S(object o) => o == null || o == DBNull.Value ? null : o.ToString();

        public static ActivationConfig Load()
        {
            var c = new ActivationConfig();
            using (var con = new SqlConnection(Program.ConnectionString))
            {
                con.Open();
                EnsureTable(con);
                using (var cmd = new SqlCommand("SELECT IsActivated, MachineSerial, ActivatedAt FROM dbo.tblActivation WHERE Id=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", RowId);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (!r.Read()) return c;
                        c.IsActivated = r["IsActivated"] != DBNull.Value && Convert.ToBoolean(r["IsActivated"]);
                        c.MachineSerial = S(r["MachineSerial"]);
                        c.ActivatedAt = r["ActivatedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["ActivatedAt"]);
                    }
                }
            }
            return c;
        }

        public void Save()
        {
            using (var con = new SqlConnection(Program.ConnectionString))
            {
                con.Open();
                EnsureTable(con);
                const string sql = @"
UPDATE dbo.tblActivation
   SET IsActivated=@IsActivated, MachineSerial=@MachineSerial, ActivatedAt=@ActivatedAt, UpdatedAt=GETDATE()
 WHERE Id=@Id;";
                using (var cmd = new SqlCommand(sql, con))
                {
                    void P(string n, object v) => cmd.Parameters.AddWithValue(n, (object)v ?? DBNull.Value);
                    P("@Id", RowId);
                    P("@IsActivated", IsActivated);
                    P("@MachineSerial", MachineSerial);
                    P("@ActivatedAt", ActivatedAt);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>هل البرنامج مفعّل لهذا الجهاز تحديداً (مطابقة رقم المعالج).</summary>
        public static bool IsActivatedForMachine(string currentSerial)
        {
            try
            {
                var c = Load();
                return c.IsActivated
                    && !string.IsNullOrEmpty(c.MachineSerial)
                    && string.Equals(c.MachineSerial, currentSerial, StringComparison.OrdinalIgnoreCase);
            }
            catch { return false; }
        }
    }
}
