using Planning.Domain.Authorization;
using Shared.Application.Interfaces.Infrastructure;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Queries.GetPlanningEntriesByEmployee
{
    public class GetPlanningEntriesByEmployeeQueryAuthorizer : ICommandAuthorizer<GetPlanningEntriesByEmployeeQuery>
    {
        public Task AuthorizeAsync(GetPlanningEntriesByEmployeeQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = PlanningPermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{query.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to read planning entries in this employee group is required.");
            }

            return Task.CompletedTask;
        }
    }
}
