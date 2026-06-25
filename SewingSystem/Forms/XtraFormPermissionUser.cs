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
using DevExpress.XtraTreeList;

namespace SewingSystem.Forms
{
    public partial class XtraFormPermissionUser : DevExpress.XtraEditors.XtraForm
    {
        public XtraFormPermissionUser()
        {
            InitializeComponent();
        }
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        public void Refrech_Data()
        {
            
            tblPermissionsBindingSource.DataSource = Session.tblPermission;
            UserGroupBindingSource.DataSource = Session.tblUserGroup;
            RefreshForPermission();
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            Refrech_Data();
        }
        private void Print_Permission_Click(object sender, EventArgs e)
        {
            Permission_dataLayoutCon.ShowPrintPreview();
        }
        public void RefreshForPermission()
        {
            if (UserGroupBindingSource.Current != null)
            {
                Permission_treeList.DataSource = null;
                tblUserGroup tblUserGroups = UserGroupBindingSource.Current as tblUserGroup;
                Permission_treeList.ClearNodes();
                Permission_treeList.DataSource = Session.tblPermission.Where(u => u.UserGroupID == tblUserGroups.ID).ToList();
            }
        }
        private void checkButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkButton1.Checked)
                Perm(false, "تفعيل كل الصلاحيات", "Enable All Roles");
            else
                Perm(true, "ايقاف كل الصلاحيات", "Stop All Roles"); 
        }
   public void Perm(bool state,string ArPer,string EnPer)
        {
            if (Properties.Settings.Default.Language == "ar-SA")
                checkButton1.Text = ArPer;
            else if (Properties.Settings.Default.Language == "en-US")
                checkButton1.Text = EnPer;
            if (UserGroupBindingSource.Current != null)
            {
                Permission_treeList.DataSource = null;
                tblUserGroup tblUserGroups = UserGroupBindingSource.Current as tblUserGroup;
                var p = Session.tblPermission.Where(f => f.UserGroupID == tblUserGroups.ID);
                foreach (var item in p)
                {
                    item.TheValues = state;
                    if (ChangPerm.Keys.Contains(item.ID))
                        ChangPerm[item.ID] = state;
                    else
                        ChangPerm.Add(item.ID,state);
                }
                Permission_treeList.ClearNodes();
                if (p != null) Permission_treeList.DataSource = p.ToList();
            }
        }
        private void PermissionSave_Click(object sender, EventArgs e)
        {
            tblPermissionsBindingSource.EndEdit();
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                foreach (var item in ChangPerm)
                {
                    tblPermission Permission1 = db.tblPermissions.SingleOrDefault(m => m.ID == item.Key);
                    Permission1.TheValues = item.Value;
                    db.tblPermissions.DeleteAllOnSubmit(db.tblPermissions.Where(m => m.ID == item.Key));
                    db.tblPermissions.InsertOnSubmit(Permission1);
                }
                db.SubmitChanges();
            }
            MyFunaction.MessageBoxSave();
            MyFunaction.PermissionUser(Session.tblUser.SingleOrDefault(u=>u.ID==Program.User.ID));
        }

        private void XtraFormPermissionUser_Load(object sender, EventArgs e)
        {
            Refrech_Data();
            Permission_treeList.CellValueChanging += Permission_treeList_CellValueChanging;
        }
        Dictionary<long, bool> ChangPerm = new Dictionary<long, bool>();
        private void Permission_treeList_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            var Permissions = Permission_treeList.GetFocusedRow() as tblPermission;
            switch (e.Column.FieldName)
            {
                case nameof(Permissions.TheValues):
                    if (ChangPerm.Keys.Contains(Permissions.ID))
                        ChangPerm[Permissions.ID]= (e.Value as bool?) ?? false;
                    else
                    ChangPerm.Add(Permissions.ID, (e.Value as bool?) ?? false);
                    Permissions.TheValues = (e.Value as bool?) ?? false;
                    break;
            }
        }
        private void UserGroupBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            RefreshForPermission();
        }
    }
}