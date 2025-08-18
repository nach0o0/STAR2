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

namespace Attendance.Application.Features.Commands.CreateWorkModel
{
    public class CreateWorkModelCommandAuthorizer : ICommandAuthorizer<CreateWorkModelCommand>
    {
        public Task AuthorizeAsync(CreateWorkModelCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = WorkModelPermissions.Create;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{command.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to create a work model in this employee group is required.");
            }

            return Task.CompletedTask;
        }
    }
}
