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
using SewingSystem.Classes;
using System.Threading;
using DevExpress.XtraEditors.Controls;
using Timer = System.Windows.Forms.Timer;
using SewingSystem.Report;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Utils.Extensions;
using CSHARPDLL;
using static SewingSystem.Classes.Master;
using ECRPaymentsAPI;

namespace SewingSystem.Forms
{
    public partial class XtraFormDefultSize2 : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        tblPermission Permission;
        List<tblPermission> tblPermissionWashType = new List<tblPermission>();
        bool IsNew = true;
        public XtraFormDefultSize2(tblSellInvoice cus, bool isnew = true)
        {
            InitializeComponent();
            ApplySizeImages();
            IsNew = isnew;
            tblSellInvoiceBindingSource.DataSource = cus;
            Permission = Session.tblPermission.FirstOrDefault(p => p.ObjectName == "العملاء" & p.UserGroupID == Program.User.UserGroupID);
            tblPermissionWashType.Clear();
            tblPermissionWashType = Session.tblPermission.Where(p => p.ParentID == Permission.ID).ToList();
            //AddNew.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false;
            PrintDirect.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues??false;
            PrintInvoice.Visible = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Print)?.TheValues??false;
            UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;

            // ★ علامة تمييز: تؤكد أن هذه الشاشة (DefultSize2) هي المعدّلة ★
            this.Text = "فاتورة التفصيل ★ نسخة معدّلة (DefultSize2) ★";
            var _mark = new DevExpress.XtraEditors.LabelControl
            {
                Name = "_lblEditMark",
                Text = "★ نسخة معدّلة ★",
                AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None,
                Size = new System.Drawing.Size(220, 26),
                Location = new System.Drawing.Point(12, 30)
            };
            _mark.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            _mark.Appearance.ForeColor = System.Drawing.Color.Red;
            _mark.Appearance.Options.UseFont = true;
            _mark.Appearance.Options.UseForeColor = true;
            this.Controls.Add(_mark);
            _mark.BringToFront();
        }
        ComponentFlyoutDialog flyDialog = new ComponentFlyoutDialog();
        // تحميل صور المقاسات المُستبدلة من قاعدة البيانات (تُبقي الصور المضمّنة لما لم يُستبدل).
        private void ApplySizeImages()
        {
            try
            {
                var overrides = Classes.SizeImageStore.GetAllOverrides();
                if (overrides.Count == 0) return;
                foreach (var kv in overrides)
                {
                    var ctl = this.Controls.Find(kv.Key, true);
                    if (ctl != null && ctl.Length > 0 && ctl[0] is DevExpress.XtraEditors.CheckEdit chk)
                    {
                        // البايتات وصلت بالفعل ضمن GetAllOverrides — نحوّلها مباشرة
                        // بدل استعلام منفصل لكل صورة (مهم جداً مع الخادم الخارجي).
                        var img = Classes.SizeImageStore.ImageFromBytes(kv.Value);
                        if (img != null) chk.BackgroundImage = img;
                    }
                }
            }
            catch { /* الصور تجميلية — لا توقف الشاشة */ }
        }

        private void XtraFormDefultSize2_Load(object sender, EventArgs e)
        {
            if (Program.Branch.UseTax ?? false)
                ItemForTheTax.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
            else
                ItemForTheTax.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            Spn_PaidCash.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            textEditPaidCreditCard.EditValueChanging += Spn_PaidCash_EditValueChanging1;
            spn_Net.EditValueChanging += Spn_Tax_EditValueChanging1;
            spn_BeforTax.EditValueChanging += Spn_Tax_EditValueChanging1;
            TheQuantityTextEdit.EditValueChanging += TheQuantityTextEdit_EditValueChanging;
            tblSellInvoiceBindingSource.CurrentChanged += TblSellInvoiceBindingSource_CurrentChanged;
            PrintDirect.Click += PrintDirect_Click;
            ecrTimer.Tick += EcrTimer_Tick;
            RefrashAll();
            PrintInvoice.Visible = PrintDirect.Visible;
            TblSizeAddingNew();
            textEditPaidCreditCard.Properties.Buttons.ForEach(x =>
            {
                x.Visible = Program.Branch.SendToECR;
                switch (x.Kind)
                {
                    case ButtonPredefines.Delete:
                        x.Visible = !((byte)NetworkPaymentType.Geidea920 == Program.Branch.NetworkPaymentType);
                        break;
                    case ButtonPredefines.OK:
                        break;
                }
            });
            if (CurrentInvoice != null)
            {
                var s = Session.tblSellInvoice.Where(i => i.ID == CurrentInvoice.ID & i.BranchID == CurrentInvoice.BranchID).ToList();
                int v = s.Count();
                if (v > 0)
                {
                    if (((CurrentInvoice.CusNumber as int?) ?? 0) > 0)
                        tblCustomerBindingSource.DataSource = Session.tblCustomer.Where(c => c.CusNumber == CurrentInvoice.CusNumber).ToList();
                    if (((CurrentInvoice.SizeID as int?) ?? 0) > 0)
                        tblDefaultSizeBindingSource.DataSource = Session.tblDefaultSize.Where(c => c.ID == CurrentInvoice.SizeID).ToList();
                    if (((s[0].HowPrint as short?) ?? 0) > 0)
                        DisaplyDeletAndEdit(false);
                    else
                        DisaplyDeletAndEdit(tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false);
                }
                else if (v <= 0)
                    DisaplyDeletAndEdit(tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues??false);
            }
            if (tblSellInvoiceDetaileBindingSource.List.Count <= 0)
                MoveFocuseToGrid();
            gridView14.CustomUnboundColumnData += gridView14_CustomUnboundColumnData;
            repoItems1.ImmediatePopup = true;
            repoItems1.ButtonClick += RepoItems_ButtonClick;
            repositoryTheNameAr.ImmediatePopup = true;
            repositoryTheNameAr.ButtonClick += RepoItems_ButtonClick;
            gridView14.CellValueChanging += gridView14_CellValueChanging;
            gridView14.RowUpdated += gridView14_RowUpdated;
        }
        private void ButtonDelete_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            GridView view = ((GridControl)((ButtonEdit)sender).Parent).MainView as GridView;
            if (view.FocusedRowHandle >= 0)
            {
                view.DeleteSelectedRows();
                gridView14_RowUpdated(sender, null);
            }
            if (view.RowCount <= 0)
            {
                ButtonAdd_ButtonClick(sender, null);
                gridView14_RowUpdated(sender, null);
            }
        }
        private void ButtonAdd_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            MoveFocuseToGrid();
        }
        void MoveFocuseToGrid()
        {
            this.gridControl11.Focus();
            gridView14.FocusedRowHandle = GridControl.InvalidRowHandle;
            gridView14.FocusedColumn = gridView14.Columns["ClassNumber"];
            tblSellInvoiceDetaileBindingSource.AddNew();
            gridView14.UpdateCurrentRow();
            tblSellInvoiceDetaile v = tblSellInvoiceDetaileBindingSource.Current as tblSellInvoiceDetaile;
            v.GomashLength = 3.50;
            v.NumClothes = 1;
            v.BranchID = Program.Branch.ID;
        }
        private void gridView14_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (e.Column.FieldName == "Index")
                e.Value = gridView14.GetVisibleRowHandle(e.ListSourceRowIndex) + 1;
        }
        private void gridView14_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            tblSellInvoiceDetaile row = gridView14.GetRow(e.RowHandle) as tblSellInvoiceDetaile;
            switch (e.Column.FieldName)
            {
                case nameof(row.ClassNumber):
                    row.ClassNumber = (e.Value as String) ?? "";
                    var p1 = Session.tblClasses.Where(f => f.ClassNumber == row.ClassNumber & f.BranchID == row.BranchID).ToList();
                    if (p1.Count() > 0)
                    {
                        if (row.NumClothes == 0)
                            row.NumClothes = 1;
                    }
                    break;
                case nameof(row.NumClothes):
                    if (e.Value.ToString() != "")
                        row.NumClothes = int.Parse(e.Value.ToString());
                    else
                        row.NumClothes = 1;
                    row.GomashLength = row.NumClothes * 3.50;
                    gridView14_CellValueChanging(sender, new CellValueChangedEventArgs(e.RowHandle, gridView14.Columns[nameof(row.GomashLength)], row.GomashLength));
                    break;
                default:
                    break;
            }
            gridView14.RefreshData();
            gridView14_RowUpdated(sender, null);
        }

        private void gridView14_RowUpdated(object sender, RowObjectEventArgs e)
        {
            if (CurrentInvoice == null)
                return;
            var items = tblSellInvoiceDetaileBindingSource.List as List<tblSellInvoiceDetaile>;
            if (items.Any())
            {
                CurrentInvoice.TheQuantity = items.Sum(x => x.NumClothes);
                CurrentInvoice.QuantityM = items.Sum(x => x.GomashLength);
            }
            else
                CurrentInvoice.QuantityM = CurrentInvoice.TheQuantity = 0;
        }

        private void RepoItems_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.Kind != ButtonPredefines.Plus)
                return;
            MyFunaction.OpenForm(new UC_Class(), "الاصناف");
            tblClassesBindingSource.DataSource = Session.tblClasses.ToList();
        }
        tblSellInvoice CurrentInvoice => tblSellInvoiceBindingSource.Current as tblSellInvoice;
        private void TheQuantityTextEdit_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (CurrentInvoice == null)
                return;
            if (e.NewValue.ToString() != "")
                CurrentInvoice.QuantityM = double.Parse(e.NewValue.ToString()) * 3.5;
        }
        int numPort = 0;
        public bool checkDevice(string valueIPOrPort)
        {
            try
            {
                if (valueIPOrPort.Contains("."))
                {
                    string[] array = valueIPOrPort.Split('.');
                    numPort = int.Parse(array[3]) & 0xFF;
                    numPort <<= 8;
                    numPort |= int.Parse(array[2]) & 0xFF;
                    numPort <<= 8;
                    numPort |= int.Parse(array[1]) & 0xFF;
                    numPort <<= 8;
                    numPort |= int.Parse(array[0]) & 0xFF;
                }
                else
                    numPort = int.Parse(valueIPOrPort);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        PaymentResponseViewModel TaskpaymentResponseViewModel;
        private void SendECR(string ecrAmount)
        {
            string ecrPort = Program.Branch.EcrPort;
            byte[] MSIGD = Encoding.ASCII.GetBytes("PUR");
            byte[] ECRno = Encoding.ASCII.GetBytes("123");
            byte[] Rcptno = Encoding.ASCII.GetBytes((CurrentInvoice?.InvoNumber??0).ToString());
            byte[] Amount = Encoding.ASCII.GetBytes(ecrAmount);
            byte[] Addfield1 = Encoding.ASCII.GetBytes("E");
            byte[] Addfield2 = Encoding.ASCII.GetBytes(" ");
            byte[] Addfield3 = Encoding.ASCII.GetBytes(" ");
            byte[] Addfield4 = Encoding.ASCII.GetBytes(" ");
            byte[] Addfield5 = Encoding.ASCII.GetBytes(" ");

            this.isProcessing = true;
            UpdateECRresponse();

            try
            {
                Task.Run(() =>
                {
                    byte[] ecr = CUSTOMDLLNET.StartAUCECRTran(MSIGD, ECRno, Rcptno, Amount, Addfield1,
                        Addfield2, Addfield3, Addfield4, Addfield5, ecrPort, 1);

                    string result = CUSTOMDLLNET.mirrorMessage();
                    this.isProcessing = false;
                });
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private bool ValidateECR()
        {
            bool isValid = true;

            try
            {
                isValid = CUSTOMDLLNET.checkDevice(Program.Branch.EcrPort);
                if (!isValid) ClsXtraMssgBox.ShowError(Session.LangEng ? "Failed to connect to ATM" : "فشل الإتصال بالصرافة الآلية");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }

            return isValid;
        }

        private bool ValidateECRamount(out string ecrAmountString)
        {
            ecrAmountString = null;
            bool isValid = !decimal.TryParse(textEditPaidCreditCard.Text, out decimal ecrAmount) || ecrAmount == 0 ? false : true;
            if (!isValid)
            {
                ClsXtraMssgBox.ShowError(Session.LangEng ? "You must enter a credit card payment first!" : "يجب إدخال المدفوع شبكه أولاً!");
                textEditPaidCreditCard.Focus();
            }
            else //if ((NetworkPaymentType)Program.Branch.NetworkPaymentType == NetworkPaymentType.alhamrani)
            {
                int ecrAmountInt = (int)ecrAmount;
                int ecrAmountCount = ecrAmountInt.ToString().Length;
                while (ecrAmountCount < 10)
                {
                    ecrAmountCount++;
                    ecrAmountString += "0";
                }
                ecrAmountString += ecrAmountInt.ToString();
                if (ecrAmount % 1 != 0)
                {
                    int ecrAmountFraction = Convert.ToInt32((ecrAmount % 1) * 100m);
                    ecrAmountString += ecrAmountFraction.ToString();
                }
                while (ecrAmountString.Length < 12) ecrAmountString += "0";
            }
            return isValid;
        }

        private void UpdateECRresponse()
        {
            this.ecrTimer.Interval = 500;
            this.ecrTimer.Start();
        }
        bool isProcessing;
        private void EcrTimer_Tick(object sender, EventArgs e)
        {
            if (Program.Branch.NetworkPaymentType == (byte)NetworkPaymentType.alhamrani)
            {
                string result = CUSTOMDLLNET.mirrorMessage();
                labelECR.Text = $"الحالة: {result}";
                if (!this.isProcessing)
                    this.ecrTimer.Stop();
            }
            else if (Program.Branch.NetworkPaymentType == (byte)NetworkPaymentType.Geidea920)
            {
                labelECR.Text = $"الحالة: {TaskpaymentResponseViewModel?.ResposeMsg}";
                if (!this.isProcessing)
                    this.ecrTimer.Stop();
            }
        }
        Timer ecrTimer = new Timer();
        private async void textEditPaidCreditCard_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                switch (e.Button.Kind)
                {
                    case ButtonPredefines.Delete:
                        if ((NetworkPaymentType)Program.Branch.NetworkPaymentType== NetworkPaymentType.alhamrani)
                                CUSTOMDLLNET.sendCancel();
                        break;
                    case ButtonPredefines.OK:
                        bool isValid;
                        textEditPaidCreditCard.Focus();
                        if (!Program.Branch.SendToECR) return;
                        switch ((NetworkPaymentType)Program.Branch.NetworkPaymentType)
                        {
                            case NetworkPaymentType.alhamrani:
                                if (!ValidateECR()) return;
                                if (!ValidateECRamount(out string ecrAmount)) return;
                                SendECR(ecrAmount);
                                break;
                            case NetworkPaymentType.Geidea920:
                                try
                                {
                                    isValid = checkDevice(Program.Branch.EcrPort);
                                    if (!isValid) ClsXtraMssgBox.ShowError(Session.LangEng ? "Failed to connect to ATM" : "فشل الإتصال بالصرافة الآلية");
                                    isValid = !double.TryParse(textEditPaidCreditCard.Text, out double ecrAmount2) || ecrAmount2 == 0 ? false : true;
                                    if (!isValid)
                                    {
                                        ClsXtraMssgBox.ShowError(Session.LangEng ? "You must enter a credit card payment first!" : "يجب إدخال المدفوع بطاقة إئتمان أولاً!");
                                        textEditPaidCreditCard.Focus();
                                        return;
                                    }
                                    PaymentRequestViewModel RequestModel = new ECRPaymentsAPI.PaymentRequestViewModel()
                                    {
                                        Mode = 1,
                                        BaudRate = 38400,
                                        Parity = 0,
                                        DataBits = 8,
                                        StopBits = 0,
                                        ECRReffrence = 1,
                                        EnableReceiptPrint = true,
                                        Amount = ecrAmount2,
                                        ComPort = numPort,
                                    };
                                    e.Button.Enabled = false;
                                    this.isProcessing = true;
                                    UpdateECRresponse();
                                    TaskpaymentResponseViewModel = await GeideaPament.Pay(RequestModel);
                                    this.isProcessing = false;
                                    e.Button.Enabled = true;
                                }
                                catch (Exception ex)
                                {
                                    e.Button.Enabled = true;
                                    XtraMessageBox.Show(ex.Message);
                                }
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                ClsXtraMssgBox.ShowError(ex.Message);
                return;
            }
        }

        private void Spn_PaidCash_EditValueChanging1(object sender, ChangingEventArgs e)
        {
            if (CurrentInvoice == null)
                return;
            switch (((TextEdit)sender).Name)
            {
                case "Spn_PaidCash":
                    CurrentInvoice.MonyOrpon = (CurrentInvoice.Chik ?? 0) + (CurrentInvoice.Network ?? 0)
                   + (CurrentInvoice.Visa ?? 0) + (CurrentInvoice.Meater ?? 0);
                    if (e.NewValue.ToString() != "")
                        CurrentInvoice.MonyOrpon += double.Parse(e.NewValue.ToString());
                    break;
                case "textEditPaidCreditCard":
                    CurrentInvoice.MonyOrpon = (CurrentInvoice.Chik ?? 0) + (CurrentInvoice.Cash ?? 0)
                   + (CurrentInvoice.Visa ?? 0) + (CurrentInvoice.Meater ?? 0);
                    if (e.NewValue.ToString() != "")
                        CurrentInvoice.MonyOrpon += double.Parse(e.NewValue.ToString());
                    break;
                default:
                    break;
            }
            CurrentInvoice.MonyRemin = (CurrentInvoice.TotalFinal ?? 0) - (CurrentInvoice.MonyOrpon ?? 0);
        }
        private void Spn_Tax_EditValueChanging1(object sender, ChangingEventArgs e)
        {
            if (CurrentInvoice == null)
                return;
            switch (((TextEdit)sender).Name)
            {
                case "spn_Net":
                    if (e.NewValue.ToString() != "")
                    {
                        if (Program.Branch.UseTax ?? false)
                            CurrentInvoice.TotalFattInvoice = ((double.Parse(e.NewValue.ToString()) / Session.Defualts.TaxOperator) * Session.Defualts.TaxRate);
                        else
                            CurrentInvoice.TotalFattInvoice = 0;
                        CurrentInvoice.TotalMony = double.Parse(e.NewValue.ToString()) - CurrentInvoice.TotalFattInvoice;
                        CurrentInvoice.MonyRemin = double.Parse(e.NewValue.ToString()) - (CurrentInvoice.MonyOrpon ?? 0);
                    }
                    break;
                case "spn_BeforTax":
                    if (e.NewValue.ToString() != "")
                    {
                        if (Program.Branch.UseTax ?? false)
                            CurrentInvoice.TotalFattInvoice = (double.Parse(e.NewValue.ToString()) * Session.Defualts.TaxRate);
                        else
                            CurrentInvoice.TotalFattInvoice = 0;
                        CurrentInvoice.TotalFinal = double.Parse(e.NewValue.ToString()) + CurrentInvoice.TotalFattInvoice;
                        CurrentInvoice.MonyRemin = double.Parse(e.NewValue.ToString()) - (CurrentInvoice.MonyOrpon ?? 0);
                    }
                    break;
                default:
                    break;
            }
        }
        private void TblSellInvoiceBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            if (!this.IsActive) return;
            if (CurrentInvoice == null)
                return;
            var s = Session.tblSellInvoice.Where(i => i.ID == CurrentInvoice.ID & i.BranchID == CurrentInvoice.BranchID).ToList();
            int v = s.Count();
            if (v > 0)
            {
                if ((CurrentInvoice.CusNumber ?? 0) > 0)
                {
                    tblCustomerBindingSource.DataSource = Session.tblCustomer.Where(c => c.CusNumber == CurrentInvoice.CusNumber).ToList();
                    tblDefaultSizeBindingSource.DataSource = Session.tblDefaultSize.Where(c => c.ID == CurrentInvoice.SizeID).ToList();
                }
                if ((s[0]?.HowPrint ?? 0) > 0)
                    DisaplyDeletAndEdit(false);
                else
                    DisaplyDeletAndEdit(tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues ?? false);
            }
            else if (v <= 0)
                DisaplyDeletAndEdit(tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Add)?.TheValues ?? false);
        }


        public void TblSizeAddingNew()
        {
            if (tblDefaultSizeBindingSource.Current != null)
                return;
            tblDefaultSize tblDefaultSize = new tblDefaultSize();
            tblCustomer tblCustomer = tblCustomerBindingSource.Current as tblCustomer;
            tblDefaultSize.EnterTime = DateTime.Now;
            tblDefaultSize.BranchID = Program.User.BranchID;
            tblDefaultSize.UserID = Program.User.ID;
            tblDefaultSize.J1 = false;
            tblDefaultSize.J2 = false;
            tblDefaultSize.J3 = false;
            tblDefaultSize.J4 = false;
            tblDefaultSize.J5 = false;
            tblDefaultSize.J6 = false;
            tblDefaultSize.J7 = false;

            tblDefaultSize.S1 = false;
            tblDefaultSize.S2 = false;
            tblDefaultSize.S3 = false;

            tblDefaultSize.Q1 = false;
            tblDefaultSize.Q2 = false;
            tblDefaultSize.Q3 = false;
            tblDefaultSize.Q4 = false;
            tblDefaultSize.Q5 = false;
            tblDefaultSize.Q6 = false;
            tblDefaultSize.Q7 = false;
            tblDefaultSize.Q8 = false;
            tblDefaultSize.Q9 = false;
            tblDefaultSize.Q10 = false;

            tblDefaultSize.K1 = false;
            tblDefaultSize.K2 = false;
            tblDefaultSize.K3 = false;
            tblDefaultSize.K4 = false;
            tblDefaultSize.K5 = false;
            tblDefaultSize.CusNumber = tblCustomer.CusNumber;
            tblDefaultSizeBindingSource.DataSource = tblDefaultSize;
        }

        private void RefrechInvoice_Click(object sender, EventArgs e)
        {
            RefrashAll();
        }
  
        bool ValidatData()
        {
            if (CurrentInvoice.QuantityM.Value <= 0)
            {
                XtraMessageBox.Show("عفوا لا يمكن ان تكون عدد الامتار اقل من او تساوي الصفر");
                return false;
            }
            if (CurrentInvoice.TheQuantity.Value <= 0)
            {
                XtraMessageBox.Show("عفوا لا يمكن ان تكون عدد الثياب اقل من او تساوي الصفر");
                return false;
            }
            if (CurrentInvoice.TotalFinal <= 0)
            {
                XtraMessageBox.Show("عفوا لا يمكن ان يكون المبلغ يساوي الصفر");
                return false;
            }
            if (CurrentInvoice.MonyRemin < 0)
            {
                XtraMessageBox.Show("عفوا لا يمكن ان يكون المبلغ المتبقي اقل من الصفر");
                return false;
            }
            return true;
        }
        private void Save_Click(object sender, EventArgs e)
        {
            if (CurrentInvoice == null)
                return;
            CurrentInvoice.QuanRemin = CurrentInvoice.TheQuantity;
            if (!ValidatData())
                return;
            try
            {
                flyDialog.WaitForm(this, 1);
                CurrentInvoice.TaxAll = CurrentInvoice.TotalFattInvoice + CurrentInvoice.TaxMaden - CurrentInvoice.TaxDayn;
                CurrentInvoice.TotalAll = (CurrentInvoice.TotalFinal ?? 0) + (CurrentInvoice.TotalMaden ?? 0) + CurrentInvoice.TaxMaden - (CurrentInvoice.TotalDayn ?? 0) - (CurrentInvoice.TaxDayn ?? 0);
                CurrentInvoice.MonyRemin = (CurrentInvoice.TotalAll ?? 0) - (CurrentInvoice.MonyPay ?? 0) - (CurrentInvoice.MonyOrpon ?? 0);
                using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    CurrentInvoice.CusNumber = CurrentCustomer?.CusNumber;
                    if (CurrentDefaultSize != null)
                    {
                        if (CurrentDefaultSize.ID > 0)
                        {
                            db.tblDefaultSizes.DeleteAllOnSubmit(db.tblDefaultSizes.Where(m => m.ID == CurrentDefaultSize.ID));
                            db.tblDefaultSizes.InsertOnSubmit(CurrentDefaultSize);
                        }
                        else
                        {
                            db.tblDefaultSizes.InsertOnSubmit(CurrentDefaultSize);
                            db.SubmitChanges();
                        }
                        if (!Session.tblDefaultSize.Any(v => v.ID == CurrentDefaultSize.ID))
                            Session.tblDefaultSize.Add(CurrentDefaultSize);
                        CurrentInvoice.SizeID = CurrentDefaultSize.ID;
                    }
                    if (IsNew)
                    {
                        if (!Session.tblSellInvoice.Any(s => s.BranchID == Program.User.BranchID))
                            CurrentInvoice.InvoNumber = 1;
                        else
                            CurrentInvoice.InvoNumber = db.tblSellInvoices.Where(s => s.BranchID == Program.User.BranchID).Max(i => i.InvoNumber) + 1;
                        db.tblSellInvoices.InsertOnSubmit(CurrentInvoice);
                    }
                    else
                    {
                        db.tblSellInvoices.DeleteAllOnSubmit(db.tblSellInvoices.Where(m => m.ID == CurrentInvoice.ID));
                        db.tblSellInvoices.InsertOnSubmit(CurrentInvoice);
                        db.tblSellInvoiceDetailes.DeleteAllOnSubmit(db.tblSellInvoiceDetailes.Where(m => m.InvoNumber == CurrentInvoice.InvoNumber));
                        var det = Session.tblSellInvoiceDetaile.Where(x => x.InvoNumber == CurrentInvoice.InvoNumber).ToList();
                        det.ForEach(x => Session.tblSellInvoiceDetaile.Remove(x));
                    }
                    db.tblSellInvoiceDetailes.InsertAllOnSubmit(GetInvoiceDetailes);
                    db.SubmitChanges();
                }
                if (IsNew && Program.Branch.SendMessOnSave)
                {
                    try
                    {
                        var cust = CurrentCustomer;
                        if (cust.Mobil.Length >= 9)
                        {
                            switch ((MessageType)Program.Branch.MessageType)
                            {
                                case MessageType.Sms:
                                    if (string.IsNullOrWhiteSpace(Program.Branch.SmsUserName) ||
                                     string.IsNullOrWhiteSpace(Program.Branch.SmsPassword) ||
                                     string.IsNullOrWhiteSpace(Program.Branch.SmsSenderName) ||
                                        string.IsNullOrWhiteSpace(Program.Branch.RequestReceiptMessage)) { }
                                    else
                                        whatsappMessage.SendSMS(cust.Mobil, Program.Branch.WelcomeMessage);
                                    break;
                                case MessageType.WhatsApp:
                                    if (string.IsNullOrWhiteSpace(Program.Branch.access_token) ||
                                        string.IsNullOrWhiteSpace(Program.Branch.instance_id) ||
                                        string.IsNullOrWhiteSpace(Program.Branch.RequestReceiptMessage)) { }
                                    else
                                        Task.Run(() => whatsappMessage.SendJsonToUrl(whatsappMessage.Getwaclient(cust, null, "text", "", Program.Branch.RequestReceiptMessage)));
                                    break;
                            }
                        }
                    }
                    catch (Exception ex) { SewingSystem.Classes.Logger.Log(ex); }
                }
                string mssg = $"الفاتورة رقم: {CurrentInvoice?.InvoNumber} ";
                if (Session.tblSellInvoice.Any(v => v.ID == CurrentInvoice.ID))
                {
                    int index = Session.tblSellInvoice.IndexOf(Session.tblSellInvoice.Single(x => x.ID == CurrentInvoice.ID));
                    Session.tblSellInvoice.Remove(Session.tblSellInvoice.Single(x => x.ID == CurrentInvoice.ID));
                    Session.tblSellInvoice.Insert(index, CurrentInvoice);
                }
                else
                    Session.tblSellInvoice.Add(CurrentInvoice);
                GetInvoiceDetailes.ForEach(x => Session.tblSellInvoiceDetaile.Add(x));
              
                spn_Paid.ReadOnly = SaveSuccess = true;
                if (((ToolStripButton)sender).Name == "SaveAndPrint")
                {
                    Program.PrintMode = "Direct";
                    var invo =MyFunaction.GetCopyFromInvoice(CurrentInvoice);
                    Task.Run(() => MyFunaction.FunactionPrintInvoice(invo));
                }
                else
                    UpdateRecord.Enabled = tblPermissionWashType.FirstOrDefault(p => p.PermissionName == Program.Update)?.TheValues??false;
                flyDialog.WaitForm(this, 0);
                flyDialog.ShowDialogForm(this, mssg, this.IsNew);
                IsNew = false;
            }
            catch (Exception ex)
            {
                flyDialog.WaitForm(this, 0);
                XtraMessageBox.Show(ex.Message);
                return;
            }
        }
        WhatsappMessage whatsappMessage = new WhatsappMessage();
        IList<tblSellInvoiceDetaile> GetInvoiceDetailes => (tblSellInvoiceDetaileBindingSource.List as IList<tblSellInvoiceDetaile>).Where(x => x.ClassNumber != null)
                     .ToList().Select(x => new tblSellInvoiceDetaile
                     {
                         BranchID = Program.Branch.ID,
                         ClassNumber = x.ClassNumber,
                         EnterTime = DateTime.Now,
                         GomashLength = x.GomashLength,
                         ID = x.ID,
                         InvoNumber = CurrentInvoice.InvoNumber,
                         Notes = x.Notes,
                         NumClothes = x.NumClothes,
                         UserID = Program.User.ID
                     }).ToList();
        public void DisaplyDeletAndEdit(bool state)
        {
            UpdateRecord.Enabled = state;
            gridView14.OptionsBehavior.ReadOnly = !state;
        }
        private void SaveAndPrint_Click(object sender, EventArgs e)
        {
            Save_Click(sender, e);
            if (SaveSuccess)
                SaveSuccess = false;
        }
       
        public void ButtonPrint()
        {
            if (CurrentInvoice == null)
                return;
            if (Session.tblSellInvoice.Any(i => i.ID == CurrentInvoice.ID & i.BranchID == CurrentInvoice.BranchID))
            {
                var invo =MyFunaction.GetCopyFromInvoice(CurrentInvoice);
                Task.Run(() => MyFunaction.FunactionPrintInvoice(invo));
                DisaplyDeletAndEdit(false);
            }
            else
            {
                if (Properties.Settings.Default.Language == "ar-SA")
                    XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "عفوا قم بحفظ الفاتورة اولا !!!!!", "حفظ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                else if (Properties.Settings.Default.Language == "en-US")
                    XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "Sorry, Save the bill first!!!!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }
        private void PrintInvoice_Click(object sender, EventArgs e)
        {
            Program.PrintMode = "ShowDialog";
            ButtonPrint();
        }


        public tblCustomer CurrentCustomer => tblCustomerBindingSource.Current as tblCustomer;
        public tblDefaultSize CurrentDefaultSize => tblDefaultSizeBindingSource.Current as tblDefaultSize;
        bool SaveSuccess = false;
        bool RefreachDeleteInvoice = false;

        private void Delete_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void PrintDirect_Click(object sender, EventArgs e)
        {
            Program.PrintMode = "Direct";
            ButtonPrint();
        }

        public void RefrashAll()
        {
            tblBranchesBindingSource.DataSource = Session.tblBranche.ToList();
            tblUsersBindingSource.DataSource = Session.tblUser.ToList();
            tblClassesBindingSource.DataSource = Session.tblClasses.ToList();
            tblTailorBindingSource.DataSource = Session.tblTailor.ToList();
            tblFactorieBindingSource.DataSource = Session.tblFactorie.ToList();
            if (CurrentInvoice != null)
            {
                tblCustomerBindingSource.DataSource = Session.tblCustomer.Where(s => s.CusNumber == (CurrentInvoice.CusNumber ?? 0) & s.BranchID == CurrentInvoice.BranchID).ToList();
                tblDefaultSizeBindingSource.DataSource = Session.tblDefaultSize.Where(s => s.ID == CurrentInvoice.SizeID & s.BranchID == CurrentInvoice.BranchID).ToList();
                tblSellInvoiceDetaileBindingSource.DataSource = Session.tblSellInvoiceDetaile.Where(d => d.InvoNumber == CurrentInvoice.InvoNumber & d.BranchID == CurrentInvoice.BranchID).ToList();
            }
            searchLookUpEditDefaultSize.Properties.DataSource = Session.tblDefaultSize.Where(s => s.CusNumber == (CurrentInvoice.CusNumber ?? 0) & s.BranchID == CurrentInvoice.BranchID).ToList();
        }

        private void SearchDefaultSizetxtedit_EditValueChanged(object sender, EventArgs e)
        {
            if (searchLookUpEditDefaultSize.EditValue == null)
                return;
            tblDefaultSize Carr = searchLookUpEditDefaultSize.GetSelectedDataRow() as tblDefaultSize;
            if (Carr == null)
                return;
            tblDefaultSizeBindingSource.DataSource = Carr;
            searchLookUpEditDefaultSize.EditValue = null;
        }
        XtraReportSize2 ReportSize;
        private void BtnPrintSize_Click(object sender, EventArgs e)
        {
            if (CurrentInvoice == null)
                return;
            if (Session.tblSellInvoice.Where(i => i.ID == CurrentInvoice.ID & i.BranchID == CurrentInvoice.BranchID).Count() <= 0)
            {
                if (Properties.Settings.Default.Language == "ar-SA")
                    XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "عفوا قم بحفظ الفاتورة اولا !!!!!", "حفظ", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                else if (Properties.Settings.Default.Language == "en-US")
                    XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "Sorry, Save the bill first!!!!", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            var size = Session.tblDefaultSize.FirstOrDefault(z => z.ID == CurrentInvoice.SizeID);
            if (size == null)
            {
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "عفوا قم بتحديد المقاسات اولا !!!!!", "'طباعة", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            tblCustomer cus = Session.tblCustomer.FirstOrDefault(u => u.CusNumber == CurrentInvoice.CusNumber & u.BranchID == CurrentInvoice.BranchID);
            var invoice = (from inv in Session.tblSellInvoice
                           where inv.ID == CurrentInvoice.ID & inv.BranchID == CurrentInvoice.BranchID
                           select new
                           {
                               inv.InvoNumber,
                               inv.CusNumber,
                               TheName = cus?.CustomerName,
                               user = Program.User.UserName,
                               Phone = cus?.Mobil,
                               inv.SellDate,
                               inv.DeliveryDate,
                               TotalFinal = inv.TotalFinal ?? 0,
                               MonyPay = (inv.MonyPay ?? 0) + (inv.MonyOrpon ?? 0) + (inv.Discount ?? 0),
                               MonyRemin = inv.MonyRemin ?? 0,
                               TotalFattInvoice = inv.TotalFattInvoice ?? 0,
                               TotalMony = inv.TotalMony ?? 0,
                               TheQuantity = inv.TheQuantity ?? 0,
                               J1 = (size.J1 ?? false),
                               J2 = (size.J2 ?? false),
                               J3 = (size.J3 ?? false),
                               J4 = (size.J4 ?? false),
                               J5 = (size.J5 ?? false),
                               J6 = (size.J6 ?? false),
                               J7 = (size.J7 ?? false),
                               K1 = (size.K1 ?? false),
                               K2 = (size.K2 ?? false),
                               K3 = (size.K3 ?? false),
                               K4 = (size.K4 ?? false),
                               K5 = (size.K5 ?? false),
                               S1 = (size.S1 ?? false),
                               S2 = (size.S2 ?? false),
                               S3 = (size.S3 ?? false),
                               Q1 = (size.Q1 ?? false),
                               Q2 = (size.Q2 ?? false),
                               Q3 = (size.Q3 ?? false),
                               Q4 = (size.Q4 ?? false),
                               Q5 = (size.Q5 ?? false),
                               Q6 = (size.Q6 ?? false),
                               Q7 = (size.Q7 ?? false),
                               Q8 = (size.Q8 ?? false),
                               Q9 = (size.Q9 ?? false),
                               Q10 = (size.Q10 ?? false),
                               ModelNum = size.ModelNum,
                               TadrezNum = size.TadrezNum,
                               AzrarNum = size.AzrarNum,
                               tall = size.tall,
                               shoulder = size.shoulder,
                               hands = size.hands,
                               middle = size.middle,
                               kapak = size.kapak,
                               kom = size.kom,
                               neck = size.neck,
                               breast = size.breast,
                               TheBase = size.TheBase,
                               Notes = size.Notes,
                               F1 = size.F1,
                               F2 = size.F2,
                               F3 = size.F3,
                               F4 = size.F4,
                               CompanyName = Program.Branch.CompanyName

                           }).ToList();

            ReportSize = new XtraReportSize2();
            ReportSize.DataSource = invoice;
            ReportSize.DetailReport.DataSource = (from inv in Session.tblSellInvoiceDetaile
                                                  where inv.InvoNumber == CurrentInvoice.InvoNumber & inv.BranchID == CurrentInvoice.BranchID
                                                  select new
                                                  {
                                                      inv.ClassNumber,
                                                      inv.NumClothes,
                                                      inv.GomashLength,
                                                      ClassName = MyFunaction.getClassName(inv.ClassNumber),
                                                  }).ToList();
            switch (radioGroup1.EditValue.ToString())
            {
                case "عادي":
                    ReportSize.xrCheckBoxN.Checked = true;
                    break;
                case "موديل":
                    ReportSize.xrCheckBoxM.Checked = true;
                    break;
                case "مطرز":
                    ReportSize.xrCheckBoxT.Checked = true;
                    break;
                case "ازرار":
                    ReportSize.xrCheckBoxZ.Checked = true;
                    break;
                default:
                    break;
            }

            //ReportSize.Parameters["p_companyName"].Value = "aششش";
            Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
            frmReport.documentViewer1.DocumentSource = ReportSize;
            frmReport.ShowDialog();
        }

        private void ClassIDLoukUp_EditValueChanging(object sender, ChangingEventArgs e)
        {
            if (!this.IsActive || e.NewValue == null)
                return;
            var row = Session.tblClasses.FirstOrDefault(c => c.ClassNumber == e.NewValue.ToString() & c.BranchID == Program.Branch.ID);
            if (row == null)
                return;
            if ((row.QuantityRemin ?? 0) <= 0)
            {
                XtraMessageBox.Show(DevExpress.LookAndFeel.UserLookAndFeel.Default, "لا يمكن اختيار هذا القماش بسبب نفاذ المخزون ", "'تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                e.NewValue = e.OldValue;
                return;
            }
        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            layoutControlItemGE.Enabled = checkEdit1.Checked;
        }

    }
}