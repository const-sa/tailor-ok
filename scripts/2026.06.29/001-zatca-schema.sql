/* ============================================================================
   سكربت ترقية قاعدة البيانات — ميزة فوترة الزكاة (المرحلة الثانية) + المرتجع
   ============================================================================
   جاهز للتشغيل مباشرة على قاعدة البيانات الأونلاين.

   طريقة التشغيل:
     • SSMS:   افتح الملف، تأكد من اختيار القاعدة الصحيحة من الأعلى، ثم Execute (F5).
     • sqlcmd: sqlcmd -S <server> -d <database> -U <user> -P <pass> -i 001-zatca-schema.sql

   خصائص الأمان:
     • Idempotent — يفحص الوجود قبل أي إنشاء/إضافة، فلا يخطئ لو شُغّل أكثر من مرة.
     • إضافي فقط — لا يحذف ولا يعدّل أي بيانات قائمة.
     • يطبع تقرير تحقق في النهاية.

   ⚠️ خذ نسخة احتياطية (Backup) للقاعدة قبل التشغيل — هذه قاعدة إنتاج.
   ============================================================================ */

SET NOCOUNT ON;
GO

/* اختياري: ألغِ التعليق وضع اسم قاعدتك لضمان التنفيذ على القاعدة الصحيحة
USE [SewingSystem2];
GO
*/

PRINT N'>>> بدء ترقية مخطط الزكاة على قاعدة: ' + DB_NAME();
GO

/* --------------------------------------------------------------------------
   1) جدول الإعدادات tblZatcaConfig  (صف واحد: Id = 1)
   -------------------------------------------------------------------------- */
