using SewingSystem.Classes;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SewingSystem.Forms
{
    public partial class UC_Tailor : UC_Master2
    {
        public UC_Tailor():base()
        {
            InitializeComponent();
            TailorBindingSource.DataSource = Session.tblTailor;
            RefreshData();
            //new ClsUserRoleValidation(this, UserControls.Tailor);
        }
        public override void RefreshData()
        {
            Session.GetDataTailor();
            base.RefreshData();
        }
    }
}
