using Organization.Application.Interfaces.Persistence;
using Organization.Domain.Authorization;
using Organization.Domain.Entities;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organization.Application.Features.Commands.DeleteHourlyRate
{
    public class DeleteHourlyRateCommandAuthorizer : ICommandAuthorizer<DeleteHourlyRateCommand>
    {
        private readonly IHourlyRateRepository _hourlyRateRepository;

        public DeleteHourlyRateCommandAuthorizer(IHourlyRateRepository hourlyRateRepository)
        {
            _hourlyRateRepository = hourlyRateRepository;
        }

        public async Task AuthorizeAsync(DeleteHourlyRateCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var hourlyRate = await _hourlyRateRepository.GetByIdAsync(command.HourlyRateId, cancellationToken);
            if (hourlyRate is null)
            {
                throw new NotFoundException(nameof(HourlyRate), command.HourlyRateId);
            }

            var requiredPermission = HourlyRatePermissions.Delete;
            var requiredScope = $"{PermittedScopeTypes.Organization}:{hourlyRate.OrganizationId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to delete an hourly rate in this organization is required.");
            }
        }
    }
}