IF OBJECT_ID(N'dbo.tblZatcaConfig', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.tblZatcaConfig
    (
        Id                  INT             NOT NULL PRIMARY KEY,
        Environment         NVARCHAR(20)    NULL,   -- sandbox | simulation | production
        Enabled             BIT             NOT NULL CONSTRAINT DF_ZatcaCfg_Enabled DEFAULT(0),

        -- بيانات المنشأة
        OrgName             NVARCHAR(200)   NULL,
        VatNumber           NVARCHAR(20)    NULL,
        CrNumber            NVARCHAR(30)    NULL,
        EgsSerialNumber     NVARCHAR(150)   NULL,

        -- العنوان الوطني
        AddrShort           NVARCHAR(20)    NULL,
        AddrBuilding        NVARCHAR(20)    NULL,
        AddrStreet          NVARCHAR(200)   NULL,
        AddrSecondary       NVARCHAR(20)    NULL,
        AddrDistrict        NVARCHAR(200)   NULL,
        AddrCity            NVARCHAR(100)   NULL,
        AddrPostal          NVARCHAR(20)    NULL,
        AddrCountry         NVARCHAR(5)     NULL CONSTRAINT DF_ZatcaCfg_Country DEFAULT(N'SA'),

        -- التشفير والتهيئة (تُخزَّن مشفّرة من داخل البرنامج)
        PrivateKeyPem       NVARCHAR(MAX)   NULL,
        Csr                 NVARCHAR(MAX)   NULL,
        ComplianceCert      NVARCHAR(MAX)   NULL,
        ComplianceSecret    NVARCHAR(MAX)   NULL,
        ComplianceRequestId NVARCHAR(100)   NULL,
        ProductionCert      NVARCHAR(MAX)   NULL,
        ProductionSecret    NVARCHAR(MAX)   NULL,
        ProductionRequestId NVARCHAR(100)   NULL,

        -- تسلسل الفواتير
        LastICV             INT             NOT NULL CONSTRAINT DF_ZatcaCfg_ICV DEFAULT(0),
        LastPIH             NVARCHAR(MAX)   NULL,

        UpdatedAt           DATETIME        NULL
    );

    PRINT N'  [+] تم إنشاء الجدول dbo.tblZatcaConfig';
END
ELSE
    PRINT N'  [=] الجدول dbo.tblZatcaConfig موجود مسبقاً';
GO

/* صف الإعدادات الافتراضي (Id = 1) — قيم افتراضية صالحة الشكل، غيّرها لاحقاً ببياناتك */
IF NOT EXISTS (SELECT 1 FROM dbo.tblZatcaConfig WHERE Id = 1)
BEGIN
    INSERT INTO dbo.tblZatcaConfig
        (Id, Environment, Enabled, OrgName, VatNumber, CrNumber, EgsSerialNumber,
         AddrShort, AddrBuilding, AddrStreet, AddrSecondary, AddrDistrict, AddrCity, AddrPostal, AddrCountry,
         LastICV, UpdatedAt)
    VALUES
        (1, N'simulation', 0, N'ثوبي الفاخر', N'300000000000003', N'1010010000',
         N'1-TailrMdinah|2-POS01|3-100000000000001',
         N'RRRD2929', N'1234', N'الملك فهد', N'1234', N'العليا', N'الرياض', N'12211', N'SA',
         0, GETDATE());

    PRINT N'  [+] تم إدراج صف الإعدادات الافتراضي (Id=1)';
END
ELSE
    PRINT N'  [=] صف الإعدادات (Id=1) موجود مسبقاً';
GO

/* --------------------------------------------------------------------------
   2) أعمدة المرحلة الثانية على جدول الفواتير tblSellInvoice (11 عموداً)
   -------------------------------------------------------------------------- */
IF COL_LENGTH(N'dbo.tblSellInvoice', N'ZatcaUUID')        IS NULL ALTER TABLE dbo.tblSellInvoice ADD ZatcaUUID        UNIQUEIDENTIFIER NULL;
GO
IF COL_LENGTH(N'dbo.tblSellInvoice', N'ZatcaICV')         IS NULL ALTER TABLE dbo.tblSellInvoice ADD ZatcaICV         INT            NULL;
GO
IF COL_LENGTH(N'dbo.tblSellInvoice', N'ZatcaPIH')         IS NULL ALTER TABLE dbo.tblSellInvoice ADD ZatcaPIH         NVARCHAR(MAX)  NULL;
GO
IF COL_LENGTH(N'dbo.tblSellInvoice', N'ZatcaInvoiceHash') IS NULL ALTER TABLE dbo.tblSellInvoice ADD ZatcaInvoiceHash NVARCHAR(MAX)  NULL;
GO
IF COL_LENGTH(N'dbo.tblSellInvoice', N'ZatcaQR')          IS NULL ALTER TABLE dbo.tblSellInvoice ADD ZatcaQR          NVARCHAR(MAX)  NULL;
GO
IF COL_LENGTH(N'dbo.tblSellInvoice', N'ZatcaTypeCode')    IS NULL ALTER TABLE dbo.tblSellInvoice ADD ZatcaTypeCode    NVARCHAR(10)   NULL;
GO
IF COL_LENGTH(N'dbo.tblSellInvoice', N'ZatcaStatus')      IS NULL ALTER TABLE dbo.tblSellInvoice ADD ZatcaStatus      NVARCHAR(50)   NULL;
GO
IF COL_LENGTH(N'dbo.tblSellInvoice', N'ZatcaResponse')    IS NULL ALTER TABLE dbo.tblSellInvoice ADD ZatcaResponse    NVARCHAR(MAX)  NULL;
GO
IF COL_LENGTH(N'dbo.tblSellInvoice', N'ZatcaReportedAt')  IS NULL ALTER TABLE dbo.tblSellInvoice ADD ZatcaReportedAt  DATETIME       NULL;
GO
IF COL_LENGTH(N'dbo.tblSellInvoice', N'IsReturn')         IS NULL ALTER TABLE dbo.tblSellInvoice ADD IsReturn         BIT            NOT NULL CONSTRAINT DF_SellInv_IsReturn DEFAULT(0);
GO
IF COL_LENGTH(N'dbo.tblSellInvoice', N'OriginalInvoiceID') IS NULL ALTER TABLE dbo.tblSellInvoice ADD OriginalInvoiceID INT          NULL;
GO

PRINT N'  [+] تمت مزامنة أعمدة tblSellInvoice الخاصة بالزكاة';
GO

/* --------------------------------------------------------------------------
   3) (اختياري) تفعيل ضريبة 15% على الفرع — الأعمدة TaxRate/UseTax موجودة أصلاً.
      ألغِ التعليق فقط إذا أردت فرض الضريبة على كل الفواتير الجديدة.
   --------------------------------------------------------------------------
UPDATE dbo.tblBranche
   SET UseTax  = 1,
       TaxRate = 15
 WHERE ISNULL(UseTax, 0) = 0 OR ISNULL(TaxRate, 0) = 0;
PRINT N'  [+] تم تفعيل ضريبة 15% على الفروع';
GO
*/

/* --------------------------------------------------------------------------
   4) تقرير التحقق النهائي
   -------------------------------------------------------------------------- */
PRINT N'=========================================================';
PRINT N'تقرير التحقق:';

SELECT
    N'tblZatcaConfig' AS [العنصر],
    CASE WHEN OBJECT_ID(N'dbo.tblZatcaConfig', N'U') IS NOT NULL THEN N'موجود ✔' ELSE N'مفقود ✘' END AS [الحالة];

SELECT
    c.[العمود],
    CASE WHEN COL_LENGTH(N'dbo.tblSellInvoice', c.[العمود]) IS NOT NULL THEN N'موجود ✔' ELSE N'مفقود ✘' END AS [الحالة]
FROM (VALUES
    (N'ZatcaUUID'),(N'ZatcaICV'),(N'ZatcaPIH'),(N'ZatcaInvoiceHash'),(N'ZatcaQR'),
    (N'ZatcaTypeCode'),(N'ZatcaStatus'),(N'ZatcaResponse'),(N'ZatcaReportedAt'),
    (N'IsReturn'),(N'OriginalInvoiceID')
) AS c([العمود]);

PRINT N'=== اكتملت ترقية مخطط الزكاة ===';
GO
