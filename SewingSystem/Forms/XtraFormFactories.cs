using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using SewingSystem.LinqModel;
using System.Data.Entity;
using SewingSystem.Classes;

namespace SewingSystem.Forms
{
    public partial class XtraFormFactories : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        public XtraFormFactories()
        {
            InitializeComponent();
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "العملاء" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            AddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            Delete.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Delete)?.TheValues??false;
            Print.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues??false;
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        public void RefrechData()
        {
            Session.GetDateFactorie();
            tblBranchesBindingSource.DataSource = Session.tblBranche;
            tblUsersBindingSource.DataSource = Session.tblUser;
            tblFactorieBindingSource.DataSource = Session.tblFactorie.ToList();
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            RefrechData();
        }
        private void AddNew_Click(object sender, EventArgs e)
        {
            tblFactorie tblFactorie = tblFactorieBindingSource.Current as tblFactorie;
            tblFactorie.EnterTime = DateTime.Now;
            tblFactorie.BranchID = Program.User.BranchID;
            tblFactorie.UserID = Program.User.ID;
        }
        private void Save_Click(object sender, EventArgs e)
        {
            if (tblFactorieBindingSource.Current != null)
            {
                tblFactorieBindingSource.EndEdit();
                tblFactorie Factorie = tblFactorieBindingSource.Current as tblFactorie;
                using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    db.tblFactories.DeleteAllOnSubmit(db.tblFactories.Where(m => m.ID == Factorie.ID));
                    db.tblFactories.InsertOnSubmit(Factorie);
                    db.SubmitChanges();
                    RefrechData();
                }
                MyFunaction.MessageBoxSave();
                UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
            }
        }
        private void tblFactoriesBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (tblFactorieBindingSource.Current != null)
            {
                tblFactorie emp = tblFactorieBindingSource.Current as tblFactorie;
                int v = Session.tblFactorie.Where(i => i.ID == emp.ID & i.BranchID == emp.BranchID).Count();
                if (v > 0)
                    UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
                else if (v <= 0)
                    UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (tblFactorieBindingSource.Current != null)
            {
                if (MyFunaction.MessageBoxDelete() == DialogResult.Yes)
                {
                    tblFactorie Factorie = tblFactorieBindingSource.Current as tblFactorie;
                    tblFactorieBindingSource.EndEdit();
                    using (var db = new DataClasses1DataContext(Program.ConnectionString))
                    {
                        db.tblFactories.DeleteAllOnSubmit(db.tblFactories.Where(m => m.ID == Factorie.ID));
                        db.SubmitChanges();
                        RefrechData();
                    }
                }
            }
        }
        //Report.XtraReportFactory rptFactorie;
        private void Print_Click(object sender, EventArgs e)
        {
            gridControl1.ShowPrintPreview();
            //rptFactorie = new Report.XtraReportFactory();
            //rptFactorie.DataSource = (from cus in Session.tblFactorie
            //                          select new
            //                          {
            //                              cus.ID,
            //                              user = getUserName((cus.UserID as short?) ?? 0),
            //                              BranchName = getBranchName((cus.BranchID as short?) ?? 0),
            //                              cus.TheNameAr,
            //                              cus.TheNameEn,
            //                              cus.Notes,
            //                              cus.EnterTime,
            //                          }).ToList();
            //PrintReport(rptFactorie);
        }
      
    
        Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
        public void PrintReport(DevExpress.XtraReports.UI.XtraReport Report)
        {
            frmReport.documentViewer1.DocumentSource = Report;
            frmReport.ShowDialog();
        }

        private void XtraFormFactories_Load(object sender, EventArgs e)
        {
            RefrechData();
        }

    }
    
}