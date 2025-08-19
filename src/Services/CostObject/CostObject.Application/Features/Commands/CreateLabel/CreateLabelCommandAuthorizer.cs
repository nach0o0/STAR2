using CostObject.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostObject.Application.Features.Commands.CreateLabel
{
    public class CreateLabelCommandAuthorizer : ICommandAuthorizer<CreateLabelCommand>
    {
        public Task AuthorizeAsync(CreateLabelCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = LabelPermissions.Create;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{command.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to create labels in this employee group is required.");
            }

            return Task.CompletedTask;
        }
    }
}
