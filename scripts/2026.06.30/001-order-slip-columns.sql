/* ============================================================================
   إضافة حقول «نموذج التفصيل» على جدول الفواتير tblSellInvoice
   ----------------------------------------------------------------------------
   حقلان نصّيان جديدان يُعبّآن أسفل شاشة التفصيل (تحت صور المقاسات):
     - Brufa       (بروفة)
     - FabricName  (اسم القماش)

   آمن للتكرار (Idempotent) وإضافي فقط. بدون GO ليعمل في لوحات تحكم الهوست أيضاً.
   ⚠️ خذ نسخة احتياطية قبل التشغيل على القاعدة الأونلاين.
   ============================================================================ */
SET NOCOUNT ON;

IF COL_LENGTH(N'dbo.tblSellInvoice', N'Brufa')      IS NULL ALTER TABLE dbo.tblSellInvoice ADD Brufa      NVARCHAR(100) NULL;
IF COL_LENGTH(N'dbo.tblSellInvoice', N'FabricName') IS NULL ALTER TABLE dbo.tblSellInvoice ADD FabricName NVARCHAR(150) NULL;

/* تقرير تحقق */
SELECT c.[العمود],
       CASE WHEN COL_LENGTH(N'dbo.tblSellInvoice', c.[العمود]) IS NOT NULL THEN N'موجود ✔' ELSE N'مفقود ✘' END AS [الحالة]
FROM (VALUES (N'Brufa'), (N'FabricName')) AS c([العمود]);
