using Planning.Domain.Authorization;
using Shared.Application.Interfaces.Clients;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByCostObject
{
    public class GetPlanningEntriesByCostObjectQueryAuthorizer : ICommandAuthorizer<GetPlanningEntriesByCostObjectQuery>
    {
        private readonly ICostObjectServiceClient _costObjectServiceClient;

        public GetPlanningEntriesByCostObjectQueryAuthorizer(ICostObjectServiceClient costObjectServiceClient)
        {
            _costObjectServiceClient = costObjectServiceClient;
        }

        public async Task AuthorizeAsync(GetPlanningEntriesByCostObjectQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Finde heraus, zu welcher Gruppe die Kostenstelle gehört
            var costObject = await _costObjectServiceClient.GetByIdAsync(query.CostObjectId, cancellationToken);
            if (costObject is null)
            {
                throw new NotFoundException("CostObject", query.CostObjectId);
            }

            var requiredPermission = PlanningPermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{costObject.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to read planning entries for this cost object's group is required.");
            }
        }
    }
}
