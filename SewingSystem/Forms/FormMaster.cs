using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraDataLayout;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using SewingSystem.Classes;
using DevExpress.XtraLayout;
using DevExpress.Utils.Extensions;
using DevExpress.Utils;
using DevExpress.Data.ODataLinq.Helpers;
using DevExpress.XtraTreeList;
using SewingSystem.Reports;

namespace SewingSystem.Forms
{
    public partial class FormMaster : DevExpress.XtraEditors.XtraForm
    {
        public BindingSource bindingSource
        {
            get { return bindingNavigator?.BindingSource; }
            set { bindingNavigator.BindingSource = value; }
        }
        private Type CurrType => bindingSource?.Current?.GetType();
        public FormMaster()
        {
            InitializeComponent();
        }
        TreeList _treeList;
        public TreeList treeList
        {
            get { return _treeList; }
            set { _treeList = value; }
        }
        GridControl _gridControl;
        public GridControl gridControl
        {
            get { return _gridControl; }
            set { _gridControl = value; }
        }
        DataLayoutControl _dataLayout;
        public DataLayoutControl dataLayout
        {
            get
            {
                return _dataLayout;
            }
            set
            {
                _dataLayout = value;
            }
        }
        LayoutControlGroup _GroupMain;
        public LayoutControlGroup GroupMain
        {
            get { return _GroupMain; }
            set { _GroupMain = value; }
        }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string SumNameEn { get; set; }
        public string SumNameAr { get; set; }
        BaseEdit nameEdit
        {
            get
            {
                try
                {
                    return dataLayout?.Items?.OfType<LayoutControlItem>().FirstOrDefault(x => x.Control is BaseEdit edit && edit?.DataBindings[0]?.BindingMemberInfo.BindingMember == "Name")?.Control as BaseEdit;

                }
                catch (Exception)
                {
                    return null;
                }
                      }
        }

        public bool IsNew;
        string MessgTextValidNo => !Session.LangEng ? $"الرقم {No} موجود من قبل !!!\n قم باختيار رقم اخر" : $"Number {No} already exists,\n choose another Number!!!";
        public string MessgTextDelete => !Session.LangEng ? $"هل انت متاكد من حذف  {NameAr} : {CurrName} ?" : $"Are you sure to delete {NameEn} : {CurrName} ?";
        public string MessgTextNoDelete => !Session.LangEng ? $"لا يمكن حذف  {NameAr} : {CurrName} حيث انه مستخدم في عمليات داخل النظام " : $"This {NameEn} : {CurrName} cannot be deleted as it is used in operations within the system.";
        public string MessgTextSave => !Session.LangEng ? $"{NameAr} : {CurrName} " : $"{NameEn} : {CurrName} ";

        public void DisableValidation()
        {
            dataLayout?.Controls?.OfType<BaseEdit>().ForEach(x => x.CausesValidation = false);
        }
        public void EnableValidation()
        {
            dataLayout?.Controls?.OfType<BaseEdit>().ForEach(x => x.CausesValidation = true);
        }
     
        public virtual void Save()
        {
            EnableOrDisyble(true);
        }
       
        public virtual void New()
        {
            EnableOrDisyble(false);
            TextReadOnly(false);
            IsNew = true;
            //InitBraAndUserForNew();
        }

        string CurrName => CurrType?.GetProperty("Name")?.GetValue(bindingSource?.Current)?.ToString();
        public void SetFont(Font font)
        {
            if (bindingNavigator != null)
            {
                bindingNavigator.Font = font;
                bindingNavigator.Items.OfType<ToolStripButton>().ForEach(x => x.Font = font);
            }
            this.Font = font;
            if (dataLayout != null)
            {
                dataLayout.Font = dataLayout.Appearance.Control.Font = font;
                dataLayout?.Items.OfType<LayoutControlItem>().Where(x => x.Control is BaseEdit).ForEach(y =>
                {
                    if (y.Control is BaseEdit edit)
                        edit.Properties.Appearance.Font = font;
                });
            }
            if (GroupMain != null)
                GroupMain.AppearanceGroup.Font = GroupMain.AppearanceItemCaption.Font = font;
            if (gridView == null) return;
            gridView.Columns.ForEach(x => x.AppearanceCell.Font = font);
            gridView.Appearance.HeaderPanel.Font = gridView.Appearance.Row.Font = gridView.Appearance.FooterPanel.Font = font;
        }
        void SetAppearance()
        {
            try
            {
                //GroupMain?.SetAppearanceGroup();
                //gridView?.SetAppearanceGridView();
                //treeList?.AppearanceTreeList();
                //dataLayout?.SetAppearanceDataLayoutControl();
                TextReadOnly(true);
            }
            catch (Exception)
            {

            }
        }
        GridView gridView => gridControl?.MainView as GridView;
        public ComponentFlyoutDialog flyDialog = new ComponentFlyoutDialog();
        public virtual void TextReadOnly(bool state)
        {
            if (dataLayout != null)
                dataLayout.OptionsView.IsReadOnly = state ? DefaultBoolean.True : DefaultBoolean.False;
        }
        string NameNo = "Number";
        List<int> liNo => bindingSource.List.Cast<object>().Select(p => Convert.ToInt32(p.GetType().GetProperty(NameNo).GetValue(p))).ToList();
        private void Edit_EditValueChanged(object sender, EventArgs e)
        {
            if (((BaseEdit)sender).EditValue is int no)
            {
                if (liNo.Where(x => x == no).Count() > 1)
                {
                    //ClsXtraMssgBox.ShowError(MessgTextValidNo);
                    ((BaseEdit)sender).EditValue = liNo.Max() + 1;
                }
            }
        }
        int id => int.Parse(CurrType.GetProperty("ID")?.GetValue(bindingSource.Current)?.ToString());
        int No
        {
            get
            {
                int no= int.Parse(CurrType.GetProperty("Number")?.GetValue(bindingSource.Current)?.ToString());
                if (no == 0)
                    return int.Parse(CurrType.GetProperty("No")?.GetValue(bindingSource.Current)?.ToString());
                else
                    return no;
            }
        }


