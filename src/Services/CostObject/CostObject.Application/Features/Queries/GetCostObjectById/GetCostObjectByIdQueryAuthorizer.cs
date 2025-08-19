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

namespace CostObject.Application.Features.Queries.GetCostObjectById
{
    public class GetCostObjectByIdQueryAuthorizer : ICommandAuthorizer<GetCostObjectByIdQuery>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public GetCostObjectByIdQueryAuthorizer(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task AuthorizeAsync(GetCostObjectByIdQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var costObject = await _costObjectRepository.GetByIdAsync(query.CostObjectId, cancellationToken);
            if (costObject is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CostObject), query.CostObjectId);
            }

            var requiredPermission = CostObjectPermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{costObject.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to read cost objects in this employee group is required.");
            }
        }
    }
}
