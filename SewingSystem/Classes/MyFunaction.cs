using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using SewingSystem.Model;
using System.Data.Entity;
using System.Data.SqlClient;
using SewingSystem.LinqModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DevExpress.XtraGrid;
using DevExpress.XtraDataLayout;
using System.Security.Cryptography;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils;
using DocumentFormat.OpenXml.Bibliography;

namespace SewingSystem.Classes
{
    class MyFunaction
    {
       
        public static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(data))
            {
                object obj = formatter.Deserialize(stream);
                return (T)obj;
                //return  (T)formatter.Deserialize(stream);
            };
        }
        public void MessageBoxDeletCustomerInvoice()
        {
            if (Properties.Settings.Default.Language == "ar-SA")
            {
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "لا يمكن حذف عميل لديه فواتير تم طباعتها", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            else if (Properties.Settings.Default.Language == "en-US")
            {
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "Saved successfully", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }
        public void MessageBoxPrintingInvoice()
        {
            if (Properties.Settings.Default.Language == "ar-SA")
            {
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "لا يمكن تعديل او حذف الفاتورة بعد طباعتها", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
            else if (Properties.Settings.Default.Language == "en-US")
            {
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "Saved successfully", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            }
        }
        public void AppearanceGridView(GridView gridView)
        {
            try
            {
                gridView.Appearance.EvenRow.BackColor = System.Drawing.Color.AliceBlue;
                gridView.OptionsView.EnableAppearanceEvenRow = true;
                gridView.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Near;
                gridView.Columns.ForEach(x => x.AppearanceHeader.BackColor = System.Drawing.Color.SteelBlue);
                gridView.OptionsBehavior.ReadOnly = true;
                gridView.Appearance.Empty.BackColor = System.Drawing.Color.AliceBlue;
            }
            catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }
        }
        public void OpenForm(UserControl Master, string Caption)
        {
            XtraForm frm = new XtraForm();
            frm.KeyPreview = true;
            frm.Text = Caption;
            frm.Controls.Add(Master);
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.AutoSize = true;
            frm.RightToLeft = RightToLeft.Yes;
            frm.RightToLeftLayout = true;
            Master.Dock = DockStyle.Fill;
            frm.Show();
        }
        public static void DecryptionSetting()
        {
            // Decrypt each persisted setting independently. A missing or invalid
            // (non-decryptable) value falls back to an empty string instead of
            // throwing — this previously aborted the whole method and triggered
            // infinite recursion, crashing the app on first run / bad config.
            Program.ProductTrue = SafeDecrypt(Properties.Settings.Default.ProductTrue);
            Program.ProductBeginDate = SafeDecrypt(Properties.Settings.Default.ProductBeginDate);
            Program.ProductEndDate = SafeDecrypt(Properties.Settings.Default.ProductEndDate);
            Program.ServerName = SafeDecrypt(Properties.Settings.Default.ServerName);
            Program.DBName = SafeDecrypt(Properties.Settings.Default.DBName);
            Program.Mode = SafeDecrypt(Properties.Settings.Default.Mode);
            Program.SqlUserName = SafeDecrypt(Properties.Settings.Default.SqlUserName);
            Program.SqlPassword = SafeDecrypt(Properties.Settings.Default.SqlPassword);
            Program.ConnType = SafeDecrypt(Properties.Settings.Default.ConnType);
        }

        /// <summary>
        /// Decrypts a setting value, returning "" for empty or non-decryptable input
        /// (so a single bad value never aborts loading the rest of the settings).
        /// When the connection settings end up empty the app falls back to the
        /// connection dialog (XtraFormConnectionDB), which is the intended setup path.
        /// </summary>
        private static string SafeDecrypt(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "";
            try
            {
                return Decryption(value);
            }
            catch
            {
                return "";
            }
        }
        private static string key = "absdefghijklmnolabsdefghijklmnol";
        private static string Iv = "absdefghijklmnol";
        public static  string Encryption(string text)
        {
            byte[] plaintext = System.Text.ASCIIEncoding.ASCII.GetBytes(text);
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(Iv);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
            byte[] encrypted = crypto.TransformFinalBlock(plaintext, 0, plaintext.Length);
            return Convert.ToBase64String(encrypted);
        }
        public static string Decryption(string encrypted)
        {
            byte[] encryptedbyte = Convert.FromBase64String(encrypted);// 
            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(Iv);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;
            ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] decry = crypto.TransformFinalBlock(encryptedbyte, 0, encryptedbyte.Length);
            crypto.Dispose();
            return System.Text.ASCIIEncoding.ASCII.GetString(decry);
        }
        public void MessageBoxAddGroupUser()
        {
            if (Properties.Settings.Default.Language == "ar-SA")
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "اسم مجموعة المستخدمين هذه موجوده من قبل قم باختيار اسم اخر!!!!", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            else if (Properties.Settings.Default.Language == "en-US")
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "This username already exists, choose another name!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        public string ConnectionString_DB()
        {
            string conStraing = "";
            if (Properties.Settings.Default.PathLocalDB)
                conStraing = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" + Properties.Settings.Default.PathLoNaDB + ";Integrated Security=True;";
            else
            {
                if (Program.Mode == "SQL")
                    conStraing = "data source=" + Program.ServerName + ";Initial Catalog=" + Program.DBName + ";user id=" + Program.SqlUserName + ";password=" + Program.SqlPassword + ";";
                else if (Program.Mode == "Windows")
                    conStraing = "data source=" + Program.ServerName + ";Initial Catalog=" + Program.DBName + ";Integrated Security=true;";
            }
            return conStraing;
        }
      
        public DialogResult MessageBoxDelete()
        {
            if (Properties.Settings.Default.Language == "ar-SA")
                return XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "هل انت متاكد من حذف السجل المحدد؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            else //(Properties.Settings.Default.Language == "en-US")
                return XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "Are you sure to delete the specified record ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }
        public DialogResult MessageBoxDeleteTafseel()
        {
            if (Properties.Settings.Default.Language == "ar-SA")
                return XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "هل انت متاكد من حذف التفصيل المحدد؟\nسوف يتم حذف جميع الاستلامات التابعه لهذا التفصيل!!!", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            else //(Properties.Settings.Default.Language == "en-US")
                return XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "Are you sure to delete the specified record ?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }
        public DialogResult MessageBoxDeleteCustomer()
        {
            if (Properties.Settings.Default.Language == "ar-SA")
                return XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "سوف يتم حذف جميع التفصيلات والاستلامات المرتبطة بهذا العميل\nهل انت متاكد من حذف العميل المحدد؟", "حذف", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            else //(Properties.Settings.Default.Language == "en-US")
                return XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "Are you sure to delete the specified record ?\n all price records associated with this record will be deleted", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }
        public void MessageBoxDeleteExcption()
        {
            if (Properties.Settings.Default.Language == "ar-SA")
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "لا يمكن حذف هذا السجل لانه مرتبط بسجلات اخرى!!!!!!", "حذف", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            else if (Properties.Settings.Default.Language == "en-US")
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "This record cannot be deleted because it is linked to other records!!!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
        }
        public void MessageBoxSave()
        {
            if (Properties.Settings.Default.Language == "ar-SA")
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "تم الحفظ بنجاح", "حفظ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            else if (Properties.Settings.Default.Language == "en-US")
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "Saved successfully", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        public void MessageBoxDeleteUserAdmin()
        {
            if (Properties.Settings.Default.Language == "ar-SA")
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "!!!!لا يمكن حذف هذاالمستخدم لانه مدير النظام", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            else if (Properties.Settings.Default.Language == "en-US")
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "This user cannot be deleted because it is the administrator of the system !!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        public void MessageBoxAddUser()
        {
            if (Properties.Settings.Default.Language == "ar-SA")
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "اسم المستخدم هذا موجود من قبل قم باختيار اسم اخر!!!!", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            else if (Properties.Settings.Default.Language == "en-US")
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "This username already exists, choose another name!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        public void MessageBoxDeleteUserGroupAdmin()
        {
            if (Properties.Settings.Default.Language == "ar-SA")
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "!!!!لا يمكن حذف هذه المجموعة لانها خاصة بإدارة النظام", "خطاء", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            else if (Properties.Settings.Default.Language == "en-US")
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "This group cannot be deleted as it is for system administration!!!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        public void MessageBoxException(Exception me)
        {
            XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, me.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public string getUserName(short UserID)=> Session.tblUser.FirstOrDefault(g => g.ID == UserID)?.UserName;
        public string getBranchName(short BranchID)=> Session.tblBranche.FirstOrDefault(g => g.ID == BranchID)?.BranchName;
        public string getCusName(int num)=> Session.tblCustomer.FirstOrDefault(g => g.CusNumber == num & g.BranchID == Program.Branch.ID)?.CustomerName;
        public string getClassName(string num)=> Session.tblClasses.FirstOrDefault(g => g.ClassNumber == num & g.BranchID == Program.Branch.ID)?.ClassName;
        public static tblSellInvoice GetCopyFromInvoice(tblSellInvoice x) => new tblSellInvoice
        {
            EnterTime = x.EnterTime,
            BranchID = x.BranchID,
            Cash = x.Cash,
            Chik = x.Chik,
            ClassNumber = x.ClassNumber,
            CusNumber = x.CusNumber,
            DeliveryDate = x.DeliveryDate,
            Discount = x.Discount,
            ExitComash = x.ExitComash,
            FactoreID = x.FactoreID,
            HowPrint = x.HowPrint,
            ID = x.ID,
            InvoNumber = x.InvoNumber,
            IsDelivery = x.IsDelivery,
            IsExitComash = x.IsExitComash,
            Meater = x.Meater,
            MonyOrpon = x.MonyOrpon,
            MonyPay = x.MonyPay,
            MonyRemin = x.MonyRemin,
            Network = x.Network,
            Notes = x.Notes,
            QuanRemin = x.QuanRemin,
            QuantityM = x.QuantityM,
            SellDate = x.SellDate,
            SizeID = x.SizeID,
            TailorID = x.TailorID,
            TaxAll = x.TaxAll,
            TaxDayn = x.TaxDayn,
            TaxMaden = x.TaxMaden,
            TheName = x.TheName,
            ThePay = x.ThePay,
            TheQuantity = x.TheQuantity,
            TotalAfterDiscount = x.TotalAfterDiscount,
            TotalAll = x.TotalAll,
            TotalDayn = x.TotalDayn,
            TotalFattInvoice = x.TotalFattInvoice,
            TotalFinal = x.TotalFinal,
            TotalMaden = x.TotalMaden,
            TotalMony = x.TotalMony,
            UserID = x.UserID,
            Visa = x.Visa,
        };
        public static tblNote GetCopyFromNote(tblNote x) => new tblNote
        {
            EnterTime = x.EnterTime,
            BranchID = x.BranchID,
            CusNumber = x.CusNumber,
            HowPrint = x.HowPrint,
            ID = x.ID,
            InvoNumber = x.InvoNumber,
            Notes = x.Notes,
            TheQuantity = x.TheQuantity,
            Dayne = x.Dayne,
            Maden = x.Maden,
            NoteDate = x.NoteDate,
            TaxDayne = x.TaxDayne,
            TaxMaden = x.TaxMaden,
            TheType = x.TheType,
            Total = x.Total,
            UserID = x.UserID,
        };
        public static void FunactionPrintInvoice(tblSellInvoice PrintInvo)
        {
            try
            {
                if (PrintInvo == null)
                    return;
                tblCustomer cus = Session.tblCustomer.FirstOrDefault(u => u.CusNumber == PrintInvo.CusNumber & u.BranchID == PrintInvo.BranchID);
                TLVCls tlv = new TLVCls(Program.Branch.CompanyName ?? "", Program.Branch.TaxNumber ?? "", PrintInvo.SellDate ?? DateTime.Now, Convert.ToDecimal(PrintInvo.TotalFinal ?? 0), Convert.ToDecimal(PrintInvo.TotalFattInvoice ?? 0));

                var invoice = (from inv in Session.tblSellInvoice
                               join pay in Session.tblPayment  on inv.InvoNumber equals pay.InvoNumber into paymentGroup
                               from pay in paymentGroup.DefaultIfEmpty()
                               group pay by inv into invoiceGroup
                               where invoiceGroup.Key.ID == PrintInvo.ID & invoiceGroup.Key.BranchID == PrintInvo.BranchID
                               select new
                               {
                                   invoiceGroup.Key.InvoNumber,
                                   invoiceGroup.Key.CusNumber,
                                   QRCode = tlv.ToBase64(),
                                   TheName = cus?.CustomerName,
                                   user = Program.User.UserName,
                                   LogoImage = Program.Branch.LogoImage,
                                   Phone = cus?.Mobil,
                                   invoiceGroup.Key.SellDate,
                                   Discount = invoiceGroup.Key.Discount ?? 0,
                                   TotalAfterDiscount = invoiceGroup.Key.TotalAfterDiscount ?? 0,
                                   TotalFinal = invoiceGroup.Key.TotalFinal ?? 0,
                                   invoiceGroup.Key.Notes,
                                   MonyPay = (invoiceGroup.Key.MonyPay ?? 0) + (invoiceGroup.Key.MonyOrpon ?? 0) + (invoiceGroup.Key.Discount ?? 0),
                                   MonyRemin = invoiceGroup.Key.MonyRemin ?? 0,
                                   TotalFattInvoice = invoiceGroup.Key.TotalFattInvoice ?? 0,
                                   TotalMony = invoiceGroup.Key.TotalMony ?? 0,
                                   TheQuantity = invoiceGroup.Key.TheQuantity ?? 0,
                                   PaymentDate = invoiceGroup.Min(p => p?.PayDate)?.ToString("yyyy/MM/dd")
                               }).ToList();
                if (invoice.Count() <= 0)
                {
                    XtraMessageBox.Show("عفوا تاكد من حفظ الفاتورة");
                    return;
                }
                Report.XtraReportPayAajel.Print(invoice, PrintInvo.ID, PrintInvo.ThePay, PrintInvo.SellDate.Value.ToString());
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }
        public decimal GetdecimalValue(string txt)
        {
            if (decimal.TryParse(txt, out decimal i))
                return i;
            return 0;
        }
        public bool ScanPermissen(tblPermission Permission)
        {
            if (Permission != null)
                return Permission.TheValues ?? false;
            else
                return Program.User.UserState == "Admin";
        }

        public  List<tblPermission> tblPermissionWashType=new List<tblPermission>();
        public void PermissionUser(tblUser User)
        {
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(par => par.UserGroupID == User.UserGroupID/*&par.PermissionName== "عرض-Show"*/).ToList();
            Properties.Settings.Default.المخزن = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "المخزن"));
            Properties.Settings.Default.العمليات = ScanPermissen( tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "العمليات" ));
            Properties.Settings.Default.التقارير = ScanPermissen( tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "التقارير" ));
            Properties.Settings.Default.اعدادات_النظام = ScanPermissen( tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "اعدادات النظام"));

            Properties.Settings.Default.الاقمشة = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "الاقمشة"));
            Properties.Settings.Default.شاشة_التوريد = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "شاشة التوريد"));

            Properties.Settings.Default.المعلمين = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "المعلمين"));
            Properties.Settings.Default.الخياطون = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "الخياطون"));
            Properties.Settings.Default.العملاء = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "العملاء" ));
            Properties.Settings.Default.اشعارات_دائنه = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "اشعارات دائنه ومدينه"));
            Properties.Settings.Default.اشعارات_SMS = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "اشعارات SMS"));
            Properties.Settings.Default.المشتريات = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "المشتريات"));
            Properties.Settings.Default.سندات_القبض = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "سندات القبض"));

            Properties.Settings.Default.التقرير_اليومي = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "التقرير اليومي"));
            Properties.Settings.Default.التقرير_الشهري = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "التقرير الشهري"));
            Properties.Settings.Default.تقرير_الفترة = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "تقرير الفترة من الى"));
            Properties.Settings.Default.الفواتير_للفترة = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "الفواتير للفترة من الى"));
            Properties.Settings.Default.لم_يدفع_عربون = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "لم يدفع عربون"));
            Properties.Settings.Default.الفواتير_غير_المدفوعة = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "تقرير الفواتير غير المدفوعة"));
            Properties.Settings.Default.الفواتير_غير_المستلمة = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "تقرير الفواتير غير المستلمة"));
            Properties.Settings.Default.الفواتير_المدفوعة = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "تقرير الفواتير المدفوعة"));
            Properties.Settings.Default.الفواتير_المستلمة = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "تقرير الفواتير المستلمة"));
            Properties.Settings.Default.بيانات_العملاء = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "تقارير بيانات العملاء"));
            Properties.Settings.Default.تقارير_المشتريات = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "تقارير المشتريات"));
            Properties.Settings.Default.تقارير_سندات_القبض = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "تقارير سندات القبض"));

            Properties.Settings.Default.المستخدمون_والمجموعات = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "المستخدمون والمجموعات"));
            Properties.Settings.Default.ادارة_الصلاحيات = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "ادارة الصلاحيات"));
            Properties.Settings.Default.معلومات_الشركة = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "معلومات الشركة"));
            Properties.Settings.Default.الفروع = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "الفروع"));
            Properties.Settings.Default.اعدادات_الاتصال = ScanPermissen(tblPermissionWashType.FirstOrDefault(p => p.ObjectName == "اعدادات الاتصال"));
            Properties.Settings.Default.Save();
        }

       public static bool BackupDatabase(string connectionString, string backupDirectory)
        {
            // النسخ الاحتياطي على مستوى الخادم (BACKUP DATABASE) يكتب على نظام ملفات
            // خادم SQL، وهذا غير متاح مع الاتصال الخارجي (استضافة مشتركة). نتخطّاه بصمت
            // لتجنّب رسالة الخطأ — تتولّى الاستضافة عمل النسخ الاحتياطي في هذه الحالة.
            if (Program.ConnType == ConnectionStatus.External)
                return false;

            string databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
            string backupFileName = Path.Combine(backupDirectory, $"{databaseName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak");

            string backupQuery = $"BACKUP DATABASE [{databaseName}] TO DISK = '{backupFileName}' WITH FORMAT, MEDIANAME = 'Z_SQLServerBackups', NAME = 'Full Backup of {databaseName}';";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(backupQuery, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                    //MessageBox.Show("تمت العملية بنجاح");

                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($" حدث خطأ عند النسخ الاحتياطي: {ex.Message}");
                return false;
            }
        }

    }
}
