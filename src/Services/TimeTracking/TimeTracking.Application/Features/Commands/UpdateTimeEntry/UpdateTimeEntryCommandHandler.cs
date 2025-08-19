using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.Commands.UpdateTimeEntry
{
    public class UpdateTimeEntryCommandHandler : IRequestHandler<UpdateTimeEntryCommand>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        public UpdateTimeEntryCommandHandler(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task Handle(UpdateTimeEntryCommand command, CancellationToken cancellationToken)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(command.TimeEntryId, cancellationToken);

            if (timeEntry is null)
            {
                throw new NotFoundException(nameof(Domain.Entities.TimeEntry), command.TimeEntryId);
            }

            timeEntry.Update(
                command.EntryDate,
                command.CostObjectId,
                command.Hours,
                command.Description
            );
        }
    }
}
