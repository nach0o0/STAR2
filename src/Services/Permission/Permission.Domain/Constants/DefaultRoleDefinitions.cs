using Permission.Domain.Entities;
using Shared.Domain.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Domain.Constants
{
    public static class DefaultRoleDefinitions
    {
        public static readonly Guid SystemAdminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public static readonly Guid OrganizationAdminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        public static readonly Guid OrganizationMemberRoleId = Guid.Parse("00000000-0000-0000-0000-000000000003");
        public static readonly Guid EmployeeGroupAdminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000004");
        public static readonly Guid EmployeeGroupMemberRoleId = Guid.Parse("00000000-0000-0000-0000-000000000005");

        public static readonly Role SystemAdminRole = new(SystemAdminRoleId, "System-Admin", "Has permissions to manage the system itself.", PermittedScopeTypes.Global);
        public static readonly Role OrganizationAdminRole = new(OrganizationAdminRoleId, "Organization-Admin", "Can fully manage an organization.");
        public static readonly Role OrganizationMemberRole = new(OrganizationMemberRoleId, "Organization-Member", "Standard user role with basic permissions in an organization.");
        public static readonly Role EmployeeGroupAdminRole = new(EmployeeGroupAdminRoleId, "Employee-group-Admin", "Can fully manage an employee group.");
        public static readonly Role EmployeeGroupMemberRole = new(EmployeeGroupMemberRoleId, "Employee-group-Member", "Standard user role with basic permissions in an employee group.");

        public static readonly IReadOnlyCollection<Role> All = new[] { SystemAdminRole, OrganizationAdminRole, OrganizationMemberRole, EmployeeGroupAdminRole, EmployeeGroupMemberRole };
    }
}
