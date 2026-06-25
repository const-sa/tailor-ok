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
using SewingSystem.LinqModel;
using SewingSystem.Classes;
using System.Threading;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Views.Layout;
using System.Collections.ObjectModel;
using System.Globalization;
using SewingSystem.Report;

namespace SewingSystem.Forms
{
    public partial class XtraFormCreditNote : DevExpress.XtraEditors.XtraForm
    {
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        public XtraFormCreditNote()
        {
            InitializeComponent();
            this.Load += XtraFormInvoices_Load;
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "اشعارات دائنه ومدينه" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            AddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues ?? false;
            PrintDirect.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues ?? false;
            PrintInvoice.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues ?? false;
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues ?? false;
        }
        private void XtraFormInvoices_Load(object sender, EventArgs e)
        {
            spn_BeforTaxDayn.EditValueChanging += Spn_Tax_EditValueChanging1;
            spn_BeforTaxMaden.EditValueChanging += Spn_Tax_EditValueChanging1;
            txtTotal.EditValueChanging += Spn_Tax_EditValueChanging1;
            tblNoteBindingSource.CurrentChanged += tblNoteBindingSource_CurrentChanged;
            PrintDirect.Click += PrintDirect_Click;
            RefrashAll();
            PrintInvoice.Visible = PrintDirect.Visible;
        }

        private void Spn_Tax_EditValueChanging1(object sender, ChangingEventArgs e)
        {
            if (this.CurrNote == null)
                return;
            if (e.NewValue.ToString() == "")
                return;
            switch (((TextEdit)sender).Name)
            {
                case "txtTotal" when radioGroup1.EditValue.ToString() == "مدين":
                    if (Program.Branch.UseTax ?? false)
                        CurrNote.TaxMaden = ((double.Parse(e.NewValue.ToString()) / Session.Defualts.TaxOperator) * Session.Defualts.TaxRate);
                    else
                        CurrNote.TaxMaden = 0;
                    CurrNote.Maden = double.Parse(e.NewValue.ToString()) - CurrNote.TaxMaden;
                    break;
                case "txtTotal" when radioGroup1.EditValue.ToString() == "دائن":
                    if (Program.Branch.UseTax ?? false)
                        CurrNote.TaxDayne = ((double.Parse(e.NewValue.ToString()) / Session.Defualts.TaxOperator) * Session.Defualts.TaxRate);
                    else
                        CurrNote.TaxDayne = 0;
                    CurrNote.Dayne = double.Parse(e.NewValue.ToString()) - CurrNote.TaxDayne;
                    break;
                case "spn_BeforTaxMaden":
                    if (Program.Branch.UseTax ?? false)
                        CurrNote.TaxMaden = (double.Parse(e.NewValue.ToString()) * Session.Defualts.TaxRate);
                    else
                        CurrNote.TaxMaden = 0;
                    CurrNote.Total = double.Parse(e.NewValue.ToString()) + CurrNote.TaxMaden;
                    break;
                case "spn_BeforTaxDayn":
                    if (Program.Branch.UseTax ?? false)
                        CurrNote.TaxDayne = double.Parse(e.NewValue.ToString()) * Session.Defualts.TaxRate;
                    else
                        CurrNote.TaxDayne = 0;
                    CurrNote.Total = double.Parse(e.NewValue.ToString()) + CurrNote.TaxDayne;
                    break;
                default:
                    break;
            }
        }
        private void tblNoteBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (!this.IsActive || this.CurrNote == null)
                return;
            var sell = Session.tblSellInvoice.FirstOrDefault(f => f.InvoNumber == CurrNote.InvoNumber & f.BranchID == CurrNote.BranchID);
            if (sell!=null)
            {
                Allspn_BeforTax.Text = (sell.TotalAll - sell.TaxAll).ToString();
                txtTheTax.Text = sell.TaxAll.ToString();
                txtTheSafy.Text = sell.TotalAll.ToString();
            }
            var s = Session.tblNote.FirstOrDefault(i => i.ID == CurrNote.ID & i.BranchID == Program.Branch.ID);
            if (s!=null)
            {
                if ((s.HowPrint ?? 0) > 0)
                    DisaplyDeletAndEdit(false);
                else
                    DisaplyDeletAndEdit(tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues ?? false);
            }
            else 
                DisaplyDeletAndEdit(tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues ?? false);
        }

