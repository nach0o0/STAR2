using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.ViewModels.Admin;

namespace WpfClient.Factories.ViewModel
{
    public interface IViewModelFactory
    {
        RoleManagementViewModel CreateRoleManagementViewModel(string scope);
        UserManagementViewModel CreateUserManagementViewModel(string scope);
    }
}
