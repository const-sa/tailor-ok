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
using System.IO;
using System.IO.Ports;

namespace SewingSystem.Forms
{
    public partial class XtraFormCompanyData : DevExpress.XtraEditors.XtraForm
    {
        Classes.MyFunaction MyFunaction = new Classes.MyFunaction();
        public XtraFormCompanyData()
        {
            InitializeComponent();
            MessageTypeRadioGroup.IntializeData(Master.MessageTypeList);
        }
 
        public void RefrechData()
        {
            tblBrancheBindingSource.DataSource = Program.Branch;
        }
        private void DataMainForWasherSave_Click(object sender, EventArgs e)
        {
            if (tblBrancheBindingSource.Current!=null)
            {
                tblBrancheBindingSource.EndEdit();
                tblBranche data = tblBrancheBindingSource.Current as tblBranche;
                data.UserID = Program.User.ID;
                data.EnterTime = DateTime.Now;
                using (var db = new DataClasses1DataContext(Program.ConnectionString))
                {
                    db.tblBranches.DeleteAllOnSubmit(db.tblBranches.Where(m => m.ID == data.ID));
                    db.tblBranches.InsertOnSubmit(data);
                    db.SubmitChanges();
                    Program.Branch = data;
                    Properties.Settings.Default.SMS_Message = data.WelcomeMessage;
                    Properties.Settings.Default.Save();
                }
                MyFunaction.MessageBoxSave();
            }
        }
        private void DataMainForWasher_print_Click(object sender, EventArgs e)
        {
            DataMainForWashergraidControl.ShowPrintPreview();
        }

        private void Refrash_Click(object sender, EventArgs e)
        {
            RefrechData();
        }
        private void XtraFormWashData_Load(object sender, EventArgs e)
        {
            radioGroupMashinType.IntializeData(Master.NetworkPaymentList);
            teECRport.Properties.DataSource = SerialPort.GetPortNames().Select(x => new { Name = x, Number = x.Replace("COM", "") }).ToList();
            RefrechData();
        }

    }
}