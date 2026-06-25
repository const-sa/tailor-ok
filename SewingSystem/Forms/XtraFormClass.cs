using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using SewingSystem.LinqModel;
using SewingSystem.Classes;

namespace SewingSystem.Forms
{
    public partial class XtraFormClass : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        public XtraFormClass()
        {
            InitializeComponent();
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "الاقمشة" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            AddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            Delete.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Delete)?.TheValues??false;
            Print.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues??false;
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        public void RefrechData()
        {
            Session.GetDataClasses();
            tblBranchesBindingSource.DataSource = Session.tblBranche;
            tblUsersBindingSource.DataSource = Session.tblUser;
            tblClasseBindingSource.DataSource = Session.tblClasses.ToList();
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            RefrechData();
        }
        private void AddNew_Click(object sender, EventArgs e)
        {
            CurrentClasse.EnterTime = DateTime.Now;
            CurrentClasse.BranchID = Program.User.BranchID;
            CurrentClasse.UserID = Program.User.ID;
            CurrentClasse.BuyPrice = 0;
            CurrentClasse.Price = 0;
            CurrentClasse.QuantityRemin = 0;
            CurrentClasse.TheQuantityM = 0;
            if (Session.tblClasses.Where(s => s.BranchID == Program.User.BranchID).Count() <= 0)
                CurrentClasse.ClassNumber = "1";
            else
                CurrentClasse.ClassNumber =(Session.tblClasses.Where(s => s.BranchID == Program.User.BranchID).Max(i =>int.Parse(i.ClassNumber)) + 1).ToString();

        }
        private void Save_Click(object sender, EventArgs e)
        {
            if (CurrentClasse == null)
                return;
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    db.tblClasses.DeleteAllOnSubmit(db.tblClasses.Where(m => m.ID == CurrentClasse.ID));
                    db.tblClasses.InsertOnSubmit(CurrentClasse);
                    db.SubmitChanges();
                    RefrechData();
                }
                MyFunaction.MessageBoxSave();
                UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        private void tblClassesBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (CurrentClasse == null)
                return;
            int v = Session.tblClasses.Where(i => i.ID == CurrentClasse.ID & i.BranchID == CurrentClasse.BranchID).Count();
                if (v > 0)
                    UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
                else if (v <= 0)
                    UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (CurrentClasse == null)
                return;
            if (MyFunaction.MessageBoxDelete() != DialogResult.Yes)
                return;
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                db.tblClasses.DeleteAllOnSubmit(db.tblClasses.Where(m => m.ID == CurrentClasse.ID));
                db.SubmitChanges();
                RefrechData();
            }
        }
        Report.XtraReportClasses rptCategorie;
        private void Print_Click(object sender, EventArgs e)
        {
            rptCategorie = new Report.XtraReportClasses();
            rptCategorie.DataSource = (from cus in Session.tblClasses
                                       select new
                                       {
                                           cus.ID,
                                           user =MyFunaction. getUserName((cus.UserID as short?) ?? 0),
                                           BranchName = MyFunaction.getBranchName((cus.BranchID as short?) ?? 0),
                                           cus.ClassName,
                                           cus.BuyPrice,
                                           cus.Price,
                                           cus.ClassColor,
                                           cus.ClassNumber,
                                           cus.QuantityRemin,
                                           cus.TheQuantityM,
                                           cus.EnterTime,
                                       }).ToList();
            PrintReport(rptCategorie);
        }
       
       
        Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
        public void PrintReport(DevExpress.XtraReports.UI.XtraReport Report)
        {
            frmReport.documentViewer1.DocumentSource = Report;
            frmReport.ShowDialog();
        }

        private void XtraFormItems_Load(object sender, EventArgs e)
        {
            RefrechData();
        }
        tblClasse CurrentClasse => tblClasseBindingSource.Current as tblClasse;
        private void TheQuantityMTextEdit_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (CurrentClasse == null || e.NewValue.ToString() == "")
                return;
            CurrentClasse.TheQuantityM = double.Parse(e.NewValue.ToString());
        }
    }
}