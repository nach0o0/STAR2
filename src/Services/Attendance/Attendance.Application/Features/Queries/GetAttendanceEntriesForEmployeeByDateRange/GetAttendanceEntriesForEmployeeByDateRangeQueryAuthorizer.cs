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

namespace Attendance.Application.Features.Queries.GetAttendanceEntriesForEmployeeByDateRange
{
    public class GetAttendanceEntriesForEmployeeByDateRangeQueryAuthorizer : ICommandAuthorizer<GetAttendanceEntriesForEmployeeByDateRangeQuery>
    {
        public Task AuthorizeAsync(GetAttendanceEntriesForEmployeeByDateRangeQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Fall 1: Der Benutzer fragt seine eigenen Einträge ab. Dies ist immer erlaubt.
            if (currentUser.EmployeeId == command.EmployeeId)
            {
                // Zusätzlich prüfen, ob der Benutzer Mitglied der angefragten Gruppen ist.
                var userGroups = new HashSet<Guid>(currentUser.EmployeeGroupIds);
                if (command.EmployeeGroupIds.All(requestedGroupId => userGroups.Contains(requestedGroupId)))
                {
                    return Task.CompletedTask;
                }
            }

            // Fall 2: Ein Administrator fragt die Einträge ab.
            var requiredPermission = AttendanceEntryPermissions.Read;
            foreach (var groupId in command.EmployeeGroupIds)
            {
                var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{groupId}";
                if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                    !permissions.Contains(requiredPermission))
                {
                    throw new ForbiddenAccessException($"Permission to read attendance entries for employee group '{groupId}' is required.");
                }
            }

            return Task.CompletedTask;
        }
    }
}
