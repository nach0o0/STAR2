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

namespace CostObject.Application.Features.Queries.GetLabelsByGroup
{
    public class GetLabelsByGroupQueryAuthorizer : ICommandAuthorizer<GetLabelsByGroupQuery>
    {
        public Task AuthorizeAsync(GetLabelsByGroupQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = LabelPermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{query.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to read labels in this employee group is required.");
            }

            return Task.CompletedTask;
        }
    }
}
