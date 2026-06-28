using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SewingSystem.Classes.Zatca
{
    /// <summary>Result of a ZATCA gateway call (CSID issuance or invoice submission).</summary>
    public class ZatcaApiResult
    {
        public bool Ok;
        public int HttpStatus;
        public string RawBody;
        public string RequestId;
        public string BinarySecurityToken; // = the issued certificate (base64)
        public string Secret;
        public string DispositionMessage;  // ISSUED / REPORTED / CLEARED / ...
        public string ReportingStatus;
        public string Errors;              // flattened warning/error text
    }

    /// <summary>
    /// Thin HTTP client over the ZATCA Fatoora gateway. Uses HttpWebRequest so no
    /// extra assembly reference / binding-redirect is needed on .NET Framework.
    /// The base URL (sandbox / simulation / production) comes from <see cref="ZatcaConfig.ApiBaseUrl"/>.
    /// </summary>
    public class ZatcaApiClient
    {
        private readonly string _baseUrl;
        public ZatcaApiClient(string baseUrl) { _baseUrl = baseUrl.TrimEnd('/'); }

        /// <summary>Step 1 — request a Compliance CSID using the CSR + portal OTP.</summary>
        public ZatcaApiResult RequestComplianceCsid(string csrBase64, string otp)
        {
            var body = JsonConvert.SerializeObject(new { csr = csrBase64 });
            return Send("POST", "/compliance", body, null, null, otp);
        }

        /// <summary>Step 2 — submit a signed sample invoice for the compliance checks.</summary>
        public ZatcaApiResult ComplianceCheckInvoice(string ccsidToken, string ccsidSecret,
            string invoiceHash, string uuid, string signedXmlBase64)
        {
            var body = JsonConvert.SerializeObject(new { invoiceHash, uuid, invoice = signedXmlBase64 });
            return Send("POST", "/compliance/invoices", body, ccsidToken, ccsidSecret, null);
        }

        /// <summary>Step 3 — exchange the passed compliance request for a Production CSID.</summary>
        public ZatcaApiResult RequestProductionCsid(string ccsidToken, string ccsidSecret, string complianceRequestId)
        {
            var body = JsonConvert.SerializeObject(new { compliance_request_id = complianceRequestId });
            return Send("POST", "/production/csids", body, ccsidToken, ccsidSecret, null);
        }

        /// <summary>Per-invoice — report a simplified (B2C) invoice/credit-note.</summary>
        public ZatcaApiResult ReportSingle(string pcsidToken, string pcsidSecret,
            string invoiceHash, string uuid, string signedXmlBase64)
        {
            var body = JsonConvert.SerializeObject(new { invoiceHash, uuid, invoice = signedXmlBase64 });
            return Send("POST", "/invoices/reporting/single", body, pcsidToken, pcsidSecret, null);
        }

        private ZatcaApiResult Send(string method, string path, string jsonBody,
            string authUser, string authPass, string otp)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            var req = (HttpWebRequest)WebRequest.Create(_baseUrl + path);
            req.Method = method;
            req.ContentType = "application/json";
            req.Accept = "application/json";
            req.Headers["Accept-Version"] = "V2";
            req.Headers["Accept-Language"] = "en";
            if (!string.IsNullOrEmpty(otp)) req.Headers["OTP"] = otp;
            if (!string.IsNullOrEmpty(authUser))
            {
                string basic = Convert.ToBase64String(Encoding.UTF8.GetBytes(authUser + ":" + authPass));
                req.Headers["Authorization"] = "Basic " + basic;
            }

            var data = Encoding.UTF8.GetBytes(jsonBody ?? "");
            req.ContentLength = data.Length;
            try { using (var s = req.GetRequestStream()) s.Write(data, 0, data.Length); }
            catch (Exception ex) { return new ZatcaApiResult { Ok = false, RawBody = "WRITE: " + ex.Message }; }

            var result = new ZatcaApiResult();
            try
            {
                using (var resp = (HttpWebResponse)req.GetResponse())
                    result = Parse(resp);
            }
            catch (WebException wex)
            {
                if (wex.Response is HttpWebResponse er) result = Parse(er);
                else { result.Ok = false; result.RawBody = "NET: " + wex.Message; }
            }
            catch (Exception ex) { result.Ok = false; result.RawBody = ex.Message; }
            return result;
        }

        private static ZatcaApiResult Parse(HttpWebResponse resp)
        {
            var r = new ZatcaApiResult { HttpStatus = (int)resp.StatusCode };
            using (var sr = new StreamReader(resp.GetResponseStream() ?? Stream.Null))
                r.RawBody = sr.ReadToEnd();
            r.Ok = r.HttpStatus >= 200 && r.HttpStatus < 300;
            try
            {
                var j = JObject.Parse(r.RawBody);
                r.RequestId = (string)(j["requestID"] ?? j["requestId"]);
                r.BinarySecurityToken = (string)j["binarySecurityToken"];
                r.Secret = (string)j["secret"];
                r.DispositionMessage = (string)j["dispositionMessage"];
                r.ReportingStatus = (string)(j["reportingStatus"] ?? j["clearanceStatus"]);
                var errs = j["errors"] ?? j["validationResults"];
                if (errs != null) r.Errors = errs.ToString();
            }
            catch { /* non-JSON body left in RawBody */ }
            return r;
        }
    }
}
