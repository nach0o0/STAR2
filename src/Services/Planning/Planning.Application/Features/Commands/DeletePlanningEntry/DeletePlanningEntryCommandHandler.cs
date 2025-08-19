using MediatR;
using Planning.Application.Interfaces.Persistence;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planning.Application.Features.Commands.DeletePlanningEntry
{
    public class DeletePlanningEntryCommandHandler : IRequestHandler<DeletePlanningEntryCommand>
    {
        private readonly IPlanningEntryRepository _planningEntryRepository;

        public DeletePlanningEntryCommandHandler(IPlanningEntryRepository planningEntryRepository)
        {
            _planningEntryRepository = planningEntryRepository;
        }

        public async Task Handle(DeletePlanningEntryCommand command, CancellationToken cancellationToken)
        {
            var planningEntry = await _planningEntryRepository.GetByIdAsync(command.PlanningEntryId, cancellationToken);

            if (planningEntry is null)
            {
                // Sollte durch den Authorizer bereits abgefangen werden, dient aber als Sicherheitsnetz.
                throw new NotFoundException(nameof(Domain.Entities.PlanningEntry), command.PlanningEntryId);
            }

            planningEntry.PrepareForDeletion();
            _planningEntryRepository.Delete(planningEntry);
        }
    }
}
