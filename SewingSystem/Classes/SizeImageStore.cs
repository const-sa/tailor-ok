using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;

namespace SewingSystem.Classes
{
    /// <summary>عنصر صورة مقاس مع اسمه وترتيبه.</summary>
    public class SizeImageInfo
    {
        public string Key;
        public string Caption;
        public int SortOrder;
        public bool HasCustomImage;
    }

    /// <summary>
    /// مخزن صور المقاسات: يقرأ/يحفظ صور وأسماء وترتيب أنماط التفصيل من dbo.tblSizeImages،
    /// مع الرجوع للصورة المضمّنة في البرنامج (Properties.Resources) إن لم تُستبدل.
    /// المفتاح = اسم الـ CheckEdit في شاشة التفصيل (مثل checkEditJ45).
    /// </summary>
    public static class SizeImageStore
    {
        /// <summary>كل صور المقاسات: (مفتاح = اسم العنصر, اسم المورد المضمّن, الاسم الافتراضي).</summary>
        public static readonly (string Key, string Resource, string DefaultCaption)[] Catalog = new[]
        {
            ("checkEditJ1","_201","كبك"),      ("checkEditJ2","_18","كبك"),        ("checkEditJ3","_212","كبك"),
            ("checkEditJ4","_17","كبك"),       ("checkEditJ44","_14","كبك"),       ("checkEditJ45","_15","مربع سحاب"),
            ("checkEditK1","_45","سادة"),      ("checkEditK2","_33","قلاب"),       ("checkEditK3","_24","قلاب"),
            ("checkEditK4","_23","قلاب"),      ("checkEditK44","_22","قلاب"),
            ("checkEditS1","_13","مخفي"),      ("checkEditS2","_555","مربع"),      ("checkEditS3","_77","دائري"),
            ("checkEditQ1","_4","جيب"),        ("checkEditQ2","Q10","جيب"),        ("checkEditQ3","Q9","جيب"),
            ("checkEditQ4","_5","قلاب واسع فرنسي"), ("checkEditQ5","_7","جيب"),     ("checkEditQ6","_11","جيب"),
            ("checkEditQ7","_31","جيب"),       ("checkEditQ8","_6","قلاب واسع فرنسي"), ("checkEditQ9","_21","جيب"),
            ("checkEditQ10","_8","جيب"),
        };

        private static readonly Dictionary<string, string> _resByKey = new Dictionary<string, string>();
        private static readonly Dictionary<string, int> _idxByKey = new Dictionary<string, int>();
        private static readonly Dictionary<string, string> _defCaptionByKey = new Dictionary<string, string>();
        static SizeImageStore()
        {
            for (int i = 0; i < Catalog.Length; i++)
            {
                _resByKey[Catalog[i].Key] = Catalog[i].Resource;
                _idxByKey[Catalog[i].Key] = i;
                _defCaptionByKey[Catalog[i].Key] = Catalog[i].DefaultCaption;
            }
        }

