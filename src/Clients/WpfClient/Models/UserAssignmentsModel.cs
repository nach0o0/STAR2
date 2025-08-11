using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Models
{
    public class UserAssignmentsModel
    {
        public List<RoleModel> AssignedRoles { get; set; } = new();
        public List<PermissionModel> DirectPermissions { get; set; } = new();
    }
}
