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
    public partial class XtraFormGomashType : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblSellInvoice sale;
        string CustName;
        public XtraFormGomashType(tblSellInvoice cus,string CustomerName)
        {
            InitializeComponent();
            sale = cus;
            CustName = CustomerName;
        }
        public void RefrechData()
        {
            tblClasseBindingSource.DataSource = Session.tblClasses.ToList();
            using (var db = new DataClasses1DataContext(Program.ConnectionString))
            {
                tblSellInvoiceDetaileBindingSource.DataSource = db.tblSellInvoiceDetailes.Where(f => f.InvoNumber == sale.InvoNumber & f.BranchID == sale.BranchID).ToList();
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            if (tblSellInvoiceDetaileBindingSource.Current != null)
            {
                tblSellInvoiceDetaileBindingSource.EndEdit();
                tblSellInvoiceDetaile pay = tblSellInvoiceDetaileBindingSource.Current as tblSellInvoiceDetaile;
                string q= gridView2.Columns["NumClothes"].SummaryItem.SummaryValue.ToString();
                if (q!="")
                {
                    if (sale.TheQuantity <int.Parse(q))
                    {
                        XtraMessageBox.Show("يوجد خطاء في عدد الثياب");
                        return;
                    }
                }
                using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    foreach (tblSellInvoiceDetaile tblSaleDetail in  tblSellInvoiceDetaileBindingSource.List)
                    {
                        db. tblSellInvoiceDetailes.DeleteAllOnSubmit(db.tblSellInvoiceDetailes.Where(m => m.ID == tblSaleDetail.ID));
                        db. tblSellInvoiceDetailes.InsertOnSubmit(tblSaleDetail);
                    }
                    db.SubmitChanges();
                }
                MyFunaction.MessageBoxSave();
                Close();
            }
        }
        private void AddNew_Click(object sender, EventArgs e)
        {
           tblSellInvoiceDetaileBindingSource.AddNew();
            tblSellInvoiceDetaile tblSellInvoiceDetaile = tblSellInvoiceDetaileBindingSource.Current as tblSellInvoiceDetaile;
            tblSellInvoiceDetaile.EnterTime = DateTime.Now;
            tblSellInvoiceDetaile.BranchID = Program.User.BranchID;
            tblSellInvoiceDetaile.UserID = Program.User.ID;
            tblSellInvoiceDetaile.InvoNumber = sale.InvoNumber;
        }
        private void XtraFormTafseel_Load(object sender, EventArgs e)
        {
            tblSellInvoicesBindingSource.DataSource = sale;
            RefrechData();
            txtCustomerName.Text = CustName;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}