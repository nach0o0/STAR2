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

namespace CostObject.Application.Features.Commands.UpdateCostObject
{
    public class UpdateCostObjectCommandAuthorizer : ICommandAuthorizer<UpdateCostObjectCommand>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public UpdateCostObjectCommandAuthorizer(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task AuthorizeAsync(UpdateCostObjectCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var costObject = await _costObjectRepository.GetByIdAsync(command.CostObjectId, cancellationToken);
            if (costObject is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CostObject), command.CostObjectId);
            }

            var requiredPermission = CostObjectPermissions.Update;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{costObject.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to update cost objects in this employee group is required.");
            }
        }
    }
}
