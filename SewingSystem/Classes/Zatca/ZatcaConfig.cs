using System;
using System.Data;
using System.Data.SqlClient;

namespace SewingSystem.Classes.Zatca
{
    /// <summary>
    /// Single-row (Id = 1) configuration / onboarding state for the ZATCA
    /// (هيئة الزكاة) Phase-2 e-invoicing integration. Backed by dbo.tblZatcaConfig.
    ///
    /// Secret material (private key, CSID secrets) is stored AES-encrypted at rest
    /// using the existing <see cref="MyFunaction.Encryption"/> helper, and is
    /// returned here already decrypted.
    /// </summary>
    public class ZatcaConfig
    {
        public const int RowId = 1;

        // --- Environment / org ---
        public string Environment { get; set; } = "simulation"; // sandbox | simulation | production
        public bool Enabled { get; set; }
        public string OrgName { get; set; }
        public string VatNumber { get; set; }
        public string CrNumber { get; set; }
        public string EgsSerialNumber { get; set; }

        // --- National address ---
        public string AddrShort { get; set; }       // العنوان المختصر (e.g. DDGA7482)
        public string AddrBuilding { get; set; }     // رقم المبنى
        public string AddrStreet { get; set; }       // الشارع
        public string AddrSecondary { get; set; }    // الرقم الفرعي (PlotIdentification)
        public string AddrDistrict { get; set; }     // الحي
        public string AddrCity { get; set; }         // المدينة
        public string AddrPostal { get; set; }       // الرمز البريدي
        public string AddrCountry { get; set; } = "SA";

        // --- Crypto / onboarding (decrypted in memory) ---
        public string PrivateKeyPem { get; set; }
        public string Csr { get; set; }
        public string ComplianceCert { get; set; }
        public string ComplianceSecret { get; set; }
        public string ComplianceRequestId { get; set; }
        public string ProductionCert { get; set; }
        public string ProductionSecret { get; set; }
        public string ProductionRequestId { get; set; }

        // --- Invoice chaining ---
        public int LastICV { get; set; }
        public string LastPIH { get; set; }

        /// <summary>Genesis hash used as PIH for the very first reported invoice.</summary>
        public const string GenesisPih = "NWZlY2ViNjZmZmM4NmYzOGQ5NTI3ODZjNmQ2OTZjNzljMmRiYzIzOWRkNGU5MWI0NjcyOWQ3M2EyN2ZiNTdlOQ==";

        /// <summary>Base URL of the ZATCA gateway for the configured environment.</summary>
        public string ApiBaseUrl
        {
            get
            {
                switch ((Environment ?? "").Trim().ToLowerInvariant())
                {
                    case "production":
                        return "https://gw-fatoora.zatca.gov.sa/e-invoicing/core";
                    case "simulation":
                        return "https://gw-fatoora.zatca.gov.sa/e-invoicing/simulation";
                    case "sandbox":
                    default:
                        return "https://gw-fatoora.zatca.gov.sa/e-invoicing/developer-portal";
                }
            }
        }

        /// <summary>
        /// CSR certificate-template name required by ZATCA, per environment.
        /// </summary>
        public string CsrTemplateName
        {
            get
            {
                switch ((Environment ?? "").Trim().ToLowerInvariant())
                {
                    case "production": return "ZATCA-Code-Signing";
                    case "simulation": return "PREZATCA-Code-Signing";
                    case "sandbox":
                    default: return "TSTZATCA-Code-Signing";
                }
            }
        }

        private static string Enc(string v) => string.IsNullOrEmpty(v) ? v : MyFunaction.Encryption(v);
        private static string Dec(string v)
        {
            if (string.IsNullOrWhiteSpace(v)) return v;
            try { return MyFunaction.Decryption(v); } catch { return v; }
        }

        private static string S(object o) => o == null || o == DBNull.Value ? null : o.ToString();

