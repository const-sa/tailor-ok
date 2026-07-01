using System;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace SewingSystem.Classes.Zatca
{
    /// <summary>Outcome of signing one invoice: the artifacts the gateway/QR need.</summary>
    public class ZatcaSignedInvoice
    {
        public string SignedXml;        // full UBL with UBLExtensions + QR filled
        public string SignedXmlBase64;  // base64(UTF8(SignedXml)) for the API body
        public string InvoiceHash;      // base64 SHA256 (also the next invoice's PIH)
        public string QrBase64;         // QR payload (tags 1-9)
    }

    /// <summary>
    /// Implements the ZATCA Phase-2 signing pipeline for a UBL invoice:
    ///   invoice hash (canonical XML, SHA256) -> XAdES signature (ECDSA) ->
    ///   QR tags 1-9 -> embed UBLExtensions + QR back into the document.
    ///
    /// IMPORTANT: canonicalization here uses .NET C14N 1.0. ZATCA specifies C14N 1.1;
    /// the structure follows the spec but MUST be validated/iterated against the
    /// simulation gateway (the gateway returns precise hash/signature errors).
    /// </summary>
    public static class ZatcaSigner
    {
        public static ZatcaSignedInvoice Sign(string ublXml, ZatcaConfig cfg, string certBase64, DateTime signingTimeUtc)
        {
            // 1) invoice hash: canonical form with UBLExtensions / Signature / QR removed
            string invoiceHash = ComputeInvoiceHash(ublXml);

            // 2) parse certificate (رمز الهيئة قد يكون base64(DER) أو base64(نص base64/PEM)) + private key
            string certBodyB64;
            var cert = ParseComplianceCert(certBase64, out certBodyB64);
            AsymmetricKeyParameter priv = ZatcaCrypto.LoadPrivateKey(cfg.PrivateKeyPem);

            // ZATCA: البصمة = Base64( hex( SHA256(...) ) ) — للشهادة و SignedProperties (مطابق للباكج الرسمي الشغّال)
            string certHash = B64Hex(Sha256(Encoding.UTF8.GetBytes(certBodyB64)));
            string issuerName = cert.IssuerDN.ToString();
            string serial = cert.SerialNumber.ToString();

            // 3) SignedProperties: قالب ثابت التنسيق بالضبط (بلا C14N)، وبصمته Base64(hex(SHA256(trim)))
            string signedProps = BuildSignedProperties(signingTimeUtc, certHash, issuerName, serial);
            string spDigest = B64Hex(Sha256(Encoding.UTF8.GetBytes(signedProps)));

            // 4) SignedInfo -> ECDSA signature
            string signedInfo = BuildSignedInfo(invoiceHash, spDigest);
            byte[] sigBytes = EcdsaSign(Encoding.UTF8.GetBytes(Canonicalize(signedInfo)), priv);
            string signatureValue = Convert.ToBase64String(sigBytes);

            // 5) QR tags 1-9
            byte[] pubKey = Org.BouncyCastle.X509.SubjectPublicKeyInfoFactory
                .CreateSubjectPublicKeyInfo(cert.GetPublicKey()).GetDerEncoded();
            var qr = BuildQr(cfg, ublXml, signingTimeUtc, invoiceHash, sigBytes, pubKey, cert.GetSignature());
            string qrBase64 = qr;

            // 6) assemble UBLExtensions (XAdES) and inject QR + signature
            string ext = BuildUblExtensions(signedInfo, signatureValue, certBodyB64, signedProps);
            string finalXml = ublXml
                .Replace(ZatcaUbl.SignaturePlaceholder, ext)
                .Replace(ZatcaUbl.QrPlaceholder, qrBase64);

            return new ZatcaSignedInvoice
            {
                SignedXml = finalXml,
                SignedXmlBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(finalXml)),
                InvoiceHash = invoiceHash,
                QrBase64 = qrBase64
            };
        }

        /// <summary>
        /// يقرأ شهادة الامتثال/الإنتاج القادمة من الهيئة بصيغة مرنة: قد تكون base64 للـDER مباشرة،
        /// أو base64 لنصٍّ base64/PEM (الشائع في ZATCA). يُعيد الشهادة مع جسمها base64 (DER) للتضمين.
        /// يرمي رسالة واضحة بدل NullReference إن تعذّرت القراءة.
        /// </summary>
        private static Org.BouncyCastle.X509.X509Certificate ParseComplianceCert(string token, out string certBodyBase64)
        {
            var parser = new X509CertificateParser();
            string t = (token ?? "").Trim();

            // محاولة 1: الرمز نفسه = base64(DER) مباشرة
            try
            {
                var der = Convert.FromBase64String(t);
                var c = parser.ReadCertificate(der);
                if (c != null) { certBodyBase64 = t; return c; }
            }
            catch { }

            // محاولة 2: الرمز = base64( نص base64/PEM للشهادة ) — فكّ مرتين
            try
            {
                string inner = Encoding.UTF8.GetString(Convert.FromBase64String(t)).Trim();
                if (inner.IndexOf("CERTIFICATE", StringComparison.OrdinalIgnoreCase) >= 0)
                    inner = inner.Replace("-----BEGIN CERTIFICATE-----", "").Replace("-----END CERTIFICATE-----", "")
                                 .Replace("\r", "").Replace("\n", "").Replace(" ", "").Trim();
                var der = Convert.FromBase64String(inner);
                var c = parser.ReadCertificate(der);
                if (c != null) { certBodyBase64 = inner; return c; }
            }
            catch { }

            throw new Exception("تعذّر قراءة شهادة الامتثال من الهيئة (صيغة الرمز غير متوقّعة).");
        }

        private static string ComputeInvoiceHash(string ublXml)
        {
            var doc = new XmlDocument { PreserveWhitespace = true };
            doc.LoadXml(ublXml.Replace(ZatcaUbl.SignaturePlaceholder, ""));
            var ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("ext", "urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2");
            ns.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
            ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");

            Remove(doc, ns, "//ext:UBLExtensions");
            Remove(doc, ns, "//cac:Signature");
            Remove(doc, ns, "//cac:AdditionalDocumentReference[cbc:ID='QR']");

            return Convert.ToBase64String(Sha256(Encoding.UTF8.GetBytes(Canonicalize(doc.OuterXml))));
        }

        private static void Remove(XmlDocument doc, XmlNamespaceManager ns, string xpath)
        {
            var node = doc.SelectSingleNode(xpath, ns);
            node?.ParentNode?.RemoveChild(node);
        }

        private static string Canonicalize(string xml)
        {
            var doc = new XmlDocument { PreserveWhitespace = true };
            doc.LoadXml(xml);
            var t = new XmlDsigC14NTransform();
            t.LoadInput(doc);
            using (var ms = (MemoryStream)t.GetOutput(typeof(Stream)))
                return Encoding.UTF8.GetString(ms.ToArray());
        }

        private static byte[] Sha256(byte[] data) { using (var s = SHA256.Create()) return s.ComputeHash(data); }

        private static byte[] EcdsaSign(byte[] data, AsymmetricKeyParameter priv)
        {
            ISigner signer = SignerUtilities.GetSigner("SHA256withECDSA");
            signer.Init(true, priv);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.GenerateSignature();
        }

        // القالب الحرفي المطابق بايت-بايت للباكج الرسمي الشغّال: مسافات ثابتة (40/44/48/52/56 والإغلاق يعكسها، الأخير 36)،
        // أسطر LF، و<ds:DigestMethod ...> </ds:DigestMethod> بمسافة داخلية (ليس self-closing). لا يُقنّن (no C14N).
        private static string BuildSignedProperties(DateTime t, string certHashB64, string issuer, string serial)
        {
            string ts = t.ToString("yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture);
            const string DS = "http://www.w3.org/2000/09/xmldsig#";
            const string NL = "\n";
            string I36 = new string(' ', 36), I40 = new string(' ', 40), I44 = new string(' ', 44),
                   I48 = new string(' ', 48), I52 = new string(' ', 52), I56 = new string(' ', 56);
            return
              "<xades:SignedProperties xmlns:xades=\"http://uri.etsi.org/01903/v1.3.2#\" Id=\"xadesSignedProperties\">" + NL +
              I40 + "<xades:SignedSignatureProperties>" + NL +
              I44 + "<xades:SigningTime>" + ts + "</xades:SigningTime>" + NL +
              I44 + "<xades:SigningCertificate>" + NL +
              I48 + "<xades:Cert>" + NL +
              I52 + "<xades:CertDigest>" + NL +
              I56 + "<ds:DigestMethod xmlns:ds=\"" + DS + "\" Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"> </ds:DigestMethod>" + NL +
              I56 + "<ds:DigestValue xmlns:ds=\"" + DS + "\">" + certHashB64 + "</ds:DigestValue>" + NL +
              I52 + "</xades:CertDigest>" + NL +
              I52 + "<xades:IssuerSerial>" + NL +
              I56 + "<ds:X509IssuerName xmlns:ds=\"" + DS + "\">" + System.Security.SecurityElement.Escape(issuer) + "</ds:X509IssuerName>" + NL +
              I56 + "<ds:X509SerialNumber xmlns:ds=\"" + DS + "\">" + serial + "</ds:X509SerialNumber>" + NL +
              I52 + "</xades:IssuerSerial>" + NL +
              I48 + "</xades:Cert>" + NL +
              I44 + "</xades:SigningCertificate>" + NL +
              I40 + "</xades:SignedSignatureProperties>" + NL +
              I36 + "</xades:SignedProperties>";
        }

        /// <summary>SHA256 مجزّأ ثم hex ثم Base64 — صيغة بصمات ZATCA (Base64(hex(sha256))).</summary>
        private static string B64Hex(byte[] hash)
            => Convert.ToBase64String(Encoding.UTF8.GetBytes(ToHex(hash)));

        private static string ToHex(byte[] b)
        {
            var sb = new StringBuilder(b.Length * 2);
            foreach (var x in b) sb.Append(x.ToString("x2"));
            return sb.ToString();
        }

        private static string BuildSignedInfo(string invoiceHashB64, string spDigestB64)
        {
            return
              "<ds:SignedInfo xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\">" +
              "<ds:CanonicalizationMethod Algorithm=\"http://www.w3.org/2006/12/xml-c14n11\"/>" +
              "<ds:SignatureMethod Algorithm=\"http://www.w3.org/2001/04/xmldsig-more#ecdsa-sha256\"/>" +
              "<ds:Reference Id=\"invoiceSignedData\" URI=\"\">" +
              "<ds:Transforms>" +
              "<ds:Transform Algorithm=\"http://www.w3.org/TR/1999/REC-xpath-19991116\"><ds:XPath>not(//ancestor-or-self::ext:UBLExtensions)</ds:XPath></ds:Transform>" +
              "<ds:Transform Algorithm=\"http://www.w3.org/TR/1999/REC-xpath-19991116\"><ds:XPath>not(//ancestor-or-self::cac:Signature)</ds:XPath></ds:Transform>" +
              "<ds:Transform Algorithm=\"http://www.w3.org/2006/12/xml-c14n11\"/>" +
              "</ds:Transforms>" +
              "<ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/>" +
              "<ds:DigestValue>" + invoiceHashB64 + "</ds:DigestValue></ds:Reference>" +
              "<ds:Reference URI=\"#xadesSignedProperties\" Type=\"http://uri.etsi.org/01903#SignedProperties\">" +
              "<ds:DigestMethod Algorithm=\"http://www.w3.org/2001/04/xmlenc#sha256\"/>" +
              "<ds:DigestValue>" + spDigestB64 + "</ds:DigestValue></ds:Reference>" +
              "</ds:SignedInfo>";
        }

        private static string BuildUblExtensions(string signedInfo, string signatureValue, string certB64, string signedProps)
        {
            return
              "<ext:UBLExtensions xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\">" +
              "<ext:UBLExtension><ext:ExtensionURI>urn:oasis:names:specification:ubl:dsig:enveloped:xades</ext:ExtensionURI>" +
              "<ext:ExtensionContent><sig:UBLDocumentSignatures xmlns:sig=\"urn:oasis:names:specification:ubl:schema:xsd:CommonSignatureComponents-2\" " +
              "xmlns:sac=\"urn:oasis:names:specification:ubl:schema:xsd:SignatureAggregateComponents-2\" " +
              "xmlns:sbc=\"urn:oasis:names:specification:ubl:schema:xsd:SignatureBasicComponents-2\">" +
              "<sac:SignatureInformation><cbc:ID xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\">urn:oasis:names:specification:ubl:signature:1</cbc:ID>" +
              "<sbc:ReferencedSignatureID>urn:oasis:names:specification:ubl:signature:Invoice</sbc:ReferencedSignatureID>" +
              "<ds:Signature xmlns:ds=\"http://www.w3.org/2000/09/xmldsig#\" Id=\"signature\">" +
              signedInfo +
              "<ds:SignatureValue>" + signatureValue + "</ds:SignatureValue>" +
              "<ds:KeyInfo><ds:X509Data><ds:X509Certificate>" + certB64 + "</ds:X509Certificate></ds:X509Data></ds:KeyInfo>" +
              "<ds:Object><xades:QualifyingProperties xmlns:xades=\"http://uri.etsi.org/01903/v1.3.2#\" Target=\"signature\">" +
              signedProps +
              "</xades:QualifyingProperties></ds:Object></ds:Signature>" +
              "</sac:SignatureInformation></sig:UBLDocumentSignatures></ext:ExtensionContent></ext:UBLExtension></ext:UBLExtensions>";
        }

        private static string BuildQr(ZatcaConfig cfg, string ublXml, DateTime t, string invoiceHash,
            byte[] signature, byte[] publicKey, byte[] certSignature)
        {
            // pull totals from the data via a light parse to avoid recomputation drift
            decimal total = 0, vat = 0;
            try
            {
                var doc = new XmlDocument(); doc.LoadXml(ublXml.Replace(ZatcaUbl.SignaturePlaceholder, "").Replace(ZatcaUbl.QrPlaceholder, ""));
                var ns = new XmlNamespaceManager(doc.NameTable);
                ns.AddNamespace("cbc", "urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2");
                ns.AddNamespace("cac", "urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2");
                var tn = doc.SelectSingleNode("//cac:LegalMonetaryTotal/cbc:TaxInclusiveAmount", ns);
                var vn = doc.SelectSingleNode("//cac:TaxTotal/cbc:TaxAmount", ns);
                if (tn != null) decimal.TryParse(tn.InnerText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out total);
                if (vn != null) decimal.TryParse(vn.InnerText, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out vat);
            }
            catch { }

            var q = new ZatcaQr
            {
                SellerName = cfg.OrgName,
                VatNumber = cfg.VatNumber,
                Timestamp = t,
                InvoiceTotal = total,
                VatTotal = vat,
                InvoiceHashBase64 = invoiceHash,
                Signature = signature,
                PublicKey = publicKey,
                CertSignature = certSignature
            };
            return q.ToBase64();
        }
    }
}
