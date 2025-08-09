using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.ViewModels.Admin;

namespace WpfClient.Factories.ViewModel
{
    public interface IViewModelFactory
    {
        RoleManagementViewModel CreateRoleManagementViewModel(string scope);
        RoleListViewModel CreateRoleListViewModel(string scope);
        RoleDetailsViewModel CreateRoleDetailsViewModel(RoleModel role, string scope);
        CreateRoleViewModel CreateCreateRoleViewModel(string scope);

        UserManagementViewModel CreateUserManagementViewModel(string scope);
    }
}
