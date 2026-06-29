using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//using SewingSystem.Model;
using System.Globalization;
using System.Threading;
//using System.Data.EntityClient;
using System.Data.Common;
using System.Data.Entity.Migrations;
using SewingSystem.LinqModel;
using SewingSystem.Classes;
using DevExpress.LookAndFeel;
using SewingSystem.Properties;
using System.Security.Principal;
using System.Diagnostics;
using System.IO;

namespace SewingSystem
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static tblUser User = new tblUser();
        public static tblBranche Branch = new tblBranche();
        public static bool LogInUser = false;
        public static string Add = "اضافة-Add";
        public static string Update = "تعديل-Update";
        public static string Delete = "حذف-Delete";
        public static string Show = "عرض-Show";
        public static string Print = "طباعة-Print";
        public static string CurruntForm = "";
        public static string ConnectionString = "";
        public static string Pay_Part = "Participation اشتراك";
        public static string Pay_Cash = "Cash نقدا";
        public static string Pay_Deferred = "Deferred آجل";
        public static string PrintMode = "Direct";
        public static string ProductTrue;//=true;
        public static string ProductKey = "KBJF-NXH6-W4JM-CRMQ-G3C";
        public static string ProductBeginDate;//= "2019-08-18";
        public static string ProductEndDate; //= "2019-08-20";
        public static string ServerName;
        public static string CompanyNam;
        public static string DBName;
        public static string Mode;
        public static string SqlUserName;
        public static string SqlPassword;
        // نوع الاتصال: داخلي (خادم محلي) أو خارجي (خادم بعيد) — Classes.ConnectionStatus.Internal/External
        public static string ConnType;

        public static bool is5DaysTrail = true;
        public static int TargetValue = 40 * 60;
        public static string DBName_static = "SewingSystem2";




        static bool IsRunAsAdministrator()
        {
            using (var identity = WindowsIdentity.GetCurrent())
            {
                var principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        /// <summary>
        /// Makes the whole app render numbers with Latin digits (0-9) instead of
        /// Arabic-Indic digits (٠-٩), while keeping the Arabic UI language. Applies to
        /// the current UI thread and any future threads.
        /// </summary>
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetThreadLocale(int Locale);
        private const int LCID_EN_US = 0x0409;

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern System.IntPtr SendMessageTimeout(System.IntPtr hWnd, uint Msg, System.UIntPtr wParam, string lParam, uint fuFlags, uint uTimeout, out System.UIntPtr lpdwResult);

        /// <summary>
        /// الحل الجذري لظهور الأرقام عربية-هندية: إعداد ويندوز «استبدال الأرقام» (NumShape) كان
        /// على وضع «حسب السياق» فيحوّل أرقام الواجهة العربية إلى ٠-٩ رغم أن البيانات مخزّنة لاتينية.
        /// نضبطه على «بدون استبدال» (1) فتظهر الأرقام لاتينية في كل الشاشات وفي كل عناصر DevExpress/ويندوز،
        /// ونُبلغ النظام بالتغيير. يُنفّذ مرة واحدة فقط إن لم يكن مضبوطاً، قبل رسم أي شاشة.
        /// </summary>
        private static void EnsureLatinDigitsLocale()
        {
            try
            {
                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true))
                {
                    if (key == null) return;
                    if ((key.GetValue("NumShape") as string) == "1") return;     // مضبوط مسبقاً
                    key.SetValue("NumShape", "1", Microsoft.Win32.RegistryValueKind.String);
                    System.UIntPtr res;
                    const uint WM_SETTINGCHANGE = 0x001A;
                    SendMessageTimeout((System.IntPtr)0xffff, WM_SETTINGCHANGE, System.UIntPtr.Zero, "intl", 2, 1000, out res);
                }
            }
            catch { /* تجميلي فقط — لا يوقف التشغيل */ }
        }

        public static void ForceLatinDigits()
        {
            var culture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            culture.NumberFormat.DigitSubstitution = DigitShapes.None;
            culture.NumberFormat.NativeDigits = new[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            Thread.CurrentThread.CurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;

            // محاولة على مستوى لغة الـ thread (تفيد بعض عناصر ويندوز القياسية).
            try { SetThreadLocale(LCID_EN_US); } catch { /* تجميلي فقط */ }
        }

        // ---- فرض الأرقام اللاتينية على كل الشاشات تلقائياً ----
        // السبب الجذري أن عناصر DevExpress تُشكّل الأرقام وقت الرسم حسب اتجاه الحقل (RTL → عربية-هندية)،
        // وهذا لا تتحكّم فيه الثقافة ولا لغة الـ thread. الحل المؤكّد: جعل الحقل الذي يحوي أرقاماً LTR
        // فتظهر أرقامه لاتينية، مع محاذاة لليمين ليبقى شكله مناسباً داخل الواجهة العربية. نطبّقه آلياً
        // على كل فورم يُفتح عبر Application.Idle دون الحاجة لتعديل كل شاشة على حدة.
        // نتتبّع كل عنصر تم ضبطه (مرة واحدة لكل عنصر) بدل تتبّع الفورم؛ لأن بيانات الشاشات
        // قد تُحمّل بعد فتحها، فنعيد المرور على كل العناصر في كل Idle ونضبط أي عنصر يعرض أرقاماً.
        private static readonly System.Collections.Generic.HashSet<DevExpress.XtraEditors.BaseEdit> _latinEdits
            = new System.Collections.Generic.HashSet<DevExpress.XtraEditors.BaseEdit>();

        private static void ApplyLatinDigitsToOpenForms(object sender, EventArgs e)
        {
            var snapshot = new System.Collections.Generic.List<Form>();
            foreach (Form f in Application.OpenForms) snapshot.Add(f);
            foreach (var f in snapshot)
                ApplyLatinDigits(f);
        }

        private static void ApplyLatinDigits(Control root)
        {
            if (root == null) return;
            if (root is DevExpress.XtraEditors.BaseEdit edit
                && !_latinEdits.Contains(edit) && NeedsLatin(edit))
            {
                try
                {
                    edit.RightToLeft = RightToLeft.No;
                    edit.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                    edit.Properties.Appearance.Options.UseTextOptions = true;
                    _latinEdits.Add(edit);
                    edit.Disposed += (s, a) => _latinEdits.Remove(edit);
                }
                catch { /* تجاهل أي عنصر لا يقبل الضبط */ }
            }
            foreach (Control child in root.Controls)
                ApplyLatinDigits(child);
        }

        private static bool NeedsLatin(DevExpress.XtraEditors.BaseEdit edit)
        {
            // عناصر رقمية/تاريخية: دائماً لاتينية (حتى وهي فارغة، فيظهر الإدخال لاتينياً)
            if (edit is DevExpress.XtraEditors.SpinEdit
                || edit is DevExpress.XtraEditors.CalcEdit
                || edit is DevExpress.XtraEditors.DateEdit
                || edit is DevExpress.XtraEditors.TimeEdit)
                return true;
            // بقية الحقول: عند ظهور أي رقم فيها (لاتيني أو عربي-هندي) نحوّلها LTR فتظهر لاتينية
            string t = edit.Text;
            if (string.IsNullOrEmpty(t)) return false;
            foreach (char ch in t)
                if ((ch >= '0' && ch <= '9') || (ch >= '٠' && ch <= '٩') || (ch >= '۰' && ch <= '۹'))
                    return true;
            return false;
        }

        [STAThread]
        static void Main()
        {
            // Administrator rights are no longer forced on every launch (that
            // produced a UAC prompt each time). Normal operation only needs to
            // connect to SQL Server via Windows auth, which does not require
            // elevation. The one path that does need admin — the first-run
            // auto-attach of the .mdf below — already fails gracefully into the
            // connection dialog, which is the intended setup path.


            //if (!Properties.Settings.Default.is_activated)
            //{
            //    Application.Run(new Forms.XtraFormActive());
            //    return;
            //}
            EnsureLatinDigitsLocale();   // أرقام لاتينية على مستوى ويندوز قبل رسم أي شاشة
            SplashScreenManager.ShowForm(Forms.XtraFormLogin.ActiveForm, typeof(Forms.WaitForm1));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(Properties.Settings.Default.Language);
            // Force Western (English) digits 0-9 everywhere, even with an Arabic RTL UI.
            // Arabic cultures shape digits to Arabic-Indic (٠-٩) by default; DigitShapes.None
            // keeps the Latin glyphs while leaving the UI language Arabic.
            ForceLatinDigits();
            Application.Idle += ApplyLatinDigitsToOpenForms;   // أرقام لاتينية على كل شاشة تُفتح
            MyFunaction myfunaction = new MyFunaction();
            MyFunaction.DecryptionSetting();
           
            string executablePath = AppDomain.CurrentDomain.BaseDirectory;

            ConnectionString = myfunaction.ConnectionString_DB();
            try
            {
                if (String.IsNullOrWhiteSpace(ConnectionString) || !Database.Exists(ConnectionString))
                {
                    // ATTACH //

                    Program.DBName = Program.DBName_static;
                    Program.ServerName = ".\\SQLEXPR14PLANETS";
                    Program.Mode = "Windows";

                    string connectionString = string.Format(@"Server={0};Integrated Security=true;", Program.ServerName);

                    string mdfFilePath = Path.Combine(executablePath, "Data", Program.DBName + ".mdf");
                    string ldfFilePath = Path.Combine(executablePath, "Data", Program.DBName + "_log.ldf");

                    Properties.Settings.Default.PathLocalDB = false;
                    Properties.Settings.Default.Mode = Classes.MyFunaction.Encryption("Windows");
                    Properties.Settings.Default.ServerName = Classes.MyFunaction.Encryption(Program.ServerName);
                    Properties.Settings.Default.DBName = Classes.MyFunaction.Encryption(Program.DBName);
                    Properties.Settings.Default.ConnType = Classes.MyFunaction.Encryption(Classes.ConnectionStatus.Internal);
                    Properties.Settings.Default.Save();

                    MyFunaction.DecryptionSetting();

                    FileFolderHelper.SetFilePermissions(mdfFilePath);
                    FileFolderHelper.SetFilePermissions(ldfFilePath);
                    FileFolderHelper.SetDirectoryPermissions(Path.Combine(executablePath, "Data"));

                    DatabaseHelper helper = new DatabaseHelper();
                    helper.AttachDatabase(Program.DBName, mdfFilePath, ldfFilePath, connectionString);
                    //helper.RestoreDatabase(Program.DBName, backupFilePath, connectionString);


                    //لا داعب لتغييرها لأنها ثابتة نفسها
                    //string newConnectionString = ConnectionStringHelper.GetConnectionString(
                    //           Classes.MyFunaction.Decryption(Properties.Settings.Default.DBName),
                    //           Classes.MyFunaction.Decryption(Properties.Settings.Default.ServerName),
                    //           Classes.MyFunaction.Decryption(Properties.Settings.Default.SqlUserName),
                    //           Classes.MyFunaction.Decryption(Properties.Settings.Default.SqlPassword),
                    //            Classes.MyFunaction.Decryption(Properties.Settings.Default.Mode) == "Windows"
                    //            );
                    //ConnectionStringHelper.UpdateConnectionString("SewingMilitary3Entities", newConnectionString);


                    ConnectionString = myfunaction.ConnectionString_DB();


                    Application.Restart();
                    return;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                Application.Run(new Forms.XtraFormConnectionDB(ex.Message));
            }


            try
            {
                if (Database.Exists(ConnectionString))
                {
                    Session.GetDataBranche();
                    Session.GetDataUserAndGroup();
                    Session.GetDataPermission();
                }
                UserLookAndFeel.Default.SkinName = Settings.Default.SkinName.ToString();
                UserLookAndFeel.Default.SetSkinStyle(Settings.Default.SkinName.ToString(), Settings.Default.PaletteName.ToString());

                Application.Run(new Forms.XtraFormLogin());
                SplashScreenManager.ShowForm(Forms.XtraFormLogin.ActiveForm, typeof(Forms.WaitForm1));
                if (LogInUser)
                {
                    //GetData();
                    myfunaction.PermissionUser(User);
                    Application.Run(new Forms.XtraFormMain());
                }
                //else
                //    Application.Run(new Forms.XtraFormConnectionDB());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Run(new Forms.XtraFormConnectionDB());
            }
        }
        //public async static void GetData()
        //{
        //    await InitObjects();
        //}
        public static async Task InitObjects()
        {
            IList<Task> taskList = new List<Task>();
            taskList.Add(Task.Run(() => Session.GetDataSellInvoice()));
            taskList.Add(Task.Run(() => Session.GetDataBuyInvoice()));
            taskList.Add(Task.Run(() => Session.GetDataBuyInvoiceDetaile()));
            taskList.Add(Task.Run(() => Session.GetDataClasses()));
            taskList.Add(Task.Run(() => Session.GetDataCustomer()));
            taskList.Add(Task.Run(() => Session.GetDataSupplier()));
            taskList.Add(Task.Run(() => Session.GetDataDefaultSize()));
            taskList.Add(Task.Run(() => Session.GetDataNote()));
            taskList.Add(Task.Run(() => Session.GetDataPayment()));
            taskList.Add(Task.Run(() => Session.GetDataSanadSarf()));
            taskList.Add(Task.Run(() => Session.GetDataSellInvoiceDetaile()));
            taskList.Add(Task.Run(() => Session.GetDataTailor()));
            taskList.Add(Task.Run(() => Session.GetDateFactorie()));
            await Task.WhenAll(taskList);
        }
    }
}
