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

namespace CostObject.Application.Features.Commands.UpdateHierarchyLevel
{
    public class UpdateHierarchyLevelCommandAuthorizer : ICommandAuthorizer<UpdateHierarchyLevelCommand>
    {
        private readonly IHierarchyLevelRepository _levelRepository;
        private readonly IHierarchyDefinitionRepository _definitionRepository;

        public UpdateHierarchyLevelCommandAuthorizer(
            IHierarchyLevelRepository levelRepository,
            IHierarchyDefinitionRepository definitionRepository)
        {
            _levelRepository = levelRepository;
            _definitionRepository = definitionRepository;
        }

        public async Task AuthorizeAsync(UpdateHierarchyLevelCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var level = await _levelRepository.GetByIdAsync(command.HierarchyLevelId, cancellationToken);
            if (level is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.HierarchyLevel), command.HierarchyLevelId);
            }

            var definition = await _definitionRepository.GetByIdAsync(level.HierarchyDefinitionId, cancellationToken);
            if (definition is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.HierarchyDefinition), level.HierarchyDefinitionId);
            }

            var requiredPermission = HierarchyPermissions.Manage;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{definition.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to manage hierarchies in this employee group is required.");
            }
        }
    }
}
