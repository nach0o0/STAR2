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

namespace CostObject.Application.Features.Queries.GetTopLevelCostObjectsByGroup
{
    public class GetTopLevelCostObjectsByGroupQueryAuthorizer : ICommandAuthorizer<GetTopLevelCostObjectsByGroupQuery>
    {
        public Task AuthorizeAsync(GetTopLevelCostObjectsByGroupQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Fall 1: Ist der Benutzer Mitglied der Gruppe?
            if (currentUser.EmployeeGroupIds.Contains(query.EmployeeGroupId))
            {
                return Task.CompletedTask; // Zugriff gewährt
            }

            // Fall 2: Wenn nicht, hat der Benutzer die explizite Leseberechtigung?
            var requiredPermission = CostObjectPermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{query.EmployeeGroupId}";

            if (currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) &&
                permissions.Contains(requiredPermission))
            {
                return Task.CompletedTask; // Zugriff gewährt
            }

            throw new ForbiddenAccessException("You must be a member of this group or have read permissions to view its cost objects.");
        }
    }
}
