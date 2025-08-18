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

namespace Attendance.Application.Features.Queries.GetAttendanceEntryByDate
{
    public class GetAttendanceEntryByDateQueryAuthorizer : ICommandAuthorizer<GetAttendanceEntryByDateQuery>
    {
        public Task AuthorizeAsync(GetAttendanceEntryByDateQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Fall 1: Der Benutzer fragt seinen eigenen Eintrag ab.
            if (currentUser.EmployeeId == command.EmployeeId)
            {
                return Task.CompletedTask;
            }

            // Fall 2: Ein Administrator fragt den Eintrag ab.
            var requiredPermission = AttendanceEntryPermissions.Read;

            // Prüfe alle Gruppen des Administrators.
            var hasPermissionInAnyGroup = currentUser.EmployeeGroupIds
                .Any(groupId => {
                    var groupScope = $"{PermittedScopeTypes.EmployeeGroup}:{groupId}";
                    return currentUser.PermissionsByScope.TryGetValue(groupScope, out var permissions) &&
                           permissions.Contains(requiredPermission);
                });

            if (hasPermissionInAnyGroup)
            {
                return Task.CompletedTask;
            }

            throw new ForbiddenAccessException("You are not authorized to view this attendance entry.");
        }
    }
}
