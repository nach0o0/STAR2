using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Application.Features.Queries.GetAssignmentsByScope
{
    public record AssignedRoleInScopeResult(Guid RoleId, string Name, List<string> PermissionsInRole);
    public record DirectPermissionInScopeResult(string PermissionId, string Description);

    public record UserAssignmentsInScopeResult(
        Guid UserId,
        string UserEmail,
        List<AssignedRoleInScopeResult> AssignedRoles,
        List<DirectPermissionInScopeResult> DirectPermissions
    );

    public record GetAssignmentsByScopeQueryResult(
        List<UserAssignmentsInScopeResult> UserAssignments
    );
}
