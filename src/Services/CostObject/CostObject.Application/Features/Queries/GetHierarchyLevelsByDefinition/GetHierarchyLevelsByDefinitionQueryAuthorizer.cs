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

namespace CostObject.Application.Features.Queries.GetHierarchyLevelsByDefinition
{
    public class GetHierarchyLevelsByDefinitionQueryAuthorizer : ICommandAuthorizer<GetHierarchyLevelsByDefinitionQuery>
    {
        private readonly IHierarchyDefinitionRepository _definitionRepository;

        public GetHierarchyLevelsByDefinitionQueryAuthorizer(IHierarchyDefinitionRepository definitionRepository)
        {
            _definitionRepository = definitionRepository;
        }

        public async Task AuthorizeAsync(GetHierarchyLevelsByDefinitionQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var definition = await _definitionRepository.GetByIdAsync(query.HierarchyDefinitionId, cancellationToken);
            if (definition is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.HierarchyDefinition), query.HierarchyDefinitionId);
            }

            // Fall 1: Ist der Benutzer Mitglied der Gruppe?
            if (currentUser.EmployeeGroupIds.Contains(definition.EmployeeGroupId))
            {
                return; // Zugriff gewährt
            }

            // Fall 2: Wenn nicht, hat der Benutzer die explizite Management-Berechtigung?
            var requiredPermission = HierarchyPermissions.Manage;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{definition.EmployeeGroupId}";

            if (currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) &&
                permissions.Contains(requiredPermission))
            {
                return; // Zugriff gewährt
            }

            throw new ForbiddenAccessException("You must be a member of this group or have hierarchy management permissions to view its levels.");
        }
    }
}
