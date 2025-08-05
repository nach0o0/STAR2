using Permission.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Domain.Constants
{
    public static class DefaultRoleDefinitions
    {
        public static readonly Role SystemAdminRole = new("System-Admin", "Has permissions to manage the system itself.");
        public static readonly Role OrganizationAdminRole = new("Organization-Admin", "Can fully manage an organization.");
        public static readonly Role OrganizationMemberRole = new("Organization-Member", "Standard user role with basic permissions in an organization.");
        public static readonly Role EmployeeGroupAdminRole = new("Employee-group-Admin", "Can fully manage an employee group.");
        public static readonly Role EmployeeGroupMemberRole = new("Employee-group-Member", "Standard user role with basic permissions in an employee group.");

        public static readonly IReadOnlyCollection<Role> All = new[] { SystemAdminRole, OrganizationAdminRole, OrganizationMemberRole, EmployeeGroupAdminRole, EmployeeGroupMemberRole };
    }
}
