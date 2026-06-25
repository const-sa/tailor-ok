using DevExpress.XtraGrid.Views.Base.ViewInfo;
using SewingSystem.LinqModel;
using System;
using System.Collections.Generic;
using System.Linq;
namespace SewingSystem.Classes
{

    public static class Session
    {

        public static class Defualts
        {
            public static double TaxRate { get => (Program.Branch?.TaxRate ?? 15) / 100; }
            public static double TaxOperator { get => 1 + TaxRate; }
        }

        public static List<LinqModel.tblTailor> tblTailor;
        public static void GetDataTailor()
        {
            using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
            {
                if (Program.User.UserState == "Admin")
                    tblTailor = db.tblTailors.ToList();
                else
                    tblTailor = db.tblTailors.Where(s => s.BranchID == Program.User.BranchID).ToList();
            }
        }

        public static List<LinqModel.tblSanadSarf> tblSanadSarf;
        public static void GetDataSanadSarf()
        {
            using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
            {
                if (Program.User.UserState == "Admin")
                    tblSanadSarf = db.tblSanadSarfs.ToList();
                else
                    tblSanadSarf = db.tblSanadSarfs.Where(s => s.BranchID == Program.User.BranchID).ToList();
            }
        }
        public static List<LinqModel.tblFactorie> tblFactorie;
        public static void GetDateFactorie()
        {
            using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
            {
                if (Program.User.UserState == "Admin")
                    tblFactorie = db.tblFactories.ToList();
                else
                    tblFactorie = db.tblFactories.Where(s => s.BranchID == Program.User.BranchID).ToList();
            }
        }

        //public static List<LinqModel.View_Invoice> _ViewSellInvoices;
        //public static List<LinqModel.View_Invoice> View_Invoice
        //{
        //    get
        //    {
        //        try
        //        {
        //            using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
        //            {
        //                if (Program.User.UserState == "Admin")
        //                    _ViewSellInvoices = db.View_Invoices.ToList();
        //                else
        //                    _ViewSellInvoices = db.View_Invoices.Where(s => s.BranchID == Program.User.BranchID).ToList();
        //            }
        //        }
        //           catch (Exception)
        //        {

        //        }

        //        return _ViewSellInvoices;
        //    }
        //}
        public enum PrintFileType
        {
            Printer = 1,
            PDF,
            Xlsx
        }
        public static bool LangEng => Properties.Settings.Default.Language == "en-US";
        public static List<LinqModel.tblClasse> tblClasses;
        public static void GetDataClasses()
        {
            try
            {
                using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
                {
                    if (Program.User.UserState == "Admin")
                        tblClasses = db.tblClasses.ToList();
                    else
                        tblClasses = db.tblClasses.Where(s => s.BranchID == Program.User.BranchID).ToList();
                }

            }
               catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }

        }

