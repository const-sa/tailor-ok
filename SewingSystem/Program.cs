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
            SplashScreenManager.ShowForm(Forms.XtraFormLogin.ActiveForm, typeof(Forms.WaitForm1));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(Properties.Settings.Default.Language);
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
