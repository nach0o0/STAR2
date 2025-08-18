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

namespace Organization.Application.Features.Queries.GetSubOrganizations
{
    public class GetSubOrganizationsQueryAuthorizer : ICommandAuthorizer<GetSubOrganizationsQuery>
    {
        public Task AuthorizeAsync(GetSubOrganizationsQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Um die Sub-Organisationen zu sehen, muss der Benutzer Leserechte für die übergeordnete Organisation haben.
            var requiredPermission = OrganizationPermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{command.ParentOrganizationId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to read the parent organization is required to view its sub-organizations.");
            }

            return Task.CompletedTask;
        }
    }
}
