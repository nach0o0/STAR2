using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Application.Security
{
    public static class CustomClaimTypes
    {
        public const string EmployeeId = "employee_id";
        public const string OrganizationId = "organization_id";
        public const string EmployeeGroupId = "employee_group_id";
        public const string PermissionsByScope = "permissions_by_scope";
    }
}
