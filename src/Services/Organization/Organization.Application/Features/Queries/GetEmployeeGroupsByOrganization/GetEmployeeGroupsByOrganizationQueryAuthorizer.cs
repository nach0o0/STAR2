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

namespace Organization.Application.Features.Queries.GetEmployeeGroupsByOrganization
{
    public class GetEmployeeGroupsByOrganizationQueryAuthorizer : ICommandAuthorizer<GetEmployeeGroupsByOrganizationQuery>
    {
        public Task AuthorizeAsync(GetEmployeeGroupsByOrganizationQuery command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = EmployeeGroupPermissions.Read;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{command.OrganizationId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to read employee groups in this organization is required.");
            }

            return Task.CompletedTask;
        }
    }
}
