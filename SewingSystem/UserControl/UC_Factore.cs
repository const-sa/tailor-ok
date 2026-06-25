using SewingSystem.Classes;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SewingSystem.Forms
{
    public partial class UC_Factorie : UC_Master2
    {
        public UC_Factorie():base()
        {
            InitializeComponent();
            FactorieBindingSource.DataSource = Session.tblFactorie;
            RefreshData();
            //new ClsUserRoleValidation(this, UserControls.Factorie);
        }
        public override void RefreshData()
        {
            Session.GetDateFactorie();
            base.RefreshData();
        }
    }
}
