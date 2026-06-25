using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting;
using SewingSystem.Classes;
using System.Linq;
using SewingSystem.LinqModel;

namespace SewingSystem.Report
{
    public partial class XtraReportPayAajel : DevExpress.XtraReports.UI.XtraReport
    {
        string Tax_Number = "";
        public XtraReportPayAajel()
        {
            InitializeComponent();
            
        }
        void BindData()
        {
            //lbl_ID.DataBindings.Add("Text", this.DataSource, "ID");
            ////XRBarCode1.DataBindings.Add("Text", this.DataSource, "QRCode");
            //lbl_TotalFinal.DataBindings.Add("Text", this.DataSource, "TotalFinal");
            ////lbl_SharingID.DataBindings.Add("Text", this.DataSource, "SharingID");
            ////lbl_RasiedPoint.DataBindings.Add("Text", this.DataSource, "RasedPoint");
            //lbl_user.DataBindings.Add("Text", this.DataSource, "user");
            //lbl_Customer.DataBindings.Add("Text", this.DataSource, "Customer");
            //lbl_CustomerPhone.DataBindings.Add("Text", this.DataSource, "Phone");
            //lbl_Total.DataBindings.Add("Text", this.DataSource, "TotalMony");
            //lbl_Discount.DataBindings.Add("Text", this.DataSource, "Discount");
            //lbl_TotalAfterDiscount.DataBindings.Add("Text", this.DataSource, "TotalAfterDiscount");
            //lbl_Tax.DataBindings.Add("Text", this.DataSource, "TotalFattInvoice");
            //lbl_TotalFinal1.DataBindings.Add("Text", this.DataSource, "TotalFinal");
            //lbl_Paid.DataBindings.Add("Text", this.DataSource, "MonyPay");
            //lbl_Remining.DataBindings.Add("Text", this.DataSource, "MonyRemin");
            //cell_Price.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "TotalFinal"));
            //cell_Product.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "ProductName"));
            //cell_Qty.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "TheQuantity"));
            //Cell_Total.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "TotalFinal"));
        }

        static string BillDate;
        public static void Print(object ds,long id,string ThePay,string sellDate)
        {
            BillDate = sellDate;
               XtraReportPayAajel rpt1 = new XtraReportPayAajel();
            Forms.XtraFormReport frmReport = new Forms.XtraFormReport();
            rpt1.ApplyLocalization(Properties.Settings.Default.Language);
           
            if (Properties.Settings.Default.Language == "ar-SA")
            {
                switch (ThePay)
                {
                    case "Cash نقدا":
                        rpt1.xrTypeInvoice.Text = "فاتورة مبيعات نقدا";
                        break;
                    case "Deferred آجل":
                        rpt1.xrTypeInvoice.Text = "فاتورة مبيعات آجل";
                        break;
                    case "Participation اشتراك":
                        rpt1.xrTypeInvoice.Text = "فاتورة مبيعات - اشتراك";
                        break;
                    default:
                        break;
                }
                rpt1.DisplayName = "فاتورة مبيعات";
            }
            else if (Properties.Settings.Default.Language == "en-US")
            {
                switch (ThePay)
                {
                    case "Cash نقدا":
                        rpt1.xrTypeInvoice.Text = "Bill Of Sale In Cash";
                        break;
                    case "Deferred آجل":
                        rpt1.xrTypeInvoice.Text = "A Deferred Sales Invoice";
                        break;
                    case "Participation اشتراك":
                        rpt1.xrTypeInvoice.Text = "Bill Of Sale - Partnership";
                        break;
                    default:
                        break;
                }
                rpt1.DisplayName = "Bill Sale";
            }
            rpt1.PrintingSystem.ContinuousPageNumbering = true;
            rpt1.DataSource = ds;
            rpt1.DetailReport.DataSource = rpt1.DataSource;
            //rpt1.DetailReport.DataMember = "Products";
            //rpt1.BindData();
            rpt1.CreateDocument();
            if ((Program.Branch.PrintTowBill as bool?)??false)
            {
                XtraReportPayAajel rpt2 = new  XtraReportPayAajel();
                rpt2.ApplyLocalization(Properties.Settings.Default.Language);
                rpt2.xrTypeInvoice.Text = rpt1.xrTypeInvoice.Text;
                rpt2.PrintingSystem.ContinuousPageNumbering = true;
                rpt2.DataSource = ds;
                rpt2.DetailReport.DataSource = rpt2.DataSource;
                //rpt2.DetailReport.DataMember = "Products";
                //rpt2.BindData();
                rpt2.CreateDocument();
                rpt1.Pages.AddRange(rpt2.Pages);
            }
            if (id > 0)
            {
                using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    short pri = ((db.tblSellInvoices.SingleOrDefault(p => p.ID == id).HowPrint as short?) ?? 0);
                    db.tblSellInvoices.SingleOrDefault(p => p.ID == id).HowPrint = ++pri;
                    db.SubmitChanges();
                }
            }
            switch (Program.PrintMode)
            {
                case "Direct":
                    rpt1.Print();
                    break;
                case "ShowDialog":
                    
                    frmReport.documentViewer1.DocumentSource
                     = rpt1;
                    frmReport.ShowDialog();
                    break;
                default:
                  
                    break;
            }
        }

