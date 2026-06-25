using DevExpress.XtraEditors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SewingSystem.LinqModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SewingSystem.Classes
{
    public class WhatsappMessage
    {
        public string SendJsonToUrl(waclient waclient)
        {
            string StrJsonData = JsonConvert.SerializeObject(waclient);
            if (StrJsonData == "") return "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://waclient.com/api/send");
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Content-Encoding", "utf-8");
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(StrJsonData);
                    streamWriter.Close();
                    var httpResponse = (HttpWebResponse)request.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        return result;
                    }
                }
            }
            catch (Exception exp)
            {
                throw new Exception(exp.Message, exp);
            }

        }

        //string messOnSave(tblCustomer customer, tblSellInvoice sellInvoice) =>
        //    $@"عزيزي العميل : {customer.CustomerName}
        //       تم استلام طلبكم وشكرا لثقتكم بنا وسوف تصل لك رسالة عند التجهيز
        //       {Program.Branch.CompanyName}
        //       تلفون :{Program.Branch.BranchTelephone}
        //       تاريخ التسليم  :{sellInvoice.DeliveryDate.GetValueOrDefault().ToString("yyyy/MM/dd")}
        //       رقم الفاتورة:{sellInvoice.InvoNumber}  
        //       الواصل:{sellInvoice.MonyOrpon.GetValueOrDefault().ToString("n2")}
        //       السعر:{sellInvoice.TotalMony.GetValueOrDefault().ToString("n2")}
        //       الضريبة:{sellInvoice.TotalFattInvoice.GetValueOrDefault().ToString("n2")}
        //       الباقي :{sellInvoice.MonyRemin.GetValueOrDefault().ToString("n2")}
        //       الرقم الضريبي:{Program.Branch.TaxNumber}";
        //string messOnDelivery(tblCustomer customer, string InvoNumber) =>
        //$@"عزيزي العميل : 
        //  {customer.CustomerName}
        //  ثيابكم جاهز للتسليم رقم {InvoNumber}  
        //  {Program.Branch.CompanyName}
        //  تلفون :{Program.Branch.BranchTelephone}";
      public  void SendSMS(string txt, string No)
        {
            try
            {
                WebClient client = new WebClient();
                Stream s = client.OpenRead(string.Format("https://www.hisms.ws/api.php?send_sms&username={0}&password={1}&numbers={2}&sender={3}&message={4}&date={5}&time={6}", Program.Branch.SmsUserName, Program.Branch.SmsPassword, No, Program.Branch.SmsSenderName, txt, DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("hh:mm")));
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
        public waclient Getwaclient(tblCustomer customer, tblSellInvoice sellInvoice, string type, string media_url, string mess2)
        {
            string mess1 = mess2 + $@"
          {Program.Branch.CompanyName}
          تلفون :{Program.Branch.BranchTelephone}";
            waclient waclient = new waclient
            {
                access_token = Program.Branch.access_token,
                instance_id = Program.Branch.instance_id,
                message = mess1,
                type = type,
            };
            if (customer.Mobil.Length == 9)
                waclient.number = Convert.ToInt64("966" + customer.Mobil);
            else if (customer.Mobil.Length == 10 && customer.Mobil.StartsWith("0"))
                waclient.number = Convert.ToInt64("966" + customer.Mobil.Substring(1));
            else
                waclient.number = Convert.ToInt64(customer.Mobil);
            if (waclient.type == "media")
            {
                waclient.media_url = media_url;//"https://b.top4top.io/p_28146fnji1.jpg";
            }
            return waclient;
        }
       
    }
    public class waclient
    {
        public Int64 number { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public string media_url { get; set; }
        public string filename { get; set; }
        public string instance_id { get; set; }
        public string access_token { get; set; }

    }
}
