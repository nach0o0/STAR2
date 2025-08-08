using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetAssignmentsForUserInScope
{
    public record AssignedRoleResult(Guid RoleId, string Name, List<string> PermissionsInRole);
    public record DirectPermissionResult(string PermissionId, string Description);

    public record GetAssignmentsForUserInScopeQueryResult(
        List<AssignedRoleResult> AssignedRoles,
        List<DirectPermissionResult> DirectPermissions
    );
}
