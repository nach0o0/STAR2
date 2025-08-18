using Attendance.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetAttendanceTypesByEmployeeGroup
{
    public class GetAttendanceTypesByEmployeeGroupQueryAuthorizer : ICommandAuthorizer<GetAttendanceTypesByEmployeeGroupQuery>
    {
        public Task AuthorizeAsync(GetAttendanceTypesByEmployeeGroupQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = AttendanceTypePermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{command.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to read attendance types for this employee group is required.");
            }

            return Task.CompletedTask;
        }
    }
}
