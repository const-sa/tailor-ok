using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraSplashScreen;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SewingSystem
{
    public partial class ComponentFlyoutDialog : Component
    {
        Timer timer = new Timer();
        IOverlaySplashScreenHandle overlaySplashScreen;

        public ComponentFlyoutDialog()
        {
            InitializeComponent();
            flyoutPanelControl1.Dock = DockStyle.Fill;
            timer.Tick += Timer_Tick1;
        }

        public ComponentFlyoutDialog(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        private void Timer_Tick1(object sender, EventArgs e)
        {
            CloseFlyoutPanel();
        }

        public void ShowDialogUC(UserControl ucOwner, string mssg, bool isNew = true)
        {
            flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Top;
            flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Slide;
            flyoutPanel1.OwnerControl = ucOwner;
            simpleLabelItem1.Text = $"{(!Classes.Session.LangEng ? (isNew ? "تم حفظ " : "تم تعديل بيانات ") + mssg + " بنجاح!" : mssg + (isNew ? " created successfully!" : " updated successfully!"))}";
            ShowFlyoutPanel();
        }

        public void ShowDialogUCUpdMsg(UserControl ucOwner, string message)
        {
            flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Top;
            flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Slide;
            flyoutPanel1.OwnerControl = ucOwner;
            simpleLabelItem1.Text = (!Classes.Session.LangEng) ? "تم تعديل " + message + " بنجاح " : message + " updated successfully";
            ShowFlyoutPanel();
        }
        public void ShowDialogUCUdeletedMsg(UserControl ucOwner, string message)
        {
            flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Top;
            flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Slide;
            flyoutPanel1.OwnerControl = ucOwner;
            simpleLabelItem1.Text = (!Classes.Session.LangEng) ? "تم حذف " + message + " بنجاح " : message + " deleted successfully";
            ShowFlyoutPanel();
        }
        public void ShowDialogUCCustomdMsg(UserControl ucOwner, string message)
        {
            flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Top;
            flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Slide;
            flyoutPanel1.OwnerControl = ucOwner;
            simpleLabelItem1.Text = message;
            ShowFlyoutPanel();
        }
        public void ShowDialogDeleteForm(Form frmOwner, string message)
        {
            SetFlyoutPanelSize(frmOwner.Width, 65);
            flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Center;
            flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Fade;
            flyoutPanel1.OwnerControl = frmOwner;
            flyoutPanel1.AnimationRate = 45;
            simpleLabelItem1.Text = (!Classes.Session.LangEng) ? "تم حذف " + message + " بنجاح " : message + " deleted successfully";
            ShowFlyoutPanel();
        }
        public void ShowDialogForm(Form frmOwner, string message)
        {
            SetFlyoutPanelSize(frmOwner.Width, 65);
            flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Center;
            flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Fade;
            flyoutPanel1.OwnerControl = frmOwner;
            flyoutPanel1.AnimationRate = 45;
            simpleLabelItem1.Text = (!Classes.Session.LangEng) ? "تم حفظ " + message + " بنجاح " : message + " saved successfully";
            ShowFlyoutPanel();
        }
        public void ShowDialogForm(Form frmOwner, string mssg, bool isNew = true)
        {
            SetFlyoutPanelSize(frmOwner.Width, 65);
            flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Center;
            flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Fade;
            flyoutPanel1.OwnerControl = frmOwner;
            flyoutPanel1.AnimationRate = 45;
            simpleLabelItem1.Text = $"{(!Classes.Session.LangEng ? (isNew ? "تم حفظ " : "تم تعديل بيانات ") + mssg + " بنجاح!" : mssg + (isNew ? " created successfully!" : " updated successfully!"))}";
            ShowFlyoutPanel();
        }
        public void ShowDialogFormBelowRIbbon(Form formOwner, RibbonControl ribbon, string message, bool isNew = true)
        {
            SetFlyoutPanelSize(formOwner.Width, 65);
            SetFlyoutPanelLocationY(ribbon.Size.Height);
            //SetFlyoutPanelLocationY((formOwner as formSupplyMain).ribbonControl1.Size.Height);
            flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Manual;
            flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Fade;
            flyoutPanel1.AnimationRate = 45;
            flyoutPanel1.OwnerControl = formOwner;
            simpleLabelItem1.Text = $"{(!Classes.Session.LangEng ? (isNew ? "تم حفظ " : "تم تعديل بيانات ") + message + " بنجاح!" : message + (isNew ? " created successfully!" : " updated successfully!"))}";// $"{(isNew ? "تم حفظ " : "تم تعديل بيانات ")} {message} بنجاح!";
            ShowFlyoutPanel();
        }

        public void ShowDialogFormCustomMsg(Form frmOwner, string message)
        {
            SetFlyoutPanelSize(frmOwner.Width, 65);
            flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Center;
            flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Fade;
            flyoutPanel1.OwnerControl = frmOwner;
            simpleLabelItem1.Text = message;
            ShowFlyoutPanel();
        }

        private void ShowFlyoutPanel()
        {
            try
            {
                flyoutPanel1.ShowPopup();
                timer.Interval =2000;
                timer.Start();
            }
            catch (Exception)
            {

                return;
            }
        }
        public void ShowDialogFormLayoutControl(Form formOwner, string message, bool isNew = true)
        {
            SetFlyoutPanelSize(formOwner.Width, 65);
            flyoutPanel1.Options.AnchorType = DevExpress.Utils.Win.PopupToolWindowAnchor.Top;
            flyoutPanel1.Options.AnimationType = DevExpress.Utils.Win.PopupToolWindowAnimation.Fade;
            flyoutPanel1.AnimationRate = 45;
            flyoutPanel1.OwnerControl = formOwner;
            simpleLabelItem1.Text = $"{(!Classes.Session.LangEng ? (isNew ? "تم حفظ " : "تم تعديل بيانات ") + message + " بنجاح!" : message + (isNew ? " created successfully!" : " updated successfully!"))}";// $"{(isNew ? "تم حفظ " : "تم تعديل بيانات ")} {message} بنجاح!";
            ShowFlyoutPanel();
        }
        private void CloseFlyoutPanel()
        {
            try
            {
                flyoutPanel1?.HidePopup();
                timer?.Stop();
            }
            catch (Exception)
            {
                return;
            }
        }

        private void SetFlyoutPanelSize(int width, int height = 55)
        {
            flyoutPanel1.Size = new System.Drawing.Size(width, height);
        }

        private void SetFlyoutPanelLocationY(int y)
        {
            flyoutPanel1.Options.Location = new System.Drawing.Point(0, y);
        }

        public void WaitForm(UserControl ucOwner, byte status)
        {
            if (status == 0) this.overlaySplashScreen?.Close();
            else if (status == 1)
                this.overlaySplashScreen = SplashScreenManager.ShowOverlayForm(ucOwner);
        }

        public void WaitForm(Form form, byte status)
        {
            if (status == 0 && overlaySplashScreen != null) this.overlaySplashScreen.Close();
            else if (status == 1) this.overlaySplashScreen = SplashScreenManager.ShowOverlayForm(form);
        }
        public void WaitForm(UserControl ucOwner, byte status, Form frm)
        {
            if (status == 0) this.overlaySplashScreen?.Close();
            else if (status == 1)
                this.overlaySplashScreen = SplashScreenManager.ShowOverlayForm(ucOwner);
            if (frm != null && status == 0) this.overlaySplashScreen?.QueueFocus(frm);
        }

        public void WaitForm(Form form, byte status, Form frm)
        {
            if (status == 0 && overlaySplashScreen != null) this.overlaySplashScreen.Close();
            else if (status == 1) this.overlaySplashScreen = SplashScreenManager.ShowOverlayForm(form);
            if (frm != null && status == 0) this.overlaySplashScreen?.QueueFocus(frm);
        }
    }
}
