using MediatR;
using Shared.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeTracking.Application.Interfaces.Persistence;

namespace TimeTracking.Application.Features.Commands.DeleteTimeEntry
{
    public class DeleteTimeEntryCommandHandler : IRequestHandler<DeleteTimeEntryCommand>
    {
        private readonly ITimeEntryRepository _timeEntryRepository;

        public DeleteTimeEntryCommandHandler(ITimeEntryRepository timeEntryRepository)
        {
            _timeEntryRepository = timeEntryRepository;
        }

        public async Task Handle(DeleteTimeEntryCommand command, CancellationToken cancellationToken)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(command.TimeEntryId, cancellationToken);

            if (timeEntry is null)
            {
                // Sollte durch den Authorizer abgedeckt sein, aber als Sicherheitsnetz.
                throw new NotFoundException(nameof(Domain.Entities.TimeEntry), command.TimeEntryId);
            }

            timeEntry.PrepareForDeletion();
            _timeEntryRepository.Delete(timeEntry);
        }
    }
}
