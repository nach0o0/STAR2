using Planning.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Commands.CreatePlanningEntry
{
    public class CreatePlanningEntryCommandAuthorizer : ICommandAuthorizer<CreatePlanningEntryCommand>
    {
        public Task AuthorizeAsync(CreatePlanningEntryCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = PlanningPermissions.Write;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{command.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to create planning entries in this employee group is required.");
            }

            return Task.CompletedTask;
        }
    }
}
