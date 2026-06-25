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
using System.Data.Entity;
using System.Globalization;
using SewingSystem.LinqModel;
using SewingSystem.Classes;
using System.Net;
using System.IO;

namespace SewingSystem.Forms
{
    public partial class XtraFormBranch : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction myfunction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        public XtraFormBranch()
        {
            InitializeComponent();
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "الفروع" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            AddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            Delete.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Delete)?.TheValues??false;
            Print.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues??false;
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        private void AddNew_Click(object sender, EventArgs e)
        {
            tblBranche tblBranch = tblBranchesBindingSource.Current as tblBranche;
            tblBranch.EnterTime = DateTime.Now;
            tblBranch.UserID = Program.User.ID;
            tblBranch.ID = (short)(Session.tblBranche.Max(i => i.ID) + 1);

        }
        private void XtraFormBranch_Load(object sender, EventArgs e)
        {
            FunactionRefrech();
        }


        #region Branches
        private void Save_Click(object sender, EventArgs e)
        {
            if (tblBranchesBindingSource.Current != null)
            {
                tblBranche Branch = tblBranchesBindingSource.Current as tblBranche;
                tblBranchesBindingSource.EndEdit();
                new Data.Repository<tblBranche>().Save(Branch, m => m.ID == Branch.ID, Branch.ID == 0);
                FunactionRefrech();
                myfunction.MessageBoxSave();
                UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
            }
        }
        private void TblBranchesBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (tblBranchesBindingSource.Current != null)
            {
                tblBranche Carr = tblBranchesBindingSource.Current as tblBranche;
                int v = Session.tblBranche.Where(i => i.ID == Carr.ID).Count();
                if (v > 0)
                    UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
                else if (v <= 0)
                    UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (tblBranchesBindingSource.Current != null)
            {
                tblBranche Branch = tblBranchesBindingSource.Current as tblBranche;
                if (Session.tblBranche.Where(b => b.UserID != null).Count() > 0)
                {
                    myfunction.MessageBoxDeleteExcption();
                    return;
                }
                if (myfunction.MessageBoxDelete() == DialogResult.Yes)
                {
                    tblBranchesBindingSource.EndEdit();
                    new Data.Repository<tblBranche>().Delete(m => m.ID == Branch.ID);
                    FunactionRefrech();
                }
            }
        }
        private void Print_Click(object sender, EventArgs e)
        {
            BranchgridControl1.ShowPrintPreview();
        }

        #endregion

        private void Refresh_Click(object sender, EventArgs e)
        {
            FunactionRefrech();
        }

        public void FunactionRefrech()
        {
            tblUsersBindingSource.DataSource = Session.tblUser.ToList();
            Session.GetDataBranche();
            tblBranchesBindingSource.DataSource = Session.tblBranche.ToList();
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
    }
}