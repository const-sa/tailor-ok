using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Text;

namespace SewingSystem.Classes.Zatca
{
    public class ZatcaLine
    {
        public string Name;
        public decimal Quantity = 1;
        public decimal UnitPrice;       // net unit price (excl. VAT)
        public decimal DiscountAmount;  // line discount (excl. VAT)
        public decimal VatRate = 15m;   // %
        public decimal LineNet => Math.Round(Quantity * UnitPrice - DiscountAmount, 2);
        public decimal VatAmount => Math.Round(LineNet * VatRate / 100m, 2);
        public decimal LineTotalWithVat => Math.Round(LineNet + VatAmount, 2);
    }

    /// <summary>Data needed to build one ZATCA simplified invoice or credit note.</summary>
    public class ZatcaInvoiceData
    {
        public string InvoiceNumber;
        public string Uuid;
        public DateTime IssueDateTime;
        public bool IsCreditNote;          // 381 vs 388
        public int Icv;                    // invoice counter value
        public string Pih;                 // previous invoice hash (base64)

        // seller (from ZatcaConfig)
        public string SellerName, SellerVat, Street, Building, Plot, District, City, Postal, Country = "SA";

        // credit-note reference
        public string OriginalInvoiceNumber;
        public string ReturnReason = "Return / مرتجع";

        public List<ZatcaLine> Lines = new List<ZatcaLine>();

        public decimal TotalNet { get { decimal s = 0; foreach (var l in Lines) s += l.LineNet; return Math.Round(s, 2); } }
        public decimal TotalVat { get { decimal s = 0; foreach (var l in Lines) s += l.VatAmount; return Math.Round(s, 2); } }
        public decimal TotalWithVat => Math.Round(TotalNet + TotalVat, 2);
        public decimal VatRate => Lines.Count > 0 ? Lines[0].VatRate : 15m;
    }

    /// <summary>
    /// Builds a ZATCA-compliant UBL 2.1 document (Invoice root, type 388 invoice / 381
    /// credit note) for simplified B2C e-invoicing. UBLExtensions (signature) and the
    /// QR AdditionalDocumentReference value are left as placeholders for <see cref="ZatcaSigner"/>.
    /// </summary>
    public static class ZatcaUbl
    {
        private static string F(decimal d) => d.ToString("0.00", CultureInfo.InvariantCulture);
        private static string E(string s) => SecurityElement.Escape(s ?? "");

        public const string QrPlaceholder = "QR_PLACEHOLDER";
        public const string SignaturePlaceholder = "<!--UBL_EXTENSIONS_PLACEHOLDER-->";

