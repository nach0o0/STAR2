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

namespace Organization.Application.Features.Queries.GetOrganizationById
{
    public class GetOrganizationByIdQueryAuthorizer : ICommandAuthorizer<GetOrganizationByIdQuery>
    {
        public Task AuthorizeAsync(GetOrganizationByIdQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            if (currentUser.OrganizationId == command.OrganizationId)
            {
                return Task.CompletedTask;
            }

            var requiredPermission = OrganizationPermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{command.OrganizationId}";

            if (currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) &&
                permissions.Contains(requiredPermission))
            {
                return Task.CompletedTask;
            }

            throw new ForbiddenAccessException("You are not authorized to view this organization's details.");
        }
    }
}
