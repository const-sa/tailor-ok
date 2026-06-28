using System;
using System.Collections.Generic;

namespace SewingSystem.Classes.Whatsapp
{
    /// <summary>
    /// Builds WhatsApp messages from templates (token replacement) and sends them
    /// via <see cref="CwtsClient"/>. Token format: {اسم_العميل}, {الشركة}, {الهاتف},
    /// {رقم_الفاتورة}, {الاجمالي}, {الضريبة}, {الواصل}, {الباقي}, {تاريخ_التسليم}, {الرقم_الضريبي}.
    /// </summary>
    public static class WhatsappService
    {
        public static string Format(string template, IDictionary<string, string> tokens)
        {
            if (string.IsNullOrEmpty(template)) return "";
            string s = template;
            foreach (var kv in tokens)
                s = s.Replace("{" + kv.Key + "}", kv.Value ?? "");
            return s;
        }

        /// <summary>Builds a token dictionary; pass null for unknown values.</summary>
        public static Dictionary<string, string> Tokens(string customerName, string invoiceNo,
            string total, string tax, string paid, string remaining, string deliveryDate)
        {
            return new Dictionary<string, string>
            {
                ["اسم_العميل"] = customerName,
                ["الشركة"] = SafeBranch(b => b.CompanyName),
                ["الهاتف"] = SafeBranch(b => b.BranchTelephone),
                ["الرقم_الضريبي"] = SafeBranch(b => b.TaxNumber),
                ["رقم_الفاتورة"] = invoiceNo,
                ["الاجمالي"] = total,
                ["الضريبة"] = tax,
                ["الواصل"] = paid,
                ["الباقي"] = remaining,
                ["تاريخ_التسليم"] = deliveryDate
            };
        }

        private static string SafeBranch(Func<LinqModel.tblBranche, string> get)
        {
            try { return Program.Branch != null ? get(Program.Branch) : ""; } catch { return ""; }
        }

        /// <summary>Saudi number normalization (matches the existing waclient logic).</summary>
        public static string NormalizeNumber(string mobile)
        {
            if (string.IsNullOrWhiteSpace(mobile)) return "";
            mobile = mobile.Trim().Replace(" ", "").Replace("+", "");
            if (mobile.Length == 9) return "966" + mobile;
            if (mobile.Length == 10 && mobile.StartsWith("0")) return "966" + mobile.Substring(1);
            return mobile;
        }

        public static CwtsResult Send(WhatsappConfig cfg, string mobile, string message)
        {
            var client = new CwtsClient(cfg.Instance, cfg.Token);
            return client.SendText(NormalizeNumber(mobile), message);
        }
    }
}
