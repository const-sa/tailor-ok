using System;
using System.Data.SqlClient;
using System.Text;

namespace SewingSystem.Classes.Zatca
{
    /// <summary>
    /// High-level orchestration of ZATCA onboarding and invoice reporting.
    /// All methods append human-readable progress to <paramref name="log"/> so the
    /// settings screen can show exactly what happened (and any gateway error text).
    /// </summary>
    public static class ZatcaService
    {
        /// <summary>
        /// Full activation against the currently configured environment
        /// (simulation = «تجريبي», production = «مباشر»):
        ///   ensure CSR -> Compliance CSID (needs OTP) -> compliance checks -> Production CSID.
        /// Each successful step is persisted, so a later failure does not lose earlier progress.
        /// </summary>
        public static bool Activate(ZatcaConfig cfg, string otp, StringBuilder log)
        {
            log.AppendLine("== بدء التفعيل على البيئة: " + cfg.Environment + " ==");
            log.AppendLine("الرابط: " + cfg.ApiBaseUrl);

            if (!RequestComplianceCsid(cfg, otp, log)) return false;
            if (!RunComplianceChecks(cfg, log)) { log.AppendLine("تنبيه: لم تكتمل فحوصات الامتثال — راجع الأخطاء أعلاه."); return false; }
            if (!RequestProductionCsid(cfg, log)) return false;

            cfg.Enabled = true;
            cfg.Save();
            log.AppendLine("== اكتمل التفعيل بنجاح. النظام جاهز للإبلاغ. ==");
            return true;
        }

        public static bool RequestComplianceCsid(ZatcaConfig cfg, string otp, StringBuilder log)
        {
            if (string.IsNullOrWhiteSpace(otp)) { log.AppendLine("✘ مطلوب رمز OTP من بوابة فاتورة."); return false; }
            if (string.IsNullOrEmpty(cfg.Csr) || string.IsNullOrEmpty(cfg.PrivateKeyPem))
            {
                log.AppendLine("توليد المفتاح وطلب الشهادة (CSR)...");
                ZatcaCrypto.GenerateKeyPairAndCsr(cfg);
                cfg.Save();
            }
            log.AppendLine("طلب شهادة الامتثال (Compliance CSID)...");
            var api = new ZatcaApiClient(cfg.ApiBaseUrl);
            var r = api.RequestComplianceCsid(cfg.Csr, otp);
            if (!r.Ok || string.IsNullOrEmpty(r.BinarySecurityToken))
            {
                log.AppendLine("✘ فشل (HTTP " + r.HttpStatus + "): " + (r.Errors ?? r.RawBody));
                return false;
            }
            cfg.ComplianceCert = r.BinarySecurityToken;
            cfg.ComplianceSecret = r.Secret;
            cfg.ComplianceRequestId = r.RequestId;
            cfg.Save();
            log.AppendLine("✔ تم الحصول على شهادة الامتثال (RequestID=" + r.RequestId + ").");
            return true;
        }

        public static bool RunComplianceChecks(ZatcaConfig cfg, StringBuilder log)
        {
            if (string.IsNullOrEmpty(cfg.ComplianceCert)) { log.AppendLine("✘ لا توجد شهادة امتثال بعد."); return false; }
            var api = new ZatcaApiClient(cfg.ApiBaseUrl);

            log.AppendLine("فحص: فاتورة مبسطة...");
            if (!CheckOne(cfg, api, false, log)) return false;
            log.AppendLine("فحص: إشعار دائن مبسط...");
            if (!CheckOne(cfg, api, true, log)) return false;

            log.AppendLine("✔ اجتازت فحوصات الامتثال.");
            return true;
        }

        private static bool CheckOne(ZatcaConfig cfg, ZatcaApiClient api, bool creditNote, StringBuilder log)
        {
            var data = BuildSample(cfg, creditNote);
            // وقت التوقيع = وقت إصدار الفاتورة نفسه، ليطابق الـQR وقت الإصدار (KSA-25)
            var signed = ZatcaSigner.Sign(ZatcaUbl.Build(data), cfg, cfg.ComplianceCert, data.IssueDateTime);
            var r = api.ComplianceCheckInvoice(cfg.ComplianceCert, cfg.ComplianceSecret,
                signed.InvoiceHash, data.Uuid, signed.SignedXmlBase64);
            bool ok = r.Ok && !"ERROR".Equals(r.ReportingStatus, StringComparison.OrdinalIgnoreCase);
            log.AppendLine((ok ? "  ✔ " : "  ✘ ") + "HTTP " + r.HttpStatus + " " +
                           (r.ReportingStatus ?? r.DispositionMessage) + " " + (r.Errors ?? ""));
            return ok;
        }

        public static bool RequestProductionCsid(ZatcaConfig cfg, StringBuilder log)
        {
            log.AppendLine("طلب شهادة الإنتاج (Production CSID)...");
            var api = new ZatcaApiClient(cfg.ApiBaseUrl);
            var r = api.RequestProductionCsid(cfg.ComplianceCert, cfg.ComplianceSecret, cfg.ComplianceRequestId);
            if (!r.Ok || string.IsNullOrEmpty(r.BinarySecurityToken))
            {
                log.AppendLine("✘ فشل (HTTP " + r.HttpStatus + "): " + (r.Errors ?? r.RawBody));
                return false;
            }
            cfg.ProductionCert = r.BinarySecurityToken;
            cfg.ProductionSecret = r.Secret;
            cfg.ProductionRequestId = r.RequestId;
            cfg.Save();
            log.AppendLine("✔ تم الحصول على شهادة الإنتاج.");
            return true;
        }