        public void DisaplyDeletAndEdit(bool state)
        {
            UpdateRecord.Enabled = state;
            gridView1.OptionsBehavior.ReadOnly = !state;
        }
        private void RefreachData()
        {
            if (this.IsActive & (!AddNew.Enabled | RefreachDeleteInvoice))
            {
                RefreachDeleteInvoice = false;
                if (Program.User.UserState == "Admin")
                    tblNoteBindingSource.DataSource = Session.tblNote.Where(c => c.BranchID == Program.User.BranchID).ToList();
            }
        }
        private void RefrechInvoice_Click(object sender, EventArgs e)
        {
            RefrashAll();
            AddNew.Enabled = true;
        }
        bool IsNew = true;
        ComponentFlyoutDialog flyDialog = new ComponentFlyoutDialog();
        private void Save_Click(object sender, EventArgs e)
        {
            if (CurrNote == null)
                return;
            if ((CurrNote.InvoNumber ?? 0) == 0)
            {
                XtraMessageBox.Show("قم باختيار رقم فاتورة البيع اولا");
                return;
            }
            else if (Session.tblSellInvoice.Where(i => i.InvoNumber == (CurrNote.InvoNumber ?? 0)).Count() >= 5)
            {
                XtraMessageBox.Show("عفوا لا يمكن التعديل على الفاتورة اكثر من خمس مرات");
                return;
            }
            try
            {
                flyDialog.WaitForm(this, 1);
                string mssg = $"اشعار الفاتورة رقم: {CurrNote?.InvoNumber} ";
                using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    if (!IsNew)
                        db.tblNotes.DeleteAllOnSubmit(db.tblNotes.Where(m => m.ID == CurrNote.ID));
                    db.tblNotes.InsertOnSubmit(CurrNote);
                    db.SubmitChanges();
                    if (Session.tblNote.Any(x => x.ID == CurrNote.ID))
                    {
                        int index = Session.tblNote.IndexOf(Session.tblNote.Single(x => x.ID == CurrNote.ID));
                        Session.tblNote.Remove(Session.tblNote.Single(x => x.ID == CurrNote.ID));
                        Session.tblNote.Insert(index, CurrNote);
                    }
                    else
                        Session.tblNote.Add(CurrNote);
                    tblSellInvoice s = db.tblSellInvoices.FirstOrDefault(g => g.InvoNumber == CurrNote.InvoNumber & g.BranchID == CurrNote.BranchID);
                   var listNot= db.tblNotes.Where(n => n.InvoNumber == s.InvoNumber & n.BranchID == s.BranchID).ToList();
                    s.TotalMaden = listNot.Sum(c => c.Maden??0);
                    s.TaxMaden = listNot.Sum(c => c.TaxMaden??0);
                    s.TotalDayn = listNot.Sum(c => c.Dayne??0);
                    s.TaxDayn = listNot.Sum(c => c.TaxDayne ?? 0);
                    s.TaxAll = s.TotalFattInvoice + s.TaxMaden - s.TaxDayn;
                    s.TotalAll = s.TotalMony + s.TotalMaden - s.TotalDayn + s.TaxAll;
                    s.MonyRemin = (s.TotalAll?? 0) - (s.MonyPay?? 0) - (s.MonyOrpon?? 0);
                    db.tblSellInvoices.DeleteAllOnSubmit(db.tblSellInvoices.Where(m => m.ID == s.ID));
                    db.tblSellInvoices.InsertOnSubmit(s);
                    db.SubmitChanges();
                    Session.RefreshDatatSellInvoice(s);
                    RefreachData();
                }
                SaveSuccess = true;
                AddNew.Enabled = true;
                tblSellInvoice PrintInvo = Session.tblSellInvoice.FirstOrDefault(v => v.InvoNumber == CurrNote.InvoNumber & v.BranchID == CurrNote.BranchID);
                if (((ToolStripButton)sender).Name == "SaveAndPrint")
                {
                    Program.PrintMode = "Direct";
                    Task.Run(() => MyFunaction.FunactionPrintInvoice(PrintInvo));
                }
                flyDialog.WaitForm(this, 0);
                flyDialog.ShowDialogForm(this, mssg, this.IsNew);
                IsNew = false;
                if (PrintInvo != null)
                {
                    gridControl1.DataSource = Session.tblNote.Where(c => c.InvoNumber == (CurrNote.InvoNumber ?? 0)).ToList();
                    Allspn_BeforTax.Text = (PrintInvo.TotalAll - PrintInvo.TaxAll).ToString();
                    txtTheTax.Text = PrintInvo.TaxAll.ToString();
                    txtTheSafy.Text = PrintInvo.TotalAll.ToString();
                }
            }
            catch (Exception ex)
            {
                flyDialog.WaitForm(this,0);
                XtraMessageBox.Show(ex.Message);
                return;
            }

        }
        private void SaveAndPrint_Click(object sender, EventArgs e)
        {
            Save_Click(sender, e);
            if (SaveSuccess)
            {
                SaveSuccess = false;
                AddNew.PerformClick();
            }
        }
        public void ButtonPrint()
        {
            if (CurrNote == null)
                return;
            string mess = Session.LangEng ? "Sorry, Save the bill first!!!!" : "عفوا قم بحفظ الاشعار اولا !!!!!";
            if (Session.tblNote.Any(i => i.ID == CurrNote.ID & i.BranchID == CurrNote.BranchID))
            {
                tblSellInvoice PrintInvo = Session.tblSellInvoice.Where(v => v.InvoNumber == CurrNote.InvoNumber & v.BranchID == CurrNote.BranchID).FirstOrDefault();
                Task.Run(() => MyFunaction.FunactionPrintInvoice(PrintInvo));
                DisaplyDeletAndEdit(false);
            }
            else
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, mess, "Save", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        private void PrintInvoice_Click(object sender, EventArgs e)
        {
            IsNew = false;
            Program.PrintMode = "ShowDialog";
            ButtonPrint();
        }

        private void AddNew_Click(object sender, EventArgs e)
        {
            IsNew = true;
            SaveSuccess = false;
            FunctionAddNewInvoice();
        }
        bool SaveSuccess = false;
        bool RefreachDeleteInvoice = false;
        public void FunctionAddNewInvoice()
        {
            tblNoteBindingSource.AddNew();
            Allspn_BeforTax.ResetText();
            txtTheTax.ResetText();
            txtTheSafy.ResetText();
            AddNew.Enabled = false;
            CurrNote.EnterTime = DateTime.Now;
            CurrNote.NoteDate = DateTime.Now;
            CurrNote.UserID = Program.User.ID;
            CurrNote.BranchID = Program.Branch.ID;
            CurrNote.TheQuantity = 0;
            CurrNote.Dayne = 0;
            CurrNote.Maden = 0;
            CurrNote.TaxDayne = 0;
            CurrNote.TaxMaden = 0;
            CurrNote.Total = 0;
            CurrNote.EnterTime = DateTime.Now;
            CurrNote.TheType = "مدين";
        }
        private void PrintDirect_Click(object sender, EventArgs e)
        {
            Program.PrintMode = "Direct";
            ButtonPrint();
        }
        tblNote CurrNote => tblNoteBindingSource.Current as tblNote;
        public void RefrashAll()
        {
            IsNew = false;
            tblBranchesBindingSource.DataSource = Session.tblBranche.ToList();
            tblUsersBindingSource.DataSource = Session.tblUser.ToList();
            tblNoteBindingSource.DataSource = Session.tblNote.ToList();
            tblSellInvoiceBindingSource.DataSource = Session.tblSellInvoice.ToList();
            if (CurrNote == null)
                return;
            var sell = Session.tblSellInvoice.FirstOrDefault(s => s.InvoNumber == CurrNote.InvoNumber& s.BranchID == CurrNote.BranchID);
            if (sell!=null)
            {
                Allspn_BeforTax.Text = (sell.TotalAll - sell.TaxAll).ToString();
                txtTheTax.Text = sell.TaxAll.ToString();
                txtTheSafy.Text = sell.TotalAll.ToString();
            }
        }

        private void SearchSaleInvoiceIDtxtedit_EditValueChanged(object sender, EventArgs e)
        {
            if (SearchSaleInvoiceIDtxtedit.EditValue == null || CurrNote == null || tblSellInvoiceBindingSource.Count <= 0)
                return;
            if (SearchSaleInvoiceIDtxtedit.GetSelectedDataRow() != null)
            {
                tblSellInvoice sale = SearchSaleInvoiceIDtxtedit.GetSelectedDataRow() as tblSellInvoice;
                gridControl1.DataSource = Session.tblNote.Where(c => c.InvoNumber == sale.InvoNumber).ToList();
                CurrNote.CusNumber = sale.CusNumber;
                CurrNote.InvoNumber = sale.InvoNumber;
            }
        }

        private void radioGroup1_EditValueChanged(object sender, EventArgs e)
        {
            if (radioGroup1.EditValue.ToString() == "مدين")
            {
                layoutControlGroupMaden.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                layoutControlGroupDayn.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            }
            else if (radioGroup1.EditValue.ToString() == "دائن")
            {
                layoutControlGroupMaden.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                layoutControlGroupDayn.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            }
        }


    }
}