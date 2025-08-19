using Planning.Application.Interfaces.Persistence;
using Planning.Domain.Authorization;
using Shared.Application.Interfaces.Security;
using Shared.Application.Security;
using Shared.Domain.Authorization;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Commands.UpdatePlanningEntry
{
    public class UpdatePlanningEntryCommandAuthorizer : ICommandAuthorizer<UpdatePlanningEntryCommand>
    {
        private readonly IPlanningEntryRepository _planningEntryRepository;

        public UpdatePlanningEntryCommandAuthorizer(IPlanningEntryRepository planningEntryRepository)
        {
            _planningEntryRepository = planningEntryRepository;
        }

        public async Task AuthorizeAsync(UpdatePlanningEntryCommand command, CurrentUser currentUser, CancellationToken cancellationToken)
        {
            var planningEntry = await _planningEntryRepository.GetByIdAsync(command.PlanningEntryId, cancellationToken);
            if (planningEntry is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.PlanningEntry), command.PlanningEntryId);
            }

            var requiredPermission = PlanningPermissions.Write;
            var requiredScope = $"{PermittedScopeTypes.EmployeeGroup}:{planningEntry.EmployeeGroupId}";

            if (!currentUser.PermissionsByScope.TryGetValue(requiredScope, out var permissions) ||
                !permissions.Contains(requiredPermission))
            {
                throw new ForbiddenAccessException("Permission to update planning entries in this employee group is required.");
            }
        }
    }
}