        public static ZatcaConfig Load()
        {
            var c = new ZatcaConfig();
            using (var con = new SqlConnection(Program.ConnectionString))
            using (var cmd = new SqlCommand("SELECT * FROM dbo.tblZatcaConfig WHERE Id=@id", con))
            {
                cmd.Parameters.AddWithValue("@id", RowId);
                con.Open();
                using (var r = cmd.ExecuteReader())
                {
                    if (!r.Read()) return c;
                    c.Environment = S(r["Environment"]) ?? "simulation";
                    c.Enabled = r["Enabled"] != DBNull.Value && Convert.ToBoolean(r["Enabled"]);
                    c.OrgName = S(r["OrgName"]);
                    c.VatNumber = S(r["VatNumber"]);
                    c.CrNumber = S(r["CrNumber"]);
                    c.EgsSerialNumber = S(r["EgsSerialNumber"]);
                    c.AddrShort = S(r["AddrShort"]);
                    c.AddrStreet = S(r["AddrStreet"]);
                    c.AddrBuilding = S(r["AddrBuilding"]);
                    c.AddrSecondary = S(r["AddrSecondary"]);
                    c.AddrDistrict = S(r["AddrDistrict"]);
                    c.AddrCity = S(r["AddrCity"]);
                    c.AddrPostal = S(r["AddrPostal"]);
                    c.AddrCountry = S(r["AddrCountry"]) ?? "SA";
                    c.PrivateKeyPem = Dec(S(r["PrivateKeyPem"]));
                    c.Csr = S(r["Csr"]);
                    c.ComplianceCert = S(r["ComplianceCert"]);
                    c.ComplianceSecret = Dec(S(r["ComplianceSecret"]));
                    c.ComplianceRequestId = S(r["ComplianceRequestId"]);
                    c.ProductionCert = S(r["ProductionCert"]);
                    c.ProductionSecret = Dec(S(r["ProductionSecret"]));
                    c.ProductionRequestId = S(r["ProductionRequestId"]);
                    c.LastICV = r["LastICV"] == DBNull.Value ? 0 : Convert.ToInt32(r["LastICV"]);
                    c.LastPIH = S(r["LastPIH"]);
                }
            }
            return c;
        }

        public void Save()
        {
            const string sql = @"
UPDATE dbo.tblZatcaConfig SET
    Environment=@Environment, Enabled=@Enabled, OrgName=@OrgName, VatNumber=@VatNumber,
    CrNumber=@CrNumber, EgsSerialNumber=@EgsSerialNumber,
    AddrShort=@AddrShort, AddrStreet=@AddrStreet, AddrBuilding=@AddrBuilding, AddrSecondary=@AddrSecondary,
    AddrDistrict=@AddrDistrict, AddrCity=@AddrCity, AddrPostal=@AddrPostal, AddrCountry=@AddrCountry,
    PrivateKeyPem=@PrivateKeyPem, Csr=@Csr,
    ComplianceCert=@ComplianceCert, ComplianceSecret=@ComplianceSecret, ComplianceRequestId=@ComplianceRequestId,
    ProductionCert=@ProductionCert, ProductionSecret=@ProductionSecret, ProductionRequestId=@ProductionRequestId,
    LastICV=@LastICV, LastPIH=@LastPIH, UpdatedAt=GETDATE()
WHERE Id=@Id;";
            using (var con = new SqlConnection(Program.ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                void P(string n, object v) => cmd.Parameters.AddWithValue(n, (object)v ?? DBNull.Value);
                P("@Id", RowId);
                P("@Environment", Environment);
                P("@Enabled", Enabled);
                P("@OrgName", OrgName);
                P("@VatNumber", VatNumber);
                P("@CrNumber", CrNumber);
                P("@EgsSerialNumber", EgsSerialNumber);
                P("@AddrShort", AddrShort);
                P("@AddrStreet", AddrStreet);
                P("@AddrBuilding", AddrBuilding);
                P("@AddrSecondary", AddrSecondary);
                P("@AddrDistrict", AddrDistrict);
                P("@AddrCity", AddrCity);
                P("@AddrPostal", AddrPostal);
                P("@AddrCountry", AddrCountry);
                P("@PrivateKeyPem", Enc(PrivateKeyPem));
                P("@Csr", Csr);
                P("@ComplianceCert", ComplianceCert);
                P("@ComplianceSecret", Enc(ComplianceSecret));
                P("@ComplianceRequestId", ComplianceRequestId);
                P("@ProductionCert", ProductionCert);
                P("@ProductionSecret", Enc(ProductionSecret));
                P("@ProductionRequestId", ProductionRequestId);
                P("@LastICV", LastICV);
                P("@LastPIH", LastPIH);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
