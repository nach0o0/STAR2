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

namespace Attendance.Application.Features.Queries.GetAttendanceSummaryForEmployee
{
    public class GetAttendanceSummaryForEmployeeQueryAuthorizer : ICommandAuthorizer<GetAttendanceSummaryForEmployeeQuery>
    {
        public Task AuthorizeAsync(GetAttendanceSummaryForEmployeeQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            if (currentUser.EmployeeId == command.EmployeeId)
            {
                return Task.CompletedTask;
            }

            var requiredPermission = AttendanceEntryPermissions.Read;
            var hasPermissionInAnyGroup = currentUser.EmployeeGroupIds
                .Any(groupId => {
                    var groupScope = $"{PermittedScopeTypes.EmployeeGroup}:{groupId}";
                    return currentUser.PermissionsByScope.TryGetValue(groupScope, out var permissions) &&
                           permissions.Contains(requiredPermission);
                });

            if (!hasPermissionInAnyGroup)
            {
                throw new ForbiddenAccessException("You are not authorized to view this employee's attendance summary.");
            }

            return Task.CompletedTask;
        }
    }
}