        public static void EnsureTable()
        {
            try
            {
                using (var con = new SqlConnection(Program.ConnectionString))
                using (var cmd = new SqlCommand(
@"IF OBJECT_ID('dbo.tblSizeImages','U') IS NULL
CREATE TABLE dbo.tblSizeImages(
  ImageKey  nvarchar(50)   NOT NULL PRIMARY KEY,
  ImageData varbinary(max) NULL,
  Caption   nvarchar(100)  NULL,
  SortOrder int            NULL,
  UpdatedAt datetime       NOT NULL DEFAULT(GETDATE()));
IF COL_LENGTH('dbo.tblSizeImages','Caption')   IS NULL ALTER TABLE dbo.tblSizeImages ADD Caption   nvarchar(100) NULL;
IF COL_LENGTH('dbo.tblSizeImages','SortOrder') IS NULL ALTER TABLE dbo.tblSizeImages ADD SortOrder int           NULL;", con))
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch { /* لا نوقف التشغيل إن تعذّر */ }
        }

        public static string DefaultCaption(string key)
            => _defCaptionByKey.TryGetValue(key, out var c) ? c : key;

        public static Image GetEmbedded(string key)
        {
            if (!_resByKey.TryGetValue(key, out var res)) return null;
            try
            {
                // اسم المورد في الـ ResourceManager بدون الشرطة السفلية البادئة (مثلاً "18" لا "_18").
                var img = Properties.Resources.ResourceManager.GetObject(res) as Image;
                if (img == null && res.StartsWith("_"))
                    img = Properties.Resources.ResourceManager.GetObject(res.Substring(1)) as Image;
                return img;
            }
            catch { return null; }
        }

        /// <summary>تحويل بايتات صورة (من القاعدة) إلى Image دون أي استعلام إضافي.</summary>
        public static Image ImageFromBytes(byte[] bytes) => FromBytes(bytes);

        private static Image FromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            try
            {
                using (var ms = new MemoryStream(bytes))
                using (var tmp = Image.FromStream(ms))
                    return new Bitmap(tmp);
            }
            catch { return null; }
        }

        public static byte[] GetBytes(string key)
        {
            try
            {
                using (var con = new SqlConnection(Program.ConnectionString))
                using (var cmd = new SqlCommand("SELECT ImageData FROM dbo.tblSizeImages WHERE ImageKey=@k", con))
                {
                    cmd.Parameters.AddWithValue("@k", key);
                    con.Open();
                    var o = cmd.ExecuteScalar();
                    return (o == null || o == DBNull.Value) ? null : (byte[])o;
                }
            }
            catch { return null; }
        }

        public static Dictionary<string, byte[]> GetAllOverrides()
        {
            var d = new Dictionary<string, byte[]>();
            try
            {
                using (var con = new SqlConnection(Program.ConnectionString))
                using (var cmd = new SqlCommand("SELECT ImageKey, ImageData FROM dbo.tblSizeImages WHERE ImageData IS NOT NULL", con))
                {
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                        while (r.Read())
                            d[r.GetString(0)] = (byte[])r[1];
                }
            }
            catch { }
            return d;
        }

        /// <summary>الصورة الحالية: من القاعدة إن وُجدت، وإلا الصورة المضمّنة.</summary>
        public static Image Get(string key)
            => FromBytes(GetBytes(key)) ?? GetEmbedded(key);

        /// <summary>صورة القاعدة فقط (null إن لم تُستبدل).</summary>
        public static Image DbImage(string key) => FromBytes(GetBytes(key));

        /// <summary>الاسم/الترتيب المحفوظ لمفتاح واحد (أو الافتراضي).</summary>
        public static SizeImageInfo LoadOne(string key, string defaultCaption, int defaultOrder)
        {
            var info = new SizeImageInfo { Key = key, Caption = defaultCaption, SortOrder = defaultOrder };
            try
            {
                using (var con = new SqlConnection(Program.ConnectionString))
                using (var cmd = new SqlCommand("SELECT Caption, SortOrder FROM dbo.tblSizeImages WHERE ImageKey=@k", con))
                {
                    cmd.Parameters.AddWithValue("@k", key);
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                        if (r.Read())
                        {
                            if (!(r["Caption"] is DBNull) && !string.IsNullOrWhiteSpace(r["Caption"].ToString())) info.Caption = r["Caption"].ToString();
                            if (!(r["SortOrder"] is DBNull)) info.SortOrder = Convert.ToInt32(r["SortOrder"]);
                        }
                }
            }
            catch { }
            return info;
        }

        /// <summary>استبدال صورة عدّة مفاتيح (نسخ نفس النمط) بصورة واحدة.</summary>
        public static void SaveImageMany(IEnumerable<string> keys, byte[] data)
        {
            foreach (var k in keys) SaveImage(k, data);
        }

        /// <summary>إرجاع عدّة مفاتيح للصورة الأصلية.</summary>
        public static void ResetMany(IEnumerable<string> keys)
        {
            foreach (var k in keys) ResetImage(k);
        }

        /// <summary>كل العناصر مرتّبة (اسم + ترتيب + هل لها صورة مخصّصة)، مدمجة مع القاعدة.</summary>
        public static List<SizeImageInfo> LoadAll()
        {
            EnsureTable();
            var captions = new Dictionary<string, string>();
            var orders = new Dictionary<string, int>();
            var hasImg = new HashSet<string>();
            try
            {
                using (var con = new SqlConnection(Program.ConnectionString))
                using (var cmd = new SqlCommand("SELECT ImageKey, Caption, SortOrder, CASE WHEN ImageData IS NULL THEN 0 ELSE 1 END AS HasImg FROM dbo.tblSizeImages", con))
                {
                    con.Open();
                    using (var r = cmd.ExecuteReader())
                        while (r.Read())
                        {
                            var k = r.GetString(0);
                            if (!(r["Caption"] is DBNull)) captions[k] = r["Caption"].ToString();
                            if (!(r["SortOrder"] is DBNull)) orders[k] = Convert.ToInt32(r["SortOrder"]);
                            if (Convert.ToInt32(r["HasImg"]) == 1) hasImg.Add(k);
                        }
                }
            }
            catch { }

            var list = Catalog.Select(c => new SizeImageInfo
            {
                Key = c.Key,
                Caption = captions.TryGetValue(c.Key, out var cap) && !string.IsNullOrWhiteSpace(cap) ? cap : c.DefaultCaption,
                SortOrder = orders.TryGetValue(c.Key, out var so) ? so : _idxByKey[c.Key],
                HasCustomImage = hasImg.Contains(c.Key)
            }).ToList();

            return list.OrderBy(x => x.SortOrder).ThenBy(x => _idxByKey[x.Key]).ToList();
        }

        public static void SaveImage(string key, byte[] data)
            => Upsert(key, "ImageData", p => p.AddWithValue("@v", (object)data ?? DBNull.Value));

        public static void SaveCaption(string key, string caption)
            => Upsert(key, "Caption", p => p.AddWithValue("@v", (object)caption ?? DBNull.Value));

        private static void Upsert(string key, string column, Action<SqlParameterCollection> addValue)
        {
            EnsureTable();
            using (var con = new SqlConnection(Program.ConnectionString))
            using (var cmd = new SqlCommand(
$@"MERGE dbo.tblSizeImages AS t
USING (SELECT @k AS ImageKey) AS s ON t.ImageKey = s.ImageKey
WHEN MATCHED THEN UPDATE SET {column}=@v, UpdatedAt=GETDATE()
WHEN NOT MATCHED THEN INSERT(ImageKey, {column}, UpdatedAt) VALUES(@k, @v, GETDATE());", con))
            {
                cmd.Parameters.AddWithValue("@k", key);
                addValue(cmd.Parameters);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>يكتب الترتيب لكل المفاتيح حسب موضعها في القائمة (0..n).</summary>
        public static void SaveOrder(IList<string> keysInOrder)
        {
            EnsureTable();
            using (var con = new SqlConnection(Program.ConnectionString))
            {
                con.Open();
                for (int i = 0; i < keysInOrder.Count; i++)
                {
                    using (var cmd = new SqlCommand(
@"MERGE dbo.tblSizeImages AS t
USING (SELECT @k AS ImageKey) AS s ON t.ImageKey = s.ImageKey
WHEN MATCHED THEN UPDATE SET SortOrder=@o, UpdatedAt=GETDATE()
WHEN NOT MATCHED THEN INSERT(ImageKey, SortOrder, UpdatedAt) VALUES(@k, @o, GETDATE());", con))
                    {
                        cmd.Parameters.AddWithValue("@k", keysInOrder[i]);
                        cmd.Parameters.AddWithValue("@o", i);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>إرجاع الصورة للأصلية المضمّنة (إفراغ ImageData مع إبقاء الاسم/الترتيب).</summary>
        public static void ResetImage(string key)
        {
            try
            {
                using (var con = new SqlConnection(Program.ConnectionString))
                using (var cmd = new SqlCommand("UPDATE dbo.tblSizeImages SET ImageData=NULL, UpdatedAt=GETDATE() WHERE ImageKey=@k", con))
                {
                    cmd.Parameters.AddWithValue("@k", key);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch { }
        }
    }
}
