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

namespace Attendance.Application.Features.Commands.CreateAttendanceType
{
    public class CreateAttendanceTypeCommandAuthorizer : ICommandAuthorizer<CreateAttendanceTypeCommand>
    {
        public Task AuthorizeAsync(CreateAttendanceTypeCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = AttendanceTypePermissions.Create;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{command.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to create attendance types for this employee group is required.");
            }

            return Task.CompletedTask;
        }
    }
}
