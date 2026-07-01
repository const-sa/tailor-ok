using System;
using System.Data.SqlClient;

namespace SewingSystem.Classes.Zatca
{
    /// <summary>
    /// Maps an existing dbo.tblSellInvoice (sale = 388, or return IsReturn=1 = 381)
    /// into a <see cref="ZatcaInvoiceData"/> ready for signing/reporting.
    ///
    /// The sell-invoice detail lines have no per-line price in this system, so the
    /// document is mapped as a single summary line (net + VAT taken from the invoice
    /// header totals). This keeps reported totals consistent with the stored invoice.
    /// </summary>
    public static class ZatcaInvoiceMapper
    {
        public static ZatcaInvoiceData BuildFromSellInvoice(int invoiceId, ZatcaConfig cfg)
        {
            var d = new ZatcaInvoiceData
            {
                SellerName = cfg.OrgName, SellerVat = cfg.VatNumber, SellerCrn = cfg.CrNumber,
                Street = cfg.AddrStreet, Building = cfg.AddrBuilding, Plot = cfg.AddrSecondary, District = cfg.AddrDistrict,
                City = cfg.AddrCity, Postal = cfg.AddrPostal, Country = cfg.AddrCountry,
                Uuid = Guid.NewGuid().ToString(),
                IssueDateTime = DateTime.Now
            };

            double total = 0, tax = 0; int branchId = 1; int? originalId = null;
            using (var con = new SqlConnection(Program.ConnectionString))
            {
                con.Open();
                using (var cmd = new SqlCommand(
                    "SELECT InvoNumber, SellDate, TotalFinal, TaxAll, ISNULL(IsReturn,0), OriginalInvoiceID, BranchID FROM dbo.tblSellInvoice WHERE ID=@id", con))
                {
                    cmd.Parameters.AddWithValue("@id", invoiceId);
                    using (var r = cmd.ExecuteReader())
                    {
                        if (!r.Read()) throw new InvalidOperationException("الفاتورة غير موجودة (ID=" + invoiceId + ").");
                        d.InvoiceNumber = r[0] == DBNull.Value ? invoiceId.ToString() : r[0].ToString();
                        if (r[1] != DBNull.Value) d.IssueDateTime = Convert.ToDateTime(r[1]);
                        total = r[2] == DBNull.Value ? 0 : Convert.ToDouble(r[2]);
                        tax = r[3] == DBNull.Value ? 0 : Convert.ToDouble(r[3]);
                        d.IsCreditNote = Convert.ToBoolean(r[4]);
                        originalId = r[5] == DBNull.Value ? (int?)null : Convert.ToInt32(r[5]);
                        branchId = r[6] == DBNull.Value ? 1 : Convert.ToInt32(r[6]);
                    }
                }

                double rate = 15;
                using (var cmd = new SqlCommand("SELECT ISNULL(TaxRate,15) FROM dbo.tblBranche WHERE ID=@b", con))
                {
                    cmd.Parameters.AddWithValue("@b", branchId);
                    var o = cmd.ExecuteScalar(); if (o != null && o != DBNull.Value) rate = Convert.ToDouble(o);
                }

                if (d.IsCreditNote && originalId.HasValue)
                {
                    using (var cmd = new SqlCommand("SELECT InvoNumber FROM dbo.tblSellInvoice WHERE ID=@id", con))
                    {
                        cmd.Parameters.AddWithValue("@id", originalId.Value);
                        var o = cmd.ExecuteScalar();
                        d.OriginalInvoiceNumber = (o == null || o == DBNull.Value) ? originalId.Value.ToString() : o.ToString();
                    }
                }

                decimal net = (decimal)Math.Round(total - tax, 2);
                d.Lines.Add(new ZatcaLine
                {
                    Name = (d.IsCreditNote ? "مرتجع فاتورة " : "مبيعات فاتورة ") + d.InvoiceNumber,
                    Quantity = 1,
                    UnitPrice = net,
                    VatRate = (decimal)rate
                });
            }
            return d;
        }
    }
}
