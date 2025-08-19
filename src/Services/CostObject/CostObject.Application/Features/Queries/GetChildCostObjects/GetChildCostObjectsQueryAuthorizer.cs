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

namespace CostObject.Application.Features.Queries.GetChildCostObjects
{
    public class GetChildCostObjectsQueryAuthorizer : ICommandAuthorizer<GetChildCostObjectsQuery>
    {
        private readonly ICostObjectRepository _costObjectRepository;

        public GetChildCostObjectsQueryAuthorizer(ICostObjectRepository costObjectRepository)
        {
            _costObjectRepository = costObjectRepository;
        }

        public async Task AuthorizeAsync(GetChildCostObjectsQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var parentCostObject = await _costObjectRepository.GetByIdAsync(query.ParentCostObjectId, cancellationToken);
            if (parentCostObject is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.CostObject), query.ParentCostObjectId);
            }

            var requiredPermission = CostObjectPermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{parentCostObject.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to read the parent cost object is required to view its children.");
            }
        }
    }
}
