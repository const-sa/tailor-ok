using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using SewingSystem.Classes;
using System;
using System.Collections.Generic;
using SewingSystem.LinqModel;

namespace SewingSystem.Forms
{
    public partial class UC_User : UC_Master2
    {
        public UC_User()
        {
            InitializeComponent();
            RefreshData();
            gridView1.CustomColumnDisplayText += GridView1_CustomColumnDisplayText;
        }
        public override void RefreshData()
        {
            using (var db=new  LinqModel.DataClasses1DataContext(Program.ConnectionString))
                RoleTbls = db.tblUserGroups.ToList();
            UserRoleIDLookUpEdit.IntializeData(RoleTbls);
            userBindingSource.DataSource = Session.tblUser/*.Select(x=>x.ShallowCopy())*/.ToList();
            Program.User = Session.tblUser.FirstOrDefault(x => x.ID == Program.User.ID);
            base.RefreshData();
        }
        List<tblUserGroup> RoleTbls = new List<tblUserGroup>();
        private void GridView1_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case nameof(tblUser.UserGroupID):
                    e.DisplayText = RoleTbls.FirstOrDefault(x => x.ID == Convert.ToInt16(e.Value))?.UserGroupName;
                    break;
                default:
                    break;
            }
        }
        public override void Save()
        {
            if (user == null) return;
            if (IsNew||user.UserPassword != oldPassword)
                user.UserPassword = BCrypt.Net.BCrypt.HashPassword(PasswordTextEdit.Text);
            base.Save();
        }
        string oldPassword;
        public override void DataUpdate()
        {
            oldPassword = PasswordTextEdit.Text;
            base.DataUpdate();
        }
        public static bool CheckIfObjectCanBeDeleted(int id)
        {
            using (var db = new  LinqModel.DataClasses1DataContext(Program.ConnectionString))
                return db.tblSellInvoices.Any(x => x.UserID == id)|| db.tblPayments.Any(x => x.UserID == id);
        }
        tblUser user => userBindingSource.Current as tblUser;
        public override void Delete()
        {
            if (user == null) return;
            if (!CheckIfObjectCanBeDeleted(user.ID))
                base.Delete();
            else
                XtraMessageBox.Show(text: "لا يمكن حذف هذا المستخدم حيث انه مستخدم في عمليات داخل النظام ",
                    caption: this.Text, buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);

        }
    }
}