        public static string Build(ZatcaInvoiceData d)
        {
            string typeCode = d.IsCreditNote ? "381" : "388";
            string typeName = "0200000"; // 02 = simplified
            var sb = new StringBuilder();

            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.Append("<Invoice xmlns=\"urn:oasis:names:specification:ubl:schema:xsd:Invoice-2\" ");
            sb.Append("xmlns:cac=\"urn:oasis:names:specification:ubl:schema:xsd:CommonAggregateComponents-2\" ");
            sb.Append("xmlns:cbc=\"urn:oasis:names:specification:ubl:schema:xsd:CommonBasicComponents-2\" ");
            sb.Append("xmlns:ext=\"urn:oasis:names:specification:ubl:schema:xsd:CommonExtensionComponents-2\">");

            sb.Append(SignaturePlaceholder); // ext:UBLExtensions inserted here by the signer

            sb.Append("<cbc:ProfileID>reporting:1.0</cbc:ProfileID>");
            sb.Append("<cbc:ID>").Append(E(d.InvoiceNumber)).Append("</cbc:ID>");
            sb.Append("<cbc:UUID>").Append(E(d.Uuid)).Append("</cbc:UUID>");
            sb.Append("<cbc:IssueDate>").Append(d.IssueDateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).Append("</cbc:IssueDate>");
            sb.Append("<cbc:IssueTime>").Append(d.IssueDateTime.ToString("HH:mm:ss", CultureInfo.InvariantCulture)).Append("</cbc:IssueTime>");
            sb.Append("<cbc:InvoiceTypeCode name=\"").Append(typeName).Append("\">").Append(typeCode).Append("</cbc:InvoiceTypeCode>");
            sb.Append("<cbc:DocumentCurrencyCode>SAR</cbc:DocumentCurrencyCode>");
            sb.Append("<cbc:TaxCurrencyCode>SAR</cbc:TaxCurrencyCode>");

            if (d.IsCreditNote && !string.IsNullOrEmpty(d.OriginalInvoiceNumber))
            {
                sb.Append("<cac:BillingReference><cac:InvoiceDocumentReference>");
                sb.Append("<cbc:ID>").Append(E(d.OriginalInvoiceNumber)).Append("</cbc:ID>");
                sb.Append("</cac:InvoiceDocumentReference></cac:BillingReference>");
            }

            // ICV
            sb.Append("<cac:AdditionalDocumentReference><cbc:ID>ICV</cbc:ID>");
            sb.Append("<cbc:UUID>").Append(d.Icv).Append("</cbc:UUID></cac:AdditionalDocumentReference>");
            // PIH
            sb.Append("<cac:AdditionalDocumentReference><cbc:ID>PIH</cbc:ID><cac:Attachment>");
            sb.Append("<cbc:EmbeddedDocumentBinaryObject mimeCode=\"text/plain\">")
              .Append(E(string.IsNullOrEmpty(d.Pih) ? ZatcaConfig.GenesisPih : d.Pih))
              .Append("</cbc:EmbeddedDocumentBinaryObject></cac:Attachment></cac:AdditionalDocumentReference>");
            // QR (value filled by signer)
            sb.Append("<cac:AdditionalDocumentReference><cbc:ID>QR</cbc:ID><cac:Attachment>");
            sb.Append("<cbc:EmbeddedDocumentBinaryObject mimeCode=\"text/plain\">")
              .Append(QrPlaceholder)
              .Append("</cbc:EmbeddedDocumentBinaryObject></cac:Attachment></cac:AdditionalDocumentReference>");
            // UBL signature reference
            sb.Append("<cac:Signature><cbc:ID>urn:oasis:names:specification:ubl:signature:Invoice</cbc:ID>");
            sb.Append("<cbc:SignatureMethod>urn:oasis:names:specification:ubl:dsig:enveloped:xades</cbc:SignatureMethod></cac:Signature>");

            // Seller
            sb.Append("<cac:AccountingSupplierParty><cac:Party>");
            sb.Append("<cac:PostalAddress>");
            sb.Append("<cbc:StreetName>").Append(E(d.Street)).Append("</cbc:StreetName>");
            sb.Append("<cbc:BuildingNumber>").Append(E(d.Building)).Append("</cbc:BuildingNumber>");
            if (!string.IsNullOrWhiteSpace(d.Plot))
                sb.Append("<cbc:PlotIdentification>").Append(E(d.Plot)).Append("</cbc:PlotIdentification>");
            sb.Append("<cbc:CitySubdivisionName>").Append(E(d.District)).Append("</cbc:CitySubdivisionName>");
            sb.Append("<cbc:CityName>").Append(E(d.City)).Append("</cbc:CityName>");
            sb.Append("<cbc:PostalZone>").Append(E(d.Postal)).Append("</cbc:PostalZone>");
            sb.Append("<cac:Country><cbc:IdentificationCode>").Append(E(d.Country)).Append("</cbc:IdentificationCode></cac:Country>");
            sb.Append("</cac:PostalAddress>");
            sb.Append("<cac:PartyTaxScheme><cbc:CompanyID>").Append(E(d.SellerVat)).Append("</cbc:CompanyID>");
            sb.Append("<cac:TaxScheme><cbc:ID>VAT</cbc:ID></cac:TaxScheme></cac:PartyTaxScheme>");
            sb.Append("<cac:PartyLegalEntity><cbc:RegistrationName>").Append(E(d.SellerName)).Append("</cbc:RegistrationName></cac:PartyLegalEntity>");
            sb.Append("</cac:Party></cac:AccountingSupplierParty>");

            // Buyer (minimal / optional for simplified)
            sb.Append("<cac:AccountingCustomerParty><cac:Party>");
            sb.Append("<cac:PartyLegalEntity><cbc:RegistrationName>عميل نقدي</cbc:RegistrationName></cac:PartyLegalEntity>");
            sb.Append("</cac:Party></cac:AccountingCustomerParty>");

            // Delivery
            sb.Append("<cac:Delivery><cbc:ActualDeliveryDate>").Append(d.IssueDateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).Append("</cbc:ActualDeliveryDate></cac:Delivery>");
            // Payment means (10 = cash); credit note carries the reason
            sb.Append("<cac:PaymentMeans><cbc:PaymentMeansCode>10</cbc:PaymentMeansCode>");
            if (d.IsCreditNote) sb.Append("<cbc:InstructionNote>").Append(E(d.ReturnReason)).Append("</cbc:InstructionNote>");
            sb.Append("</cac:PaymentMeans>");

            // TaxTotal (with subtotal)
            sb.Append("<cac:TaxTotal><cbc:TaxAmount currencyID=\"SAR\">").Append(F(d.TotalVat)).Append("</cbc:TaxAmount>");
            sb.Append("<cac:TaxSubtotal>");
            sb.Append("<cbc:TaxableAmount currencyID=\"SAR\">").Append(F(d.TotalNet)).Append("</cbc:TaxableAmount>");
            sb.Append("<cbc:TaxAmount currencyID=\"SAR\">").Append(F(d.TotalVat)).Append("</cbc:TaxAmount>");
            sb.Append("<cac:TaxCategory><cbc:ID>S</cbc:ID><cbc:Percent>").Append(F(d.VatRate)).Append("</cbc:Percent>");
            sb.Append("<cac:TaxScheme><cbc:ID>VAT</cbc:ID></cac:TaxScheme></cac:TaxCategory></cac:TaxSubtotal></cac:TaxTotal>");
            // TaxTotal (total only, in tax currency)
            sb.Append("<cac:TaxTotal><cbc:TaxAmount currencyID=\"SAR\">").Append(F(d.TotalVat)).Append("</cbc:TaxAmount></cac:TaxTotal>");

            // LegalMonetaryTotal
            sb.Append("<cac:LegalMonetaryTotal>");
            sb.Append("<cbc:LineExtensionAmount currencyID=\"SAR\">").Append(F(d.TotalNet)).Append("</cbc:LineExtensionAmount>");
            sb.Append("<cbc:TaxExclusiveAmount currencyID=\"SAR\">").Append(F(d.TotalNet)).Append("</cbc:TaxExclusiveAmount>");
            sb.Append("<cbc:TaxInclusiveAmount currencyID=\"SAR\">").Append(F(d.TotalWithVat)).Append("</cbc:TaxInclusiveAmount>");
            sb.Append("<cbc:PayableAmount currencyID=\"SAR\">").Append(F(d.TotalWithVat)).Append("</cbc:PayableAmount>");
            sb.Append("</cac:LegalMonetaryTotal>");

            // Lines
            int i = 1;
            foreach (var l in d.Lines)
            {
                sb.Append("<cac:InvoiceLine>");
                sb.Append("<cbc:ID>").Append(i++).Append("</cbc:ID>");
                sb.Append("<cbc:InvoicedQuantity unitCode=\"PCE\">").Append(F(l.Quantity)).Append("</cbc:InvoicedQuantity>");
                sb.Append("<cbc:LineExtensionAmount currencyID=\"SAR\">").Append(F(l.LineNet)).Append("</cbc:LineExtensionAmount>");
                sb.Append("<cac:TaxTotal><cbc:TaxAmount currencyID=\"SAR\">").Append(F(l.VatAmount)).Append("</cbc:TaxAmount>");
                sb.Append("<cbc:RoundingAmount currencyID=\"SAR\">").Append(F(l.LineTotalWithVat)).Append("</cbc:RoundingAmount></cac:TaxTotal>");
                sb.Append("<cac:Item><cbc:Name>").Append(E(l.Name)).Append("</cbc:Name>");
                sb.Append("<cac:ClassifiedTaxCategory><cbc:ID>S</cbc:ID><cbc:Percent>").Append(F(l.VatRate)).Append("</cbc:Percent>");
                sb.Append("<cac:TaxScheme><cbc:ID>VAT</cbc:ID></cac:TaxScheme></cac:ClassifiedTaxCategory></cac:Item>");
                sb.Append("<cac:Price><cbc:PriceAmount currencyID=\"SAR\">").Append(F(l.UnitPrice)).Append("</cbc:PriceAmount></cac:Price>");
                sb.Append("</cac:InvoiceLine>");
            }

            sb.Append("</Invoice>");
            return sb.ToString();
        }
    }
}