        public virtual void Delete()
        {

        }
       
      
        public virtual void DataUpdate()
        {
            IsNew = false;
            EnableOrDisyble(false);
            TextReadOnly(false);
        }
        
        public virtual void Print()
        {
            if (gridControl != null)
                GridReportP.Print(gridControl, Session.LangEng ? $"List of {SumNameEn}" : $"قائمة {SumNameAr}", "", false, (gridControl.MainView as GridView).Columns.Count > 5);
            else if (treeList != null)
                GridReportP.Print(treeList, Session.LangEng ? $"List of {SumNameEn}" : $"قائمة {SumNameAr}", "");
            else GridReportP.Print(dataLayout, Session.LangEng ? $"List of {SumNameEn}" : $"قائمة {SumNameAr}", "");
        }
        public virtual void RefreshData()
        {
            TextReadOnly(true);
        }
        public virtual void Reset()
        {
            IsNew = false;
            EnableOrDisyble(true);
            RefreshData();
        }
        public virtual void EnableOrDisyble(bool state)
        {
            if (state)
            {
                bindingNavigator.MoveFirstItem = Movefirst;
                bindingNavigator.MovePreviousItem = Moveprevious;
                bindingNavigator.MoveNextItem = Movenext;
                bindingNavigator.MoveLastItem = Movelast;
            }
            else bindingNavigator.MoveFirstItem = bindingNavigator.MovePreviousItem =
                 bindingNavigator.MoveNextItem = bindingNavigator.MoveLastItem = null;
            Movefirst.Enabled = Moveprevious.Enabled = Movenext.Enabled = Movelast.Enabled = state;
            btnAddNew.Enabled = btnDelete.Enabled = btnUpdate.Enabled = btnPrint.Enabled = state;
            btnSave.Enabled = btnReset.Enabled = !state;
            if (treeList != null) treeList.Enabled = state;
            if (gridControl != null) gridControl.Enabled = state;
        }
      
        private void btnSaveAndNew_Click(object sender, EventArgs e)
        {
            btnSave.PerformClick();
            btnAddNew.PerformClick();
        }

        private void btnSaveAndClose_Click(object sender, EventArgs e)
        {
            btnSave.PerformClick();
            btnClose.PerformClick();
        }
       
        private void UC_MasterAll_Load(object sender, EventArgs e)
        {
            SetAppearance();
            //TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));
            //SetFont((Font)converter.ConvertFromString(Session.My_Setting.SystemFont));
            btnAddNew.Click += BtnAddNew_Click;
            btnDelete.Click += BtnDelete_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnPrint.Click += BtnPrint_Click;
            btnRefresh.Click += BtnRefresh_Click;
            btnReset.Click += BtnReset_Click;
            this.KeyDown += UC_MasterAll_KeyDown;
            DisableValidation();
        }
      
        private void BtnReset_Click(object sender, EventArgs e) => Reset();
        private void BtnPrint_Click(object sender, EventArgs e) => Print();
        private void BtnRefresh_Click(object sender, EventArgs e) => RefreshData();
        public void UC_MasterAll_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                btnRefresh.PerformClick();
            if (e.KeyCode == Keys.F2)
                btnAddNew.PerformClick();
            if (e.KeyCode == Keys.F3)
                btnSave.PerformClick();
            if (e.KeyCode == Keys.F4)
                btnUpdate.PerformClick();
            if (e.KeyCode == Keys.F5)
                btnReset.PerformClick();
            if (e.KeyCode == Keys.F6)
                btnDelete.PerformClick();
            if (e.KeyCode == Keys.F7)
                btnPrint.PerformClick();
        }
        private void BtnUpdate_Click(object sender, EventArgs e) => DataUpdate();
        private void BtnDelete_Click(object sender, EventArgs e) => Delete();
        private void BtnAddNew_Click(object sender, EventArgs e) => New();
        private void btnSave_Click(object sender, EventArgs e) => Save();
        private void btnClose_Click(object sender, EventArgs e) => this.Close();
    }

}
