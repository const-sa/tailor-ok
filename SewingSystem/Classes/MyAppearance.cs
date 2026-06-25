using DevExpress.Utils;
using DevExpress.Utils.Extensions;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraLayout;
using DevExpress.XtraTreeList;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SewingSystem.Classes
{
    public static class MyAppearance
    {
        public static void SetAppearanceGroup(this LayoutControlGroup GroupMain, BindingNavigator bindingNavigator = null)
        {
            if (GroupMain == null) return;
            try
            {
                GroupMain.Padding = new DevExpress.XtraLayout.Utils.Padding(4, 4, 4, 4);
                GroupMain.CustomDrawBackground += LayoutGroupInvo_CustomDrawBackground;
                GroupMain.AppearanceGroup.BorderColor = Color.SeaGreen;
                GroupMain.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
                GroupMain.Parent?.Items?.OfType<LayoutControlGroup>().ForEach(x => x.AppearanceGroup.BorderColor = Color.SeaGreen);
                if (bindingNavigator != null)
                {
                    //bindingNavigator.Font = (Font)converter.ConvertFromString(Session.SystemFont);
                    bindingNavigator.BackColor = System.Drawing.Color.AliceBlue;
                }
            }
            catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }

        }
        public static void SetAppearanceDataLayoutControl(this LayoutControl dataLayout)
        {
            if (dataLayout == null) return;
            dataLayout?.Items.OfType<LayoutControlItem>().Where(x => x.Control is BaseEdit).ForEach(y =>
            {
                if (y.Control is BaseEdit edit && edit.Properties.Appearance.TextOptions.HAlignment == HorzAlignment.Far)
                    edit.Properties.Appearance.TextOptions.HAlignment = HorzAlignment.Near;

            });
        }
        public static void AppearanceTreeList(this TreeList TreeList)
        {
            if (TreeList == null) return;
            TreeList.OptionsBehavior.AllowExpandAnimation = DefaultBoolean.True;
            TreeList.OptionsDragAndDrop.DragNodesMode = DragNodesMode.Single;
            TreeList.OptionsDragAndDrop.DropNodesMode = DropNodesMode.Standard;
            TreeList.OptionsFilter.ExpandNodesOnFiltering = TreeList.OptionsFind.ExpandNodesOnIncrementalSearch = true;
            TreeList.OptionsView.ShowTreeLines = DefaultBoolean.True;

            TreeList.OptionsView.ShowColumns = TreeList.OptionsBehavior.Editable = false;
            TreeList.OptionsBehavior.PopulateServiceColumns = TreeList.OptionsView.ShowAutoFilterRow = true;
            //TreeList.Appearance.EvenRow.BackColor = Color.AliceBlue;
            TreeList.OptionsView.EnableAppearanceEvenRow = true;
            TreeList.Appearance.Row.TextOptions.HAlignment = HorzAlignment.Near;
            TreeList.Columns.ForEach(x => x.AppearanceHeader.BackColor = System.Drawing.Color.SeaGreen);
            TreeList.OptionsBehavior.ReadOnly = true;
            TreeList.AppearancePrint.Row.TextOptions.HAlignment = HorzAlignment.Near;
            TreeList.AppearancePrint.EvenRow.BackColor = Color.AliceBlue;
            TreeList.AppearancePrint.HeaderPanel.BackColor = System.Drawing.Color.SeaGreen;
            TreeList.AppearancePrint.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Near;
            TreeList.Appearance.Empty.BackColor = System.Drawing.Color.AliceBlue;
        }

        public static void LayoutAppearanceInvo(this LayoutControlGroup layoutControlGroup, BindingNavigator bindingNavigator, Color systemColors)
        {
            try
            {
                layoutControlGroup.CustomDrawBackground += LayoutGroupInvo_CustomDrawBackground;
                layoutControlGroup.AppearanceGroup.BackColor = systemColors;
                if (bindingNavigator != null)
                    bindingNavigator.BackColor = systemColors;
                layoutControlGroup.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Far;
            }
            catch (Exception logEx) { SewingSystem.Classes.Logger.Log(logEx); }
        }
        static TypeConverter converter = TypeDescriptor.GetConverter(typeof(Font));

        public static void SetAppearanceGridView(this GridView gridView)
        {
            if (gridView == null) return;
            gridView.OptionsView.ShowGroupPanel = false;
            gridView.AppearancePrint.EvenRow.BackColor = gridView.Appearance.EvenRow.BackColor = Color.AliceBlue;
            gridView.OptionsView.EnableAppearanceEvenRow = gridView.OptionsBehavior.ReadOnly = true;
            gridView.Appearance.Row.TextOptions.HAlignment =
            gridView.Appearance.HeaderPanel.TextOptions.HAlignment =
            gridView.AppearancePrint.HeaderPanel.TextOptions.HAlignment =
            gridView.AppearancePrint.Row.TextOptions.HAlignment = HorzAlignment.Center;

            gridView.Columns.ForEach(x => {
                //x.AppearanceCell.Font = (Font)converter.ConvertFromString(Session.My_Setting.SystemFont);
                x.AppearanceHeader.BackColor = Color.SeaGreen;
            });
            gridView.AppearancePrint.HeaderPanel.BackColor = Color.SeaGreen;
            gridView.AppearancePrint.HeaderPanel.ForeColor = Color.White;
            gridView.CustomColumnDisplayText += GridView_CustomColumnDisplayText;
        }
        private static void GridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.Value == null) return;
                switch (e.Column.FieldName)
                {
                    case "BranchID" when e.Value is short b:
                        e.DisplayText = Session.tblBranche?.FirstOrDefault(x => x.ID == b)?.BranchName;
                        break;
                    case "BrnId" when e.Value is short b1:
                        e.DisplayText = Session.tblBranche?.FirstOrDefault(x => x.ID == b1)?.BranchName;
                        break;
                    case "UserID" when e.Value is short b:
                        e.DisplayText = Session.tblUser?.FirstOrDefault(x => x.ID == b)?.UserName;
                        break;
                    //case "CostCenterID" when e.Value is int c:
                    //    e.DisplayText = Session.CostCenters?.FirstOrDefault(x => x.ID == c)?.Name;
                    //    break;
                    //case "PersonalID" when e.Value is int c:
                    //    e.DisplayText = Session.Personals?.FirstOrDefault(x => x.ID == c)?.Name;
                    //    break;
                    //case nameof(Model.Invoices.Invoice.CusOrSupID) when e.Value is int c:
                    //    e.DisplayText = Session.Personals?.FirstOrDefault(x => x.ID == c)?.Name;
                    //    break;
                    //case nameof(Model.Invoices.Invoice.SalesmanId) when e.Value is int c:
                    //    e.DisplayText = Session.Representatives?.FirstOrDefault(x => x.ID == c)?.Name;
                    //    break;
                    //case nameof(Currency.Type) when e.Value is byte c:
                    //    e.DisplayText = Session.CurrencyTypeList?.FirstOrDefault(x => (byte)x.ID == c)?.Name;
                    //    break;
                    //case nameof(Bank.Currency) when e.Value is byte c:
                    //    e.DisplayText = Session.Currencies?.FirstOrDefault(x => x.ID == c)?.Name;
                    //    break;
                    //case nameof(Model.Invoices.InvoiceDetail.ProductID) when e.Value is int c:
                    //    e.DisplayText = Session.Products?.FirstOrDefault(x => x.ID == c)?.Name;
                    //    break;
                    //case nameof(Model.Invoices.InvoiceDetail.UnitID) when e.Value is int c:
                    //    e.DisplayText = Session.MeasurementUnits?.FirstOrDefault(x => x.ID == c)?.NameOfUnit;
                    //    break;
                    //case nameof(Model.Invoices.InvoiceDetail.GroupID) when e.Value is int c:
                    //    e.DisplayText = Session.ProductGroups?.FirstOrDefault(x => x.ID == c)?.Name;
                    //    break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                return;
            }
        }
        private static void LayoutGroupInvo_CustomDrawBackground(object sender, GroupBackgroundCustomDrawEventArgs e)
        {
            try
            {
                LayoutControlGroup controlGroup = ((LayoutControlGroup)sender);
                e.DefaultDraw();
                e.Graphics.FillRectangle(new SolidBrush(controlGroup.AppearanceGroup.BackColor), e.Bounds);
                e.Handled = true;
            }
            catch (Exception)
            {
                return;
            }
        }
    }
}
