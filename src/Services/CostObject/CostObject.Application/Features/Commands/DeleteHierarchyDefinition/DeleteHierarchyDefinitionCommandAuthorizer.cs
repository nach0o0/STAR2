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

namespace CostObject.Application.Features.Commands.DeleteHierarchyDefinition
{
    public class DeleteHierarchyDefinitionCommandAuthorizer : ICommandAuthorizer<DeleteHierarchyDefinitionCommand>
    {
        private readonly IHierarchyDefinitionRepository _definitionRepository;

        public DeleteHierarchyDefinitionCommandAuthorizer(IHierarchyDefinitionRepository definitionRepository)
        {
            _definitionRepository = definitionRepository;
        }

        public async Task AuthorizeAsync(DeleteHierarchyDefinitionCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var definition = await _definitionRepository.GetByIdAsync(command.HierarchyDefinitionId, cancellationToken);
            if (definition is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.HierarchyDefinition), command.HierarchyDefinitionId);
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
