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

namespace Organization.Application.Features.Commands.CreateHourlyRate
{
    public class CreateHourlyRateCommandAuthorizer : ICommandAuthorizer<CreateHourlyRateCommand>
    {
        public Task AuthorizeAsync(CreateHourlyRateCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var requiredPermission = HourlyRatePermissions.Create;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{command.OrganizationId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to create an hourly rate in this organization is required.");
            }

            return Task.CompletedTask;
        }
    }
}