        private void XtraReportPayAajel_BeforePrint(object sender, CancelEventArgs e)
        {
         
            if (xrTypeInvoice.Text == "فاتورة مبيعات - اشتراك")
            {
                Tabl_Customer.Rows[2].Cells[0].Visible = true;
                Tabl_Customer.Rows[2].Cells[1].Visible = true;
                Tabl_Customer.Rows[3].Cells[0].Visible = true;
                Tabl_Customer.Rows[3].Cells[1].Visible = true;
            }

            //lbl_user.Text = Program.User.UserName;
            lbl_SellDate.Text = BillDate;
            TaxNumber.Visible = (Program.Branch.UseTax as bool?) ?? false;
            tblBranche branch = Session.tblBranche.SingleOrDefault(b => b.ID == Program.User.BranchID);
            if (Program.Branch != null)
            {
                lbl_CompanyName.Text = Program.Branch.CompanyName;
                lbl_CompanyAddress.Text = "العنوان :"+(branch.BranchAddress as string) ?? "";
                lbl_CompanyPhone.Text ="الجوال :"+ (branch.BranchTelephone as string) ?? "";
                lbl_CompanyDesc.Text = Program.Branch.Notes;
                Tax_Number = Program.Branch.TaxNumber;
                if (Program.Branch.LogoImage != null)
                    xrPictureBox1.Image = Master.FromByteArray<Image>(Program.Branch.LogoImage.ToArray());
            }
            if (Properties.Settings.Default.Language == "ar-SA")
            {
                this.RightToLeft = RightToLeft.No;
                this.RightToLeftLayout = RightToLeftLayout.No;
                if (Program.Branch.UseTax?? false)
                {
                    TaxNumber.Text = "الرقم الضريبي:" + Tax_Number;
                    //xrTableCell31.Text = "السعر شامل الضريبة";
                    //xrTableCell31.Font.Size.Equals(6);
                }
                //else
                //{
                //    //xrTableCell31.Text = "السعر";
                //    //xrTableCell31.Font.Size.Equals(8);
                //}
            }
            else if (Properties.Settings.Default.Language == "en-US")
            {
                this.RightToLeft = RightToLeft.Yes;
                this.RightToLeftLayout = RightToLeftLayout.Yes;
                if (Program.Branch.UseTax?? false)
                {
                    TaxNumber.Text = "Tax Number:" + Tax_Number;
                    //xrTableCell31.Text = "Price includes tax";
                    //xrTableCell31.Font.Size.Equals(6);
                }
                //else
                //{
                //    xrTableCell31.Text = "Price";
                //    xrTableCell31.Font.Size.Equals(8);
                //}
            }
            if (Program.Branch.UseTax?? false)
            {
                //lbl_TotalAfterDiscount.Visible = true;
                //lbl_TotalAfterDiscount.Row.Visible = true;
                //xrTableCell24.Visible = true;
                lbl_Tax.Visible = true;
                lbl_Tax.Row.Visible = true;
                xrTableCell26.Visible = true;
            }
            else
            {
                //lbl_TotalAfterDiscount.Row.Visible = false;
                //  lbl_TotalAfterDiscount.Visible = false;
                //xrTableCell24.Visible = false;
                lbl_Tax.Row.Visible = false;
                lbl_Tax.Visible = false;
                xrTableCell26.Visible = false;
            }
        }

       
    }
}
