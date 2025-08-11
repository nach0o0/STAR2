using Organization.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Queries.GetEmployeesByEmployeeGroup
{
    public class GetEmployeesByEmployeeGroupQueryAuthorizer : ICommandAuthorizer<GetEmployeesByEmployeeGroupQuery>
    {
        public Task AuthorizeAsync(GetEmployeesByEmployeeGroupQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            // Fall 1: Ist der Benutzer selbst Mitglied in dieser Gruppe?
            if (currentUser.EmployeeGroupIds.Contains(command.EmployeeGroupId))
            {
                return Task.CompletedTask; // Erlaubt
            }

            // Fall 2: Wenn nicht, hat der Benutzer die explizite Berechtigung?
            var requiredPermission = EmployeePermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{command.EmployeeGroupId}";

            if (currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) &&
                permissions.Contains(requiredPermission))
            {
                return Task.CompletedTask; // Erlaubt
            }

            throw new ForbiddenAccessException("You are not authorized to view employees of this employee group.");
        }
    }
}
