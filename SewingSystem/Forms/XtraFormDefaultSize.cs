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
    public partial class XtraFormDefaultSize : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        int CusNum;
        public XtraFormDefaultSize(int CusNumber)
        {
            InitializeComponent();
            CusNum = CusNumber;
            //Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "ترميز المقاسات" & p.UserGroupID == Program.User.UserGroupID);
            //tblPermissionWashType.Clear();
            //tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            //AddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            //Print.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues??false;
            //Delete.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Delete)?.TheValues??false;
            //UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
        }
        public void RefrechData()
        {
            Session.GetDataDefaultSize();
            tblDefaultSizeBindingSource.DataSource = Session.tblDefaultSize.Where(f => f.CusNumber == CusNum & f.BranchID == Program.Branch.ID).ToList();
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            RefrechData();
        }
        private void AddNew_Click(object sender, EventArgs e)
        {
            tblDefaultSizeBindingSource.AddNew();
            tblDefaultSize tblDefaultSize = tblDefaultSizeBindingSource.Current as tblDefaultSize;
                tblDefaultSize.EnterTime = DateTime.Now;
                tblDefaultSize.BranchID = Program.User.BranchID;
                tblDefaultSize.UserID = Program.User.ID;
                tblDefaultSize.CusNumber = CusNum;
        }
        private void Save_Click(object sender, EventArgs e)
        {
            if (tblDefaultSizeBindingSource.Current != null)
            {
                tblDefaultSizeBindingSource.EndEdit();
                tblDefaultSize DefaultSize = tblDefaultSizeBindingSource.Current as tblDefaultSize;
                new Data.Repository<tblDefaultSize>().Save(DefaultSize, m => m.ID == DefaultSize.ID, DefaultSize.ID == 0);
                RefrechData();
                MyFunaction.MessageBoxSave();
            }
        }
        private void Print_Click(object sender, EventArgs e)
        {
            dataLayoutControl1.ShowPrintPreview();
        }

        Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
        public void PrintReport(DevExpress.XtraReports.UI.XtraReport Report)
        {
            frmReport.documentViewer1.DocumentSource = Report;
            frmReport.ShowDialog();
        }

        private void XtraFormDefaultSizes_Load(object sender, EventArgs e)
        {
            tblCustomerBindingSource.DataSource = Session.tblCustomer.Where(c => c.CusNumber == CusNum & c.BranchID == Program.Branch.ID).ToList();
            RefrechData();
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void Delete_Click(object sender, EventArgs e)
        {
            if (tblDefaultSizeBindingSource.Current != null)
            {
                if (MyFunaction.MessageBoxDelete() == DialogResult.Yes)
                {
                    tblDefaultSize Size = tblDefaultSizeBindingSource.Current as tblDefaultSize;
                    tblDefaultSizeBindingSource.EndEdit();
                    new Data.Repository<tblDefaultSize>().Delete(m => m.ID == Size.ID);
                    RefrechData();
                }
            }
        }
    }
    
}