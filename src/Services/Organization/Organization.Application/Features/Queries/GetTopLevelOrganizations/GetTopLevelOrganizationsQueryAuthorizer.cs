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

namespace Organization.Application.Features.Queries.GetTopLevelOrganizations
{
    public class GetTopLevelOrganizationsQueryAuthorizer : ICommandAuthorizer<GetTopLevelOrganizationsQuery>
    {
        public Task AuthorizeAsync(GetTopLevelOrganizationsQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Nur Benutzer mit der globalen Leseberechtigung dürfen alle Top-Level-Organisationen sehen.
            var requiredPermission = OrganizationPermissions.Read;
            var requiredScope = PermittedScopeTypes.Global;

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Global read permission for organizations is required to view top-level organizations.");
            }

            return Task.CompletedTask;
        }
    }
}
