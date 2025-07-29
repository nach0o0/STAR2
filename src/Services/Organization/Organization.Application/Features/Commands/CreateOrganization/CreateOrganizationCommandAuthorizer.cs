using Organization.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.CreateOrganization
{
    public class CreateOrganizationCommandAuthorizer : ICommandAuthorizer<CreateOrganizationCommand>
    {
        public Task AuthorizeAsync(CreateOrganizationCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            if (command.ParentOrganizationId.HasValue)
            {
                var requiredPermission = OrganizationPermissions.Create;
                var requiredScope = $"{PermittedScopeTypes.Organization}:{command.ParentOrganizationId.Value}";
                if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                    !permissions.Contains(requiredPermission))
                {
                    throw new ForbiddenAccessException("Permission to create a sub-organization is required.");
                }
            }
            else
            {
                var requiredPermission = OrganizationPermissions.Create;
                var requiredScope = PermittedScopeTypes.Global;
                if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                    !permissions.Contains(requiredPermission))
                {
                    throw new ForbiddenAccessException("Permission to create a top-level organization is required.");
                }
            }

            return Task.CompletedTask;
        }
    }
}