        /// <summary>Sign + report a real invoice/credit-note; updates ICV/PIH on success.</summary>
        public static ZatcaApiResult ReportInvoice(ZatcaConfig cfg, ZatcaInvoiceData data, out ZatcaSignedInvoice signed)
        {
            if (string.IsNullOrEmpty(cfg.ProductionCert))
                throw new InvalidOperationException("النظام غير مفعّل (لا توجد شهادة إنتاج).");

            data.Icv = cfg.LastICV + 1;
            data.Pih = string.IsNullOrEmpty(cfg.LastPIH) ? ZatcaConfig.GenesisPih : cfg.LastPIH;
            signed = ZatcaSigner.Sign(ZatcaUbl.Build(data), cfg, cfg.ProductionCert, data.IssueDateTime);

            var api = new ZatcaApiClient(cfg.ApiBaseUrl);
            var r = api.ReportSingle(cfg.ProductionCert, cfg.ProductionSecret,
                signed.InvoiceHash, data.Uuid, signed.SignedXmlBase64);

            if (r.Ok)
            {
                cfg.LastICV = data.Icv;
                cfg.LastPIH = signed.InvoiceHash;
                cfg.Save();
            }
            return r;
        }

        /// <summary>
        /// Map an existing sell invoice/return (by tblSellInvoice.ID) to a ZATCA
        /// document, sign + report it, and persist the result on the invoice row.
        /// Safe to call manually; never throws into the sales path.
        /// </summary>
        public static ZatcaApiResult ReportExistingInvoice(int invoiceId, StringBuilder log)
        {
            var cfg = ZatcaConfig.Load();
            if (!cfg.Enabled || string.IsNullOrEmpty(cfg.ProductionCert))
                throw new InvalidOperationException("النظام غير مفعّل (لا توجد شهادة إنتاج). فعّل الربط أولاً.");

            var data = ZatcaInvoiceMapper.BuildFromSellInvoice(invoiceId, cfg);
            log?.AppendLine($"إبلاغ الفاتورة {data.InvoiceNumber} (نوع {(data.IsCreditNote ? "381 مرتجع" : "388 فاتورة")})...");
            var r = ReportInvoice(cfg, data, out var signed);

            using (var con = new SqlConnection(Program.ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand(@"UPDATE dbo.tblSellInvoice SET
                        ZatcaStatus=@st, ZatcaResponse=@resp, ZatcaQR=@qr, ZatcaInvoiceHash=@h,
                        ZatcaICV=@icv, ZatcaPIH=@pih, ZatcaTypeCode=@tc, ZatcaUUID=@uuid, ZatcaReportedAt=GETDATE()
                       WHERE ID=@id", con))
                {
                    cmd.Parameters.AddWithValue("@st", r.Ok ? (object)(r.ReportingStatus ?? "REPORTED") : "ERROR");
                    cmd.Parameters.AddWithValue("@resp", (object)(r.RawBody ?? "") ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@qr", (object)signed.QrBase64 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@h", (object)signed.InvoiceHash ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@icv", data.Icv);
                    cmd.Parameters.AddWithValue("@pih", (object)data.Pih ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@tc", data.IsCreditNote ? "381" : "388");
                    cmd.Parameters.AddWithValue("@uuid", Guid.Parse(data.Uuid));
                    cmd.Parameters.AddWithValue("@id", invoiceId);
                    cmd.ExecuteNonQuery();
                }
            }
            log?.AppendLine(r.Ok ? "✔ تم الإبلاغ (" + (r.ReportingStatus ?? "REPORTED") + ")."
                                 : "✘ فشل (HTTP " + r.HttpStatus + "): " + (r.Errors ?? r.RawBody));
            return r;
        }

        private static ZatcaInvoiceData BuildSample(ZatcaConfig cfg, bool creditNote)
        {
            var d = new ZatcaInvoiceData
            {
                InvoiceNumber = (creditNote ? "CN-SAMPLE-" : "INV-SAMPLE-") + (cfg.LastICV + 1),
                Uuid = Guid.NewGuid().ToString(),
                IssueDateTime = DateTime.Now,
                IsCreditNote = creditNote,
                Icv = cfg.LastICV + 1,
                Pih = string.IsNullOrEmpty(cfg.LastPIH) ? ZatcaConfig.GenesisPih : cfg.LastPIH,
                SellerName = cfg.OrgName, SellerVat = cfg.VatNumber,
                Street = cfg.AddrStreet, Building = cfg.AddrBuilding, Plot = cfg.AddrSecondary, District = cfg.AddrDistrict,
                City = cfg.AddrCity, Postal = cfg.AddrPostal, Country = cfg.AddrCountry,
                OriginalInvoiceNumber = creditNote ? "INV-SAMPLE-1" : null
            };
            d.Lines.Add(new ZatcaLine { Name = "Sample item", Quantity = 1, UnitPrice = 100m, VatRate = 15m });
            return d;
        }
    }
}
