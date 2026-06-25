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
using System.Net;
using System.IO;
using SewingSystem.Classes;
using DevExpress.Utils.Extensions;
using static SewingSystem.Classes.WhatsappMessage;

namespace SewingSystem.Forms
{
    public partial class XtraFormSMS : DevExpress.XtraEditors.XtraForm
    {
        public XtraFormSMS()
        {
            InitializeComponent();
            BtnSendWhatsApp.Click += BtnSendWhatsApp_Click;
        }

        private void XtraFormSMS_Load(object sender, EventArgs e)
        {
            tblCustomerBindingSource.DataSource = Classes.Session.tblCustomer.ToList();
        }

        private void searchLookUpEditCustomer_EditValueChanged(object sender, EventArgs e)
        {
            if (searchLookUpEditCustomer.EditValue != null)
            {
                tblCustomer Carr = searchLookUpEditCustomer.GetSelectedDataRow() as tblCustomer;
                if (Carr != null)
                {
                    txtMobil.Text = ((Carr.Mobil as string) ?? "");
                }
            }
        }
        WhatsappMessage whatsappMessage = new WhatsappMessage();

        private void BtnSendWhatsApp_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Program.Branch.access_token))
                    XtraMessageBox.Show("access_token غير موجود قم باضافة access_token من شاشة بيانات المؤسسة", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (string.IsNullOrWhiteSpace(Program.Branch.instance_id))
                    XtraMessageBox.Show("instance_id غير موجود قم باضافة instance_id من شاشة بيانات المؤسسة", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (string.IsNullOrWhiteSpace(txtMessage.Text))
                    XtraMessageBox.Show("قم بكتابة الرسالة النصية!!!! ", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                searchLookUpEdit1View2.GetSelectedRows().ForEach(x =>
                {
                    if (searchLookUpEdit1View2.GetRow(x) is tblCustomer cust && cust.Mobil.Length >= 9)
                        Task.Run(() => whatsappMessage.SendJsonToUrl(whatsappMessage.Getwaclient(cust, null, "text","", txtMessage.Text)));
                });
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        private void BtnSend_Click(object sender, EventArgs e)
        {
            try
            {
                WebClient client = new System.Net.WebClient();
                Stream s = client.OpenRead(
               string.Format("https://www.hisms.ws/api.php?send_sms&username={0}&password={1}&numbers={2}&sender={3}&message={4}&date={5}&time={6}",
               ((Program.Branch.SmsUserName as string) ?? ""), ((Program.Branch.SmsPassword as string) ?? ""),
               txtMobil.Text, ((Program.Branch.SmsSenderName as string) ?? ""), txtMessage.Text,
               DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("hh:mm")));
                StreamReader reader = new StreamReader(s);
                string result = reader.ReadToEnd();
                if (result == "1")
                    XtraMessageBox.Show("اسم المستخدم غير صحيح", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "2")
                    XtraMessageBox.Show("كلمة المرور غير صحيحة", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "404")
                    XtraMessageBox.Show("لم يتم ادخال جميع البيانات المطلوبة", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "403")
                    XtraMessageBox.Show("تم تجاوز عدد المحاولات المسموحه", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "504")
                    XtraMessageBox.Show("الحساب معطل", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "3")
                    XtraMessageBox.Show("تم الارسال", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "4")
                    XtraMessageBox.Show("لا يوجد ارقام", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "5")
                    XtraMessageBox.Show("لا يوجد رساله", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "6")
                    XtraMessageBox.Show("خطاء في اسم المرسل", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "7")
                    XtraMessageBox.Show("اسم المرسل هذا غير مفعل", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "8")
                    XtraMessageBox.Show("الرساله تحوي كلمة ممنوعه", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "9")
                    XtraMessageBox.Show("لا يوجد رصيد", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "10")
                    XtraMessageBox.Show("صيغة التاريخ خاطئة", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (result == "11")
                    XtraMessageBox.Show("صيغة الوقت خاطئة", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    XtraMessageBox.Show(result, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BtnSend1_Click(object sender, EventArgs e)
        {
            try
            {
                string numbers="";
                foreach (var item in Classes.Session.tblCustomer)
                {
                    if (((item.Mobil as string)??"")!=string.Empty)
                    numbers +=item.Mobil + ",";
                }
                WebClient client = new System.Net.WebClient();
                Stream s = client.OpenRead(string.Format("https://www.hisms.ws/api.php?send_sms&username={0}&password={1}&numbers={2}&sender={3}&message={4}&date={5}&time={6}", ((Program.Branch.SmsUserName as string) ?? ""), ((Program.Branch.SmsPassword as string) ?? ""), numbers, ((Program.Branch.SmsSenderName as string) ?? ""), txtMessage.Text, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("hh:mm")));
                StreamReader reader = new StreamReader(s);
                string result = reader.ReadToEnd();
                XtraMessageBox.Show(result, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
    }
}