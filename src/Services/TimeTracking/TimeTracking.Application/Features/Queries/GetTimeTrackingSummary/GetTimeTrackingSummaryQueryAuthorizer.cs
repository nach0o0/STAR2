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

namespace TimeTracking.Application.Features.Queries.GetTimeTrackingSummary
{
    public class GetTimeTrackingSummaryQueryAuthorizer : ICommandAuthorizer<GetTimeTrackingSummaryQuery>
    {
        public Task AuthorizeAsync(GetTimeTrackingSummaryQuery query, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = TimeEntryPermissions.ReadAll;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{query.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to read time entry summaries in this employee group is required.");
            }
            return Task.CompletedTask;
        }
    }
}
