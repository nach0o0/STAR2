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

namespace Organization.Application.Features.Commands.DeleteOrganization
{
    public class DeleteOrganizationCommandAuthorizer : ICommandAuthorizer<DeleteOrganizationCommand>
    {
        public Task AuthorizeAsync(DeleteOrganizationCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = OrganizationPermissions.Delete;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{command.OrganizationId}";
            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to delete this organization is required.");
            }

            return Task.CompletedTask;
        }
    }
}
