using MediatR;
using Planning.Application.Interfaces.Persistence;
using Planning.Domain.Entities;
using Shared.Application.Interfaces.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Commands.CreatePlanningEntry
{
    public class CreatePlanningEntryCommandHandler : IRequestHandler<CreatePlanningEntryCommand, Guid>
    {
        private readonly IPlanningEntryRepository _planningEntryRepository;
        private readonly IUserContext _userContext;

        public CreatePlanningEntryCommandHandler(IPlanningEntryRepository planningEntryRepository, IUserContext userContext)
        {
            _planningEntryRepository = planningEntryRepository;
            _userContext = userContext;
        }

        public async Task<Guid> Handle(CreatePlanningEntryCommand command, CancellationToken cancellationToken)
        {
            var currentUser = _userContext.GetCurrentUser()!;

            // Die PlannerId wird sicher aus dem Kontext des angemeldeten Benutzers geholt.
            var plannerId = currentUser.EmployeeId ?? throw new UnauthorizedAccessException("Current user must be an employee to create a planning entry.");

            var planningEntry = new PlanningEntry(
                command.EmployeeGroupId,
                command.EmployeeId,
                command.CostObjectId,
                command.PlannedHours,
                command.PlanningPeriodStart,
                command.PlanningPeriodEnd,
                plannerId,
                command.Notes
            );

            await _planningEntryRepository.AddAsync(planningEntry, cancellationToken);

            return planningEntry.Id;
        }
    }
}
