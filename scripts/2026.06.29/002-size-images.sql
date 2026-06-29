/* جدول صور المقاسات القابلة للتعديل (إدارة من شاشة «صور المقاسات»).
   المفتاح = اسم الـ CheckEdit في شاشة التفصيل (مثل checkEditJ45).
   البرنامج ينشئ الجدول تلقائياً عند أول استبدال، وهذا السكربت للتوثيق/الإنشاء اليدوي. */
IF OBJECT_ID('dbo.tblSizeImages','U') IS NULL
CREATE TABLE dbo.tblSizeImages(
    ImageKey  nvarchar(50)   NOT NULL PRIMARY KEY,
    ImageData varbinary(max) NULL,
    UpdatedAt datetime       NOT NULL DEFAULT(GETDATE())
);
GO
