using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Security
{
    public static class PermissionKeys
    {
        // === Auth Service ===
        public const string PrivilegedResetPassword = "user:privileged-reset-password";

        // === Organization Service ===
        public const string OrganizationCreate = "organization:create";
        public const string OrganizationRead = "organization:read";
        public const string OrganizationUpdate = "organization:update";
        public const string OrganizationDelete = "organization:delete";
        public const string OrganizationReassignToParent = "organization:reassign-to-parent";

        public const string EmployeeGroupCreate = "employee-group:create";
        public const string EmployeeGroupRead = "employee-group:read";
        public const string EmployeeGroupUpdate = "employee-group:update";
        public const string EmployeeGroupDelete = "employee-group:delete";
        public const string EmployeeGroupTransfer = "employee-group:transfer";

        public const string EmployeeCreate = "employee:create";
        public const string EmployeeRead = "employee:read";
        public const string EmployeeUpdate = "employee:update";
        public const string EmployeeDelete = "employee:delete";
        public const string EmployeeAssignToGroup = "employee:assign-to-group";
        public const string EmployeeRemoveFromOrganization = "employee:remove-from-organization";
        public const string EmployeeAssignHourlyRate = "employee:assign-hourly-rate";

        public const string HourlyRateCreate = "hourly-rate:create";
        public const string HourlyRateRead = "hourly-rate:read";
        public const string HourlyRateUpdate = "hourly-rate:update";
        public const string HourlyRateDelete = "hourly-rate:delete";

        public const string InvitationCreate = "invitation:create";
        public const string InvitationRead = "invitation:read";
        public const string InvitationRevoke = "invitation:revoke";

        // === Permission Service ===
        public const string RoleRead = "role:read";
        public const string RoleCreate = "role:create";
        public const string RoleUpdate = "role:update";
        public const string RoleDelete = "role:delete";
        public const string RoleAssignPermission = "role:assign-permission";

        public const string PermissionRead = "permission:read";
        public const string AssignmentRead = "permission:read-assignments";
        public const string PermissionAssignRole = "permission:assign-role";
        public const string PermissionAssignDirect = "permission:assign-direct";
    }
}
