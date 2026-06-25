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
    public partial class XtraFormUsers : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        tblPermission Permission2;
        List<tblPermission> tblPermissionWashType2 = new List<tblPermission>();
        public XtraFormUsers()
        {
            InitializeComponent();
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "المستخدمين" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            AddNewUser.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            UsersDelete.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Delete)?.TheValues??false;
            Users_print.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues??false;
            User_layoutControlGroup43.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;

            Permission2 = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "مجموعات المستخدمين" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType2.Clear();
            tblPermissionWashType2 = Session.tblPermission.Where(p => p.ParentID == Permission2.ID).ToList();
            AddNewUserGroup.Visible = tblPermissionWashType2.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            UserGroupDelete.Visible = tblPermissionWashType2.FirstOrDefault(p => p.PermissionName == Program.Delete)?.TheValues??false;
            UserGroup_print.Visible = tblPermissionWashType2.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues??false;
            UserGroup_layoutControlGroup41.Enabled = tblPermissionWashType2.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }

        public void RefrechUser()
        {
            Session.GetDataUserAndGroup();
            tblBranchesBindingSource.DataSource = Session.tblBranche.ToList();
            tblUsersBindingSource.DataSource = Session.tblUser.ToList();
            tblUserGroupBindingSource.DataSource = Session.tblUserGroup.ToList();
            UserGroup_layoutControlGroup41.Enabled = tblPermissionWashType2.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
            User_layoutControlGroup43.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            RefrechUser();
        }

        private void UserSave_Click(object sender, EventArgs e)
        {
            try
            {
                tblUsersBindingSource.EndEdit();
                if (tblUsersBindingSource.Current!=null)
                {
                    tblUser user = tblUsersBindingSource.Current as tblUser;
                    int users = Session.tblUser.Where(u => u.UserName == user.UserName).Count();
                    if (users > 0 & user.ID == 0)
                    {
                        MyFunaction.MessageBoxAddUser();
                        RefrechUser();
                        return;
                    }
                    new Data.Repository<tblUser>().Save(user, m => m.ID == user.ID, user.ID == 0);
                    RefrechUser();

                    MyFunaction.MessageBoxSave();
                }
            }
            catch (Exception)
            {
                User_layoutControlGroup43.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
                return;
            }
            User_layoutControlGroup43.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        private void TblUsersBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (tblUsersBindingSource.Current != null)
            {
                tblUser Cus = tblUsersBindingSource.Current as tblUser;
                if (Cus.ID > 0)
                {
                    User_layoutControlGroup43.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
                }
                else if (Cus.ID == 0)
                {
                    User_layoutControlGroup43.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
                }
            }
        }


        private void UserGroupSavebtn_Click(object sender, EventArgs e)
        {
            try
            {
                tblUserGroupBindingSource.EndEdit();
                if (tblUserGroupBindingSource.Current != null)
                {
                    tblUserGroup tblUserGroupss = tblUserGroupBindingSource.Current as tblUserGroup;
                    int users = Session.tblUserGroup.Where(u => u.UserGroupName == tblUserGroupss.UserGroupName).Count();
                    if (users > 0 & tblUserGroupss.ID == 0)
                    {
                        MyFunaction.MessageBoxAddGroupUser();
                        RefrechUser();
                        return;
                    }
                    new Data.Repository<tblUserGroup>().Save(tblUserGroupss, m => m.ID == tblUserGroupss.ID, tblUserGroupss.ID == 0);
                    RefrechUser();
                    MyFunaction.MessageBoxSave();
                }
            }
            catch (Exception)
            {
                UserGroup_layoutControlGroup41.Enabled = tblPermissionWashType2.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
                return;
            }
         
         
                 }
        private void TblUserGroupBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            //RefreshForPermission();
            if (tblUserGroupBindingSource.Current != null)
            {
                tblUserGroup Cus = tblUserGroupBindingSource.Current as tblUserGroup;
                if (Cus.ID > 0)
                {
                    UserGroup_layoutControlGroup41.Enabled = tblPermissionWashType2.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
                }
                else if (Cus.ID == 0)
                {
                    UserGroup_layoutControlGroup41.Enabled = tblPermissionWashType2.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
                }
            }
        }
        private void UserGroupDelete_Click(object sender, EventArgs e)
        {
            if (tblUserGroupBindingSource.Current != null)
            {
                tblUserGroup UserGroup = tblUserGroupBindingSource.Current as tblUserGroup;
                var UserState = Classes.Session.tblUser.Where(u => u.UserGroupID == UserGroup.ID).Select(u => u.UserState);
                if (UserState.Contains("Admin"))
                {
                    MyFunaction.MessageBoxDeleteUserGroupAdmin();
                    return;
                }
                if (MyFunaction.MessageBoxDelete() == DialogResult.Yes)
                {
                    tblUserGroupBindingSource.EndEdit();
                    new Data.Repository<tblUserGroup>().Delete(m => m.ID == UserGroup.ID);
                    RefrechUser();
                }
            }
        }
        private void UserGroup_print_Click(object sender, EventArgs e)
        {
            UserGroupgridControl8.ShowPrintPreview();
        }

        private void UsersDelete_Click(object sender, EventArgs e)
        {
            if (tblUsersBindingSource.Current != null)
            {
                tblUser User = tblUsersBindingSource.Current as tblUser;
                if (User.UserState == "Admin")
                {
                    MyFunaction.MessageBoxDeleteUserAdmin();
                    return;
                }
                if (MyFunaction.MessageBoxDelete() == DialogResult.Yes)
                {
                    tblUsersBindingSource.EndEdit();
                    new Data.Repository<tblUser>().Delete(m => m.ID == User.ID);
                    RefrechUser();
                }
            }
        }
        private void Users_print_Click(object sender, EventArgs e)
        {
            UsersgridControl1.ShowPrintPreview();
        }
        private void AddNewUserGroup_Click(object sender, EventArgs e)
        {
            tblUserGroup tblUserGroup = tblUserGroupBindingSource.Current as tblUserGroup;
            tblUserGroup.EnterTime = DateTime.Now;
            tblUserGroup.BranchID = Program.User.BranchID;
            tblUserGroup.UserID = Program.User.ID;
        }
        private void AddNewUser_Click(object sender, EventArgs e)
        {
            tblUser tblUser = tblUsersBindingSource.Current as tblUser;
            tblUser.EnterTime = DateTime.Now;
            tblUser.BranchID = Program.User.BranchID;
            tblUser.UserID = Program.User.ID;
        }

        private void XtraFormUsers_Load(object sender, EventArgs e)
        {
            RefrechUser();
        }
    }
}