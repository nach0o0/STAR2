using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Contracts.Responses
{
    public record UserAssignmentsResponse(
        List<AssignedRoleResponse> AssignedRoles,
        List<DirectPermissionResponse> DirectPermissions
    );
}
