using SewingSystem.Classes;
using SewingSystem.Forms;

namespace SewingSystem
{
    public partial class UC_Customer : UC_Master2
    {
        public UC_Customer() : base()
        {
            InitializeComponent();
            CustomerBindingSource.DataSource = Session.tblCustomer;
            RefreshData();
            //new ClsUserRoleValidation(this, UserControls.Branch);
        }
    
        public override void RefreshData()
        {
            Session.GetDataCustomer();
            BranchLookupEdit.IntializeData(Session.tblBranche);
            base.RefreshData();
        }
    
    }
}
