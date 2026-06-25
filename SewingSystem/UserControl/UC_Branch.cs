using SewingSystem.Classes;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SewingSystem.Forms
{
    public partial class UC_Branch : UC_Master2
    {
        public UC_Branch():base()
        {
            InitializeComponent();
            BranchBindingSource.DataSource = Session.tblBranche;
            RefreshData();
            //new ClsUserRoleValidation(this, UserControls.Branch);
        }
        public override void RefreshData()
        {
            Session.GetDataBranche();
            base.RefreshData();
        }
    }
}
