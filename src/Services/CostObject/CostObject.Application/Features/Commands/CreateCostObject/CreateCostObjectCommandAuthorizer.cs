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

namespace CostObject.Application.Features.Commands.CreateCostObject
{
    public class CreateCostObjectCommandAuthorizer : ICommandAuthorizer<CreateCostObjectCommand>
    {
        public Task AuthorizeAsync(CreateCostObjectCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = CostObjectPermissions.Create;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{command.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to create cost objects in this employee group is required.");
            }

            return Task.CompletedTask;
        }
    }
}
