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

namespace Organization.Application.Features.Commands.UpdateOrganization
{
    public class UpdateOrganizationCommandAuthorizer : ICommandAuthorizer<UpdateOrganizationCommand>
    {
        public Task AuthorizeAsync(UpdateOrganizationCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = OrganizationPermissions.Update;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{command.OrganizationId}";
            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to update this organization is required.");
            }

            return Task.CompletedTask;
        }
    }
}
