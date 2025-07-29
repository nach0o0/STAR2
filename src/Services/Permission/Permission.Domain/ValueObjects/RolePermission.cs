using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Domain.ValueObjects
{
    public class RolePermission
    {
        public Guid RoleId { get; private set; }
        public string PermissionId { get; private set; }

        private RolePermission() { }

        public RolePermission(Guid roleId, string permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }
    }
}
