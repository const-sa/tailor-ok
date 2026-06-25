using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace SewingSystem.Classes
{
    public static class FileFolderHelper
    {
        public static void SetFilePermissions(string filePath)
        {
            var fileSecurity2 = File.GetAccessControl(filePath);
            //Create a new rule set, based on "Everyone"
            var fileAccessRule = new FileSystemAccessRule(new NTAccount("", "Everyone"),
                  FileSystemRights.FullControl,
                  AccessControlType.Allow);
            // Append the new rule set to the file
            fileSecurity2.AddAccessRule(fileAccessRule);
            // And persist it to the filesystem
            File.SetAccessControl(filePath, fileSecurity2);

        }

        public static void SetDirectoryPermissions(string directoryPath)
        {
            // الحصول على معلومات المجلد
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

            // الحصول على إعدادات الأمان للمجلد
            DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();

            // إنشاء قاعدة الوصول التي تسمح بالصلاحيات الكاملة لـ "Everyone"
            FileSystemAccessRule accessRule = new FileSystemAccessRule(
                "Everyone",
                FileSystemRights.FullControl,
                AccessControlType.Allow
            );

            // إضافة قاعدة الوصول إلى إعدادات الأمان
            directorySecurity.AddAccessRule(accessRule);

            // تطبيق إعدادات الأمان المعدلة على المجلد
            directoryInfo.SetAccessControl(directorySecurity);
        }
    }
}
