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
    public partial class XtraFormSanadSarf : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        public XtraFormSanadSarf()
        {
            InitializeComponent();
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "المشتريات" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            AddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            Delete.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Delete)?.TheValues??false;
            Print.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues??false;
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        public void RefrechData()
        {
            tblBranchesBindingSource.DataSource = Session.tblBranche.ToList();
            tblSuppliersBindingSource.DataSource = Session.tblSupplier.ToList();
            tblUsersBindingSource.DataSource = Session.tblUser.ToList();
            tblSanadSarfBindingSource.DataSource = Session.tblSanadSarf.ToList();
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        #region SanadSarfs
        private void AddNew_Click(object sender, EventArgs e)
        {
            CurrSanadSarf.EnterTime = DateTime.Now;
            CurrSanadSarf.TheDate = DateTime.Now;
            CurrSanadSarf.BranchID = Program.User.BranchID;
            CurrSanadSarf.UserID = Program.User.ID;
            isNew = true;
            if (!Session.tblSanadSarf.Any(s => s.BranchID == Program.User.BranchID))
                    CurrSanadSarf.SanadID ="1";
            else
                CurrSanadSarf.SanadID = (Session.tblSanadSarf.Where(s => s.BranchID == Program.User.BranchID).Max(i =>int.Parse(i.SanadID)) + 1).ToString();

        }
        ComponentFlyoutDialog flyDialog = new ComponentFlyoutDialog();
        bool isNew=false;
        tblSanadSarf CurrSanadSarf => tblSanadSarfBindingSource.Current as tblSanadSarf;
        private void Save_Click(object sender, EventArgs e)
        {
            if (CurrSanadSarf == null)
                return; 
            flyDialog.WaitForm(this, 1);
            string mssg = $"المشتريات رقم : {CurrSanadSarf?.SanadID} ";
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                if (db.tblSanadSarfs.Where(l => l.SanadID == CurrSanadSarf.SanadID & l.ID != CurrSanadSarf.ID).Count() > 0)
                {
                    TheNameTextEdit.ErrorText = "رقم الشراء اليدوي موجود من قبل اختار رقم سند اخر!!!!";
                    return;
                }
                db.tblSanadSarfs.DeleteAllOnSubmit(db.tblSanadSarfs.Where(m => m.ID == CurrSanadSarf.ID));
                db.tblSanadSarfs.InsertOnSubmit(CurrSanadSarf);
                db.SubmitChanges();
                if (Session.tblSanadSarf.Where(x => x.ID == CurrSanadSarf.ID).Count() > 0)
                {
                    int index = Session.tblSanadSarf.IndexOf(Session.tblSanadSarf.Single(x => x.ID == CurrSanadSarf.ID));
                    Session.tblSanadSarf.Remove(Session.tblSanadSarf.Single(x => x.ID == CurrSanadSarf.ID));
                    Session.tblSanadSarf.Insert(index, CurrSanadSarf);
                }
                else if (Session.tblSanadSarf.Where(v => v.ID == CurrSanadSarf.ID).Count() <= 0)
                    Session.tblSanadSarf.Add(CurrSanadSarf);
                RefrechData();
            }
            flyDialog.WaitForm(this, 0);
            flyDialog.ShowDialogForm(this, mssg, this.isNew);
            isNew = false;
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues ?? false;
        }
        private void tblsanadmBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (CurrSanadSarf == null)
                return;
            if (Session.tblSanadSarf.Any(i => i.ID == CurrSanadSarf.ID))
                UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues ?? false;
            else 
                UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues ?? false;
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (CurrSanadSarf == null)
                return;
            if (MyFunaction.MessageBoxDelete() != DialogResult.Yes)
                return;
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                db.tblSanadSarfs.DeleteAllOnSubmit(db.tblSanadSarfs.Where(m => m.ID == CurrSanadSarf.ID));
                db.SubmitChanges();
                Session.tblSanadSarf.Remove(Session.tblSanadSarf.Single(x => x.ID == CurrSanadSarf.ID));
                RefrechData();
            }
         
        }
     
        #endregion
        Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
        public void F_Print(DevExpress.XtraReports.UI.XtraReport Report)
        {
            frmReport.documentViewer1.DocumentSource = Report;
            frmReport.ShowDialog();
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            RefrechData();
        }
        private void XtraFormSanadm_Load(object sender, EventArgs e)
        {
            RefrechData();
        }

    }
}