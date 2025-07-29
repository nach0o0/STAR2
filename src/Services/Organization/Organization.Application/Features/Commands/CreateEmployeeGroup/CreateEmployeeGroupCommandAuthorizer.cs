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

namespace Organization.Application.Features.Commands.CreateEmployeeGroup
{
    public class CreateEmployeeGroupCommandAuthorizer : ICommandAuthorizer<CreateEmployeeGroupCommand>
    {
        public Task AuthorizeAsync(CreateEmployeeGroupCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = EmployeeGroupPermissions.Create;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{command.LeadingOrganizationId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to create an employee group in this organization is required.");
            }

            return Task.CompletedTask;
        }
    }
}
