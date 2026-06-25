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
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraEditors.Repository;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;
using System.Collections.ObjectModel;
using System.Threading;
using DevExpress.Utils.Extensions;

namespace SewingSystem.Forms
{

    public partial class XtraFormBuyInvoice : FormMaster
    {
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        public XtraFormBuyInvoice()
        {
            InitializeComponent();
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "شاشة التوريد" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            btnAddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add).TheValues.Value;
            btnDelete.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Delete).TheValues.Value;
            btnUpdate.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update).TheValues.Value;
        }

        private void XtraFormSaleInvoice_Load(object sender, EventArgs e)
        {
            if (Program.Branch.UseTax ?? false)
                ItemForTheTax.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            else
                ItemForTheTax.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            spn_Paid.EditValueChanging += Spn_Paid_EditValueChanging;
            spn_DiscountValue.EditValueChanging += Spn_DiscountValue_EditValueChanging;
            TheTaxTextEdit.EditValueChanging += TheTaxTextEdit_EditValueChanging;
            gridView1.CustomUnboundColumnData += GridView1_CustomUnboundColumnData;
            repoItems1.ImmediatePopup = true;
            repoItems1.ButtonClick += RepoItems_ButtonClick;
            repositoryTheNameAr.ImmediatePopup = true;
            repositoryTheNameAr.ButtonClick += RepoItems_ButtonClick;
            gridView1.CellValueChanging += GridView1_CellValueChanging;
            gridView1.RowUpdated += GridView1_RowUpdated;
            RefreshData();
            btnAddNew.PerformClick();
        }
        tblBuyInvoice CurrBuyInvoice => tblBuyInvoiceBindingSource.Current as tblBuyInvoice;

        private void TheTaxTextEdit_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (CurrBuyInvoice == null) return;
                if (e.NewValue.ToString() != "")
                    CurrBuyInvoice.TheTax = double.Parse(e.NewValue.ToString());
                CurrBuyInvoice.TotalAftDis = (CurrBuyInvoice.InvoValue - CurrBuyInvoice.Discount);
                CurrBuyInvoice.TheSafy = (CurrBuyInvoice.TotalAftDis + CurrBuyInvoice.TheTax);
        }
        private void GridView1_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Index")
                e.Value = gridView1.GetVisibleRowHandle(e.ListSourceRowIndex) + 1;
        }
        private void GridView1_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            tblBuyInvoiceDetaile row = gridView1.GetRow(e.RowHandle) as tblBuyInvoiceDetaile;
            switch (e.Column.FieldName)
            {
                case nameof(row.ClassNumber):
                    row.ClassNumber = (e.Value as String) ?? "";
                    var p1 = Session.tblClasses.Where(f => f.ClassNumber == row.ClassNumber & f.BranchID == row.BranchID).ToList();
                    if (p1.Count() > 0)
                    {
                        if (p1[0].Price != null)
                            row.Price = p1[0].Price.Value;
                        if (row.TheQuantity == 0)
                            row.TheQuantity = 1;
                        GridView1_CellValueChanging(sender, new CellValueChangedEventArgs(e.RowHandle, gridView1.Columns[nameof(row.Price)], row.Price));
                        if (gridView1.FocusedColumn.FieldName == "ClassNumber" & gridView1.IsLastVisibleRow)
                            MoveFocuseToGrid();
                    }
                    break;
                case nameof(row.Price):
                    if (e.Value != null)
                    {
                        if (e.Value.ToString() != "")
                            row.Price = double.Parse(e.Value.ToString());
                        else
                            row.Price = 0;
                    }
                    else
                        row.Price = 0;
                    row.Total = (row.TheQuantity) * (row.Price /*+ row.SaleTax*/);
                    GridView1_CellValueChanging(sender, new CellValueChangedEventArgs(e.RowHandle, gridView1.Columns[nameof(row.Total)], row.Total));
                    break;
                case nameof(row.TheQuantity):
                    if (e.Value.ToString() != "")
                        row.TheQuantity = double.Parse(e.Value.ToString());
                    else
                        row.TheQuantity = 1;
                    row.Total = (row.TheQuantity) * (row.Price/*+row.SaleTax*/);
                    GridView1_CellValueChanging(sender, new CellValueChangedEventArgs(e.RowHandle, gridView1.Columns[nameof(row.Total)], row.Total));
                    break;
                default:
                    break;
            }
            gridView1.RefreshData();
            GridView1_RowUpdated(sender, null);
        }

        private void GridView1_RowUpdated(object sender, RowObjectEventArgs e)
        {
            if (CurrBuyInvoice == null) return;
            var items = tblBuyInvoiceDetaileBindingSource.List as List<tblBuyInvoiceDetaile>;
            if (items.Count() == 0)
            {
                CurrBuyInvoice.InvoValue = 0;
                CurrBuyInvoice.Discount = 0;
                CurrBuyInvoice.TheQuantity = 0;
            }
            else
            {
                CurrBuyInvoice.TheQuantity = items.Sum(x => x.TheQuantity);
                CurrBuyInvoice.InvoValue = items.Sum(x => x.Total);
                var discount = CurrBuyInvoice.Discount ?? 0;
                CurrBuyInvoice.TotalAftDis = (CurrBuyInvoice.InvoValue - CurrBuyInvoice.Discount);
                CurrBuyInvoice.TheSafy = (CurrBuyInvoice.TotalAftDis + CurrBuyInvoice.TheTax);
                if (CurrBuyInvoice.ThePay == Program.Pay_Deferred)
                    CurrBuyInvoice.MonyRemin = CurrBuyInvoice.TheSafy - CurrBuyInvoice.MonyPay;
                else
                {
                    CurrBuyInvoice.MonyPay = CurrBuyInvoice.TheSafy;
                    CurrBuyInvoice.MonyRemin = 0;
                }
            }
        }

        private void ButtonDelete_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            GridView view = ((GridControl)((ButtonEdit)sender).Parent).MainView as GridView;
            if (view.FocusedRowHandle >= 0)
            {
                view.DeleteSelectedRows();
                GridView1_RowUpdated(sender, null);
            }
            if (view.RowCount <= 0)
            {
                ButtonAdd_ButtonClick(sender, null);
                GridView1_RowUpdated(sender, null);
            }
        }
        private void ButtonAdd_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            MoveFocuseToGrid();
        }
        private void RepoItems_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind != ButtonPredefines.Plus) return;
            MyFunaction.OpenForm(new UC_Class(), "الاصناف");// new XtraFormClass().ShowDialog();
                tblClassesBindingSource.DataSource = Session.tblClasses.ToList();
        }
        private void Spn_DiscountValue_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            if (CurrBuyInvoice != null)
            {
                if (e.NewValue.ToString() != "")
                    CurrBuyInvoice.Discount = double.Parse(e.NewValue.ToString());
                CurrBuyInvoice.TotalAftDis = CurrBuyInvoice.InvoValue - CurrBuyInvoice.Discount;
                CurrBuyInvoice.TheSafy = CurrBuyInvoice.TotalAftDis + CurrBuyInvoice.TheTax;
            }
        }
        IList<tblBuyInvoiceDetaile> GetInvoiceDetailes => (tblBuyInvoiceDetaileBindingSource.List as IList<tblBuyInvoiceDetaile>).Where(x => x.ClassNumber != null)
                     .ToList().Select(x => new tblBuyInvoiceDetaile
                     {
                         BranchID = Program.Branch.ID,
                         ClassNumber = x.ClassNumber,
                         EnterTime = DateTime.Now,
                         ID = x.ID,
                         Notes = x.Notes,
                         UserID = Program.User.ID,
                         ParentID= CurrBuyInvoice.ID,
                         Price = x.Price,
                         SaleTax = x.SaleTax,
                         TheQuantity = x.TheQuantity,
                         Total= x.Total,
                         UnitID=x.UnitID,
                     }).ToList();
        public override void Save()
        {
            if (CurrBuyInvoice == null) return;
            try
            {
                flyDialog.WaitForm(this, 1);
                CurrBuyInvoice.TheQuantity = (gridView1.Columns["TheQuantity"].SummaryItem.SummaryValue as double?) ?? 0;
                using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    if (IsNew)
                    {
                        if (!Session.tblBuyInvoice.Any(s => s.BranchID == Program.User.BranchID))
                            CurrBuyInvoice.InvoNumber = 1;
                        else
                            CurrBuyInvoice.InvoNumber = db.tblBuyInvoices.Where(s => s.BranchID == Program.User.BranchID).Max(i => i.InvoNumber) + 1;
                        db.tblBuyInvoices.InsertOnSubmit(CurrBuyInvoice);
                        db.SubmitChanges();
                    }
                    else
                    {
                        db.tblBuyInvoices.DeleteAllOnSubmit(db.tblBuyInvoices.Where(m => m.ID == CurrBuyInvoice.ID));
                        db.tblBuyInvoices.InsertOnSubmit(CurrBuyInvoice);
                        db.tblBuyInvoiceDetailes.DeleteAllOnSubmit(db.tblBuyInvoiceDetailes.Where(m => m.ParentID == CurrBuyInvoice.ID));
                        var det = Session.tblBuyInvoiceDetaile.Where(x => x.ParentID == CurrBuyInvoice.ID).ToList();
                        det.ForEach(x => Session.tblBuyInvoiceDetaile.Remove(x));
                    }
                    db.tblBuyInvoiceDetailes.InsertAllOnSubmit(GetInvoiceDetailes);
                    db.SubmitChanges();
                }
                string mssg = $"الفاتورة رقم: {CurrBuyInvoice?.InvoNumber} ";
                if (Session.tblBuyInvoice.Any(v => v.ID == CurrBuyInvoice.ID))
                {
                    int index = Session.tblBuyInvoice.IndexOf(Session.tblBuyInvoice.Single(x => x.ID == CurrBuyInvoice.ID));
                    Session.tblBuyInvoice.Remove(Session.tblBuyInvoice.Single(x => x.ID == CurrBuyInvoice.ID));
                    Session.tblBuyInvoice.Insert(index, CurrBuyInvoice);
                }
                else
                    Session.tblBuyInvoice.Add(CurrBuyInvoice);
                GetInvoiceDetailes.ForEach(x => Session.tblBuyInvoiceDetaile.Add(x));
                spn_Paid.ReadOnly = true;
                flyDialog.WaitForm(this, 0);
                flyDialog.ShowDialogForm(this, mssg, this.IsNew);
                TextReadOnly(true);
            }
            catch (Exception ex)
            {
                flyDialog.WaitForm(this, 0);
                XtraMessageBox.Show(ex.Message);
                return;
            }
            base.Save();
        }
        public override void EnableOrDisyble(bool state)
        {
            SearchInvoiceIDtxtedit.Enabled = state;
            SearchInvoiceIDtxtedit.ReadOnly = !state;
            base.EnableOrDisyble(state);
        }
        public override void TextReadOnly(bool state)
        {
            gridView1.OptionsBehavior.Editable = !state;
            gridView1.OptionsBehavior.ReadOnly = state;
            base.TextReadOnly(state);
        }
        public override void New()
        {
            if (!Session.tblClasses.Any())
            {
                XtraMessageBox.Show("لا يمكن اضافة توريد جديد الا بعد اضافة الاصناف!!!");
                return;
            }
            if (!Session.tblBuyInvoice.Any(s => s.BranchID == Program.User.BranchID))
                CurrBuyInvoice.InvoNumber = 1;
            else
                CurrBuyInvoice.InvoNumber = Session.tblBuyInvoice.Where(s => s.BranchID == Program.User.BranchID).Max(i => i.InvoNumber) + 1;
            CurrBuyInvoice.Discount = CurrBuyInvoice.TotalAftDis = 
            CurrBuyInvoice.TheTax = CurrBuyInvoice.TheSafy = CurrBuyInvoice.InvoValue = 
            CurrBuyInvoice.MonyPay =  CurrBuyInvoice.MonyRemin = CurrBuyInvoice.TheQuantity = 0;
            CurrBuyInvoice.EnterTime = CurrBuyInvoice.TheDate = DateTime.Now;
            IDTextEdit.EditValue = CurrBuyInvoice.InvoNumber;
            CurrBuyInvoice.BranchID = Program.Branch.ID;
            CurrBuyInvoice.UserID = Program.User.ID;
            MoveFocuseToGrid();
            spn_Paid.ReadOnly = false;
            base.New();
        }

        #region invpura
        private void Spn_Paid_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (CurrBuyInvoice == null) return;
            var net = CurrBuyInvoice.TheSafy?? 0;
            if (e.NewValue.ToString() != "")
                CurrBuyInvoice.MonyPay = double.Parse(e.NewValue.ToString());
            if (CurrBuyInvoice.MonyPay > CurrBuyInvoice.TheSafy)
            {
                spn_Paid.EditValue = CurrBuyInvoice.MonyPay;
                CurrBuyInvoice.MonyPay = CurrBuyInvoice.TheSafy;
            }
            CurrBuyInvoice.MonyRemin = net - CurrBuyInvoice.MonyPay;
            if (CurrBuyInvoice.MonyRemin > 0)
                CurrBuyInvoice.ThePay = Program.Pay_Deferred;
            else if (CurrBuyInvoice.MonyRemin == 0 & CurrBuyInvoice.MonyPay > 0)
                CurrBuyInvoice.ThePay = Program.Pay_Cash;
        }
        private void tblBuyInvoiceBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (CurrBuyInvoice == null) return;
            setDetail();
        }
        MyFunaction MyFunaction = new MyFunaction();
        private void Delete_Click(object sender, EventArgs e)
        {
            if (CurrBuyInvoice == null) return;
            if (MyFunaction.MessageBoxDelete() != DialogResult.Yes) return;
            try
            {
                flyDialog.WaitForm(this, 1);
                string mssg = $"الفاتورة رقم: {CurrBuyInvoice?.InvoNumber} ";
                using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    db.tblBuyInvoices.DeleteAllOnSubmit(db.tblBuyInvoices.Where(m => m.ID == CurrBuyInvoice.ID));
                    db.SubmitChanges();
                    Session.tblBuyInvoice.Remove(Session.tblBuyInvoice.Single(x => x.ID == CurrBuyInvoice.ID));
                    Session.tblBuyInvoiceDetaile.Where(x => x.ParentID == CurrBuyInvoice.ID).ToList().ForEach(x => Session.tblBuyInvoiceDetaile.Remove(x));
                    setDetail();
                }
                flyDialog.WaitForm(this, 0);
                flyDialog.ShowDialogDeleteForm(this, mssg);
            }
            catch (Exception ex)
            {
                flyDialog.WaitForm(this, 0);
                XtraMessageBox.Show(ex.Message);
            }
        }
       
        #endregion
        public override void Reset()
        {
            base.Reset();
            GridView1_RowUpdated(null, null);
        }
        private void SearchInvoiceIDtxtedit_EditValueChanged(object sender, EventArgs e)
        {
            if (SearchInvoiceIDtxtedit.EditValue == null) return;
            if (tblBuyInvoiceBindingSource.Count > 0)
            {
                tblBuyInvoiceBindingSource.Position = tblBuyInvoiceBindingSource.IndexOf(SearchInvoiceIDtxtedit.GetSelectedDataRow());
                dataLayoutControl1.Refresh();
            }
            SearchInvoiceIDtxtedit.EditValue = null;
        }

        public override void RefreshData()
        {
            tblBranchesBindingSource.DataSource = Session.tblBranche.ToList();
            tblBuyInvoiceBindingSource.DataSource = Session.tblBuyInvoice.ToList(); 
            tblUsersBindingSource.DataSource = Session.tblUser.ToList();
            tblClassesBindingSource.DataSource = Session.tblClasses.ToList();
            setDetail();
            base.RefreshData();
        }
        void setDetail()
        {
            tblBuyInvoiceDetaileBindingSource.DataSource = Session.tblBuyInvoiceDetaile.Where(d => d.ParentID == CurrBuyInvoice.ID).Select(x => new tblBuyInvoiceDetaile
            {
                BranchID = x.BranchID,
                ClassNumber = x.ClassNumber,
                EnterTime =x.EnterTime,
                ID = x.ID,
                Notes = x.Notes,
                UserID =x.UserID,
                ParentID = CurrBuyInvoice.ID,
                Price = x.Price,
                SaleTax = x.SaleTax,
                TheQuantity = x.TheQuantity,
                Total = x.Total,
                UnitID = x.UnitID,
            }).ToList();
        }

        void MoveFocuseToGrid()
        {
            this.gridControl1.Focus();
            gridView1.FocusedRowHandle = GridControl.InvalidRowHandle;
            gridView1.FocusedColumn = gridView1.Columns["ClassNumber"];
            tblBuyInvoiceDetaileBindingSource.AddNew();
            gridView1.UpdateCurrentRow();
            tblBuyInvoiceDetaile v = tblBuyInvoiceDetaileBindingSource.Current as tblBuyInvoiceDetaile;
            v.TheQuantity = 0;
            v.Price = 0;
            v.BranchID = Program.Branch.ID;
        }

    }
}