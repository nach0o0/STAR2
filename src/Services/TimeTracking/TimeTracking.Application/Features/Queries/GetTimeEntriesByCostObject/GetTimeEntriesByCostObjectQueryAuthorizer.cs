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
using TimeTracking.Domain.Authorization;

namespace TimeTracking.Application.Features.Queries.GetTimeEntriesByCostObject
{
    public class GetTimeEntriesByCostObjectQueryAuthorizer : ICommandAuthorizer<GetTimeEntriesByCostObjectQuery>
    {
        private readonly ICostObjectServiceClient _costObjectServiceClient;

        public GetTimeEntriesByCostObjectQueryAuthorizer(ICostObjectServiceClient costObjectServiceClient)
        {
            _costObjectServiceClient = costObjectServiceClient;
        }

        public async Task AuthorizeAsync(GetTimeEntriesByCostObjectQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var costObject = await _costObjectServiceClient.GetByIdAsync(query.CostObjectId, cancellationToken);
            if (costObject is null)
            {
                throw new NotFoundException("CostObject", query.CostObjectId);
            }

            var requiredPermission = TimeEntryPermissions.ReadAll;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{costObject.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to read time entries in this employee group is required.");
            }
        }
    }
}
