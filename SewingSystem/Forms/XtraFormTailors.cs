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
    public partial class XtraFormTailor : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        public XtraFormTailor()
        {
            InitializeComponent();
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "الخياطون" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            AddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            Delete.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Delete)?.TheValues??false;
            Print.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues??false;
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        public void RefrechData()
        {
            Session.GetDataTailor();
            tblBranchesBindingSource.DataSource = Session.tblBranche;
            tblUsersBindingSource.DataSource = Session.tblUser;
            tblTailorBindingSource.DataSource = Session.tblTailor.ToList();
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            RefrechData();
        }
        private void AddNew_Click(object sender, EventArgs e)
        {
            tblTailor tblTailor = tblTailorBindingSource.Current as tblTailor;
            tblTailor.EnterTime = DateTime.Now;
            tblTailor.BranchID = Program.User.BranchID;
            tblTailor.UserID = Program.User.ID;
        }
        private void Save_Click(object sender, EventArgs e)
        {
            if (tblTailorBindingSource.Current != null)
            {
                tblTailorBindingSource.EndEdit();
                tblTailor Tailor = tblTailorBindingSource.Current as tblTailor;
                using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    db.tblTailors.DeleteAllOnSubmit(db.tblTailors.Where(m => m.ID == Tailor.ID));
                    db.tblTailors.InsertOnSubmit(Tailor);
                    db.SubmitChanges();
                    RefrechData();
                }
                MyFunaction.MessageBoxSave();
                UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
            }
        }
        private void tbltailorsBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (tblTailorBindingSource.Current != null)
            {
                tblTailor emp = tblTailorBindingSource.Current as tblTailor;
                int v = Session.tblTailor.Where(i => i.ID == emp.ID & i.BranchID == emp.BranchID).Count();
                if (v > 0)
                    UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
                else if (v <= 0)
                    UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (tblTailorBindingSource.Current != null)
            {
                if (MyFunaction.MessageBoxDelete() == DialogResult.Yes)
                {
                    tblTailor Tailor = tblTailorBindingSource.Current as tblTailor;
                    tblTailorBindingSource.EndEdit();
                    using (var db = new DataClasses1DataContext(Program.ConnectionString))
                    {
                        db.tblTailors.DeleteAllOnSubmit(db.tblTailors.Where(m => m.ID == Tailor.ID));
                        db.SubmitChanges();
                        RefrechData();
                    }
                }
            }
        }
        //Report.XtraReportTailors rptTailor;
        private void Print_Click(object sender, EventArgs e)
        {
            gridControl1.ShowPrintPreview();
            //rptTailor = new Report.XtraReportTailors();
            //rptTailor.DataSource = (from cus in Session.tblTailor
            //                        select new
            //                        {
            //                            cus.ID,
            //                            user = getUserName((cus.UserID as short?) ?? 0),
            //                            BranchName = getBranchName((cus.BranchID as short?) ?? 0),
            //                            cus.TtailorName,
            //                            cus.Email,
            //                            cus.Address,
            //                            cus.Mobil,
            //                            cus.EnterTime,
            //                            cus.Mobil2,
            //                        }).ToList();
            //PrintReport(rptTailor);
        }
       
        Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
        public void PrintReport(DevExpress.XtraReports.UI.XtraReport Report)
        {
            frmReport.documentViewer1.DocumentSource = Report;
            frmReport.ShowDialog();
        }

        private void XtraFormTailors_Load(object sender, EventArgs e)
        {
            RefrechData();
        }

    }
   
}