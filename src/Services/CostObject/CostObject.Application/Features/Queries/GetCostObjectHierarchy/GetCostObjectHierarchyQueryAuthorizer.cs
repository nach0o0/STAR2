using CostObject.Application.Interfaces.Persistence;
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

namespace CostObject.Application.Features.Queries.GetCostObjectHierarchy
{
    public class GetCostObjectHierarchyQueryAuthorizer : ICommandAuthorizer<GetCostObjectHierarchyQuery>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public GetCostObjectHierarchyQueryAuthorizer(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task AuthorizeAsync(GetCostObjectHierarchyQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var rootCostObject = await _costObjectRepository.GetByIdAsync(query.RootCostObjectId, cancellationToken);
            if (rootCostObject is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CostObject), query.RootCostObjectId);
            }

            // Fall 1: Ist der Benutzer direkt Mitglied der Gruppe?
            if (currentUser.EmployeeGroupIds.Contains(rootCostObject.EmployeeGroupId))
            {
                return; // Zugriff gewährt
            }

            // Fall 2: Wenn nicht, hat der Benutzer die explizite Leseberechtigung?
            var requiredPermission = CostObjectPermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{rootCostObject.EmployeeGroupId}";

            if (currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) &&
                permissions.Contains(requiredPermission))
            {
                return; // Zugriff gewährt
            }

            throw new ForbiddenAccessException("You must be a member of this group or have read permissions to view this cost object hierarchy.");
        }
    }
}
