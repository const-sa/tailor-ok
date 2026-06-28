using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace SewingSystem.Classes.Zatca
{
    /// <summary>
    /// Cryptographic primitives for ZATCA Phase-2 onboarding:
    ///   - generate a secp256k1 EC key pair,
    ///   - build a PKCS#10 CSR carrying the ZATCA certificate template + the
    ///     SubjectAltName directory attributes (EGS serial, VAT, invoice-type
    ///     map, address, business category) that the Compliance-CSID API requires.
    ///
    /// NOTE: the CSR shape follows the published ZATCA spec. It must be validated
    /// against the (simulation) gateway during onboarding; this code is the
    /// foundation, not yet a gateway-verified artifact.
    /// </summary>
    public static class ZatcaCrypto
    {
        // OIDs used inside the SubjectAltName directoryName (per ZATCA template).
        private static readonly DerObjectIdentifier OID_SN = new DerObjectIdentifier("2.5.4.4");   // surname -> EGS serial
        private static readonly DerObjectIdentifier OID_UID = new DerObjectIdentifier("0.9.2342.19200300.100.1.1"); // VAT
        private static readonly DerObjectIdentifier OID_Title = new DerObjectIdentifier("2.5.4.12"); // invoice-type map
        private static readonly DerObjectIdentifier OID_RegAddr = new DerObjectIdentifier("2.5.4.26"); // registeredAddress
        private static readonly DerObjectIdentifier OID_BizCat = new DerObjectIdentifier("2.5.4.15"); // businessCategory
        private static readonly DerObjectIdentifier OID_Template = new DerObjectIdentifier("1.3.6.1.4.1.311.20.2");

        /// <summary>Invoice-type map for the CSR title: standard|simplified|0|0. B2C simplified only = "0100".</summary>
        public const string InvoiceTypeMap = "0100";

        /// <summary>
        /// Generates a fresh key pair + CSR and writes both into <paramref name="cfg"/>
        /// (<see cref="ZatcaConfig.PrivateKeyPem"/> and <see cref="ZatcaConfig.Csr"/> as
        /// base64 of the CSR PEM, which is the format the gateway expects).
        /// </summary>
        public static void GenerateKeyPairAndCsr(ZatcaConfig cfg)
        {
            var random = new SecureRandom();

            // 1) secp256k1 EC key pair
            var keyGen = new ECKeyPairGenerator();
            keyGen.Init(new ECKeyGenerationParameters(SecObjectIdentifiers.SecP256k1, random));
            AsymmetricCipherKeyPair kp = keyGen.GenerateKeyPair();

            // 2) Subject DN
            var subject = new X509Name(
                new List<DerObjectIdentifier> { X509Name.C, X509Name.OU, X509Name.O, X509Name.CN },
                new List<string>
                {
                    string.IsNullOrWhiteSpace(cfg.AddrCountry) ? "SA" : cfg.AddrCountry,
                    Safe(cfg.AddrCity, "Riyadh"),
                    Safe(cfg.OrgName, "Organization"),
                    Safe(cfg.EgsSerialNumber, "EGS-Unit")
                });

            // 3) SubjectAltName directoryName attributes required by ZATCA
            var sanDn = new X509Name(
                new List<DerObjectIdentifier> { OID_SN, OID_UID, OID_Title, OID_RegAddr, OID_BizCat },
                new List<string>
                {
                    Safe(cfg.EgsSerialNumber, "1-Tailr|2-POS|3-0001"),
                    Safe(cfg.VatNumber, "300000000000003"),
                    InvoiceTypeMap,
                    BuildAddressLine(cfg),
                    "Tailoring"
                });
            var altNames = new GeneralNames(new GeneralName(GeneralName.DirectoryName, sanDn));

            // 4) Extensions: certificate template name + SAN
            var extGen = new X509ExtensionsGenerator();
            extGen.AddExtension(OID_Template, false, new DerUtf8String(cfg.CsrTemplateName));
            extGen.AddExtension(X509Extensions.SubjectAlternativeName, false, altNames);
            X509Extensions extensions = extGen.Generate();

            var attribute = new AttributePkcs(
                PkcsObjectIdentifiers.Pkcs9AtExtensionRequest,
                new DerSet(extensions));
            var attributes = new DerSet(attribute);

            // 5) Sign the CSR with SHA256withECDSA
            ISignatureFactory sigFactory = new Asn1SignatureFactory("SHA256WITHECDSA", kp.Private, random);
            var csr = new Pkcs10CertificationRequest(sigFactory, subject, kp.Public, attributes);

            cfg.PrivateKeyPem = ToPem(kp.Private);
            string csrPem = ToPem(csr);
            // ZATCA wants the CSR base64-encoded (base64 of the PEM text).
            cfg.Csr = Convert.ToBase64String(Encoding.UTF8.GetBytes(csrPem));
        }

        /// <summary>Loads the stored EC private key (PEM) for invoice signing.</summary>
        public static AsymmetricKeyParameter LoadPrivateKey(string privateKeyPem)
        {
            using (var sr = new StringReader(privateKeyPem))
            {
                var pr = new PemReader(sr);
                object obj = pr.ReadObject();
                if (obj is AsymmetricCipherKeyPair pair) return pair.Private;
                if (obj is AsymmetricKeyParameter k) return k;
                throw new InvalidOperationException("Unrecognized private-key PEM.");
            }
        }

        private static string ToPem(object obj)
        {
            using (var sw = new StringWriter())
            {
                var pw = new PemWriter(sw);
                pw.WriteObject(obj);
                pw.Writer.Flush();
                return sw.ToString();
            }
        }

        private static string BuildAddressLine(ZatcaConfig cfg)
        {
            var parts = new List<string>();
            foreach (var p in new[] { cfg.AddrBuilding, cfg.AddrStreet, cfg.AddrDistrict, cfg.AddrCity, cfg.AddrPostal })
                if (!string.IsNullOrWhiteSpace(p)) parts.Add(p.Trim());
            return parts.Count > 0 ? string.Join(" ", parts) : "NA";
        }

        private static string Safe(string v, string fallback) =>
            string.IsNullOrWhiteSpace(v) ? fallback : v.Trim();
    }
}
