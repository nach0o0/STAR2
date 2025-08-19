using MediatR;
using Planning.Application.Interfaces.Persistence;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Commands.UpdatePlanningEntry
{
    public class UpdatePlanningEntryCommandHandler : IRequestHandler<UpdatePlanningEntryCommand>
    {
        private readonly IPlanningEntryRepository _planningEntryRepository;

        public UpdatePlanningEntryCommandHandler(IPlanningEntryRepository planningEntryRepository)
        {
            _planningEntryRepository = planningEntryRepository;
        }

        public async Task Handle(UpdatePlanningEntryCommand command, CancellationToken cancellationToken)
        {
            var planningEntry = await _planningEntryRepository.GetByIdAsync(command.PlanningEntryId, cancellationToken);

            if (planningEntry is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.PlanningEntry), command.PlanningEntryId);
            }

            planningEntry.Update(command.PlannedHours, command.Notes);
        }
    }
}
