using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Services.Application.Permission
{
    public interface IPermissionService
    {
        bool HasPermission(string permissionId);
        bool HasPermissionInScope(string permissionId, string scope);
    }
}
