using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Contracts.Responses
{
    public record UserAssignmentsInScopeResponse(
        Guid UserId,
        string UserEmail,
        List<AssignedRoleResponse> AssignedRoles,
        List<DirectPermissionResponse> DirectPermissions
    );
}
