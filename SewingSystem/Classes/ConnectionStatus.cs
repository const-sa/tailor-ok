using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SewingSystem.Classes
{
    /// <summary>
    /// Helpers for the "internal / external" database connection feature:
    /// builds/tests a SQL Server connection and draws the red (disconnected) /
    /// green (connected) status dot used in the connection dialog and the main
    /// form status bar.
    /// </summary>
    public static class ConnectionStatus
    {
        // Connection-type keys persisted (encrypted) in Properties.Settings.ConnType.
        public const string Internal = "Internal"; // اتصال داخلي (خادم محلي)
        public const string External = "External"; // اتصال خارجي (خادم بعيد)

        // ----- External (remote) server preset -----
        public const string ExternalServer = "SQL9001.site4now.net";
        public const string ExternalDB = "db_aa69fb_dbtsh2";
        public const string ExternalUser = "db_aa69fb_dbtsh2_admin";
        public const string ExternalPassword = "Qaz21002100";

        // ----- Internal (local) server preset -----
        public const string InternalServer = ".\\SQLEXPR14PLANETS";
        public const string InternalDB = "SewingSystem2";

        /// <summary>
        /// Opens a short-timeout connection to verify the server/DB is reachable.
        /// Returns true when connected, false otherwise (error text in <paramref name="error"/>).
        /// </summary>
        public static bool Test(string server, string db, bool windowsAuth,
            string user, string password, out string error, int timeoutSeconds = 5)
        {
            error = "";
            try
            {
                var b = new SqlConnectionStringBuilder
                {
                    DataSource = server,
                    InitialCatalog = db,
                    ConnectTimeout = timeoutSeconds
                };
                if (windowsAuth)
                {
                    b.IntegratedSecurity = true;
                }
                else
                {
                    b.UserID = user;
                    b.Password = password;
                }

                using (var con = new SqlConnection(b.ConnectionString))
                {
                    con.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>Tests the connection currently active in Program.* (decrypted settings).</summary>
        public static bool TestCurrent()
        {
            string err;
            return Test(Program.ServerName, Program.DBName,
                Program.Mode == "Windows", Program.SqlUserName, Program.SqlPassword, out err);
        }

        /// <summary>The Arabic label for the persisted connection type.</summary>
        public static string TypeCaption(string connType)
        {
            return connType == External ? "خارجي" : "داخلي";
        }

        /// <summary>Draws a filled circle: green when connected, red when not.</summary>
        public static Image Dot(bool connected, int size = 14)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                Color fill = connected
                    ? Color.FromArgb(46, 184, 92)   // أخضر = متصل
                    : Color.FromArgb(214, 48, 48);  // أحمر = غير متصل
                using (var brush = new SolidBrush(fill))
                    g.FillEllipse(brush, 1, 1, size - 3, size - 3);
                using (var pen = new Pen(Color.FromArgb(90, 0, 0, 0)))
                    g.DrawEllipse(pen, 1, 1, size - 3, size - 3);
            }
            return bmp;
        }
    }
}
