using Attendance.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Features.Queries.GetPublicHolidaysByDateRange
{
    public class GetPublicHolidaysByDateRangeQueryAuthorizer : ICommandAuthorizer<GetPublicHolidaysByDateRangeQuery>
    {
        public Task AuthorizeAsync(GetPublicHolidaysByDateRangeQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = PublicHolidayPermissions.Read;

            foreach (var groupId in command.EmployeeGroupIds)
            {
                var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{groupId}";
                if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                    !permissions.Contains(requiredPermission))
                {
                    throw new ForbiddenAccessException($"Permission to read public holidays for employee group '{groupId}' is required.");
                }
            }

            return Task.CompletedTask;
        }
    }
}
