using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SewingSystem.DAL
{
    internal class Class1
    {
        public int SettingGetValue()
        {
            using (DAL.SewingSystem2Entities db = new DAL.SewingSystem2Entities())
            {
                string val = db.Settings.FirstOrDefault().Value;
                return int.Parse(Classes.MyFunaction.Decryption(val));
            }
        }
        public void SettingUpdateValue(int newValue)
        {
            using (DAL.SewingSystem2Entities db = new DAL.SewingSystem2Entities())
            {
                string enc = Classes.MyFunaction.Encryption(newValue.ToString());

                var stt = db.Settings.Find(1);
                stt.Value = enc;
                db.SaveChanges();
            }
        }
    }
}
