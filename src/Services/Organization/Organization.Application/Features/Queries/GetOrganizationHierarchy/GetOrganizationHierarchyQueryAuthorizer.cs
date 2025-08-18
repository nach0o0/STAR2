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

namespace Organization.Application.Features.Queries.GetOrganizationHierarchy
{
    public class GetOrganizationHierarchyQueryAuthorizer : ICommandAuthorizer<GetOrganizationHierarchyQuery>
    {
        public Task AuthorizeAsync(GetOrganizationHierarchyQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Um die Hierarchie zu sehen, muss der Benutzer Leserechte für die Wurzel-Organisation haben.
            var requiredPermission = OrganizationPermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{command.OrganizationId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to read the root organization is required to view its hierarchy.");
            }

            return Task.CompletedTask;
        }
    }
}
