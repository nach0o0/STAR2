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

namespace CostObject.Application.Features.Commands.CreateHierarchyDefinition
{
    public class CreateHierarchyDefinitionCommandAuthorizer : ICommandAuthorizer<CreateHierarchyDefinitionCommand>
    {
        public Task AuthorizeAsync(CreateHierarchyDefinitionCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = HierarchyPermissions.Manage;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{command.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to manage hierarchies in this employee group is required.");
            }

            return Task.CompletedTask;
        }
    }
}
