using System;
using System.Collections.Generic;
using System.Text;

namespace SewingSystem.Classes.Zatca
{
    /// <summary>
    /// Phase-2 TLV QR builder. A simplified (B2C) invoice QR carries 9 tags:
    ///   1 seller name, 2 VAT number, 3 timestamp, 4 invoice total (incl. VAT),
    ///   5 VAT total, 6 invoice XML hash, 7 ECDSA signature,
    ///   8 EC public key, 9 signature of the public key by ZATCA's cert.
    ///
    /// Tags 6-9 are produced by the signing step; until signing exists they may
    /// be left null and a Phase-1 (tags 1-5) QR is emitted instead.
    /// </summary>
    public class ZatcaQr
    {
        public string SellerName;
        public string VatNumber;
        public DateTime Timestamp;
        public decimal InvoiceTotal;   // total incl. VAT
        public decimal VatTotal;
        public string InvoiceHashBase64;     // tag 6
        public byte[] Signature;             // tag 7
        public byte[] PublicKey;             // tag 8
        public byte[] CertSignature;         // tag 9

        private static void AddTag(List<byte> buf, int tag, byte[] value)
        {
            buf.Add((byte)tag);
            buf.Add((byte)value.Length);
            buf.AddRange(value);
        }

        private static byte[] U(string s) => Encoding.UTF8.GetBytes(s ?? string.Empty);

        /// <summary>Builds the TLV byte stream and returns it base64-encoded.</summary>
        public string ToBase64()
        {
            var buf = new List<byte>();
            AddTag(buf, 1, U(SellerName));
            AddTag(buf, 2, U(VatNumber));
            AddTag(buf, 3, U(Timestamp.ToString("yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "Z"));
            AddTag(buf, 4, U(InvoiceTotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)));
            AddTag(buf, 5, U(VatTotal.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)));

            if (!string.IsNullOrEmpty(InvoiceHashBase64))
                AddTag(buf, 6, U(InvoiceHashBase64));
            if (Signature != null && Signature.Length > 0)
                AddTag(buf, 7, U(Convert.ToBase64String(Signature)));  // نص Base64 ليطابق ds:SignatureValue في الـXML
            if (PublicKey != null && PublicKey.Length > 0)
                AddTag(buf, 8, PublicKey);
            if (CertSignature != null && CertSignature.Length > 0)
                AddTag(buf, 9, CertSignature);

            return Convert.ToBase64String(buf.ToArray());
        }

        /// <summary>Renders the QR payload to a PNG-encoded byte array via ZXing.</summary>
        public static byte[] RenderPng(string qrBase64Payload, int size = 300)
        {
            var writer = new ZXing.BarcodeWriterPixelData
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions { Width = size, Height = size, Margin = 1 }
            };
            var pixel = writer.Write(qrBase64Payload);
            using (var bmp = new System.Drawing.Bitmap(pixel.Width, pixel.Height,
                       System.Drawing.Imaging.PixelFormat.Format32bppRgb))
            {
                var data = bmp.LockBits(
                    new System.Drawing.Rectangle(0, 0, pixel.Width, pixel.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                try { System.Runtime.InteropServices.Marshal.Copy(pixel.Pixels, 0, data.Scan0, pixel.Pixels.Length); }
                finally { bmp.UnlockBits(data); }
                using (var ms = new System.IO.MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }
    }
}
