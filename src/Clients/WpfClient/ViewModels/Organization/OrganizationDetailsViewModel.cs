using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Factories.ViewModel;
using WpfClient.ViewModels.Admin;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Organization
{
    public partial class OrganizationDetailsViewModel : ViewModelBase
    {
        public RoleManagementViewModel RoleManagementViewModel { get; }
        public UserManagementViewModel UserManagementViewModel { get; }

        public OrganizationDetailsViewModel(IViewModelFactory viewModelFactory, Guid organizationId)
        {
            var organizationScope = $"organization:{organizationId}";

            RoleManagementViewModel = viewModelFactory.CreateRoleManagementViewModel(organizationScope);
            UserManagementViewModel = viewModelFactory.CreateUserManagementViewModel(organizationScope);
        }
    }
}