        public static List<LinqModel.tblSellInvoice> tblSellInvoice;
        public static void GetDataSellInvoice()
        {
            try
            {
                using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
                {
                    if (Program.User.UserState == "Admin")
                        tblSellInvoice = db.tblSellInvoices.ToList();
                    else
                        tblSellInvoice = db.tblSellInvoices.Where(s => s.BranchID == Program.User.BranchID).ToList();
                }
            }
            catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }

        }
        public static List<LinqModel.tblPayment> tblPayment;
        public static void GetDataPayment()
        {
            try
            {
                using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
                {
                    if (Program.User.UserState == "Admin")
                        tblPayment = db.tblPayments.ToList();
                    else
                        tblPayment = db.tblPayments.Where(s => s.BranchID == Program.User.BranchID).ToList();
                }
            }
               catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }

        }
        public static void RefreshDatatSellInvoice(tblSellInvoice sell)
        {
            if (sell == null) return;
            if (tblSellInvoice.Any(s => s.ID == sell.ID))
            {
                var cus = tblSellInvoice.FirstOrDefault(s => s.ID == sell.ID);
                int index = tblSellInvoice.IndexOf(cus);
                tblSellInvoice.Remove(cus);
                tblSellInvoice.Insert(index, sell);
            }
            else
                tblSellInvoice.Add(sell);
        }
        public static List<LinqModel.tblSellInvoiceDetaile> tblSellInvoiceDetaile;
        public static void GetDataSellInvoiceDetaile()
        {
            try
            {
                using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
                {
                    if (Program.User.UserState == "Admin")
                        tblSellInvoiceDetaile = db.tblSellInvoiceDetailes.ToList();
                    else
                        tblSellInvoiceDetaile = db.tblSellInvoiceDetailes.Where(s => s.BranchID == Program.User.BranchID).ToList();
                }
            }
               catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }

        }

        public static List<LinqModel.tblBranche> tblBranche;
        public static void GetDataBranche()
        {
            using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
            {
                if (!db.tblBranches.Any())
                {
                    tblBranche FirstBranch = new tblBranche { ID = 1, BranchName = "الرئيسي", TaxRate = 15, UseTax = true, PrintTowBill = true, EnterTime = DateTime.Now };
                    db.tblBranches.InsertOnSubmit(FirstBranch);
                    db.SubmitChanges();
                }
                tblBranche = db.tblBranches.ToList();
            }
        }

        public static List<LinqModel.tblNote> tblNote;
        public static void GetDataNote()
        {
            using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
                tblNote = db.tblNotes.ToList();
        }

        public static List<LinqModel.tblBuyInvoice> tblBuyInvoice;
        public static void GetDataBuyInvoice()
        {
            try
            {
                using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
                {
                    if (Program.User.UserState == "Admin")
                        tblBuyInvoice = db.tblBuyInvoices.ToList();
                    else
                        tblBuyInvoice = db.tblBuyInvoices.Where(s => s.BranchID == Program.User.BranchID).ToList();
                }
            }
               catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }

        }

        public static List<LinqModel.tblBuyInvoiceDetaile> tblBuyInvoiceDetaile;
        public static void GetDataBuyInvoiceDetaile()
        {
            try
            {
                using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
                {
                    if (Program.User.UserState == "Admin")
                        tblBuyInvoiceDetaile = db.tblBuyInvoiceDetailes.ToList();
                    else
                        tblBuyInvoiceDetaile = db.tblBuyInvoiceDetailes.Where(s => s.BranchID == Program.User.BranchID).ToList();
                }
            }
               catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }

        }

        public static List<LinqModel.tblUser> tblUser;
        public static void GetDataUserAndGroup()
        {
            try
            {
                using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
                {
                    tblUser = db.tblUsers.ToList();
                    tblUserGroup = db.tblUserGroups.ToList();
                }
            }
               catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }


        }

        public static List<LinqModel.tblUserGroup> tblUserGroup;

        public static List<LinqModel.tblPermission> tblPermission;
        public static void GetDataPermission()
        {
            using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
                tblPermission = db.tblPermissions.ToList();
        }

        public static List<LinqModel.tblCustomer> tblCustomer;
        public static void GetDataCustomer()
        {
            try
            {
                using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
                {
                    if (Program.User.UserState == "Admin")
                        tblCustomer = db.tblCustomers.ToList();
                    else
                        tblCustomer = db.tblCustomers.Where(s => s.BranchID == Program.User.BranchID).ToList();
                }
            }
               catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }

        }

        public static List<LinqModel.tblSupplier> tblSupplier;
        public static void GetDataSupplier()
        {
            try
            {
                using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
                {
                    if (Program.User.UserState == "Admin")
                        tblSupplier = db.tblSuppliers.ToList();
                    else
                        tblSupplier = db.tblSuppliers.Where(s => s.BranchID == Program.User.BranchID).ToList();
                }
            }
            catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }

        }


        public static List<LinqModel.tblDefaultSize> tblDefaultSize;
        public static void GetDataDefaultSize()
        {
            try
            {
                using (var db = new LinqModel.DataClasses1DataContext(Program.ConnectionString))
                {
                    if (Program.User.UserState == "Admin")
                        tblDefaultSize = db.tblDefaultSizes.ToList();
                    else
                        tblDefaultSize = db.tblDefaultSizes.Where(s => s.BranchID == Program.User.BranchID).ToList();
                }
            }
               catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }

        }
    }
}

