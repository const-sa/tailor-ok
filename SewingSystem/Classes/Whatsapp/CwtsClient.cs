using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace SewingSystem.Classes.Whatsapp
{
    public class CwtsResult
    {
        public bool Ok;
        public int HttpStatus;
        public string RawBody;
        public string Status;       // connected / disconnected ...
        public string Phone;
        public string MessageId;
        public int? DaysRemaining;
        public string Error;
    }

    /// <summary>
    /// Client for the c-wts.com WhatsApp gateway.
    /// Credentials (instance_id + access_token) are passed in the query string
    /// (as the provider documents) to survive HTTP→HTTPS redirects.
    /// </summary>
    public class CwtsClient
    {
        private const string BaseUrl = "https://c-wts.com";
        private readonly string _instance, _token;

        public CwtsClient(string instance, string token) { _instance = instance; _token = token; }

        private string Auth(string path) =>
            $"{BaseUrl}{path}?instance_id={Uri.EscapeDataString(_instance ?? "")}&access_token={Uri.EscapeDataString(_token ?? "")}";

        /// <summary>GET /api/status — connection state + subscription.</summary>
        public CwtsResult GetStatus()
        {
            return Request("GET", Auth("/api/status"), null);
        }

        /// <summary>POST /api/send — plain text message.</summary>
        public CwtsResult SendText(string number, string message)
        {
            string body = "number=" + Uri.EscapeDataString(number ?? "") + "&message=" + Uri.EscapeDataString(message ?? "");
            return Request("POST", Auth("/api/send"), body);
        }

        /// <summary>POST /api/send-media — file via URL.</summary>
        public CwtsResult SendMedia(string number, string mediaUrl, string type, string caption)
        {
            string body = "number=" + Uri.EscapeDataString(number ?? "") +
                          "&media_url=" + Uri.EscapeDataString(mediaUrl ?? "") +
                          "&type=" + Uri.EscapeDataString(type ?? "document") +
                          "&caption=" + Uri.EscapeDataString(caption ?? "");
            return Request("POST", Auth("/api/send-media"), body);
        }

        private CwtsResult Request(string method, string url, string formBody)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
            var r = new CwtsResult();
            try
            {
                var req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = method;
                req.Accept = "application/json";
                if (method == "POST")
                {
                    req.ContentType = "application/x-www-form-urlencoded";
                    var data = Encoding.UTF8.GetBytes(formBody ?? "");
                    req.ContentLength = data.Length;
                    using (var s = req.GetRequestStream()) s.Write(data, 0, data.Length);
                }
                using (var resp = (HttpWebResponse)req.GetResponse())
                    Parse(resp, r);
            }
            catch (WebException wex)
            {
                if (wex.Response is HttpWebResponse er) Parse(er, r);
                else { r.Ok = false; r.Error = wex.Message; r.RawBody = wex.Message; }
            }
            catch (Exception ex) { r.Ok = false; r.Error = ex.Message; r.RawBody = ex.Message; }
            return r;
        }

        private static void Parse(HttpWebResponse resp, CwtsResult r)
        {
            r.HttpStatus = (int)resp.StatusCode;
            using (var sr = new StreamReader(resp.GetResponseStream() ?? Stream.Null))
                r.RawBody = sr.ReadToEnd();
            try
            {
                var j = JObject.Parse(r.RawBody);
                r.Ok = (bool?)j["ok"] ?? (r.HttpStatus >= 200 && r.HttpStatus < 300);
                r.Status = (string)j["status"];
                r.Phone = (string)j["phone"];
                r.MessageId = (string)j["message_id"];
                r.Error = (string)(j["error"] ?? j["message"]);
                if (j["subscription"]?["days_remaining"] != null)
                    r.DaysRemaining = (int?)j["subscription"]["days_remaining"];
            }
            catch { r.Ok = r.HttpStatus >= 200 && r.HttpStatus < 300; }
        }
    }
}
